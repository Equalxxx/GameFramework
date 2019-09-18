using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : Singleton_Prefab<PopupUIManager>
{
    private ResourceManager.LinkType resLinkType = ResourceManager.LinkType.AssetBundle;

    public ProtoPopupUI curPopupUI;
    public GameObject popupModal;
    public Transform popupParent;

    public List<ProtoPopupUI> popupUIList;

    //PopupUI Data
    private string tablePath = "datatable";
    private PopupUITable popupUITable;

    private void Awake()
    {
        popupUITable = ResourceManager.LoadAsset(tablePath, "PopupUITable", resLinkType) as PopupUITable;

        ShowModal(false);
    }

    private void OnDestroy()
    {
        UnloadResourcesAll();
    }

    public void ShowPopupUI(string popupName)
    {
        if (curPopupUI != null)
            return;

        ProtoPopupUI popupUI = GetPopupUI(popupName);
        if (popupUI == null)
        {
            return;
        }

        curPopupUI = popupUI;

        popupUI.InitPopupUI();
        popupUI.ShowPopupUI(true);
        ShowModal(true);
    }

    public void ClosePopupUI()
    {
        if (curPopupUI == null)
            return;

        curPopupUI.ShowPopupUI(false);
        curPopupUI = null;
        ShowModal(false);
    }

    ProtoPopupUI GetPopupUI(string _tag)
    {
        ProtoPopupUI popupUI = popupUIList.Find(x => string.Equals(x.name, _tag));
        if (popupUI == null)
        {
            //Resources 경로 설정
            PopupUITable.PopupUIInfo popupUIInfo = popupUITable.GetPopupUIInfo(_tag);
            if (popupUIInfo == null)
            {
                Debug.LogError("Not found PopupUIInfo : " + _tag);
                return null;
            }

            //string resourcePath = string.Format("{0}{1}", popupUIInfo.path, popupUIInfo.tag);

            GameObject newObj = ResourceManager.LoadAsset(popupUIInfo.path, popupUIInfo.tag, resLinkType) as GameObject;
            if(newObj == null)
            {
                Debug.LogError("Not found popup ui : " + _tag);
                return null;
            }

            newObj = Instantiate(newObj);

            newObj.name = _tag;
            newObj.transform.SetParent(popupParent);
            newObj.transform.localPosition = Vector3.zero;
            newObj.transform.localRotation = Quaternion.identity;

            popupUI = newObj.GetComponent<ProtoPopupUI>();
            popupUIList.Add(popupUI);
        }

        return popupUI;
    }

    public void ShowModal(bool _show)
    {
        popupModal.SetActive(_show);
    }

    ///<summary>
    ///Unload resource all
    ///</summary>
    public void UnloadResourcesAll()
    {
        popupUIList.Clear();
        Resources.UnloadUnusedAssets();
    }
}
