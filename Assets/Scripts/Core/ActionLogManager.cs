using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIWorldAdminRPG.Core
{
    public enum PlayerActionType
    {
        Talk,
        Help,
        Ignore,
        VisitLocation,
        AcceptQuest,
        CompleteQuest,
        RiskyChoice
    }

    [Serializable]
    public class PlayerActionLog
    {
        public PlayerActionType actionType;
        public string targetName;
        public string description;
        public DateTime timestamp;

        public PlayerActionLog(PlayerActionType actionType, string targetName, string description)
        {
            this.actionType = actionType;
            this.targetName = targetName;
            this.description = description;
            timestamp = DateTime.UtcNow;
        }
    }

    public class ActionLogManager : MonoBehaviour
    {
        public static ActionLogManager Instance { get; private set; }
        public List<PlayerActionLog> Logs { get; } = new List<PlayerActionLog>();

        public event Action OnLogsChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void AddLog(PlayerActionType type, string targetName, string description)
        {
            Logs.Add(new PlayerActionLog(type, targetName, description));
            OnLogsChanged?.Invoke();
        }
    }
}
