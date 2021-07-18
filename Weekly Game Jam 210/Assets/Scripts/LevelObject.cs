using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public int mySector;
    private SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        gameObject.layer = 6 + mySector;
    }

    private void Update()
    {
        if (LevelManager.Instance.revealedSectors[mySector])
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }
    }
}
