using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public struct AttentionTrackingObjectData
    {
        public string name;
        public float cumulativeAttention;
    }

    public class AttentionDatastore : MonoBehaviour
    {
        public static AttentionDatastore attentionDatastore;

        [SerializeField] private Canvas dataDisplayCanvas;
        [SerializeField] private TMP_Text dataDisplayText;

        public List<AttentionTrackingObjectData> AttentionTrackingObjects = new List<AttentionTrackingObjectData>();

        // Start is called before the first frame update
        private void Awake()
        {
            // Singleton adapted from: https://videlais.com/2021/02/20/singleton-global-instance-pattern-in-unity/
            if (attentionDatastore && attentionDatastore != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                attentionDatastore = this;
            }
        }

        private void Update()
        {
            dataDisplayText.text = "";
            foreach (var obj in AttentionTrackingObjects)
            {
                dataDisplayText.text += $"{obj.name}: {Mathf.Round(obj.cumulativeAttention * 1000f) / 1000f} \n";
            }
            if (Input.GetKey(KeyCode.E)) {
                dataDisplayCanvas.gameObject.SetActive(true);
            }
            else
            {
                dataDisplayCanvas.gameObject.SetActive(false);
            }
        }
    }
}