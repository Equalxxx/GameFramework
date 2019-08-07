using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ProtoPopupUI : MonoBehaviour {
    
    public abstract void InitPopupUI();
    public abstract void RefreshUI();

    public virtual void ShowPopupUI(bool _show)
    {
        this.gameObject.SetActive(_show);
    }
}
