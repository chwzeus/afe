using System.Collections.Generic;
using AIWorldAdminRPG.Core;
using AIWorldAdminRPG.NPC;

namespace AIWorldAdminRPG.AI
{
    public interface IAIAdminService
    {
        AIAdminDecision Evaluate(WorldState worldState, List<PlayerActionLog> logs, List<NPCData> npcs);
    }
}
