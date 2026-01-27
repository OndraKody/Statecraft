using UnityEngine;
using UnityEngine.UI;
using static PolyciPanelUI;

public class PolicyButtonUI : MonoBehaviour
{
    public PolicyItem policyData;        // data této politiky
    public PolyciPanelUI policyPanel;    // spoleèný panel se sliderem

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        
    }
    
    private void OnClick()
    {
        if (policyPanel != null && policyData != null)
        {
            policyPanel.Setup(policyData);
        }
        else
        {
            Debug.LogWarning("PolicyButtonUI: Chybí data nebo panel");
        }
    }
}
