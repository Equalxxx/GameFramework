using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageType { KR, ENG }

//[ExecuteInEditMode]
public class StringManager : Singleton<StringManager> {

    private ResourceManager.LinkType resLinkType = ResourceManager.LinkType.AssetBundle;
    public StringTable stringTable;

    public LanguageType langType;
    private LanguageType oldType;

    public bool showStringIndex;
    private bool oldShowStringIndex;

    private string dataPath = "datatable";
    public static Action OnChangeLanguage;
    public static Action OnShowStringindex;


    private void Awake()
    {
        LoadStringTable();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(showStringIndex != oldShowStringIndex)
        {
            oldShowStringIndex = showStringIndex;
            if (OnShowStringindex != null)
                OnShowStringindex();
        }
#endif

        if (langType != oldType)
        {
            stringTable = null;
            Resources.UnloadUnusedAssets();
        }

        if (stringTable == null)
        {
            oldType = langType;
            LoadStringTable();
        }
    }

    public static string GetStr(int strIndex)
    {
        return Instance.stringTable.GetStringData(strIndex);
    }

    void LoadStringTable()
    {
        stringTable = ResourceManager.LoadAsset(dataPath, "StringTable_" + langType.ToString(), resLinkType) as StringTable;

        Debug.Log("Load Complete StringTable : " + langType);

        if (OnChangeLanguage != null)
            OnChangeLanguage();
    }
}
