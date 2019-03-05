using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class TestNetwork : MonoBehaviour
{

    public string host = ""; //"dev.fairgame.kedunwl.com";
    public int port = 7001;

    public void Start()
    {
        //Connect2Server.instance.OnConnectedToServer();
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Test();
        }
    }

    [NUnit.Framework.Test(Description = "网络测试")]
    public void Init()
    {
        //        ResourcesConfigManager.Initialize();
        //JsonMessageProcessingController.Init();
        NetworkManager.GameConnection.Init();
        NetworkManager.GameConnection.SetIPAddress(host, port);
        NetworkManager.GameConnection.Connect();
        //BattleJoin battleJoin = new BattleJoin();

        //Invoke("Test", 2f);
        //  Test();
    }

    void Test()
    {
        Debug.Log("AAAAAAAAAAAAAAAA");
        //BattleJoin battleJoin = new BattleJoin();
        //GlobalEvent.AddEvent(battleJoin.resPath, (msg) => {
        //    Debug.Log((msg[0] as BattleJoin).battleId);
        //});
        //BattleTest battleTest = new BattleTest();
        //GlobalEvent.AddEvent(battleTest.resPath, (msg) => {
        //    Debug.Log("test");
        //});
        //InputManager.AddListener<CommandMessageEvent>(battleJoin.resPath, (msg) => {
        //    // battleJoin.decode(msg.Data);
        //    Debug.Log(battleJoin.userId);
        //});
        //Request(battleJoin);
    }

    //public void Request<T>(T msg) where T : _streamCode, _getMsg
    //{
    //    msg.encode();
    //    NetworkManager.GameConnection.SendMessage(msg.getRequestPath(), msg.getData());
    //}

    //public void Receive<T>(T msg) where T : _streamCode, _getMsg
    //{


    //}

}
