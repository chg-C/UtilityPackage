///스크립트 생성 일자 - 2025 - 03 - 19
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.0.9

using CHG.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace CHG.Utilities.Triggers
{
	using Editor = UnityEditor.Editor;

	[CustomEditor(typeof(TriggerTaskScheduler))]
	public class TriggerTaskSchdulerEditor : Editor
	{
		TriggerTaskScheduler targetObject;
		
		ReorderableListWrapper taskList;
		void OnEnable()
		{
			targetObject = (TriggerTaskScheduler)target;
			taskList = new ReorderableListWrapper(serializedObject.FindProperty("_tasks"), "Tasks");
		}
		void OnDisable()
		{
		}
		
		public override void OnInspectorGUI()
		{
            serializedObject.Update();
			targetObject.Mode = (TriggerTaskScheduler.ProcessingMode)EditorGUILayout.EnumPopup("Execution Mode", targetObject.Mode);
			
			taskList.DrawLayoutList();
		}
	}
}