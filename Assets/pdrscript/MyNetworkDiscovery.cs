using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkDiscovery : NetworkDiscovery {
    private UILabel TweenLabel;
    public UIPlayTween Play;
    public MyNetManager netManager;
    void Awake()
    {
        netManager = GameObject.FindWithTag("NetworkManager").GetComponent<MyNetManager>( );
        TweenLabel = GameObject.FindWithTag("TweenLabel").GetComponent<UILabel>();
        Initialize();
    }
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        //서버로부터 브로드캐스트 메시지를 받았을 때 실행됩니다.
        base.OnReceivedBroadcast(fromAddress, data);

        Debug.Log(fromAddress);

        if ( !netManager.isConnected() )
        {
            netManager.ConnectToServer(fromAddress);
            Debug.Log("연결중이 아니므로 접속합니다");
            TweenLabel.text = "Connect to server...";
        }
        else
        {
            TweenLabel.text = "Now Connected";

        }      
        
    }
}
