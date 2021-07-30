using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelFragment
{
    public List<int> indices;
    public int fragment;
}

public class FractureData : MonoBehaviour
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<LevelFragment> levelFragments = new List<LevelFragment>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
