using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Network
{

    public abstract class SocketBase
    {

        public string m_IPAddress = "";
        public int m_Port;

        public bool m_IsConnect = false;

        public ProtocolType m_protocolType;

        public ByteCallBack m_ByteCallBack;
        public MessageCallBack m_MessageCallBack;
        public ConnectStatusCallBack m_ConnectStatusCallback;

        public byte[] m_ReadData = new byte[1024];
        public byte[] m_SendData = new byte[1024 * 10];

        public virtual void Init()
        {

        }

        public virtual void Dispose()
        {

        }

        /// <summary>
        /// 设置IP及端口
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public virtual void SetIPAdress(string ip, int port)
        {
            m_IPAddress = ip;
            m_Port = port;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public abstract void Send(byte[] buffer, int offset, int size);

        ///// <summary>
        ///// 收到数据
        ///// </summary>
        ///// <param name="message"></param>
        //public abstract void ReceviceMeaasge(NetWorkMessage message);

        /// <summary>
        /// 建立连接
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// 关闭连接
        /// </summary>
        public abstract void Close();

        public abstract void Update();

    }

}
