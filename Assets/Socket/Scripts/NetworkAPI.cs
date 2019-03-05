using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Network
{

    public class NetworkAPI
    {

        /************************************************* 账号接口 ****************************************************/

        public static void LoginAccount(AccountLoginReq req)
        {
            NetworkManager.LoginConnection.SendRequest("/account/login", req);
        }

        /************************************************* 场景接口 ****************************************************/

        public static void LoginScene(SceneLoginReq req)
        {
            NetworkManager.SceneConnection.SendRequest("/scene/login", req);
        }

        /************************************************* 战斗接口 ****************************************************/

        public static void Command(CommandReq req)
        {
            NetworkManager.GameConnection.SendRequest("/battle/command", req);
        }

    }

}
