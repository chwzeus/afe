using System.Collections.Generic;
using AIWorldAdminRPG.Core;
using AIWorldAdminRPG.NPC;
using UnityEngine;

namespace AIWorldAdminRPG.AI
{
    // 실제 LLM API 연동용 뼈대. API 키는 절대 코드에 하드코딩하지 마세요.
    // 추천: 환경변수, 안전한 서버 프록시, Secret Manager 사용.
    public class LLMAdminService : IAIAdminService
    {
        public AIAdminDecision Evaluate(WorldState worldState, List<PlayerActionLog> logs, List<NPCData> npcs)
        {
            Debug.LogWarning("LLMAdminService is a scaffold only. Connect your API client later.");
            return new AIAdminDecision { summary = "LLM service scaffold: no live decision." };
        }
    }
}
