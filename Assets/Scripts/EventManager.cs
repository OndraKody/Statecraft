using UnityEngine;

public class EventManager : MonoBehaviour {

    [System.Serializable]
    public class GameEvent 
    {
        public string title;
        [TextArea]
        public string description;

        public EventOption optionA;
        public EventOption optionB;
    }

    [System.Serializable]
    public class EventOption
    {
        public string text;

        public float incomeChange;
        public float expenseChange;
       
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
        if (events.Length == 0) return;

        GameEvent randomEvent = events[Random.Range(0, events.Length)];
        eventPanel.Show(randomEvent);
    }
}
