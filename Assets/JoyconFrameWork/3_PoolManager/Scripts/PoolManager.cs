using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, List<GameObject>> poolDic;
    public List<Transform> parentList;

    //Pool Data
    [SerializeField]
    private string tablePath = "DataTable/PoolTable";
    private PoolTable poolTable;

    private void Awake()
    {
        poolDic = new Dictionary<string, List<GameObject>>();
        parentList = new List<Transform>();
        poolTable = Resources.Load(tablePath) as PoolTable;
    }

    private void OnDestroy()
    {
        UnloadResourcesAll();
    }

    ///<summary>
    ///Prepare Sound Asset
    ///</summary>
    public void PrepareAssets(string _tag, Transform _parentTrans = null)
    {
        if (poolDic.ContainsKey(_tag))
            return;

        PoolTable.PoolInfo poolInfo = poolTable.GetPoolInfo(_tag);
        if (poolInfo == null)
        {
            Debug.LogError("Prepare Faild. Not found pool info : " + _tag);
            return;
        }

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
        if (poolList == null)
        {
            return null;
        }

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
        if (poolList == null)
        {
            return null;
        }

        //Resources 경로 설정
        PoolTable.PoolInfo poolInfo = poolTable.GetPoolInfo(_tag);
        if(poolInfo == null)
        {
            Debug.LogError("Not found PoolInfo : " + _tag);
            return null;
        }
        string resourcePath = string.Format("{0}{1}", poolInfo.path, poolInfo.tag);

        //Resources 에서 오브젝트 로드
        GameObject newObj = Resources.Load(resourcePath) as GameObject;
        if (newObj == null)
        {
            return null;
        }

        newObj = Instantiate(newObj);

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

    ///<summary>
    ///Unload resource with tag
    ///</summary>
    public void UnloadResource(string _tag)
    {
        if (poolDic.ContainsKey(_tag))
            poolDic.Remove(_tag);

        Resources.UnloadUnusedAssets();
    }

    ///<summary>
    ///Unload resource all
    ///</summary>
    public void UnloadResourcesAll()
    {
        foreach (KeyValuePair<string, List<GameObject>> keyPair in poolDic)
        {
            foreach (GameObject poolObj in keyPair.Value)
            {
                Destroy(poolObj);
            }
        }

        poolDic.Clear();
        Resources.UnloadUnusedAssets();
    }
}