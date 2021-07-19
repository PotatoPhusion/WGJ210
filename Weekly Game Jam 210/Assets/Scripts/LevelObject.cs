using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public int mySector;

    private SpriteRenderer[] childRenderers;

    private void Start()
    {
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
        gameObject.layer = 6 + mySector;
    }

    private void Update()
    {
        if (LevelManager.Instance.revealedSectors[mySector])
        {
            if (childRenderers != null)
            {
                for (int i = 0; i < childRenderers.Length; i++)
                {
                    childRenderers[i].enabled = true;
                }
            }
        }
        else
        {
            if (childRenderers != null)
            {
                for (int i = 0; i < childRenderers.Length; i++)
                {
                    childRenderers[i].enabled = false;
                }
            }
        }
    }
}
