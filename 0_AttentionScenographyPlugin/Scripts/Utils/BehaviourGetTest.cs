using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class BehaviourGetTest : MonoBehaviour
    {
        public Behaviour GetBehaviour;
        private PropertyInfo[] properties;
        // Start is called before the first frame update
        void Start()
        {
            properties = GetBehaviour.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                print(prop);
            }
        }
    }
}
