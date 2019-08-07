using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageType { KR, ENG }

[ExecuteInEditMode]
public class StringManager : Singleton_Prefab<StringManager> {

    public StringTable stringTable;

    public LanguageType langType;
    private LanguageType oldType;

    public bool showStringIndex;
    private bool oldShowStringIndex;

    public string dataPath = "DataTable/StringTable_";
    public static Action OnChangeLanguage;
    public static Action OnShowStringindex;

    public static string GetStr(int strIndex)
    {
        return Instance.stringTable.GetStringData(strIndex);
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
            StartCoroutine(LoadStringTable());
        }
    }

    IEnumerator LoadStringTable()
    {
        stringTable = Resources.Load(dataPath + langType.ToString()) as StringTable;
        yield return stringTable;

        Debug.Log("Load Complete StringTable : " + langType);

        if (OnChangeLanguage != null)
            OnChangeLanguage();
    }
}
