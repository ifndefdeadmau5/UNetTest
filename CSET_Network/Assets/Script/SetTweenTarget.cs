using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetTweenTarget : MonoBehaviour {
    public int op_num;
    public HttpRequestManager requestManager;

	// Use this for initialization
	void Start () {
        List<GameObject> gArray = new List<GameObject>();
        int i = 0;
        gArray.Add( GetComponent<UIPlayTween>().tweenTarget = GameObject.FindGameObjectWithTag("server") );
        gArray.Add(GetComponent<UIPlayTween>().tweenTarget = GameObject.FindGameObjectWithTag("OrderList") );

        requestManager = GameObject.FindGameObjectWithTag("RequestManager").GetComponent<HttpRequestManager>();

        foreach ( UIPlayTween item in GetComponents<UIPlayTween>())
        {
            item.tweenTarget = gArray[i];
            i++;
        }  
	}

    public void SetOperationNumber()
    {
        WWWHelper helper = WWWHelper.Instance;
        helper.op_num = op_num;
        Debug.Log("op_num : " + op_num);

        requestManager.ChildListRequest();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
