using System;
using System.Collections.Generic;
using System.Linq;
using AIWorldAdminRPG.Core;
using AIWorldAdminRPG.Events;
using AIWorldAdminRPG.NPC;
using AIWorldAdminRPG.Quest;

namespace AIWorldAdminRPG.AI
{
    public class MockAIAdminService : IAIAdminService
    {
        public AIAdminDecision Evaluate(WorldState worldState, List<PlayerActionLog> logs, List<NPCData> npcs)
        {
            var decision = new AIAdminDecision { summary = "Mock AI admin evaluated the current world." };

            foreach (var npc in npcs)
            {
                int talks = logs.Count(l => l.actionType == PlayerActionType.Talk && l.targetName == npc.npcName);
                if (talks >= 3)
                {
                    npc.favorability += 5;
                    npc.mood = "Friendly";
                    decision.changedTrust += 2;
                    decision.npcDialogueUpdates[npc.npcName] = $"{npc.npcName}: 요즘 당신과 이야기하는 게 즐거워요.";
                }
            }

            int guardIgnores = logs.Count(l => l.actionType == PlayerActionType.Ignore && l.targetName.Contains("경비"));
            if (guardIgnores > 0)
            {
                decision.changedTension += 8;
            }

            if (worldState.tension + decision.changedTension >= 60)
            {
                decision.newEvents.Add(new EventData
                {
                    eventId = Guid.NewGuid().ToString(),
                    title = "수상한 소문",
                    description = "마을에 정체불명의 방문자에 대한 소문이 퍼진다.",
                    triggerReason = "긴장도 상승",
                    severity = 2,
                    createdAt = DateTime.UtcNow
                });
            }

            if (worldState.trust + decision.changedTrust >= 65 && worldState.activeQuests.All(q => q.title != "주민의 부탁"))
            {
                decision.newQuests.Add(new QuestData
                {
                    questId = Guid.NewGuid().ToString(),
                    title = "주민의 부탁",
                    description = "마을 주민을 도와 분실물을 찾아주세요.",
                    giverNpcName = "마을 주민",
                    status = QuestStatus.NotStarted,
                    rewardDescription = "신뢰도 +10",
                    generatedByAI = true
                });
            }

            var repeatedLocation = logs.Where(l => l.actionType == PlayerActionType.VisitLocation)
                .GroupBy(l => l.targetName).FirstOrDefault(g => g.Count() >= 3);
            if (repeatedLocation != null)
            {
                decision.newEvents.Add(new EventData
                {
                    eventId = Guid.NewGuid().ToString(),
                    title = "새로운 방문자 등장",
                    description = $"{repeatedLocation.Key} 주변에 수상한 여행자가 나타났다.",
                    triggerReason = "반복 방문",
                    severity = 1,
                    createdAt = DateTime.UtcNow
                });
            }

            return decision;
        }
    }
}
