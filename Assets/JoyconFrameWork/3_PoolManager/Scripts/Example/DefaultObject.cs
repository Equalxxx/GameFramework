using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultObject : MonoBehaviour, IPoolObject {

    public bool autoDisable;
    public float disableDelay = 3f;

    public void OnSpawnObject()
    {
        Debug.Log("Spawn : " + this.gameObject.name);

        Vector3 rndPos = this.transform.position;
        rndPos.x += Random.Range(0f, 1f);
        rndPos.y += Random.Range(0f, 1f);
        this.transform.position = rndPos;

        if (autoDisable)
        {
            Invoke("DisableObject", disableDelay);
        }
    }

    void DisableObject()
    {
        this.gameObject.SetActive(false);
    }
}
