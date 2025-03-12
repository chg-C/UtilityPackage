///스크립트 생성 일자 - 2025 - 02 - 25
///스크립트 생성 버전 - 0.1

using UnityEngine;
using UnityEditor;
using System;

namespace CHG.Editor.Placement
{
	public class TilingWindow : EditorWindow
	{
		GameObject prefab;
		Vector3Int tileCount = Vector3Int.one;
		Vector3 scale = Vector3.one;
		Vector3 offset = Vector3.zero;
		bool isCenterPivot = true;
		bool isLocalPosition = true;
		Vector3 spacing;
		Transform originPoint;

		private void OnGUI()
		{
			prefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", prefab, typeof(GameObject), true);
			originPoint = (Transform)EditorGUILayout.ObjectField("Origin Point Transform", originPoint, typeof(Transform), true);
			if(originPoint != null && prefab != null)
			{
				if(originPoint == prefab.transform || originPoint.IsChildOf(prefab.transform))
				{
					Debug.LogWarning("Origin Point can't be Prefab's Transform");
					originPoint = null;
				}
			}
			
			tileCount = EditorGUILayout.Vector3IntField("Rows & Cols", tileCount);
			offset = EditorGUILayout.Vector3Field("Offset", offset);
			scale = EditorGUILayout.Vector3Field("Scale Of Tile", scale);
			spacing = EditorGUILayout.Vector3Field("Spacing", spacing);
			
			using(new EditorGUILayout.HorizontalScope())
			{
				isCenterPivot = EditorGUILayout.Toggle("Using Center Pivot", isCenterPivot);
				if(originPoint != null)
				{
					isLocalPosition = EditorGUILayout.Toggle("Using Local Position", isLocalPosition);
				}
			}
			bool was = GUI.enabled;
			GUI.enabled = prefab != null;
			if(GUILayout.Button("Tiling Prefab"))
			{
				Tiling();
			}
			GUI.enabled = was;
		}

        private void Tiling()
        {
			Undo.IncrementCurrentGroup();
			int group = Undo.GetCurrentGroup();

			Transform parent = originPoint;
			if(parent == null)
			{
				parent = new GameObject("TileHolder").transform;
				Undo.RegisterCreatedObjectUndo(parent.gameObject, "Tiling GameObject");
			}			

			Vector3 center = offset;
			if(isCenterPivot)
			{
				center.x -= (tileCount.x/2)*spacing.x;
				center.y -= (tileCount.y/2)*spacing.y;
				center.z -= (tileCount.z/2)*spacing.z;
			}

			Vector3 position = Vector3.zero;
			for(int i = 0; i < tileCount.x; ++i)
			{
				for(int j = 0; j < tileCount.y; ++j)
				{
					for(int k = 0; k < tileCount.z; ++k)
					{
						position = center;

						position.x += (i*spacing.x);
						position.y += (j*spacing.y);
						position.z += (k*spacing.z);

						GameObject newOne = Instantiate(prefab);
						
						newOne.transform.SetParent(parent);
						if(isLocalPosition)
							newOne.transform.localPosition = position;
						else
							newOne.transform.position = position;
						newOne.transform.localRotation = prefab.transform.localRotation;
						newOne.transform.localScale = scale;
						Undo.RegisterCreatedObjectUndo(newOne, "Tiling GameObject");
					}
				}
			}
			Undo.CollapseUndoOperations(group);
        }

        #region Window Opening	
        [MenuItem("Tools/CHG/GameObjects/Tiling GameObjects", priority = 51)]
		public static void ShowWindow()
		{
			GetWindow<TilingWindow>("Tiling GameObjects");
		}
		#endregion
	}
}