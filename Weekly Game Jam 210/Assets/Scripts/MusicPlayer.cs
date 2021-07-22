using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    private AudioSource audioSource;
    private Button muteButton;
    private GameObject redX;

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        GameObject mutePanel = new GameObject();

        if (scene.name == "MainMenu")
        {
            Destroy(mutePanel);
            mutePanel = GameObject.Find("Mute Panel");
        }
        else
        {
            return;
        }

        muteButton = mutePanel.transform.GetChild(0).GetComponent<Button>();
        redX = mutePanel.transform.GetChild(1).gameObject;
        redX.SetActive(audioSource.mute);

        muteButton.onClick.AddListener(OnToggleMute);
    }

    private void OnToggleMute()
    {
        audioSource.mute = !audioSource.mute;
        redX.SetActive(audioSource.mute);
    }
}
