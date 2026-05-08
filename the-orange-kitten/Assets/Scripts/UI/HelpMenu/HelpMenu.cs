using System;
using System.Collections.Generic;
using Models.HelpMenu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.HelpMenu
{
    public class HelpMenu : MonoBehaviour
    {
        [SerializeField] private List<HelpMenuEntry> helpEntries;
        [SerializeField] private HelpEntryElement helpEntryPrefab;
        [SerializeField] private GameObject helpEntryComponentList;

        private void Start()
        {
            foreach (var entry in helpEntries)
            {
                var entryElem = Instantiate(helpEntryPrefab, helpEntryComponentList.transform);
                entryElem.SetContent(entry);
            }
        }
    }
}
