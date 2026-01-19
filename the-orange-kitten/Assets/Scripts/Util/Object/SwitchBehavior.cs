using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util.Object
{
    public class SwitchBehavior : MonoBehaviour
    {
        [SerializeField] private int selected;
        private List<GameObject> _children;

        private void Start()
        {
            _children = transform.Cast<Transform>().Select(t => t.gameObject).ToList();
            _children.ForEach(child => child.SetActive(false));
            _children[selected].SetActive(true);
        }

        public void Select(int child)
        {
            _children[selected].SetActive(false);
            selected = child;
            _children[selected].SetActive(true);
        }

        public int GetIndex() => selected;
        
        public GameObject GetSelected() => _children[selected];
    }
}