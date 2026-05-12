using System;

namespace AIWorldAdminRPG.Events
{
    [Serializable]
    public class EventData
    {
        public string eventId;
        public string title;
        public string description;
        public string triggerReason;
        public int severity;
        public DateTime createdAt;
    }
}
