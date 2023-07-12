using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AttentionDrivenScenography
{
    public class TriggerEvents : MonoBehaviour
    {
        public UnityEvent triggerEnter;
        public UnityEvent triggerStay;
        public UnityEvent triggerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player") triggerEnter.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player") triggerStay.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player") triggerExit.Invoke();
        }
    }
}
