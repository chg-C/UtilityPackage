///스크립트 생성 일자 - 2025 - 03 - 12
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1
using UnityEditor;
using UnityEngine;

namespace CHG.EventDriven
{
	using Editor = UnityEditor.Editor;
	
	[CustomEditor(typeof(GlobalEventManager))]
	public class GlobalEventManagerEditor : Editor
	{
		GlobalEventManager targetObject;
		private string searchString;
		
		void OnEnable()
		{
		}
		void OnDisable()
		{
		}
		
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
		}
	}
}