﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class MyNetManager : NetworkManager
{
	NetworkClient myClient;
	public NetworkDiscovery discovery;
	public string Address;
	/* 안드로이드 기기 버튼 처리(홈, 뒤로가기, ... )
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home))
            {
                //home button
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                //back button
                //Play
            }
            else if (Input.GetKey(KeyCode.Menu))
            {
                //menu button
            }
        }

    }*/
	void Start ()
	{
		//SceneManager.LoadScene("stage03");
		SetupClient ();
	}

	/* 메시지 전송 관련 */
	public class ScoreMessage : MessageBase
	{
		public int completeNumber;
	}

	public class StartMessage : MessageBase
	{
		public int Number;
	}

	// 전송할 UID
	public class UidMessage : MessageBase
	{
		public string uid;
	}

	public class MyMsgType
	{
		public static short CompleteNumber = MsgType.Highest + 1;
		// 검사문항 결과값
		public static short Number = MsgType.Highest + 2;
		// 시작 신호
		public static short UID = MsgType.Highest + 3; // 기기 고유값
	};

	public void SendUID ( )
	{
		UidMessage msg = new UidMessage ();
		msg.uid = SystemInfo.deviceUniqueIdentifier;
		  
		myClient.Send (MyMsgType.UID, msg);
	}

    // Returns:
    //     ///
    //     True if message was sent.
    //     ///
    public bool SendScore (int score)
	{
        bool sendResult;
		ScoreMessage msg = new ScoreMessage ();
		msg.completeNumber = score;

        //NetworkServer.SendToAll(MyMsgType.CompleteNumber, msg);

        sendResult = myClient.Send (MyMsgType.CompleteNumber, msg);

        return sendResult;
	}

	/* 콜백 모음 */
	public void OnStartSignal (NetworkMessage netMsg)
	{
		StartMessage msg = netMsg.ReadMessage<StartMessage> ();
		Debug.Log ("OnStartSignal");

		SceneManager.LoadScene ("NetworkTest");
	}

	public void OnConnected (NetworkMessage netMsg)
	{  
		Debug.Log ("Connected :" + netMsg.conn.address);
		SendUID ();
	}

	public void OnDisconnected (NetworkMessage netMsg)
	{  
		Debug.Log ("Disconnected :" + netMsg.conn.address);
		  
		myClient.Connect (Address, 4444);
		//Debug.Log("OnClientDisconnect( )");
		//Debug.Log(conn.connectionId);
		//discovery.showGUI = true;
	}
	  


	/* 아래는 NeteworkManager 에서 상속된 메서드 */
	public override void OnStartClient (NetworkClient client)
	{
		Debug.Log ("OnStartClient( )");
		discovery.showGUI = false;
	}

	public override void OnStopClient ()
	{
		Debug.Log ("OnStopClient( )");
		discovery.showGUI = true;
	}

	/* 사용자 정의 함수 */

	public void SetupClient ()
	{
		Debug.Log ("SetupClient()");
		myClient = new NetworkClient ();
        
		discovery.Initialize ();
		discovery.StartAsClient ();

		myClient.RegisterHandler (MsgType.Connect, OnConnected);
		myClient.RegisterHandler (MyMsgType.Number, OnStartSignal);

		myClient.RegisterHandler (MsgType.Disconnect, OnDisconnected);
	}

	public void ConnectToServer (string address)
	{
		Address = address;
		myClient.Connect (address, 4444);
	}
}