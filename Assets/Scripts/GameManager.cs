using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 

    public Party selectedParty;
    public double expenses;
    public double income;
    public double dept;
    public string gamePhase; 

    

    private void Start()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
