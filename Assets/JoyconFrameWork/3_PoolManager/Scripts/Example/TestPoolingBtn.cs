using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPoolingBtn : MonoBehaviour {
    
    public string objectName;

    public Transform targetTrans;
    public Transform parentTrans;

    private Button button;

    // Use this for initialization
    void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(PressedBtn);
        PoolManager.Instance.PrepareAssets(objectName, parentTrans);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(PressedBtn);
    }

    void PressedBtn()
    {
        if(parentTrans)
            PoolManager.Instance.Spawn(objectName, targetTrans.position, Quaternion.identity, parentTrans);
        else
            PoolManager.Instance.Spawn(objectName, targetTrans.position, Quaternion.identity);
    }
}
