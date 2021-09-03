using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EdgeHelpers
{
    public struct Edge
    {
        public int v1;
        public int v2;
        public int triangleIndex;
        public Edge(int aV1, int aV2, int aIndex)
        {
            v1 = aV1;
            v2 = aV2;
            triangleIndex = aIndex;
        }
    }

    public static List<Edge> GetEdges(int[] aIndices)
    {
        List<Edge> result = new List<Edge>();
        for (int i = 0; i < aIndices.Length; i += 3)
        {
            int v1 = aIndices[i];
            int v2 = aIndices[i + 1];
            int v3 = aIndices[i + 2];
            result.Add(new Edge(v1, v2, i));
            result.Add(new Edge(v2, v3, i));
            result.Add(new Edge(v3, v1, i));
        }
        return result;
    }

    public static List<Edge> FindBoundary(this List<Edge> aEdges)
    {
        List<Edge> result = new List<Edge>(aEdges);
        for (int i = result.Count - 1; i > 0; i--)
        {
            for (int n = i - 1; n >= 0; n--)
            {
                if (result[i].v1 == result[n].v2 && result[i].v2 == result[n].v1)
                {
                    // Shared edge so remove both
                    result.RemoveAt(i);
                    result.RemoveAt(n);
                    i--;
                    break;
                }
            }
        }
        return result;
    }

    public static List<List<Edge>> SeparateIslands(this List<Edge> aEdges)
    {
        List<List<Edge>> separatedEdges = new List<List<Edge>>();
        List<Edge> result = new List<Edge>();
        int firstVertex = -1;
        for (int i = 0; i < aEdges.Count; i++)
        {
            if (firstVertex == -1)
                firstVertex = aEdges[i].v1;
            result.Add(aEdges[i]);
            // If the second vertex of the current edge matches the first vertex of the next edge
            // add the current list to separeted edges and start a new list for the next island
            Debug.Log($"Vertex 1: {aEdges[i].v1} | Vertex 2: {aEdges[i].v2} | First Vertex: {firstVertex}");
            if (aEdges[i].v2 == firstVertex)
            {
                separatedEdges.Add(result);
                result.Clear();
                firstVertex = -1;
            }
        }
        return separatedEdges;
    }

    public static List<Edge> SortEdges(this List<Edge> aEdges)
    {
        List<Edge> result = new List<Edge>(aEdges);
        for (int i = 0; i < result.Count - 2; i++)
        {
            Edge E = result[i];
            for (int n = 0; n < result.Count; n++)
            {
                Edge a = result[n];
                if (E.v2 == a.v1)
                {
                    // the edges are already in order so just continue
                    if (n == i + 1)
                        break;
                    // if we found a match, swap them with the next one after "i"
                    result[n] = result[i + 1];
                    result[i + 1] = a;
                    break;
                }
            }
        }
        return result;
    }

    public static List<List<Edge>> SortEdges(this List<List<Edge>> separatedEdges)
    {
        List<List<Edge>> sortedEdges = new List<List<Edge>>();
        for (int j = 0; j < separatedEdges.Count; j++)
        {
            List<Edge> result = new List<Edge>(separatedEdges[j]);
            for (int i = 0; i < result.Count - 2; i++)
            {
                Edge E = result[i];
                for (int n = 0; n < result.Count; n++)
                {
                    Edge a = result[n];
                    if (E.v2 == a.v1)
                    {
                        // the edges are already in order so just continue
                        if (n == i + 1)
                            break;
                        // if we found a match, swap them with the next one after "i"
                        result[n] = result[i + 1];
                        result[i + 1] = a;
                        break;
                    }
                }
            }
            sortedEdges.Add(result);
        }
        return sortedEdges;
    }

    public static List<int> SortVertexIndices(this List<Edge> aEdges)
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < aEdges.Count; i++)
        {
            Edge E = aEdges[i];
            if (!indices.Contains(aEdges[i].v1))
                indices.Add(aEdges[i].v1);
            if (!indices.Contains(aEdges[i].v2))
                indices.Add(aEdges[i].v2);
        }
        return indices;
    }
}
