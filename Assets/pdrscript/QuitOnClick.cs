using UnityEngine;
using System.Collections;

public class QuitOnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void QuitApplication( ){
        Application.Quit();
    }
}
