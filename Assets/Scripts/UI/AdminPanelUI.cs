using System.Linq;
using AIWorldAdminRPG.AI;
using AIWorldAdminRPG.Core;
using TMPro;
using UnityEngine;

namespace AIWorldAdminRPG.UI
{
    public class AdminPanelUI : MonoBehaviour
    {
        public TMP_Text worldStateText;
        public TMP_Text logsText;
        public TMP_Text eventsText;
        public TMP_Text questsText;
        public TMP_Text npcStateText;

        private void Start()
        {
            Refresh();
            WorldStateManager.Instance.OnWorldStateChanged += Refresh;
            ActionLogManager.Instance.OnLogsChanged += Refresh;
        }

        public void OnRunAIAdminClicked()
        {
            AIAdminManager.Instance.RunDecision();
            Refresh();
        }

        public void Refresh()
        {
            var ws = WorldStateManager.Instance.Current;
            worldStateText.text = $"Trust:{ws.trust}\nTension:{ws.tension}\nRep:{ws.reputation}\nTime:{ws.timeOfDay}";
            logsText.text = string.Join("\n", ActionLogManager.Instance.Logs.TakeLast(8).Select(l => $"- [{l.actionType}] {l.description}"));
            eventsText.text = string.Join("\n", ws.activeEvents.TakeLast(5).Select(e => $"- {e.title}"));
            questsText.text = string.Join("\n", ws.activeQuests.TakeLast(5).Select(q => $"- {q.title} ({q.status})"));
            npcStateText.text = string.Join("\n", AIAdminManager.Instance.npcControllers.Select(n => $"- {n.data.npcName}: mood={n.data.mood}, fav={n.data.favorability}"));
        }
    }
}
