using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class ModifyPopupList : MonoBehaviour {
    UIPopupList PopupList;

    void Awake()
    {

    }
    // Use this for initialization
    void Start () {
        PlayerManager playerManager = PlayerManager.Instance;
        

        PopupList = gameObject.GetComponent<UIPopupList>();
        Debug.Log(playerManager.childList.Count);

        //foreach (var item in playerManager.ChildList)
        for ( int i = 0; i < playerManager.childList.Count; i++ )
        {
       

            PopupList.AddItem( playerManager.childList[i].cm_name + "/" + playerManager.childList[i].m_name );
        }
    }

    public void SelectChild( UIPopupList list )
    {
        PlayerManager playerManager = PlayerManager.Instance;

        // 선택된 유저 항목에서 connID 값을 가진 라벨에서 아동명/부모명을 얻어온다.
        UILabel connIdLabel = gameObject.transform.parent.parent.transform.FindChild("connIdLabel").GetComponent<UILabel>();
        int connId = System.Convert.ToInt32(connIdLabel.text);

        string destString = list.value;
        string m_name = "";
        string cm_name = "";

        string[] StringArray = destString.Split('/');
        cm_name = StringArray[0];
        m_name = StringArray[1];

        for (int i = 0; i < playerManager.childList.Count; i++)
        {
            if ( (playerManager.childList[i].cm_name == cm_name) && (playerManager.childList[i].m_name == m_name) )
            // childList 내에서 팝업리스트에서 선택한 아동/부모명과 일치하는 데이터를 검색
            {
 
                playerManager.setChildToDevice(cm_name, m_name, connId, playerManager.childList[i].cm_num, playerManager.childList[i].m_num, playerManager.childList[i].oi_num );
            }
        }
        
        
    }

   
}
