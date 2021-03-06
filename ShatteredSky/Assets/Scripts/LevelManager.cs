using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public Renderer blackScreenRenderer;
    private Material fractureMat;

    public int activeCollisionLayers;

    public bool[] revealedSectors = new bool[10];

    private int[] sectorMasks = new int[]
    {
        64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768
    };

    private const float REVEALED_FRAGMENT = 0.3f;
    private const float HIDDEN_FRAGMENT = -0.1f;

	public static LevelManager Instance { get; private set; }

	private void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
        }
    }

    void Start()
    {
        fractureMat = blackScreenRenderer.sharedMaterial;
        HideAllFragments();
        for (int i = 0; i < revealedSectors.Length; i++)
        {
            if (revealedSectors[i])
                RevealFragment(i);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ReloadLevel();

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("MainMenu");
    }

    public void ChangeFragmentLayers(int mask, bool overwrite)
    {
        int layers = 1;         // Always collide with default

        if (overwrite)
        {
            layers |= mask;
        }
        else
        {
            layers |= activeCollisionLayers | mask;
        }

        activeCollisionLayers = layers;
    }

    private void HideAllFragments()
    {
        for (int i = 0; i < revealedSectors.Length; i++)
        {
            fractureMat.SetFloat($"Range{i}", HIDDEN_FRAGMENT);
            int mask = 1;
            ChangeFragmentLayers(mask, true);
        }
    }

    public void RevealFragment(int fragmentSector)
    {
        fractureMat.SetFloat($"Range{fragmentSector}", REVEALED_FRAGMENT);
        revealedSectors[fragmentSector] = true;
        ChangeFragmentLayers(sectorMasks[fragmentSector], false);
    }

    public void LevelComplete()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScene = (sceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        if (nextScene == 0)
            nextScene = 1;

        SceneManager.LoadScene(nextScene);
    }

    public void LevelFailed()
    {
        ReloadLevel();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}