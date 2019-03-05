using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Network
{

    public enum NetworkState
    {
        Connected,                // 已连接
        Connecting,               // 连接中
        ConnectBreak,             // 连接中断
        FaildToConnect,           // 连接失败
    }

    public struct NetWorkMessage
    {
        //public string m_onCommand;

        public string m_path;

        public string m_param;

        //public JsonData m_data;
    }

    public delegate void ByteCallBack(byte[] data, int offset, int length);
    public delegate void MessageCallBack(NetWorkMessage receStr);
    public delegate void ConnectStatusCallBack(NetworkState connectStstus);

    public class NetworkManager
    {
        private static NetworkBase s_LoginConnection;
        public static NetworkBase LoginConnection
        {
            get
            {
                if (s_LoginConnection == null)
                {
                    s_LoginConnection = new JsonNetworkService();
                    s_LoginConnection.m_SocketServer = new SocketServer();
                }

                return s_LoginConnection;
            }
        }

        private static NetworkBase s_SceneConnection;
        public static NetworkBase SceneConnection
        {
            get
            {
                if (s_SceneConnection == null)
                {
                    s_SceneConnection = new JsonNetworkService();
                    s_SceneConnection.m_SocketServer = new SocketServer();
                }

                return s_SceneConnection;
            }
        }

        private static NetworkBase s_GameConnection;
        public static NetworkBase GameConnection
        {
            get
            {
                if (s_GameConnection == null)
                {
                    s_GameConnection = new JsonNetworkService();
                    s_GameConnection.m_SocketServer = new SocketServer();
                }

                return s_GameConnection;
            }
        }

        // 将消息并入主线程
        public static void Update()
        {
            if (s_LoginConnection != null)
                s_LoginConnection.Update();

            if (s_SceneConnection != null)
                s_SceneConnection.Update();

            if (s_GameConnection != null)
                s_GameConnection.Update();
        }

    }

}
