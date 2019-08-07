using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUIModal : MonoBehaviour {

    private Button modalBtn;
    public bool useClosePopup;

    private void Awake()
    {
        modalBtn = this.GetComponent<Button>();
        if(useClosePopup)
            modalBtn.onClick.AddListener(ClosePopupUI);
    }

    void ClosePopupUI()
    {
        PopupUIManager.Instance.ClosePopupUI();
    }
}
