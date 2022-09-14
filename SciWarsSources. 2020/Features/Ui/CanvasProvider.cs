using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public class CanvasProvider : MonoBehaviour {
     
        public Panel[] Panels;

        void OnValidate()
        {
            Panels = GetComponentsInChildren<Panel>();
        }

        void Awake()
        {
            GetComponent<Canvas>().enabled = true;
        }
    }
}