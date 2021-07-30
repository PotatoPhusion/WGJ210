using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FractureData))]
public class FractureDataEditor : Editor
{
	public void OnSceneGUI()
	{
		FractureData t = target as FractureData;
		Color color = new Color(1f, 0.8f, 0.4f, 1f);
		Handles.color = color;
		for (int i = 0; i < t.fractureMap.Count; i++)
		{
			
		}
		Handles.DrawWireDisc(t.transform.position, -t.transform.forward, 1.0f);
	}
}
