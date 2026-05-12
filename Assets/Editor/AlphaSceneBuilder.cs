using System.Collections.Generic;
using AIWorldAdminRPG.AI;
using AIWorldAdminRPG.Core;
using AIWorldAdminRPG.Events;
using AIWorldAdminRPG.NPC;
using AIWorldAdminRPG.Player;
using AIWorldAdminRPG.Quest;
using AIWorldAdminRPG.UI;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class AlphaSceneBuilder
{
    [MenuItem("Tools/AI World Admin RPG/Build Alpha Scene")]
    public static void BuildAlphaScene()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.name = "Ground";
        plane.transform.position = Vector3.zero;

        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(0f, 1f, -4f);
        player.AddComponent<Rigidbody>();
        player.AddComponent<PlayerController>();

        var npcControllers = new List<NPCController>
        {
            CreateNpc("Villager", "마을 주민", "주민", "친절함", new Vector3(-3f, 1f, 0f)),
            CreateNpc("ShopOwner", "상점 주인", "상인", "실리적", new Vector3(0f, 1f, 2f)),
            CreateNpc("Guard", "경비병", "경비", "신중함", new Vector3(3f, 1f, 0f))
        };

        var cam = Camera.main;
        if (cam == null)
        {
            var camObj = new GameObject("Main Camera");
            cam = camObj.AddComponent<Camera>();
            camObj.tag = "MainCamera";
        }
        cam.transform.position = new Vector3(0f, 14f, -10f);
        cam.transform.rotation = Quaternion.Euler(45f, 0f, 0f);

        var systems = new GameObject("GameSystems");
        systems.AddComponent<GameManager>();
        systems.AddComponent<WorldStateManager>();
        systems.AddComponent<ActionLogManager>();
        var aiManager = systems.AddComponent<AIAdminManager>();
        systems.AddComponent<QuestManager>();
        systems.AddComponent<EventManager>();
        aiManager.npcControllers = npcControllers;

        var canvas = CreateCanvas();
        var dialogueUI = BuildDialogueUI(canvas.transform);
        var adminUI = BuildAdminPanelUI(canvas.transform);
        canvas.AddComponent<WorldStateUI>();
        canvas.AddComponent<ActionLogUI>();

        EditorUtility.SetDirty(canvas);
        EditorUtility.SetDirty(systems);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        Debug.Log("Alpha scene build complete.");
    }

    private static NPCController CreateNpc(string objectName, string displayName, string role, string personality, Vector3 pos)
    {
        var npc = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        npc.name = objectName;
        npc.transform.position = pos;

        var controller = npc.AddComponent<NPCController>();
        controller.data = new NPCData
        {
            npcId = objectName.ToLower(),
            npcName = displayName,
            role = role,
            personality = personality,
            mood = "Neutral",
            favorability = 50,
            baseDialogueLines = new List<string> { $"나는 {displayName}이야.", "요즘 마을 분위기가 조금 변했어." }
        };

        var interaction = npc.AddComponent<NPCInteraction>();
        interaction.npcController = controller;
        return controller;
    }

    private static Canvas CreateCanvas()
    {
        var canvasObj = new GameObject("Canvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    private static DialogueUI BuildDialogueUI(Transform parent)
    {
        var panel = CreatePanel("DialoguePanel", parent, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 120f), new Vector2(700f, 200f));
        var ui = panel.gameObject.AddComponent<DialogueUI>();
        ui.panel = panel.gameObject;

        ui.npcNameText = CreateTMPText("NpcName", panel, new Vector2(20f, -20f), "NPC");
        ui.dialogueText = CreateTMPText("Dialogue", panel, new Vector2(20f, -70f), "대화 내용", 24);

        ui.kindButton = CreateButton("KindBtn", panel, new Vector2(-180f, 30f), "친절하게 말한다", ui.ChooseKind);
        ui.ignoreButton = CreateButton("IgnoreBtn", panel, new Vector2(0f, 30f), "무시한다", ui.ChooseIgnore);
        ui.helpButton = CreateButton("HelpBtn", panel, new Vector2(180f, 30f), "도움을 제안한다", ui.ChooseHelp);

        return ui;
    }

    private static AdminPanelUI BuildAdminPanelUI(Transform parent)
    {
        var panel = CreatePanel("AdminPanel", parent, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-230f, 0f), new Vector2(440f, 760f));
        var ui = panel.gameObject.AddComponent<AdminPanelUI>();

        ui.worldStateText = CreateTMPText("WorldStateText", panel, new Vector2(12f, -10f), "World", 20);
        ui.logsText = CreateTMPText("LogsText", panel, new Vector2(12f, -140f), "Logs", 18);
        ui.eventsText = CreateTMPText("EventsText", panel, new Vector2(12f, -320f), "Events", 18);
        ui.questsText = CreateTMPText("QuestsText", panel, new Vector2(12f, -470f), "Quests", 18);
        ui.npcStateText = CreateTMPText("NpcStateText", panel, new Vector2(12f, -620f), "NPC", 18);

        CreateButton("RunAdminBtn", panel, new Vector2(0f, 32f), "AI Admin 판단 실행", ui.OnRunAIAdminClicked);

        return ui;
    }

    private static RectTransform CreatePanel(string name, Transform parent, Vector2 min, Vector2 max, Vector2 pos, Vector2 size)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        go.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);
        return rt;
    }

    private static TMP_Text CreateTMPText(string name, Transform parent, Vector2 pos, string text, int size = 28)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(-20f, 120f);

        var tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.TopLeft;
        return tmp;
    }

    private static Button CreateButton(string name, Transform parent, Vector2 pos, string text, UnityEngine.Events.UnityAction action)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(160f, 40f);
        go.GetComponent<Image>().color = new Color(0.2f, 0.3f, 0.45f, 0.95f);

        var btn = go.GetComponent<Button>();
        btn.onClick.AddListener(action);

        var txt = CreateTMPText("Label", go.transform, new Vector2(0f, 0f), text, 20) as TextMeshProUGUI;
        var tr = txt.rectTransform;
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = Vector2.zero;
        tr.offsetMax = Vector2.zero;
        txt.alignment = TextAlignmentOptions.Center;

        return btn;
    }
}
