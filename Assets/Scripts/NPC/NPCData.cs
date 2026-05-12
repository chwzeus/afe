using System;
using System.Collections.Generic;

namespace AIWorldAdminRPG.NPC
{
    [Serializable]
    public class NPCData
    {
        public string npcId;
        public string npcName;
        public string role;
        public string personality;
        public string mood;
        public int favorability;
        public List<string> baseDialogueLines = new List<string>();
        public List<string> dynamicDialogueLines = new List<string>();
    }
}
