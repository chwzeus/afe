using System.Collections.Generic;
using AIWorldAdminRPG.Core;
using UnityEngine;

namespace AIWorldAdminRPG.NPC
{
    public class NPCController : MonoBehaviour
    {
        public NPCData data;

        private void Start()
        {
            if (data.baseDialogueLines.Count == 0)
            {
                data.baseDialogueLines = new List<string> { "안녕하세요.", "오늘도 평화롭네요." };
            }
            WorldStateManager.Instance?.SetNPCMood(data);
        }

        public string GetDialogue()
        {
            if (data.dynamicDialogueLines.Count > 0) return data.dynamicDialogueLines[data.dynamicDialogueLines.Count - 1];
            return data.baseDialogueLines[Random.Range(0, data.baseDialogueLines.Count)];
        }
    }
}
