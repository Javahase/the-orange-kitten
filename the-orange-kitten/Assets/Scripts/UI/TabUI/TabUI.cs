using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util.Object;

namespace UI.TabUI
{
    public class TabUI : MonoBehaviour
    {
        
        private List<Tab> _tabs;
        private SwitchBehavior _contentArea;
        
        private void Start()
        {
            _contentArea = transform.Find("ContentArea").GetComponent<SwitchBehavior>();
            _tabs = _contentArea.transform.Cast<Tab>().ToList();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}