using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectActivator : MonoBehaviour
{
    public GameObject[] objectsToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ActivateList();
        }
    }

    private void ActivateList()
    {
        foreach (var obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
        Destroy(this);
    }
}
