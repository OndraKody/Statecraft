using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class EventManager : MonoBehaviour
{
    [System.Serializable]
    public class StatEffect
    {
        public StatType statType;
        public float value;
    }

    [System.Serializable]
    public class GroupEffect
    {
        public GroupType groupType;
        public float value;
    }

    [System.Serializable]
    public class EventOption
    {
        public LocalizedString text;
        public float incomeChange;
        public float expenseChange;

        // Dopady na statistiky a skupiny - stejne jako u projektu
        public List<StatEffect> statEffects = new List<StatEffect>();
        public List<GroupEffect> groupEffects = new List<GroupEffect>();
    }

    [System.Serializable]
    public class GameEvent
    {
        public LocalizedString title;
        public LocalizedString description;
        public EventOption optionA;
        public EventOption optionB;
    }

    public static EventManager Instance;

    public GameEvent[] events;
    public EventPanelUI eventPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void TriggerRandomEvent()
    {
        if (events == null || events.Length == 0) return;
        GameEvent randomEvent = events[Random.Range(0, events.Length)];
        eventPanel.Show(randomEvent);
    }
}