using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;


public class LoginCheck : MonoBehaviour
{
    public string ID;
    public string PW;
    public HttpRequestManager requestManager;
    GameObject tweenLabel;
    WWWHelper helper;

    void Start()
    {
        SqliteHelper sqliteHelper = SqliteHelper.Instance;
        sqliteHelper.Init();
    }
    public void setID(GameObject _inputIdField)
    {
        ID = _inputIdField.GetComponent<UIInput>().value;
    }
    public void setPW(GameObject _inputPWField)
    {
        PW = _inputPWField.GetComponent<UIInput>().value;
    }

    public void Submit()
    {
        requestManager.LoginRequest( ID, PW );
    }

    
}
