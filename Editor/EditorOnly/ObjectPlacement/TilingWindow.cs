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
		bool isCenterPivot = true;
		Vector3 spacing;
		Transform originPoint;

		
		
		private void OnGUI()
		{
			prefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", prefab, typeof(GameObject), true);
			originPoint = (Transform)EditorGUILayout.ObjectField("Origin Point Transform", originPoint, typeof(Transform), true);
			tileCount = EditorGUILayout.Vector3IntField("Rows & Cols", tileCount);
			scale = EditorGUILayout.Vector3Field("Scale Of Tile", scale);
			spacing = EditorGUILayout.Vector3Field("Spacing", spacing);
			isCenterPivot = EditorGUILayout.Toggle("Is Using Center Pivot?", isCenterPivot);

			if(GUILayout.Button("Tiling Prefab"))
			{
				Tiling();
			}
		}

        private void Tiling()
        {
			Vector3 center = Vector3.zero;
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
						newOne.transform.SetParent(originPoint);

						newOne.transform.localPosition = position;
						newOne.transform.localRotation = Quaternion.identity;
						newOne.transform.localScale = scale;
					}
				}
			}
        }

        #region Window Opening	
        [MenuItem("Tools/CHG/Placement/Tiling GameObjects")]
		public static void ShowWindow()
		{
			GetWindow<TilingWindow>("Tiling GameObjects");
		}
		#endregion
	}
}