using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

public class PlayerManager : MonoBehaviour
{
	/** 이 클래스의 싱글톤 객체 */
	static PlayerManager current = null;

	/** 객체를 생성하기 위한 GameObject */
	static GameObject container = null;

	/** 전역 변수 **/
	public class playerData
	{
		public int ConnID;
		// 클라이언트 식별
		public string uid;
		public List<int> AnswerList;
		//= new List<int>();
		public string cm_name;
		// 아동명
		public string m_name;
		public int cm_num;
		public int oi_num;
		public int m_num;

		public playerData (int connId, string uid, List<int> list, string cm_name, string m_name, int cm_num, int m_num, int oi_num)
		{
			ConnID = connId;
			this.uid = uid;
			AnswerList = list;
			this.m_name = m_name;
			this.cm_name = cm_name;
			this.cm_num = cm_num;
			this.m_num = m_num;
			this.oi_num = oi_num;
		}
	}

	public class PopupData
    // 아동/부모명 팝업리스트에 들어갈 데이터
	{
		public string cm_name;
		public string m_name;
		public int cm_num;
		public int m_num;
		public int oi_num;

		public PopupData (string name, string m_name, int cm_num, int m_num, int oi_num)
		{
			this.cm_name = name;
			this.m_name = m_name;
			this.cm_num = cm_num;
			this.m_num = m_num;
			this.oi_num = oi_num;
		}
	}

	public List<PopupData> childList = new List<PopupData> ();
	public List<playerData> playerList = new List<playerData> ();


	/** 싱글톤 객체 만들기 */
	public static PlayerManager Instance {
		get {
			if (current == null) {
				container = new GameObject ();
				container.name = "PlayerManager";
				current = container.AddComponent (typeof(PlayerManager)) as PlayerManager;
               
			}
			return current;
		}
	}

	public void addPlayer (int connId, string uid)
	{
		playerList.Add (new playerData (connId, uid, new List<int> (), "cm_name", "m_name", 0, 0, 0));
		Debug.Log ("PlayerAdded");
	}

    // just for debugging. print all PlayerList
    public void print()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            NGUIDebug.Log(playerList[i].ConnID + " " + playerList[i].uid);
        }

    }

    // 연결 해제시 playerList 에서 connId 를 '9999(오프라인 상태)'로 변경합니다.
    public void SetPlayerOffline (int connId)
	{
		for (int i = 0; i < playerList.Count; i++) {
			// 선택한 항목에 해당하는 유저를 검색해서 아동명을 대응시킴.
			if (connId == playerList [i].ConnID) {
				Debug.Log ("SetPlayerOffline : 찾았어요" + connId +"를");
				playerList [i].ConnID = 9999;
			}
		}
	}

    // 연결 해제시 playerList 에서 connId 를 '9999(오프라인 상태)'로 변경합니다.
    public void SetPlayerConnId(int connId, string uid)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            // 선택한 항목에 해당하는 유저를 검색해서 아동명을 대응시킴.
            if (uid == playerList[i].uid)
            {
                Debug.Log("SetPlayerConnID : 찾았어요" + "(" + connId+ ")");
                playerList[i].ConnID = connId;
            }
        }
    }


    public bool IsExsistUID( string uid )
	{
		for ( int i = 0; i < playerList.Count; i++)
		{
			// 선택한 항목에 해당하는 유저를 검색해서 아동명을 대응시킴.
			if( uid == playerList[i].uid){
				Debug.Log("IsExsistUID : 찾았어요");
				return true;
			}
		}

		return false;
	}

	public void clearPlayerlist ()
	{
		//playerList.Clear();
	}

	public void addChild (string name, string m_name, int cm_num, int m_num, int oi_num)
	{
		childList.Add (new PopupData (name, m_name, cm_num, m_num, oi_num));
		Debug.Log (name + " " + m_num + " " + cm_num + " " + oi_num);
		//여기까지 값들어오네
	}

  
	public void clearChildlist ()
	{
		childList.Clear ();
	}
   
	// Device 고유값에 아동명을 매치시킵니다.
	public void setChildToDevice (string cm_name, string m_name, int connID, int cm_num, int m_num, int oi_num)
	{
		Debug.Log (m_name + " " + cm_name + " " + m_num + " " + cm_num + " " + oi_num);
		// 여기서 000이 와버리네

		for (int i = 0; i < playerList.Count; i++) {
			// 선택한 항목에 해당하는 유저를 검색해서 아동명을 대응시킴.
			if (connID == playerList [i].ConnID) {
				Debug.Log ("찾았어요");
				playerList [i].cm_name = cm_name;
				playerList [i].m_name = m_name;
				playerList [i].cm_num = cm_num;
				playerList [i].m_num = m_num;
				playerList [i].oi_num = oi_num;
			}
		}
	}

	public void setChildAnswer (int connID, int Answer)
	{
		Debug.Log ("SetChildAnswer connID : " + connID + "player : " + playerList [0].ConnID);
		for (int i = 0; i < playerList.Count; i++) {
			Debug.Log ("forloop");
            
			if (connID == playerList [i].ConnID) {
                
				Debug.Log (playerList [i].cm_num
				+ " " + playerList [i].m_num
				+ " " + playerList [i].oi_num
				+ " " + playerList [i].cm_name
				+ " " + playerList [i].m_name);
				playerList [i].AnswerList.Add (Answer);
				Debug.Log ("Added Answer : " + Answer);
				Debug.Log ("playerList[i].AnswerList.Count : " + playerList [i].AnswerList.Count);

				for (int k = 0; k < playerList [i].AnswerList.Count; k++) {
					Debug.Log (playerList [i].AnswerList [k]);
				}
			}
		}
	}

}
