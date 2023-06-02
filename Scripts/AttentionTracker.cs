using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class AttentionTracker : MonoBehaviour
    {
        // Attention Values
        private float currentAttention = 0f;
        private float cumulativeAttention = 0f;
        public float cumulativeAttentionMaximum = 300f;
        public float CurrentAttention { get => currentAttention; }
        public float CumulativeAttention { get => cumulativeAttention; set => cumulativeAttention = value; }

        // Other Properties
        [Tooltip("Set the distance after which the object will not be considered on-screen.")]
        [Range(0.0f, 1000.0f)][SerializeField] private double distanceThreshold = 50.0f;
        [Tooltip("Specify a camera if issues with attention occur, e.g. if using multiple cams.")]
        public Camera cam;
        private List<Collider> childColliders;
        private List<Renderer> childRenderers;
        private Bounds meshBounds;
        private AttentionDatastore AttnDatastore;

        // Start is called before the first frame update
        void Start()
        {
            CamSetup();
            meshBounds = GetCombinedRendererBounds();
            childColliders = GetComponentsInChildren<Collider>().ToList();
            if (GetComponent<Collider>() != null) childColliders.Add(GetComponent<Collider>());

            try
            {
                AttnDatastore = FindObjectOfType<AttentionDatastore>();
            }
            catch (Exception)
            {
                Debug.Log("Attention Datastore not found. Data will be stored locally to this component only.");
            }
        }

        private void CamSetup()
        {
            if (!cam)
            {
                GameObject Viewer = GameObject.FindGameObjectWithTag("Player");
                if (Viewer)
                {
                    if (Viewer.GetComponent<Camera>()) cam = Viewer.GetComponent<Camera>();
                    else if (Viewer.GetComponentInChildren<Camera>()) cam = Viewer.GetComponentInChildren<Camera>();
                    else if (Viewer.GetComponentInParent<Camera>()) cam = Viewer.GetComponentInParent<Camera>();
                }
                else cam = Camera.main;
            }
        }

        private Bounds GetCombinedRendererBounds()
        {
            childRenderers = gameObject.GetComponentsInChildren<Renderer>().ToList();
            if (GetComponent<Renderer>() != null) childRenderers.Add(GetComponent<Renderer>());
            Bounds combinedBounds = childRenderers[0].bounds;
            foreach (Renderer r in childRenderers) combinedBounds.Encapsulate(r.bounds);
            return combinedBounds;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            meshBounds = GetCombinedRendererBounds();
            currentAttention = GetAttentionValue();
            if (cumulativeAttention < cumulativeAttentionMaximum) cumulativeAttention += currentAttention * Time.deltaTime; // multiplier needed here?
            else if (cumulativeAttention >= cumulativeAttentionMaximum) cumulativeAttention = cumulativeAttentionMaximum;
            if (AttnDatastore) WriteToDatastore();
        }

        private void OnDisable ()
        {
            if (AttnDatastore) WriteToDatastore();
        }

        private void WriteToDatastore()
        {
            // TODO: check if object already exists by name? If not exist then new struct, else update cumulative attn.
            // extract to method to use here and fixedupdate.
            AttentionTrackingObjectData objectData = new AttentionTrackingObjectData
            {
                name = name,
                cumulativeAttention = cumulativeAttention
            };
            if (!AttnDatastore.AttentionTrackingObjects.Any(attnStruct => attnStruct.name == name))
            {
                AttnDatastore.AttentionTrackingObjects.Add(objectData);
            }
            else
            {
                AttnDatastore.AttentionTrackingObjects.Remove(AttnDatastore.AttentionTrackingObjects.Find(attnStruct => attnStruct.name == name));
                AttnDatastore.AttentionTrackingObjects.Add(objectData);
            }
        }

        private float GetAttentionValue()
        {
            float currentAttentionValue = 0f;
            if (ObjectFrustrumCheck() && ObjectLineOfSightCheck() && ObjectDistanceCheck())
            {
                // TODO: check these magic numbers for mapping here - mostly seem okay but is there a better way?
                currentAttentionValue = MapValue(GetObjectScreenPosition(), 0, 0.5f, 1, 0);
                currentAttentionValue = Mathf.Clamp(currentAttentionValue, 0, 1);
            }
            else
            {
                currentAttentionValue = 0f;
            }
            return currentAttentionValue;
        }
        private float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        private float GetObjectScreenPosition()
        {
            // TODO: compare doing this inversely, i.e. viewporttoworldpoint? - might have better results?
            Vector2 gameObjectViewportPosition = cam.WorldToViewportPoint(meshBounds.center);
            Vector2 viewportCentre = new Vector2(0.5f, 0.5f);
            float dist = Vector2.Distance(gameObjectViewportPosition, viewportCentre);
            return dist;
        }

        private bool ObjectDistanceCheck()
        {
            if (Vector3.Distance(meshBounds.center, cam.transform.position) < distanceThreshold) return true;
            else return false;
        }

        private bool ObjectLineOfSightCheck()
        {
            if (Physics.Linecast(cam.transform.position, meshBounds.center, out RaycastHit hit, 1 << gameObject.layer, QueryTriggerInteraction.Ignore)
                && childColliders.Contains(hit.collider))
            {
                Debug.DrawLine(cam.transform.position, meshBounds.center, Color.green);
                return true;
            }
            else
            {
                Debug.DrawLine(cam.transform.position, meshBounds.center, Color.red);
                return false;
            }
        }

        private bool ObjectFrustrumCheck()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(planes, meshBounds)) return true;
            else return false;
        }

#if UNITY_EDITOR
        public Texture2D image;
        void OnDrawGizmos()
        {
            GUIStyle style = new GUIStyle();
            style.normal.background = image;
            style.normal.textColor = Color.white;
            style.padding = new RectOffset(10, 10, 5, 5);
            // Round the numbers?
            Handles.Label(position:transform.position, $"{name}\nCurrent: {Mathf.Round(currentAttention * 1000f) / 1000f}\nCumulative: {Mathf.Round(cumulativeAttention * 1000f) / 1000f}", style);
        }
#endif
    }
}