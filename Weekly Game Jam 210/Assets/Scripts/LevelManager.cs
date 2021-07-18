using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public Renderer levelGeoRenderer;
    private Material fractureMat;

    public int activeCollisionLayers;

    public bool[] revealedSectors = new bool[10];

    private int[] sectorMasks = new int[]
    {
        64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768
    };

    private const float REVEALED_FRAGMENT = 0.3f;
    private const float HIDDEN_FRAGMENT = -0.1f;

    private static LevelManager instance;
    public static LevelManager Instance { 
        get {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        fractureMat = levelGeoRenderer.sharedMaterial;
        HideAllFragments();
        for (int i = 0; i < revealedSectors.Length; i++)
        {
            if (revealedSectors[i])
                RevealFragment(i);
        }
    }

    void Update()
    {

    }

    public void ChangeFragmentLayers(int mask, bool overwrite)
    {
        int layers = 1;

        if (overwrite)
        {
            layers |= mask;
        }
        else
        {
            layers |= activeCollisionLayers | mask;
        }

        activeCollisionLayers = layers;
        Physics2D.SetLayerCollisionMask(3, activeCollisionLayers);
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
        SceneManager.LoadScene(sceneIndex + 1);
    }
}