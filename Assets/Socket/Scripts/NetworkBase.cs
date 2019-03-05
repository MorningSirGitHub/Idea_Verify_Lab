using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Network
{

    public abstract class NetworkBase
    {

        public SocketBase m_SocketServer;

        private float m_TempHeatBeatTimer;
        private bool m_StartSendHeatBeat = false;

        #region 心跳包
        private static float m_HeatBeatDelayTime = 10f;
        /// <summary>
        /// 设置心跳包发送间隔时间
        /// </summary>
        public static float HeatBeatDelayTime
        {
            get
            {
                return m_HeatBeatDelayTime;
            }
            set
            {
                m_HeatBeatDelayTime = Mathf.Clamp(value, 2f, 100);
            }
        }
        #endregion

        private Queue<byte[]> m_SendQueue = new Queue<byte[]>();
        private Queue<NetworkState> m_StatusQueue = new Queue<NetworkState>();
        private Queue<NetWorkMessage> m_MessageQueue = new Queue<NetWorkMessage>();

        public virtual void Init()
        {
            m_SocketServer.Init();
            m_SocketServer.m_ConnectStatusCallback = ConnectStatusChange;
            m_SocketServer.m_ByteCallBack = ProcessBytes;
            m_SocketServer.m_MessageCallBack = ProcessMessage;
        }

        public bool IsConnected()
        {
            return m_SocketServer != null && m_SocketServer.m_IsConnect;
        }

        public virtual void GetIPAddress()
        {

        }

        public virtual void SetIPAddress(string ip, int port)
        {
            m_SocketServer.SetIPAdress(ip, port);
        }

        public virtual void Connect()
        {
            m_SocketServer.Connect();
        }

        public virtual void Close()
        {
            m_SocketServer.Close();
        }

        public virtual void Update()
        {
            if (!IsConnected())
                return;

            lock (m_StatusQueue)
            {
                while (m_StatusQueue.Count > 0)
                {
                    NetworkState networkState = m_StatusQueue.Dequeue();
                    Dispatch(networkState);
                }
            }

            lock (m_MessageQueue)
            {
                while (m_MessageQueue.Count > 0)
                {
                    NetWorkMessage netWorkMessage = m_MessageQueue.Dequeue();
                    Dispatch(netWorkMessage);
                }
            }

            lock (m_SendQueue)
            {
                while (m_SendQueue.Count > 0)
                {
                    byte[] buffer = m_SendQueue.Dequeue();
                    m_SocketServer.Send(buffer, 0, buffer.Length);
                }
            }

            TrySendPing();
            m_SocketServer.Update();
        }

        public abstract void SendRequest(string path, INetworkReq data);

        //public abstract void SendMessage(string path, Dictionary<string, object> data);

        public abstract void ProcessBytes(byte[] data, int offset, int length);

        //public void SendMessage<T>(T msg) where T : _streamCode, _getMsg
        //{
        //    msg.encode();
        //    SendMessage(msg.getRequestPath(), msg.getData());
        //}

        public void SendBytes(byte[] buffer, int offset, int size)
        {
            Debug.Log("Send byte : __ " + buffer);
            //lock (m_sendQueue)
            //{
            //    m_sendQueue.Enqueue(buffer);
            //}
            m_SocketServer.Send(buffer, offset, size);
        }

        private void ConnectStatusChange(NetworkState status)
        {
            Debug.Log("ConnectStatusChange: __ " + status);
            lock (m_StatusQueue)
            {
                m_StatusQueue.Enqueue(status);
            }
        }

        public void ProcessMessage(NetWorkMessage msg)
        {
            if (msg.m_path != null)
            {
                lock (m_MessageQueue)
                {
                    m_MessageQueue.Enqueue(msg);
                }
            }
            else
            {
                Debug.Log("ReceviceMeaasge m_MessagePath is null !");
            }
        }

        private void Dispatch(NetworkState state)
        {
            //InputNetworkEventProxy.DispatchStatusEvent(this, networkState);

            if (state == NetworkState.Connected)
            {
                m_StartSendHeatBeat = true;
            }
            else
            {
                m_StartSendHeatBeat = false;
            }
        }

        private void Dispatch(NetWorkMessage netWorkMessage)
        {
            try
            {
                // TODO: 处理接口回调 --> GameNetworkManager
                //InputNetworkEventProxy.DispatchMessageEvent(netWorkMessage.m_path, netWorkMessage.m_param);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void TrySendPing()
        {
            if (m_StartSendHeatBeat)
            {
                if (m_TempHeatBeatTimer <= 0)
                {
                    m_TempHeatBeatTimer = HeatBeatDelayTime;
                    //SendMessage(ReqPath.ping, null);
                }
                else
                {
                    m_TempHeatBeatTimer -= Time.unscaledDeltaTime;
                }
            }
        }

    }

}
