using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PieChart : MonoBehaviour
{
    public Image[] images;                 // vysece
    public TextMeshProUGUI[] legendTexts;  // text legendy (stejne poradi)

    public void SetValues(float[] values, string[] names)
    {
        if (images == null || images.Length == 0)
        {
            Debug.LogWarning("PieChart nema prirazene zadne vysece.");
            return;
        }

        if (values == null) values = new float[0];
        if (names == null) names = new string[0];

        int visibleCount = Mathf.Min(values.Length, images.Length);
        float overflowValue = 0f;

        if (values.Length > images.Length)
        {
            visibleCount = Mathf.Max(0, images.Length - 1);
            for (int i = visibleCount; i < values.Length; i++)
                overflowValue += values[i];
        }

        float total = 0f;
        for (int i = 0; i < visibleCount; i++)
            total += values[i];
        total += overflowValue;

        float currentRotation = 0f;

        for (int i = 0; i < images.Length; i++)
        {
            bool isOverflowSlice = values.Length > images.Length && i == images.Length - 1;
            bool hasValue = i < visibleCount || isOverflowSlice;
            float value = isOverflowSlice ? overflowValue : (i < visibleCount ? values[i] : 0f);
            float percent = total == 0 ? 0 : value / total;

            if (images[i] != null)
            {
                images[i].fillAmount = percent;
                images[i].transform.rotation = Quaternion.Euler(0, 0, -360f * currentRotation);
            }

            currentRotation += percent;

            if (legendTexts != null && i < legendTexts.Length && legendTexts[i] != null)
            {
                if (!hasValue)
                {
                    legendTexts[i].text = "";
                    continue;
                }

                string label = isOverflowSlice ? "Ostatni" : (i < names.Length ? names[i] : "");
                legendTexts[i].text = label + " - " + (percent * 100f).ToString("0.0") + " %";
            }
        }
    }
}