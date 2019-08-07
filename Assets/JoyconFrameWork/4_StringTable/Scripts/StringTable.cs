using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringTable : ScriptableObject {
    [System.Serializable]
    public class StringInfo
    {
        public int index;
        public string stringData;
    }

    public List<StringInfo> stringDataList = new List<StringInfo>();

    public string GetStringData(int _index)
    {
        StringInfo stringInfo = stringDataList.Find(x => x.index.Equals(_index));

        if(stringInfo == null)
        {
            Debug.LogError("Not found StringData : " + _index);
            return "";
        }

        return stringInfo.stringData;
    }
}
