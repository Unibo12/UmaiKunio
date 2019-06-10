﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class Settings : SingletonMonoBehaviour<Settings>
	{
		public MoveSettings Move;
    	public ItemSettings Item;

		public void LoadObject()
		{
			Move = Resources.FindObjectsOfTypeAll<MoveSettings>().First()as MoveSettings;
            Item = Resources.FindObjectsOfTypeAll<ItemSettings>().First() as ItemSettings;
        }
	}

[CustomEditor(typeof(Settings))]
public class DefineEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("LoadObject"))
		{
			(target as Settings).LoadObject();
		}
	}

}