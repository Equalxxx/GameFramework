using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPopupUI : ProtoPopupUI
{
    public Button btn1;
    public Button btn2;

    private void Awake()
    {
        btn1.onClick.AddListener(() => PressedBtn(0));
        btn2.onClick.AddListener(() => PressedBtn(1));
    }

    public override void InitPopupUI()
    {
        Debug.Log("Init Popup UI : " + name);
    }

    public override void RefreshUI()
    {
        Debug.Log("Refresh Popup UI : " + name);
    }

    public void PressedBtn(int btnIndex)
    {
        if(btnIndex == 0)
        {
            Debug.Log("TestPopup!");
        }
        else
        {
            PopupUIManager.Instance.ClosePopupUI();
        }
    }
}
