using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public JsonLouder.Party selectedParty;
    public double expenses;
    public double income;
    public double dept;
    public string gamePhase; 

    

    private void Start()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
