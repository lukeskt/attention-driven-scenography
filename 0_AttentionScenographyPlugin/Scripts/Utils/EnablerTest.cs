using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablerTest : MonoBehaviour
{
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float nowTime = Time.time;
        if (nowTime - startTime >= 10f)
        {
            SetActiveRecursively(transform, true);
        }
    }

    private void SetActiveRecursively(Transform parent, bool active)
    {
        parent.gameObject.SetActive(active);
        foreach (Transform child in parent)
        {
            SetActiveRecursively(child, active);
        }
    }
}
