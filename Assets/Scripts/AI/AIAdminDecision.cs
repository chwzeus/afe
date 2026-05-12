using System.Collections.Generic;
using AIWorldAdminRPG.Events;
using AIWorldAdminRPG.Quest;

namespace AIWorldAdminRPG.AI
{
    public class AIAdminDecision
    {
        public string summary;
        public int changedTrust;
        public int changedTension;
        public int changedReputation;
        public List<EventData> newEvents = new List<EventData>();
        public List<QuestData> newQuests = new List<QuestData>();
        public Dictionary<string, string> npcDialogueUpdates = new Dictionary<string, string>();
    }
}
