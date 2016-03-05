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
    }
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        //서버로부터 브로드캐스트 메시지를 받았을 때 실행됩니다.
        base.OnReceivedBroadcast(fromAddress, data);

        //string destString = fromAddress;


        //destString = destString.Substring(6);
        //Debug.Log("recived broadcast from : " + destString);
        Debug.Log(fromAddress);
        netManager.ConnectToServer(fromAddress);
        StopBroadcast( );
        TweenLabel.text = "Server Connected..Send UID";
    }
}
