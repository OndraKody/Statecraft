using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PieChart : MonoBehaviour
{
    public Image[] image;
    public float[] value;

    void Start()
    {
        SetValues(value);
    }

    void Update()
    {
        
    }
    public void SetValues(float[] valuesToSet)
    {
        float totalValues = 0;
        for (int i = 0; i < valuesToSet.Length; i++)
        {
            totalValues += FindPercentage(valuesToSet, i);
            image[i].fillAmount = totalValues;
            
        }
    }
    private float FindPercentage(float[] valueToSet, int index)
    {
        float totalAmnount = 0;
        for(int i = 0;i < valueToSet.Length;i++)
        {
            totalAmnount += valueToSet[i];
        }
        return valueToSet[index] / totalAmnount;
    }
}
