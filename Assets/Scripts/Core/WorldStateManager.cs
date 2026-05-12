using System;
using System.Collections.Generic;
using AIWorldAdminRPG.Events;
using AIWorldAdminRPG.NPC;
using AIWorldAdminRPG.Quest;
using UnityEngine;

namespace AIWorldAdminRPG.Core
{
    [Serializable]
    public class WorldState
    {
        public int trust = 50;
        public int tension = 20;
        public int reputation = 50;
        public string timeOfDay = "Morning";
        public List<EventData> activeEvents = new List<EventData>();
        public List<QuestData> activeQuests = new List<QuestData>();
        public Dictionary<string, string> npcMoods = new Dictionary<string, string>();
    }

    public class WorldStateManager : MonoBehaviour
    {
        public static WorldStateManager Instance { get; private set; }
        public WorldState Current = new WorldState();
        public event Action OnWorldStateChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void ApplyDelta(int trust, int tension, int reputation)
        {
            Current.trust = Mathf.Clamp(Current.trust + trust, 0, 100);
            Current.tension = Mathf.Clamp(Current.tension + tension, 0, 100);
            Current.reputation = Mathf.Clamp(Current.reputation + reputation, 0, 100);
            OnWorldStateChanged?.Invoke();
        }

        public void SetNPCMood(NPCData npc)
        {
            Current.npcMoods[npc.npcName] = npc.mood;
            OnWorldStateChanged?.Invoke();
        }
    }
}
