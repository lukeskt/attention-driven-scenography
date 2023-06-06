using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectDestroyer : MonoBehaviour
{
    public GameObject[] objectsToDestroy;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            DestroyList();
        }
    }

    public void DestroyList()
    {
        foreach (var obj in objectsToDestroy)
        {
            Destroy(obj);
        }
        Destroy(this);
    }
}
