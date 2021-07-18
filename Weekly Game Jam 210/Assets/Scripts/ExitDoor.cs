using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : LevelObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (!Physics2D.GetIgnoreLayerCollision(collision.gameObject.layer, gameObject.layer))
            {
                LevelManager.Instance.LevelComplete();
            }
        }
    }
}
