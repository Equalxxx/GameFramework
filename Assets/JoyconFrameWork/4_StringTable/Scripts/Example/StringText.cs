using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StringText : MonoBehaviour {

    public int stringIndex;
    private int oldIndex;

    private Text strText;
    
    private void Awake()
    {
        strText = this.GetComponent<Text>();
        strText.text = StringManager.GetStr(stringIndex);
    }

    private void OnEnable()
    {
        StringManager.OnChangeLanguage += ShowString;
        StringManager.OnShowStringindex += ShowString;
    }

    private void OnDisable()
    {
        StringManager.OnChangeLanguage -= ShowString;
        StringManager.OnShowStringindex -= ShowString;
    }

    private void Update()
    {
        if(stringIndex != oldIndex)
        {
            oldIndex = stringIndex;

            ShowString();
        }
    }

    void ShowString()
    {
        if (StringManager.Instance.showStringIndex)
        {
            strText.horizontalOverflow = HorizontalWrapMode.Overflow;
            strText.text = string.Format("{0} ({1})", StringManager.GetStr(stringIndex), stringIndex);
        }
        else
        {
            strText.horizontalOverflow = HorizontalWrapMode.Wrap;
            strText.text = StringManager.GetStr(stringIndex);
        }
    }
}
