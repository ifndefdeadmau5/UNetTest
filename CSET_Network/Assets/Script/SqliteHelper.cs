using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.Collections.Generic;

public class SqliteHelper : MonoBehaviour {

    /** 이 클래스의 싱글톤 객체 */
    static SqliteHelper current = null;

    /** 객체를 생성하기 위한 GameObject */
    static GameObject container = null;

    /** 싱글톤 객체 만들기 */
    public static SqliteHelper Instance
    {
        get
        {
            if (current == null)
            {
                container = new GameObject();
                container.name = "SqliteHelper";
                current = container.AddComponent(typeof(SqliteHelper)) as SqliteHelper;
            }
            return current;
        }

    }
    // Use this for initialization
    public void Init()
    {

        string conn = "URI=file:" + Application.dataPath + "/test_result.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT cm_num FROM result";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            int cm_num = reader.GetInt32(0);

            Debug.Log("cm_num= " + cm_num);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    // SQLite Insertion
    // Store testData in device local database
    public void Insert(int cm_num, int m_num, int oi_num, List<int> Answerlist)
    {

        string conn = "URI=file:" + Application.dataPath + "/test_result.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "INSERT INTO result( cm_num, m_num, oi_num, cr_1, cr_2, cr_3, cr_4) VALUES("
            + cm_num + "," + m_num + "," + oi_num + "," + Answerlist[0] + "," + +Answerlist[1] + "," + Answerlist[2] + "," + Answerlist[3]
            + ");";

        Debug.Log(sqlQuery);
        dbcmd.CommandText = sqlQuery;
        Debug.Log(dbcmd.ExecuteNonQuery());

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

}
