using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPopupUI2 : ProtoPopupUI
{
    public Button btn1;

    private void Awake()
    {
        btn1.onClick.AddListener(() => PressedBtn(0));
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
        PopupUIManager.Instance.ClosePopupUI();
    }
}
