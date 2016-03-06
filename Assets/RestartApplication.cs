using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class RestartApplication : MonoBehaviour {


    public GameObject MyNetManager;

    // Use this for initialization
    void Awake()
    {
        MyNetManager = GameObject.FindGameObjectWithTag("NetworkManager");
    }
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Restart()
    {
        GameObject.Destroy(MyNetManager);
        SceneManager.LoadScene("menu");
    }
}
