using System;
using UnityEngine;

namespace Util.Object
{
    public class SwitchBehavior : MonoBehaviour
    {
        [SerializeField] private bool on;
        private GameObject _child1;
        private GameObject _child2;

        private void Start()
        {
            if (transform.childCount != 2)
            {
                throw new ArgumentException("Must have exactly two children");
            }
            _child1 = transform.GetChild(0).gameObject;
            _child2 = transform.GetChild(1).gameObject;
            
            SetOn(on);
        }
        
        public void Toggle() => SetOn(!IsOn());

        public void SetOn(bool isOn)
        {
            on = isOn;
            if (on)
            {
                _child1.gameObject.SetActive(true);
                _child2.gameObject.SetActive(false);
            }
            else
            {
                _child1.gameObject.SetActive(false);
                _child2.gameObject.SetActive(true);
            }
        }

        public bool IsOn() => on;
    }
}