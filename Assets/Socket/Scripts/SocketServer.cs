using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;

namespace Network
{

    public class SocketServer : SocketBase
    {

        protected Socket m_Socket;

        private AsyncCallback m_ConnAyncCallback = null;
        private AsyncCallback m_SendAyncCallback = null;
        private AsyncCallback m_RecvAyncCallback = null;

        public const int HEAD_LENGTH = 4;

        private byte[] m_DealData;
        private int m_BufferSize = 0;//处理过的大小
        private int m_PackageSize = 0; // 逻辑包大小

        public override void Init()
        {
            m_ConnAyncCallback = new AsyncCallback(ConnectCallback);
            m_SendAyncCallback = new AsyncCallback(SendCallback);
            m_RecvAyncCallback = new AsyncCallback(ReceiveCallback);

            m_DealData = new byte[1024 * 1024]; // 暂时设置为1M
        }

        public override void Close()
        {
            //ApplicationManager.s_OnApplicationQuit -= Close;
            try
            {
                m_IsConnect = true;
                if (m_Socket != null)
                {
                    m_Socket.Shutdown(SocketShutdown.Both);
                    m_Socket.Close();
                    m_Socket = null;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override void Connect()
        {
            Close();
            RequestConnect();
        }

        public override void Send(byte[] buffer, int offset, int size)
        {
            m_Socket.BeginSend(buffer, offset, size, SocketFlags.None, m_SendAyncCallback, null);
        }

        public override void Update()
        {

        }

        void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Complete the connection.
                m_Socket.EndConnect(ar);

                m_ConnectStatusCallback(NetworkState.Connected);
                m_IsConnect = true;

                Debug.Log("Socket connected to " + m_Socket.RemoteEndPoint.ToString());
                StartReceive();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        void SendCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = m_Socket.EndSend(ar);
                Debug.LogFormat("Sent {0} bytes to server.", bytesSent);
            }
            catch (Exception e)
            {
                m_IsConnect = false;
                m_ConnectStatusCallback(NetworkState.ConnectBreak);
                Debug.LogException(e);
            }
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            int bytesRead = m_Socket.EndReceive(ar);
            Debug.Log("ReceiveCallback bytesRead:" + bytesRead);
            if (bytesRead > 0)
            {
                DealByte(m_ReadData, 0, bytesRead);
                StartReceive();
            }
        }

        void StartReceive()
        {
            Debug.Log("StartReceive !! ");
            m_Socket.BeginReceive(m_DealData, 0, m_DealData.Length, SocketFlags.None, m_RecvAyncCallback, null);
        }

        void RequestConnect()
        {
            try
            {
                m_ConnectStatusCallback(NetworkState.Connecting);
                SocketType socketType = SocketType.Stream;

                if (m_protocolType == ProtocolType.Udp)
                    socketType = SocketType.Dgram;

                m_Socket = new Socket(AddressFamily.InterNetwork, socketType, m_protocolType);
                IPAddress ipAddress = null;
                try
                {
                    if (!IPAddress.TryParse(m_IPAddress, out ipAddress))
                    {
                        IPAddress[] addresses = Dns.GetHostEntry(m_IPAddress).AddressList;
                        foreach (var item in addresses)
                        {
                            if (item.AddressFamily == AddressFamily.InterNetwork || item.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                ipAddress = item;
                                break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                IPEndPoint ipep = new IPEndPoint(ipAddress, m_Port);
                m_Socket.NoDelay = true;

                Debug.LogFormat("BeginConnect \n\n IP: {0} \n Port: {1} \n", m_IPAddress, m_Port);
                m_Socket.BeginConnect(ipep, m_ConnAyncCallback, m_Socket);

                //ApplicationManager.s_OnApplicationQuit += Close;
                //StartReceive();
            }
            catch (Exception e)
            {
                m_IsConnect = false;
                m_ConnectStatusCallback(NetworkState.FaildToConnect);
                Debug.LogException(e);
            }
        }

        void DealByte(byte[] data, int offset, int length)
        {
            Debug.Log("Split Message Length: " + length);
            int leftSize = length;
            while (leftSize > 0)
            {
                int startPos = offset + length - leftSize;
                Debug.Log("leftSize:" + leftSize + " startPos: " + startPos);
                if (m_BufferSize < HEAD_LENGTH)
                {
                    int need = HEAD_LENGTH - m_BufferSize; // 4
                    Debug.Log("need: " + need);
                    if (leftSize < need)
                    {
                        //加起来不够四节时,先拿data填充
                        for (int i = 0; i < need; i++)
                        {
                            m_DealData[m_BufferSize + i] = data[startPos + i];
                        }
                        m_BufferSize += need;
                        startPos += need;
                        leftSize -= need;
                    }
                    else
                    {
                        for (int i = 0; i < need; i++)
                        {
                            m_DealData[m_BufferSize + i] = data[startPos + i];
                        }
                        m_BufferSize += need;
                        startPos += need;
                        leftSize -= need;

                        int pos = 0;
                        this.m_PackageSize = ByteTool.ReadInt(m_DealData, ref pos); // Bug点
                        Debug.Log("packageSize: " + m_PackageSize);

                        if (leftSize < m_PackageSize) // 不够完整包，先放入buffer，等下个物理包接着处理
                        {
                            for (int i = 0; i < leftSize; i++)
                            {
                                m_DealData[m_BufferSize + i] = data[startPos + i];
                            }
                            m_BufferSize += leftSize;
                            startPos += leftSize;
                            leftSize -= leftSize;
                        }
                        else
                        {
                            for (int i = 0; i < m_PackageSize; i++)
                            {
                                m_DealData[m_BufferSize + i] = data[startPos + i];
                            }
                            m_BufferSize += m_PackageSize;
                            startPos += m_PackageSize;
                            leftSize -= m_PackageSize;

                            m_ByteCallBack(m_DealData, 4, m_PackageSize);
                            string msg = System.Text.Encoding.UTF8.GetString(m_DealData, 4, m_PackageSize);
                            Debug.Log(" Message 1: " + msg);
                            //DealMessage(msg);

                            // reset buffer
                            m_PackageSize = 0;
                            for (int i = 0; i < m_BufferSize; i++)
                            {
                                m_DealData[i] = 0;
                            }
                            m_BufferSize = 0;
                        }
                    }
                }
                else
                {
                    int need = m_PackageSize + 4 - m_BufferSize;
                    if (leftSize < need)
                    {
                        for (int i = 0; i < leftSize; i++)
                        {
                            m_DealData[m_BufferSize + i] = data[startPos + i];
                        }
                        m_BufferSize += leftSize;
                        startPos += leftSize;
                        leftSize -= leftSize;
                    }
                    else
                    {
                        for (int i = 0; i < need; i++)
                        {
                            m_DealData[m_BufferSize + i] = data[startPos + i];
                        }
                        m_BufferSize += need;
                        startPos += need;
                        leftSize -= need;

                        m_ByteCallBack(m_DealData, 4, m_PackageSize);
                        //msg = System.Text.Encoding.UTF8.GetString(m_dealData, 4, packageSize);
                        //Debug.Log(" Message 2: " + msg);
                        //DealMessage(msg);

                        // reset buffer
                        m_PackageSize = 0;
                        for (int i = 0; i < m_BufferSize; i++)
                        {
                            m_DealData[i] = 0;
                        }
                        m_BufferSize = 0;
                    }
                }
            } // end for while
        }

    }

}


