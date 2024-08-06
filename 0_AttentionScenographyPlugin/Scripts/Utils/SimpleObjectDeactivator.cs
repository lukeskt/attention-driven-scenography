using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectDeactivator : MonoBehaviour
{
    public GameObject[] objectsToDeactivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DeactivateList();
        }
    }

    private void DeactivateList()
    {
        foreach (var obj in objectsToDeactivate)
        {
            obj.SetActive(false);
        }
        Destroy(this);
    }
}
