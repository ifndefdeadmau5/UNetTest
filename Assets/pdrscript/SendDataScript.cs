﻿using UnityEngine;
using System.Collections;

public class SendDataScript : MonoBehaviour {

    MyNetManager NetManager;
    int result;
    int sendCount;
    static int QuestionNumber;
    private UILabel SendedLabel;

    // Use this for initialization
    void Start()
    {
        NetManager = GameObject.FindWithTag("NetworkManager").GetComponent<MyNetManager>( );
        SendedLabel = GameObject.FindWithTag("SendLabel").GetComponent<UILabel>();

        if ( null != NetManager)
        {
            NGUIDebug.Log("NetManager OK");
        }
        else {
            NGUIDebug.Log("NetManager NOT OK");
        }

        result = 0;
        sendCount = 0;
    }

    public void SendResult( )
    {
        
        NGUIDebug.Log("Sended " + sendCount + " time");
        
        if( NetManager.SendScore(result) )
        {
            sendCount++;
        }

        SendedLabel.text = "Sended " + sendCount + " time";
    }

    public void StoreData( int Result )
    {
        Debug.Log("StoreData");
        result = Result;
    }


    // Update is called once per frame
    void Update () {
	
	}
}
