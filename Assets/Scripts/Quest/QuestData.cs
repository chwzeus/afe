using System;

namespace AIWorldAdminRPG.Quest
{
    public enum QuestStatus { NotStarted, InProgress, Completed }

    [Serializable]
    public class QuestData
    {
        public string questId;
        public string title;
        public string description;
        public string giverNpcName;
        public QuestStatus status;
        public string rewardDescription;
        public bool generatedByAI;
    }
}
