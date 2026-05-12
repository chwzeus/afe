using AIWorldAdminRPG.Core;
using UnityEngine;

namespace AIWorldAdminRPG.Quest
{
    public class QuestManager : MonoBehaviour
    {
        public void AcceptQuest(QuestData quest)
        {
            quest.status = QuestStatus.InProgress;
            ActionLogManager.Instance.AddLog(PlayerActionType.AcceptQuest, quest.title, "퀘스트 수락");
        }

        public void CompleteQuest(QuestData quest)
        {
            quest.status = QuestStatus.Completed;
            ActionLogManager.Instance.AddLog(PlayerActionType.CompleteQuest, quest.title, "퀘스트 완료");
        }
    }
}
