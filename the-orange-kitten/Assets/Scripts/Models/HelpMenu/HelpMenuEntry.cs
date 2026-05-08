using System;
using UnityEngine;

namespace Models.HelpMenu
{
    [Serializable]
    public struct HelpMenuEntry
    {
        public string question;
        
        [TextArea]
        public string answer;

        public HelpMenuEntry(string question, string answer)
        {
            this.question = question;
            this.answer = answer;
        }
    }
}