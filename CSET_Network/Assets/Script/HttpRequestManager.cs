using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class HttpRequestManager : MonoBehaviour {

    string login_url = "http://www.csetest.com/CSET/application/login.php";
    string child_url = "http://www.csetest.com/CSET/application/child_member.php";
    string order_url = "http://www.csetest.com/CSET/application/order_paper.php";
    string send_url =  "http://www.csetest.com/CSET/application/cset_result.php";

    //1.223.7.2:8600
//    string login_url = "http://1.223.7.2:8600/CSET/application/login.php";
//    string child_url = "http://1.223.7.2:8600/CSET/application/child_member.php";
//    string order_url = "http://1.223.7.2:8600/CSET/application/order_paper.php";
//    string send_url = "http://1.223.7.2:8600/CSET/application/cset_result.php";
    WWWHelper helper;
    public AcceptClientGridManager OrderListGrid;
    bool isFirst;
    static int pre_op_num = 0; // 이전 아동명 목록 요청번호
    UILabel DebugLabel;
    UILabel DebugLabel2;

    // Use this for initialization
    void Awake( ){
        DebugLabel = GameObject.FindGameObjectWithTag("DebugLabel").GetComponent<UILabel>();
        DebugLabel2 = GameObject.FindGameObjectWithTag("DebugLabel2").GetComponent<UILabel>();
    }
    public void LoginRequest(string ID, string PW)
    {
        IDictionary<string, string> dic = new Dictionary<string, string> { };
        dic.Add("e_id", ID);
        dic.Add("e_password", PW);

        helper.OnHttpLoginRequest += OnLoginHttpRequest;

        helper.postLogin((int)WWWHelper.RequestType.Login, login_url, dic);
    }
    void OnLoginHttpRequest(int id, WWW www)
    {
        Debug.Log(id);

        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
            DebugLabel2.text = www.error;
        }
        else {
            string destString = www.text;
            string searchText = "{";
            destString = destString.Substring(destString.IndexOf(searchText)); // 상단의 HTML 코드 제거, JSON 데이터부터 시작하도록

            DebugLabel2.text = destString;
            JsonData json = JsonMapper.ToObject(destString);
            JsonData items = json["result"];

            JsonData item = items[0];
            string result = item["e_num"].ToString();

            Debug.Log("추출한 값은" + result);

            helper.e_num = System.Convert.ToInt32(result);
        }
    }
    public void OrderListRequest()
    {
        if ( isFirst ) { 
            // 처음이 아니면 요청 X
            isFirst = false;
            IDictionary<string, int> dic = new Dictionary<string, int> { };

            dic.Add("e_num", helper.e_num);

            helper.OnHttpOrderRequest += OnOrderHttpRequest;
            helper.postOrder((int)WWWHelper.RequestType.Order, order_url, dic);
         }
    }
    void OnOrderHttpRequest(int id, WWW www)
    {
        Debug.Log(id);

        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
            DebugLabel.text = www.error;
        }
        else {
            string destString = www.text;
            string searchText = "{";

            destString = destString.Substring(destString.IndexOf(searchText)); // 상단의 HTML 코드 제거, JSON 데이터부터 시작하도록
            Debug.Log(destString);

            JsonData json = JsonMapper.ToObject(destString);
            JsonData items = json["result"];
            int count = items.Count;

            NGUIDebug.Log("주문서 요청");
           DebugLabel.text = destString;
            for (int i = 0; i < count; i++)
            {
                //m_name, op_test, op_reserv_num, op_reserv_date
                JsonData item = items[i];

                string m_name = item["m_name"].ToString();
                string op_reserv_date = item["op_reserv_date"].ToString();
                string op_address = item["op_address"].ToString();
                int op_reserv_num = System.Convert.ToInt32(item["op_reserv_num"].ToString());
                int op_num = System.Convert.ToInt32(item["op_num"].ToString());

                NGUIDebug.Log(m_name + " " + op_reserv_num + " " + op_reserv_date + " " + op_address + " " + op_num);
                OrderListGrid.addOrderItem(op_reserv_date, op_address, op_reserv_num, op_num);
          
    
                
                helper.op_num = op_num;
            }


        }



    }
    public void ChildListRequest( )
    {
        WWWHelper helper = WWWHelper.Instance;

        if ( (pre_op_num == helper.op_num))
        {
            Debug.Log("동일한 주문서 요청이므로 거부");
            return;
        }
        System.Collections.Generic.IDictionary<string, int> dic = new Dictionary<string, int> { };

        dic.Add("op_num", helper.op_num);
        helper.OnHttpChildRequest += OnHttpChildRequest;
        helper.postChild((int)WWWHelper.RequestType.Child, child_url, dic);

        pre_op_num = helper.op_num; // 요청한 주문서 번호 저장
    }
    void OnHttpChildRequest(int id, WWW www)
    {
        PlayerManager playerManager = PlayerManager.Instance;

        playerManager.clearChildlist();

        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
        }
        else {
            string destString = www.text;
            string searchText = "{";

            destString = destString.Substring(destString.IndexOf(searchText)); // 상단의 HTML 코드 제거, JSON 데이터부터 시작하도록
            Debug.Log(destString);

            JsonData json = JsonMapper.ToObject(destString);
            JsonData items = json["result"];

			int count = 0;

			if (null == items.Count) {
				Debug.Log ("empty childlist");
			} else {
				 count = items.Count;
			}
           

            for (int i = 0; i < count; i++)
            {
                JsonData item = items[i];

                string cm_name = item["cm_name"].ToString();
                string m_name = item["m_name"].ToString();
                int cm_num = System.Convert.ToInt32(item["cm_num"].ToString());
                int m_num = System.Convert.ToInt32(item["m_num"].ToString());
                int oi_num = System.Convert.ToInt32(item["oi_num"].ToString());

                Debug.Log(cm_name + " " + m_num + " " + cm_num + " " + oi_num);

                playerManager.addChild(cm_name, m_name, cm_num, m_num, oi_num); ////////////////////////////PopupList.items.Add(cm_name);
            }
        }

    }

    /*
    public void SendResultRequest()
    {
        WWWHelper helper = WWWHelper.Instance;

        IDictionary<string, int> dic = new Dictionary<string, int> { };

        dic.Add("op_num", helper.op_num);
        helper.OnHttpChildRequest += OnHttpChildRequest;
        helper.postChild((int)WWWHelper.RequestType.Child, child_url, dic);

        pre_op_num = helper.op_num; // 요청한 주문서 번호 저장
    }
    */
 

    public void SendRequest(int cm_num, int m_num, int oi_num, int cr_1, int cr_2, int cr_3, int cr_4)
    {
        IDictionary<string, int> dic = new Dictionary<string, int> { };
        dic.Add("cm_num", cm_num);
        dic.Add("m_num", m_num);
        dic.Add("oi_num", oi_num);
        dic.Add("ccr_1", cr_1);
        dic.Add("ccr_2", cr_1);
        dic.Add("ccr_3", cr_1);
        dic.Add("ccr_4", cr_4);

        helper.OnHttpSendRequest += OnHttpSendRequest;
        helper.postOrder((int)WWWHelper.RequestType.Send, send_url, dic);  
    }

    void OnHttpSendRequest(int id, WWW www)
    {

    }


    void Start()
    {
        isFirst = true;
        helper = WWWHelper.Instance;
        Debug.Log("send ok");
        IDictionary<string, int> dic = new Dictionary<string, int> { };
        dic.Add("cm_num", 1);
        dic.Add("m_num", 1);
        dic.Add("oi_num", 1);
        dic.Add("ccr_1", 1);
        dic.Add("ccr_2", 1);
        dic.Add("ccr_3", 1);
        dic.Add("ccr_4", 1);

        helper.OnHttpSendRequest += OnHttpSendRequest;
        helper.postOrder((int)WWWHelper.RequestType.Send, send_url, dic);
    }
}

