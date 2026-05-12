using AIWorldAdminRPG.Core;
using AIWorldAdminRPG.UI;
using UnityEngine;

namespace AIWorldAdminRPG.NPC
{
    public class NPCInteraction : MonoBehaviour
    {
        public NPCController npcController;

        public void Interact()
        {
            DialogueUI.Instance.Open(npcController, npcController.GetDialogue());
            ActionLogManager.Instance.AddLog(PlayerActionType.Talk, npcController.data.npcName, $"{npcController.data.npcName}와 대화함");
        }
    }
}
