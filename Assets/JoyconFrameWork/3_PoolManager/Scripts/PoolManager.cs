using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class PoolManager : Singleton<PoolManager>
{
    public ResourceManager.LinkType resLinkType = ResourceManager.LinkType.AssetBundle;
    private Dictionary<string, List<GameObject>> poolDic = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
    private List<Transform> parentList = new List<Transform>();

    //Pool Data
    [SerializeField]
    private string tablePath = "datatable";
    private PoolTable poolTable;

    ///<summary>
    ///Prepare Sound Asset
    ///</summary>
    public void PrepareAssets(string _tag, Transform _parentTrans = null)
    {
        if(poolTable == null)
        {
            poolTable = ResourceManager.LoadAsset(tablePath, "PoolTable", resLinkType) as PoolTable;
            if (poolTable == null)
            {
                Debug.LogError("Not found PoolTable");
                return;
            }
        }

        if (poolDic.ContainsKey(_tag))
            return;

        PoolTable.PoolInfo poolInfo = poolTable.GetPoolInfo(_tag);
        if (poolInfo == null)
        {
            Debug.LogError("Prepare Faild. Not found pool info : " + _tag);
            return;
        }

        GameObject newPrefab = ResourceManager.LoadAsset(poolInfo.path, poolInfo.tag, resLinkType) as GameObject;
        prefabDic.Add(poolInfo.tag, newPrefab);

        for (int i = 0; i < poolInfo.preloadCount; i++)
        {
            GameObject newObj = CreatePoolObject(poolInfo.tag, _parentTrans);
            if (newObj == null)
            {
                Debug.LogError("Prepare Faild. Not found PoolObject : " + _tag);
                return;
            }
        }

        Debug.Log("Prepare Success PoolObject : " + _tag);
    }

    ///<summary>
    ///Spawn PoolObject
    ///</summary>
    public GameObject Spawn(string _tag, Vector3 _spawnPos, Quaternion _spawnRot, Transform _parentTrans = null)
    {
        GameObject spawnObj = GetPoolObject(_tag, _parentTrans);
        if (spawnObj == null)
        {
            Debug.LogError("Not found GameObject : " + _tag);
            return null;
        }

        spawnObj.SetActive(true);
        spawnObj.transform.position = _spawnPos;
        spawnObj.transform.rotation = _spawnRot;

        IPoolObject poolObj = spawnObj.GetComponent<IPoolObject>();
        if (poolObj == null)
        {
            Debug.LogError("Not found IPoolObject : " + _tag);
            return null;
        }

        poolObj.OnSpawnObject();

        return spawnObj;
    }

    GameObject GetPoolObject(string _tag, Transform _parentTrans = null)
    {
        List<GameObject> poolList = GetPoolList(_tag);

        GameObject spawnObj = poolList.Find(x => !x.activeSelf);
        if (spawnObj == null)
        {
            spawnObj = CreatePoolObject(_tag, _parentTrans);
        }

        return spawnObj;
    }

    GameObject CreatePoolObject(string _tag, Transform _parentTrans = null)
    {
        List<GameObject> poolList = GetPoolList(_tag);

        //Resources 경로 설정
        PoolTable.PoolInfo poolInfo = poolTable.GetPoolInfo(_tag);
        if (poolInfo == null)
        {
            Debug.LogError("Not found PoolInfo : " + _tag);
            return null;
        }
        
        if (!prefabDic.ContainsKey(_tag))
        {
            return null;
        }

        GameObject newObj = Instantiate(prefabDic[_tag]);

        newObj.SetActive(false);

        newObj.name = string.Format("{0} ({1})", _tag, poolList.Count);

        if (_parentTrans == null)
        {
            string parentName = string.Format("{0}Parent", _tag);

            Transform parentTrans = parentList.Find(x => x.name.Equals(parentName));
            if (parentTrans == null)
            {
                parentTrans = new GameObject().GetComponent<Transform>();
                parentTrans.parent = this.transform;
                parentTrans.name = parentName;
                parentList.Add(parentTrans);
            }

            newObj.transform.parent = parentTrans;
        }
        else
        {
            newObj.transform.parent = _parentTrans;
        }

        poolList.Add(newObj);

        return newObj;
    }

    List<GameObject> GetPoolList(string _tag)
    {
        if (!poolDic.ContainsKey(_tag))
        {
            //poolDic 에 추가
            List<GameObject> newPoolObjectList = new List<GameObject>();

            poolDic.Add(_tag, newPoolObjectList);
        }

        return poolDic[_tag];
    }
}