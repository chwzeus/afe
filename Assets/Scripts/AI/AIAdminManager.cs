using System.Collections.Generic;
using AIWorldAdminRPG.Core;
using AIWorldAdminRPG.NPC;
using UnityEngine;

namespace AIWorldAdminRPG.AI
{
    public class AIAdminManager : MonoBehaviour
    {
        public static AIAdminManager Instance { get; private set; }
        public bool useMockService = true;
        public List<NPCController> npcControllers = new List<NPCController>();

        private IAIAdminService service;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            service = useMockService ? new MockAIAdminService() : new LLMAdminService();
        }

        public void RunDecision()
        {
            var npcs = new List<NPCData>();
            foreach (var controller in npcControllers) npcs.Add(controller.data);

            var decision = service.Evaluate(WorldStateManager.Instance.Current, ActionLogManager.Instance.Logs, npcs);
            WorldStateManager.Instance.ApplyDelta(decision.changedTrust, decision.changedTension, decision.changedReputation);

            foreach (var e in decision.newEvents) WorldStateManager.Instance.Current.activeEvents.Add(e);
            foreach (var q in decision.newQuests) WorldStateManager.Instance.Current.activeQuests.Add(q);

            foreach (var npc in npcControllers)
            {
                if (decision.npcDialogueUpdates.TryGetValue(npc.data.npcName, out var line))
                {
                    npc.data.dynamicDialogueLines.Add(line);
                }
                WorldStateManager.Instance.SetNPCMood(npc.data);
            }

            Debug.Log(decision.summary);
        }
    }
}
