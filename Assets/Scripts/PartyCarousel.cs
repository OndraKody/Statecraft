using UnityEngine;

public class PartyCarousel : MonoBehaviour
{
    public RectTransform slotHolder;
    public float slotWidth = 100f; // tvoje velikost
    public int visibleCount = 3;
    private int index = 0;

    public void MoveRight()
    {
        index++;
        UpdatePosition();
    }

    public void MoveLeft()
    {
        index--;
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        int maxIndex = Mathf.Max(0, slotHolder.childCount - visibleCount);
        index = Mathf.Clamp(index, 0, maxIndex);
        slotHolder.anchoredPosition = new Vector2(-index * (slotWidth), 0);

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
