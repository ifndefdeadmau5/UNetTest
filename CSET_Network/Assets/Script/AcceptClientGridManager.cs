using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class AcceptClientGridManager : MonoBehaviour
{
    public GameObject ItemPrefab;
    UIGrid grid;
    UIPanel panel;
    IDictionary<int, GameObject> UserDic = new Dictionary<int, GameObject> { };
    GameObject GetPrefab;

    void Awake()
    {
        grid = GetComponent<UIGrid>();
        panel = GetComponent<UIPanel>();
        grid.gameObject.SetActive(true);
        
    }
   
   
    public void resetGrid()
    {
        foreach (Transform child in grid.GetChildList())
        {
            child.DestroyChildren();
        }
    }
   
    // playerGrid 에 아이템 추가
    // UserDic 에 connId, 리스트 아이템 프리팹을 한 세트로 추가한다.
	public void addItem(int connId, string uid)
    {
		// uid will store here
		//ItemPrefab.
		ItemPrefab.transform.FindChild("uidLabel").GetComponent<UILabel>().text = uid; 
		ItemPrefab.transform.FindChild("connIdLabel").GetComponent<UILabel>().text = connId.ToString();

        ItemPrefab.transform.position.Set(ItemPrefab.transform.position.x + 310f, ItemPrefab.transform.position.y + 176f, 0.0f);

        UserDic.Add(connId, NGUITools.AddChild(gameObject, ItemPrefab));
  
        panel.Refresh();
        grid.gameObject.SetActive(true);
        grid.Reposition();

    }

    public void deleteItem(int connId)
    {
        NGUIDebug.Log("delete Target connID : " + connId);
        NGUIDebug.Log("grid count: " + grid.GetChildList().Count);

        string temp="in list...";
        foreach (Transform child in grid.GetChildList())
        {
            temp += child.gameObject.transform.Find("connIdLabel").GetComponent<UILabel>().text + ",";
          
        }

        NGUIDebug.Log(temp);

        foreach (Transform child in grid.GetChildList())
        {
          
            if (child.gameObject.transform.Find("connIdLabel").GetComponent<UILabel>().text == connId.ToString())
            // 삭제하려는 커넥션 아이디와 일치하면
            {
                if (grid.RemoveChild(child))
                {
                    Debug.Log("찾았고 지웠다");
                    NGUITools.Destroy(child.gameObject);
                }
                //grid.transform.DetachChildren();
                UserDic.Remove(connId);
            }
        }
       
        panel.Refresh();
        grid.gameObject.SetActive(true);
        grid.Reposition();
    }


    // OrderListGrid 에 아이템 추가
    public void addOrderItem(string date, string place, int child, int op_num )
    {
        ItemPrefab.transform.FindChild("Date").GetComponent<UILabel>().text = date;
        ItemPrefab.transform.FindChild("Place").GetComponent<UILabel>().text = place;
        ItemPrefab.transform.FindChild("Childs").GetComponent<UILabel>().text = child.ToString();
        ItemPrefab.transform.FindChild("OpNumber").GetComponent<UILabel>().text = op_num.ToString();
        ItemPrefab.transform.FindChild("StartOrder").GetComponent<SetTweenTarget>().op_num = op_num;

        GameObject InstantiatedObject = (NGUITools.AddChild(gameObject, ItemPrefab));
        InstantiatedObject.transform.position.Set(182.54f, 0.0f, 0.0f);

        grid.Reposition();
        panel.Refresh();
    }
}
