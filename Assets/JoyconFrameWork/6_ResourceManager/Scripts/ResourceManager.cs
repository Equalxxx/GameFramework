using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceManager : Singleton<ResourceManager>
{
    public enum LinkType { AssetBundle, Resources }
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    private string editorDataPath = "D:/UnityProject/JoyconFramework/client/JoyconFramework/Assets/AssetBundles/";
    private string appDataPath = "/AssetBundles/";

    private bool isLoadBundle;

    private void Awake()
    {
        if (Instance != this)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);

        appDataPath = Application.dataPath + appDataPath;
    }

    public static IEnumerator LoadAssetAsync<T>(string _path, string _tag,
         Action<UnityEngine.Object> onComplete, LinkType _linkType = LinkType.AssetBundle)
    {
        switch (_linkType)
        {
            case LinkType.AssetBundle:
                yield return LoadBundleAsync(_path);

                AssetBundleRequest abRequest = Instance.abDic[_path].LoadAssetAsync<T>(_tag);
                yield return abRequest;

                if (onComplete != null)
                    onComplete(abRequest.asset);
                break;
            case LinkType.Resources:
                ResourceRequest resRequest = Resources.LoadAsync(Path.Combine(_path, _tag));
                yield return resRequest;

                if (onComplete != null)
                    onComplete(resRequest.asset);
                break;
        }
    }

    public static UnityEngine.Object LoadAsset(string _path, string _tag, LinkType _linkType = LinkType.AssetBundle)
    {
        UnityEngine.Object loadAsset = null;
        switch (_linkType)
        {
            case LinkType.AssetBundle:
                if (!Instance.abDic.ContainsKey(_path))
                {
                    string folderPath = "";
#if UNITY_EDITOR
                    folderPath = Instance.editorDataPath;
#else
                    folderPath = Instance.appDataPath;
#endif
                    AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(folderPath, _path));
                    if (bundle == null)
                        break;

                    Instance.abDic.Add(_path, bundle);
                }

                loadAsset = Instance.abDic[_path].LoadAsset(_tag);
                break;
            case LinkType.Resources:
                loadAsset = Resources.Load(Path.Combine(_path, _tag));
                break;
        }

        return loadAsset;
    }

    public static void Release(string _path)
    {
        if(Instance.abDic.ContainsKey(_path))
        {
            AssetBundle bundle = Instance.abDic[_path];

            Instance.abDic.Remove(_path);
            bundle.Unload(true);
        }

        Resources.UnloadUnusedAssets();
    }

    public static IEnumerator LoadBundleAsync(string _bundleName)
    {
        while(Instance.isLoadBundle)
        {
            yield return null;
        }

        if (!Instance.abDic.ContainsKey(_bundleName))
        {
            string folderPath = "";
#if UNITY_EDITOR
            folderPath = Instance.editorDataPath;
#else
            folderPath = Instance.appDataPath;
#endif

            Instance.isLoadBundle = true;
            var request = AssetBundle.LoadFromFileAsync(Path.Combine(folderPath, _bundleName));
            yield return request;

            Instance.abDic.Add(_bundleName, request.assetBundle);
            Instance.isLoadBundle = false;
        }
    }
}