using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public int fragmentIndex;
    [HideInInspector]
    public Path path;

    public void CreatePath()
    {
        path = new Path(transform.position);
    }
}
