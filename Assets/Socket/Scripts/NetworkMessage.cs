using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Network
{

    public abstract class NetworkMessage
    {

    }

    public abstract class INetworkReq
    {

    }

    public abstract class INetworkResp
    {

    }

    public class PingReq : INetworkReq
    {

    }

    public class PongResq : INetworkResp
    {

    }

    public class ErrorResp : INetworkResp
    {
        public int code;
        public string msg;
    }

    /************************************************* 账号信息 ****************************************************/

    public class AccountLoginReq : INetworkReq
    {
        public string name;
    }

    /************************************************* 场景信息 ****************************************************/

    public class SceneLoginReq : INetworkReq
    {
        public string id;
        public string token;
    }

    /************************************************* 战斗信息 ****************************************************/

    public class CommandReq : INetworkReq
    {
        public byte[] data;
    }

}
