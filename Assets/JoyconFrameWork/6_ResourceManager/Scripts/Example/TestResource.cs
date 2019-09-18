using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResource : MonoBehaviour {

    public GameObject testObject;
    public string abName1;
    public string asName1;
    public string abName2;
    public string asName2;
    public Dictionary<string, AssetBundle> bundleDic = new Dictionary<string, AssetBundle>();
    public GameObject testObj;

    // Use this for initialization
    //void Start () {
    //    testObj = ResourceManager.LoadAsset<GameObject>(abName1, asName1);
    //    //for (int i = 0; i < 10; i++)
    //    //{
    //    //    GameObject newObj = ResourceManager.LoadAsset<GameObject>(abName1, asName1);
    //    //}

    //    for (int i = 0; i < 10; i++)
    //    {
    //        StartCoroutine(ResourceManager.Instance.LoadAsset<GameObject>(abName2, asName2, x => {
    //            Instantiate(x);
    //        }));
    //    }
    //}

    IEnumerator Start()
    {
        for(int i = 0; i < 5; i++)
        {
            yield return ResourceManager.LoadAssetAsync<GameObject>(abName1, asName1, x =>
            {
                Instantiate(x);
            });
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject newObj = ResourceManager.LoadAsset(abName2, asName2) as GameObject;
            yield return newObj;
            Instantiate(newObj);
        }
    }

    //// Update is called once per frame
    //void Update () {
    //	if(Input.GetKeyDown(KeyCode.Space))
    //       {
    //           StartCoroutine(LoadAsset<GameObject>(abName, asName, x =>
    //           {
    //               testObject = x as GameObject;
    //               Instantiate(testObject);
    //           }));
    //       }
    //}

    //   IEnumerator LoadAsset<T>(string assetBundleName, string assetName, Action<UnityEngine.Object> onComplete)
    //   {
    //       if(!bundleDic.ContainsKey(assetBundleName))
    //       {
    //           string path = Application.dataPath + "/AssetBundles/" + assetBundleName;
    //           var request = AssetBundle.LoadFromFileAsync(path);
    //           yield return request;
    //           bundleDic.Add(assetBundleName, request.assetBundle);
    //       }

    //       var loadAsset = bundleDic[assetBundleName].LoadAssetAsync<T>(assetName);
    //       yield return loadAsset;

    //       if (onComplete != null)
    //           onComplete(loadAsset.asset);
    //   }
}
