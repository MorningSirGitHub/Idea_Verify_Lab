using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Network
{

    public class JsonNetworkService : NetworkBase
    {

        public override void Init()
        {
            base.Init();
        }

        public override void ProcessBytes(byte[] data, int offset, int length)
        {
            int pos = 0;
            for (int i = offset; i < offset + length; i++)
            {
                if (data[i] == ';')
                {
                    pos = i;
                    break;
                }
            }
            int pathLen = pos - offset;
            string path = System.Text.Encoding.UTF8.GetString(data, offset, pos - offset);
            string paramJson = System.Text.Encoding.UTF8.GetString(data, pos + 1, length - pathLen - 1);

            DealMessage(path, paramJson);
        }

        private void DealMessage(string path, string paramJson)
        {
            try
            {
                Debug.Log("[Network Resp]:" + path + " paramJson:" + paramJson);

                NetWorkMessage msg = new NetWorkMessage();

                msg.m_path = path;
                msg.m_param = paramJson;

                ProcessMessage(msg);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override void SendRequest(string path, INetworkReq data)
        {
            try
            {
                Debug.Log("[Network Req]: " + path);
                Debug.Log("[Network Req]: " + JsonTool.ObjectToJson(data).ToString());
                string msg = path + ";";
                string json = null;
                if (data != null)
                {
                    json = JsonTool.ObjectToJson(data);
                    msg += json;
                }
                int len = System.Text.Encoding.UTF8.GetBytes(msg, 0, msg.Length, m_SocketServer.m_SendData, 4);
                int pos = 0;
                ByteTool.WriteInt(len, m_SocketServer.m_SendData, ref pos);
                SendBytes(m_SocketServer.m_SendData, 0, len + 4);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

    }

}
