using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PieChart : MonoBehaviour
{
    public Image[] images;                 // výseèe
    public TextMeshProUGUI[] legendTexts;  // text legendy (stejné poøadí)

    public void SetValues(float[] values, string[] names)
    {
        if (values.Length != images.Length)
        {
            Debug.LogError("Poèet hodnot neodpovídá poètu výseèí!");
            return;
        }

        float total = 0f;
        for (int i = 0; i < values.Length; i++)
            total += values[i];

        float currentRotation = 0f;

        for (int i = 0; i < values.Length; i++)
        {
            float percent = total == 0 ? 0 : values[i] / total;

            images[i].fillAmount = percent;
            images[i].transform.rotation =
                Quaternion.Euler(0, 0, -360f * currentRotation);

            currentRotation += percent;

            if (legendTexts != null && i < legendTexts.Length)
            {
                legendTexts[i].text =
                    names[i] + " - " + (percent * 100f).ToString("0.0") + " %";
            }
        }
    }
}