using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class CollisionGenerator
{
    private static TilemapCollider2D tmCollider;

    [MenuItem("Tools/Generate Colliders")]
    public static void GenerateCollision()
    {
        tmCollider = GameObject.FindObjectOfType<TilemapCollider2D>();

        if (tmCollider == null)
        {
            Debug.LogError("Could not find a TilemapCollider2D in active scene");
            return;
        }

        Mesh colliderMesh = tmCollider.CreateMesh(false, false);

        GameObject newMesh = new GameObject();
        MeshFilter meshFilter = newMesh.AddComponent<MeshFilter>();
        meshFilter.mesh = colliderMesh;
        MeshRenderer debugRenderer = newMesh.AddComponent<MeshRenderer>();
    }
}
