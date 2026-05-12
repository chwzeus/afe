using AIWorldAdminRPG.Core;
using AIWorldAdminRPG.NPC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AIWorldAdminRPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        public static DialogueUI Instance { get; private set; }
        public GameObject panel;
        public TMP_Text npcNameText;
        public TMP_Text dialogueText;
        public Button kindButton;
        public Button ignoreButton;
        public Button helpButton;

        private NPCController currentNPC;

        private void Awake()
        {
            Instance = this;
            panel.SetActive(false);
        }

        public void Open(NPCController npc, string line)
        {
            currentNPC = npc;
            panel.SetActive(true);
            npcNameText.text = npc.data.npcName;
            dialogueText.text = line;
        }

        public void ChooseKind()
        {
            currentNPC.data.favorability += 3;
            ActionLogManager.Instance.AddLog(PlayerActionType.Help, currentNPC.data.npcName, "친절하게 말함");
            Close();
        }

        public void ChooseIgnore()
        {
            currentNPC.data.favorability -= 2;
            ActionLogManager.Instance.AddLog(PlayerActionType.Ignore, currentNPC.data.npcName, "무시함");
            Close();
        }

        public void ChooseHelp()
        {
            currentNPC.data.favorability += 5;
            ActionLogManager.Instance.AddLog(PlayerActionType.Help, currentNPC.data.npcName, "도움을 제안함");
            Close();
        }

        public void Close() => panel.SetActive(false);
    }
}
