using UnityEngine;

public class PartySlotGenerator : MonoBehaviour
{
    public JsonLouder jsonLoader;      // sem dáš JsonLouder objekt
    public RectTransform slotHolder;   // to samé co v PartyCarousel
    public GameObject partySlotPrefab; // tvùj PartySlot prefab
    public PartyDetailUI detailPanel;
    private ScenaLouder scenaLouder;
    public void GenerateSlots()
    {

        var parties = jsonLoader.LoadedParties;

        for (int i = 0; i < parties.Length; i++)
        {
            GameObject slotGO = Instantiate(partySlotPrefab, slotHolder);
            var ui = slotGO.GetComponent<PartySlotUI>();
            if (ui == null)
            {
                Debug.LogError("PartySlot prefab nemá PartySlotUI komponentu!");
                continue;
            }

            ui.detailPanel = detailPanel;
            ui.scenaLouder = scenaLouder;  
            ui.Setup(parties[i]);
        }
    }
   
    void Start()
    {
        GenerateSlots();
        scenaLouder = FindFirstObjectByType<ScenaLouder>();
    }

    void Update()
    {
        
    }
}
