using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTriggerEnableDisable : MonoBehaviour
{
    public GameObject[] objectsToTrigger;
    public bool enableOnlyOneShot;
    public bool enableDisableOneShot;

    private void OnTriggerEnter(Collider other)
    {
        foreach (var obj in objectsToTrigger)
        {
            obj.SetActive(true);
        }

        if (enableOnlyOneShot)
        {
            Destroy(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var obj in objectsToTrigger)
        {
            obj.SetActive(false);
        }

        if (enableDisableOneShot)
        {
            Destroy(this);
        }
    }
}
