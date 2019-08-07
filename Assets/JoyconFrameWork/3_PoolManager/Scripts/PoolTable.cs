using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTable : ScriptableObject {
    [System.Serializable]
    public class PoolInfo
    {
        public int index;
        public string path;
        public string tag;
        public int preloadCount;
    }

    public List<PoolInfo> poolInfoList = new List<PoolInfo>();

    public PoolInfo GetPoolInfo(string _tag)
    {
        PoolInfo poolInfo = poolInfoList.Find(x => x.tag.Equals(_tag));

        return poolInfo;
    }
}
