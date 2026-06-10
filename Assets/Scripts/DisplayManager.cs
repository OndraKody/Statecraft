using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class DisplayManager : MonoBehaviour
{
    private const string FullscreenPreference = "Display.Fullscreen";
    private const float CanvasMatch = 0.5f;
    private const int DefaultWindowWidth = 1280;
    private const int DefaultWindowHeight = 720;

    private static DisplayManager instance;
    private int lastScreenWidth;
    private int lastScreenHeight;
    private bool lastFullscreen;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Create()
    {
        if (instance != null) return;

        GameObject managerObject = new GameObject(nameof(DisplayManager));
        instance = managerObject.AddComponent<DisplayManager>();
        DontDestroyOnLoad(managerObject);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        bool useFullscreen = PlayerPrefs.GetInt(FullscreenPreference, 1) == 1;
        ApplyDisplayMode(useFullscreen);
        RememberDisplayState();
    }

    private void OnDestroy()
    {
        if (instance != this) return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        instance = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
            ApplyDisplayMode(!Screen.fullScreen);

        if (Screen.width == lastScreenWidth &&
            Screen.height == lastScreenHeight &&
            Screen.fullScreen == lastFullscreen)
            return;

        ConfigureCanvasScalers();
        RememberDisplayState();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ConfigureCanvasScalers();
    }

    private void ApplyDisplayMode(bool useFullscreen)
    {
        if (useFullscreen)
        {
            Resolution nativeResolution = Screen.currentResolution;
            Screen.SetResolution(
                nativeResolution.width,
                nativeResolution.height,
                FullScreenMode.FullScreenWindow,
                nativeResolution.refreshRateRatio);
        }
        else
        {
            int width = Mathf.Min(DefaultWindowWidth, Mathf.RoundToInt(Screen.currentResolution.width * 0.9f));
            int height = Mathf.Min(DefaultWindowHeight, Mathf.RoundToInt(Screen.currentResolution.height * 0.9f));
            Screen.SetResolution(width, height, FullScreenMode.Windowed);
        }

        PlayerPrefs.SetInt(FullscreenPreference, useFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private static void ConfigureCanvasScalers()
    {
        CanvasScaler[] scalers = FindObjectsByType<CanvasScaler>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (CanvasScaler scaler in scalers)
        {
            Canvas canvas = scaler.GetComponent<Canvas>();
            if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
                continue;

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = CanvasMatch;
        }
    }

    private void RememberDisplayState()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        lastFullscreen = Screen.fullScreen;
    }
}
