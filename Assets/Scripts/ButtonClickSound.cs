using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    private AudioSource clickSound;
    private readonly HashSet<Button> configuredButtons = new HashSet<Button>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Create()
    {
        GameObject manager = new GameObject(nameof(ButtonClickSound));
        DontDestroyOnLoad(manager);
        manager.AddComponent<ButtonClickSound>();
    }

    private IEnumerator Start()
    {
        while (true)
        {
            if (clickSound == null)
            {
                GameObject sourceObject = GameObject.Find("ClickSound");
                if (sourceObject != null)
                    clickSound = sourceObject.GetComponent<AudioSource>();
            }

            if (clickSound != null)
                AddSoundToButtons();

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void AddSoundToButtons()
    {
        foreach (Button button in FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (configuredButtons.Add(button) && !HasClickSound(button))
                button.onClick.AddListener(clickSound.Play);
        }
    }

    private static bool HasClickSound(Button button)
    {
        for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
        {
            if (button.onClick.GetPersistentTarget(i) is AudioSource &&
                button.onClick.GetPersistentMethodName(i) == nameof(AudioSource.Play))
                return true;
        }

        return false;
    }
}
