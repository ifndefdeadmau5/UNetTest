using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using LitJson;

public class MyNetManager : NetworkManager
{
    NetworkClient myClient;
    public UIPlayTween Play;
    public NetworkDiscovery discovery;
    public AcceptClientGridManager playerListGrid;
    public csetGridManager CsetGridManager;
    WWWHelper helper;

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
    void Start()
    {
    }

    /* 메시지 전송 관련 */

    // 클라이언트로부터 받을 검사 문항 결과
    public class ScoreMessage : MessageBase 
    {
        public int completeNumber;
        public string uid;
    }

    // 시작 명령
    public class StartMessage : MessageBase
    {
        public int Number;
    }
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
		public static short UID = MsgType.Highest + 3;
		// 기기 고유
	};
    public void SendScore(int score)
    {
        ScoreMessage msg = new ScoreMessage();
        msg.completeNumber = score;

        myClient.Send(MyMsgType.CompleteNumber, msg);
    }

    public void SendStartSignal(/*int number*/)
    {
        StartMessage msg = new StartMessage();
        msg.Number = 1;

        NetworkServer.SendToAll(MyMsgType.Number, msg);
    }

    /* 콜백 모음 */
    public void OnScore(NetworkMessage netMsg)
    {
        PlayerManager playerManager = PlayerManager.Instance;
        //클라이언트로 부터 데이터를 받습니다.
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>();
        Debug.Log( "OnScoreMessage " + msg.uid + ":" + msg.completeNumber );
        //List.Progress( netMsg.conn.connectionId.ToString() );
        CsetGridManager.Progress( msg.uid );
        playerManager.setChildAnswer( netMsg.conn.connectionId, msg.completeNumber );

        /** 이 부분에서 sqlite 에 수신한 데이터를 저장하는 루틴을 구현합니다. **/
    }


	// 접속 후 기기 고유값을 받았을 때, 유저 추가
	public void OnUID (NetworkMessage netMsg)
	{
		UidMessage msg = netMsg.ReadMessage<UidMessage> ();
		Debug.Log ("OnUID " + msg.uid);

		PlayerManager playerManager = PlayerManager.Instance;

		// If UID is already exsist, do not add new player
        // 재접속 했을 경우
		if ( playerManager.IsExsistUID( msg.uid ) ) {
			NGUIDebug.Log ("already exsist.");
            playerManager.SetPlayerConnId(netMsg.conn.connectionId, msg.uid);// connID 새로 부여
            if (null != playerListGrid)
            {
                playerListGrid.addItem(netMsg.conn.connectionId, msg.uid);
            }
        } else {
            // 신규 접속
			playerManager.addPlayer( netMsg.conn.connectionId, msg.uid ); //
            if( null != playerListGrid)
            {
                playerListGrid.addItem(netMsg.conn.connectionId, msg.uid);
            }
            else { NGUIDebug.Log("PlayerListGrid is NULL"); }
        }
	}


    // 클라이언트 접속시
    public void OnConnected(NetworkMessage netMsg)
    {
        // playManager 의 playerList 와 (데이터 관리용도)
        // playerListGrid 스크립트의 리스트(유저 목록 인터페이스 표시 용도)에 하나씩 추가한다
        Debug.Log("Connected address:" + netMsg.conn.address);
    }

    public void OnDisconnected(NetworkMessage netMsg)
    {
        Debug.Log("Disconnected :" + netMsg.conn.connectionId);
        PlayerManager playerManager = PlayerManager.Instance;

        playerManager.SetPlayerOffline(netMsg.conn.connectionId);
        playerListGrid.deleteItem(netMsg.conn.connectionId);
    }

    // 모든 클라이언트 팅구기
    public void kickAllClient()
    {
        NetworkServer.DisconnectAll();
    }

    public void printPlayerList()
    {
        PlayerManager playerManager = PlayerManager.Instance;
        playerManager.print();
    }

    /* 아래는 NeteworkManager 에서 상속된 메서드 */
    public override void OnStartHost( )
    {
        Debug.Log("OnStartHost( )");
    }

    public override void OnStartClient( NetworkClient client )
    {
        Debug.Log("OnStartClient( )");

        discovery.showGUI = false;
    }

    public override void OnStopClient( )
    {
        Debug.Log("OnStopClient( )");
        discovery.StopBroadcast();
        discovery.showGUI = true;
    }

    /* 사용자 정의 함수 */
    public void SetupServer( )
    {
        if (!NetworkServer.active)
        {
            Debug.Log("SetupServer( )");


            NetworkServer.Listen(4444);
            NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
            NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnected);
            NetworkServer.RegisterHandler(MyMsgType.CompleteNumber, OnScore);
            NetworkServer.RegisterHandler(MyMsgType.UID, OnUID);
        }

        if( !discovery.running)
        {
            discovery.Initialize();
            discovery.StartAsServer();
        }
    }


    public void SetupClient()
    {
        Debug.Log("SetupClient()");
        myClient = new NetworkClient();
        discovery.Initialize();
        discovery.StartAsClient();
    }

    public void DisconnectAllClient( )
    {
        NetworkServer.DisconnectAll();
    }


    public void DisableServer()
    {
        PlayerManager playerManager = PlayerManager.Instance;

        Debug.Log("StopServer");
        if( NetworkServer.active )
        {
            Debug.Log("서버 가동중이므로 닫습니다");
            NetworkServer.Shutdown();
        }
        if (discovery.running)
        {
            Debug.Log("브로드캐스팅 중이므로 중지합니디/");

            discovery.StopBroadcast();
        }
    }
}
