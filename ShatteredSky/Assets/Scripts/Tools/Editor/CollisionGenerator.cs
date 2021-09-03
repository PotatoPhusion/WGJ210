using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class CollisionGenerator
{
    private static TilemapCollider2D tmCollider;

    private enum PointStatus
    {
        None,
        Available,
        Used
    }

    private struct Point
    {
        public Vector2 position;
        public PointStatus status;
        public Vector2Int fragmentEdges;
        public Vector2Int meshEdges;
        public Point(Vector2 position, Vector2Int meshEdges, Vector2Int fragmentEdges, PointStatus status)
        {
            this.position = position;
            this.meshEdges = meshEdges;
            this.fragmentEdges = fragmentEdges;
            this.status = status;
        }
    }

    [MenuItem("Tools/Generate Colliders")]
    public static void GenerateCollision()
    {
        tmCollider = GameObject.FindObjectOfType<TilemapCollider2D>();
        PathCreator[] creators = GameObject.FindObjectsOfType<PathCreator>();
        Path[] paths = new Path[creators.Length];
        for (int i = 0; i < creators.Length; i++)
        {
            for (int j = 0; j < creators.Length; j++)
            {
                if (creators[j].fragmentIndex == i)
                    paths[i] = creators[j].path;
            }
        }

        if (tmCollider == null)
        {
            Debug.LogError("Could not find a TilemapCollider2D in active scene");
            return;
        }

        Mesh colliderMesh = tmCollider.CreateMesh(false, false);

        // DEBUG
        GameObject newMesh = new GameObject("Tilemap collider mesh");
        MeshFilter meshFilter = newMesh.AddComponent<MeshFilter>();
        meshFilter.mesh = colliderMesh;
        MeshRenderer debugRenderer = newMesh.AddComponent<MeshRenderer>();
        // END DEBUG

        // Get the outer edges of the collision mesh
        List<EdgeHelpers.Edge> boundaryPath = EdgeHelpers.GetEdges(colliderMesh.triangles).FindBoundary().SortEdges();
        List<List<EdgeHelpers.Edge>> separatedBoundaryPath = boundaryPath.SeparateIslands();

        List<List<Point>> meshPoints = new List<List<Point>>();
        List<Point> intersectPoints = new List<Point>();
        List<Point> fragmentPoints = new List<Point>();
        for (int i = 0; i < paths.Length; i++)
        {
            // Test each vertex of the collider mesh to see if it falls within the fragment
            for (int j = 0; j < separatedBoundaryPath.Count; j++)
            {
                List<Point> island = new List<Point>();
                for (int m = 0; m < separatedBoundaryPath[j].Count; m++)
                {
                    if (IsPointInPolygon(colliderMesh.vertices[separatedBoundaryPath[j][m].v1], paths[i]))
                    {
                        island.Add(new Point(
                            colliderMesh.vertices[separatedBoundaryPath[j][m].v1],
                            new Vector2Int((m - 1 + separatedBoundaryPath[j].Count) % separatedBoundaryPath[j].Count, m),
                            new Vector2Int(-1, -1),
                            PointStatus.Available));
                    }
                }
                meshPoints.Add(island);
            }
            // Test each vertex of the fragment to see if it falls within the collider mesh
            for (int f = 0; f < paths[i].NumPoints; f++)
            {
                if (IsPointInPolygon(paths[i][f], boundaryPath, colliderMesh.vertices))
                {
                    fragmentPoints.Add(new Point(
                        paths[i][f], new Vector2Int(-1, -1),
                        new Vector2Int(paths[i].LoopIndex(f - 1), f),
                        PointStatus.Available));
                }
            }

            // Check each boundary edge of the collision mesh with each segment of the fragment for intersection
            Vector2 intersect;
            for (int m = 0; m < boundaryPath.Count; m++)
            {
                for (int f = 0; f < paths[i].NumSegments; f++)
                {
                    if (Intersect(out intersect,
                        colliderMesh.vertices[boundaryPath[m].v1],
                        colliderMesh.vertices[boundaryPath[m].v2],
                        paths[i].GetPointsInSegment(f)[0],
                        paths[i].GetPointsInSegment(f)[1]))
                    {
                        intersectPoints.Add(new Point(
                            intersect,
                            new Vector2Int(m, m),
                            new Vector2Int(f, f),
                            PointStatus.Available));
                    }
                }
            }

            GameObject colliderObject = new GameObject($"Fragment_{i}");
            PolygonCollider2D polyCollider = colliderObject.AddComponent<PolygonCollider2D>();
            polyCollider.pathCount = meshPoints.Count;

            // Begin assembling the collider in the correct order
            List<Point> colliderPoints = new List<Point>();

            for (int k = 0; k < fragmentPoints.Count; k++)
            {
                colliderPoints.Add(fragmentPoints[k]);
            }
            for (int k = 0; k < intersectPoints.Count; k++)
            {
                colliderPoints.Add(intersectPoints[k]);
            }
            for (int islandIndex = 0; islandIndex < meshPoints.Count; islandIndex++)
            {
                bool loopComplete = false;
                bool checkMesh = true;
                Point previousPoint = new Point { status = PointStatus.None };
                int meshIndex = 0, fragmentIndex = 0, intersectIndex = 0;
                while (!loopComplete)
                {
                    // Mesh point check
                    if (meshPoints[islandIndex].Count > 0 && checkMesh)
                    {
                        if (previousPoint.status == PointStatus.None)
                        {
                            Point point = meshPoints[islandIndex][meshIndex];
                            colliderPoints.Add(point);
                            point.status = PointStatus.Used;
                        }
                    }
                    else
                    {
                        checkMesh = false;
                    }

                    // Fragment point check
                    if (fragmentPoints.Count > 0 && !checkMesh)
                    {

                    }
                    else
                    {
                        checkMesh = true;
                    }

                    // Intersect point check
                    if (intersectPoints.Count > 0)
                    {
                        if (previousPoint.status == PointStatus.None)
                        {

                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Determine if two line segments intersect and where
    /// </summary>
    /// <param name="point">The point of intersection</param>
    /// <param name="p1">Line one, point one</param>
    /// <param name="p2">Line one, point two</param>
    /// <param name="q1">Line two, point one</param>
    /// <param name="q2">Line two, point two</param>
    /// <returns>True if the line segments intersect</returns>
    public static bool Intersect(out Vector2 point, Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
    {
        float denom = (q2.y - q1.y) * (p2.x - p1.x) - (q2.x - q1.x) * (p2.y - p1.y);
        if (denom == 0.0f)  // Lines are parallel
        {
            point = Vector2.zero;
            return false;
        }

        float ua = ((q2.x - q1.x) * (p1.y - q1.y) - (q2.y - q1.y) * (p1.x - q1.x)) / denom;
        float ub = ((p2.x - p1.x) * (p1.y - q1.y) - (p2.y - p1.y) * (p1.x - q1.x)) / denom;
        if (ua >= 0.0f && ua <= 1.0f && ub >= 0.0f && ub <= 1.0f)
        {
            point = new Vector2(p1.x + ua * (p2.x - p1.x), p1.y + ua * (p2.y - p1.y));
            return true;
        }

        point = Vector2.zero;
        return false;
    }

    public static bool IsPointInPolygon(Vector2 p, Path path)
    {
        Vector2 p1 = p + Vector2.right * 1000;
        bool inside = false;
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector2 q1 = path.GetPointsInSegment(i)[0];
            Vector2 q2 = path.GetPointsInSegment(i)[1];
            if (Intersect(out _, p, p1, q1, q2))
            {
                inside = !inside;
            }
        }
        return inside;
    }

    public static bool IsPointInPolygon(Vector2 p, List<EdgeHelpers.Edge> edges, Vector3[] vertices)
    {
        Vector2 p1 = p + Vector2.right * 1000;
        bool inside = false;
        for (int i = 0; i < edges.Count; i++)
        {
            Vector2 q1 = vertices[edges[i].v1];
            Vector2 q2 = vertices[edges[i].v2];
            if (Intersect(out _, p, p1, q1, q2))
            {
                inside = !inside;
            }
        }
        return inside;
    }
}
