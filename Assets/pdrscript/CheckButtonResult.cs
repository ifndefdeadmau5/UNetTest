using UnityEngine;
using System.Collections;

public class CheckButtonResult : MonoBehaviour {

    int SendResult; // 기기로 전송될 값
    MyNetManager NetManager;

	// Use this for initialization
	void Start () {
        GameObject.FindWithTag("NetworkManager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReceiveButtonData ( int result ) // state 와 선택결과를 같이. 받는다면.
    {
        Debug.Log(result);
        SendResult = result;

        switch(name)
        {
            case "Button 0":
                break;
            case "Button 1":
                break;
        }
    }

    
}
