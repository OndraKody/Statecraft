using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // odpauzuje hru
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
