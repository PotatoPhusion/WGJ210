using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : LevelObject
{
    public enum FragmentType { constructive, destructive };
    public FragmentType fragmentType;
    public int fragmentSector;

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            if (!Physics2D.GetIgnoreLayerCollision(other.gameObject.layer, gameObject.layer))
            {
                LevelManager.Instance.RevealFragment(fragmentSector);
                DestroySelf();
            }
        }
    }
}
