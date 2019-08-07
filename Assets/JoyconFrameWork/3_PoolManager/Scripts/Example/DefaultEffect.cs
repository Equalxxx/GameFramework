using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEffect : MonoBehaviour, IPoolObject {
    
    public void OnSpawnObject()
    {
        Debug.Log("Spawn : " + this.gameObject.name);
    }
}
