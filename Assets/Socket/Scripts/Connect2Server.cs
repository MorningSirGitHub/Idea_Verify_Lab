using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class Connect2Server : MonoSingleton<Connect2Server>
{

    public void OnConnectedToServer()
    {
        OnConnectedToLoginServer();
        OnConnectedToSceneServer();
        OnConnectedToGameServer();
    }

    //连接登录服
    public void OnConnectedToLoginServer()
    {
        NetworkBase connection = NetworkManager.LoginConnection;
        connection.Close();
        //JsonMessageProcessingController.Init();

        connection.Init();
        connection.SetIPAddress(NetworkConfig.LOGINHOST, NetworkConfig.LOGINPORT);
        connection.Connect();
    }

    //连接场景服 
    public void OnConnectedToSceneServer()
    {
        NetworkBase connection = NetworkManager.SceneConnection;
        connection.Close();
        //JsonMessageProcessingController.Init();

        connection.Init();
        connection.SetIPAddress(NetworkConfig.SCENEHOST, NetworkConfig.SCENEPORT);
        connection.Connect();
    }

    //连接战斗服 
    public void OnConnectedToGameServer()
    {
        NetworkBase connection = NetworkManager.GameConnection;
        connection.Close();
        //JsonMessageProcessingController.Init();

        connection.Init();
        connection.SetIPAddress(NetworkConfig.GAMEHOST, NetworkConfig.GAMEPORT);
        connection.Connect();
    }

}
