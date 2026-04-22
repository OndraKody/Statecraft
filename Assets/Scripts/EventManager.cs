using UnityEngine;
using UnityEngine.Localization;

public class EventManager : MonoBehaviour
{
    [System.Serializable]
    public class GameEvent
    {
        public LocalizedString title;
        public LocalizedString description;

        public EventOption optionA;
        public EventOption optionB;
    }

    [System.Serializable]
    public class EventOption
    {
        // ZMÌNA: Tady jsme zmìnili obyèejný string na LocalizedString!
        public LocalizedString text;

        public float incomeChange;
        public float expenseChange;
    }

    public static EventManager Instance;

    public GameEvent[] events;
    public EventPanelUI eventPanel; // Ten EventPanelUI skript z mé pøedchozí zprávy

    private void Awake()
    {
        Instance = this;
    }

    public void TriggerRandomEvent()
    {
        if (events.Length == 0) return;

        GameEvent randomEvent = events[Random.Range(0, events.Length)];
        eventPanel.Show(randomEvent);
    }
}