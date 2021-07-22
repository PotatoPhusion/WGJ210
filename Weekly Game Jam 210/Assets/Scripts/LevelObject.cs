using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public int mySector;

    private SpriteRenderer[] childRenderers;
    private Collider2D[] colliders;

    private void Start()
    {
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();
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
            if (colliders != null)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = true;
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
            if (colliders != null)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = false;
                }
            }
        }
    }
}
