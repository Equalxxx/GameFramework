using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPopupControl : MonoBehaviour {

    public string openPopupName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
        {
            PopupUIManager.Instance.ShowPopupUI(openPopupName);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PopupUIManager.Instance.ClosePopupUI();
        }
	}
}
