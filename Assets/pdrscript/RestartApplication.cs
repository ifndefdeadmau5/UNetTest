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

    public void Restart()
    {
        // host cannot exceed 16 에러가 발생하면
        // 수동으로 재시작. NetworkManager 파괴 후
        // 씬을 리로드한다.
        GameObject.Destroy(MyNetManager);
        SceneManager.UnloadScene("menu");
        SceneManager.LoadScene("menu");
    }
}
