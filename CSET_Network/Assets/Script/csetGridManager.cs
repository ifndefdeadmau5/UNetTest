using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class csetGridManager : MonoBehaviour {
    public GameObject ItemPrefab; // Prefab 폴더에 CSETItem
    public HttpRequestManager requestManager;
    GameObject GetPrefab;
    UIGrid grid;
    UIPanel panel;

    //Connection ID, 그리드 아이템 이 세트로 묶여서 저장
    IDictionary<string, GameObject> dic = new Dictionary<string, GameObject> { };

    // Use this for initialization
    void Awake()
    {
        grid = GetComponent<UIGrid>();
        panel = GetComponent<UIPanel>();

    }
    public void Init () {
        PlayerManager playerManager = PlayerManager.Instance;

        Debug.Log("OnInit");
        foreach (var item in playerManager.playerList)
        {
            Debug.Log(playerManager.playerList.Count + "Cleints Started");
            ItemPrefab.transform.FindChild("connID").GetComponent<UILabel>().text = item.uid;
            ItemPrefab.transform.FindChild("childInfo").GetComponent<UILabel>().text = item.cm_name + "/" + item.m_name;            

            dic.Add( item.uid, NGUITools.AddChild(gameObject, ItemPrefab));
        }
      
        grid.Reposition();
        panel.Refresh();
    }

    public void Progress(string uid)
    {

        int dicCount = dic.Count;
        int i = 0;


        UISlider slider;
        //Transform[] allChilds;
        //int childCount = 0; 

        Debug.Log("On Progress" + uid);
        dic.TryGetValue(uid, out GetPrefab);
        Debug.Log("GetPrefabID + " + GetPrefab.GetInstanceID());

        slider = GetPrefab.transform.FindChild("Progress").GetComponent<UISlider>();

        // Change Progress..............
        slider.value += 0.1f;
        slider.ForceUpdate();
        slider.foregroundWidget.Update();
        panel.Refresh();

        // 모든 플레이어의 진행률이 100%인지 검사
        foreach (KeyValuePair<string, GameObject> items in dic)
        {
            if (items.Value.transform.FindChild("Progress").GetComponent<UISlider>().value == 1) // 검사 진행률 100%인지?
                i++;
        }
        // 모든 플레이어가 검사 완료했다면
        if (dicCount == i)
        {
            NGUIDebug.Log("Progress : 검사가 종료되었습니다.");
            //SendData();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void SendData()
    {
        PlayerManager playerManager = PlayerManager.Instance;
        SqliteHelper sqliteHelper = SqliteHelper.Instance;
        int i = 0;

        Debug.Log("검사 종료");
        for (i = 0; i < playerManager.playerList.Count; i++)
        {
            // Sqlite 에 insert
            sqliteHelper.Insert(playerManager.playerList[i].cm_num,
            playerManager.playerList[i].m_num,
            playerManager.playerList[i].oi_num,
            playerManager.playerList[i].AnswerList);

            // 서버로 전송
            Debug.Log(playerManager.playerList[i].cm_num);
            Debug.Log("??");

            requestManager.SendRequest(
                playerManager.playerList[i].cm_num,
                playerManager.playerList[i].m_num,
                playerManager.playerList[i].oi_num,
                playerManager.playerList[i].AnswerList[0],
                playerManager.playerList[i].AnswerList[1],
                playerManager.playerList[i].AnswerList[2],
                playerManager.playerList[i].AnswerList[3]);
        }
    }
}

  