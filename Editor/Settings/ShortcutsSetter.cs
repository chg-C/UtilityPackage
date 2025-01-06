using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System;
using System.Collections.Generic;

namespace CHG.Utilities.Setting
{
    public class ShortcutsSetter
    {
        
        private const string closeTabKey = "CloseTabShortcut";
        private static bool closeTab = false;
        
        private const string inspectorLockKey = "InspectorLockShortcut";
        private static bool inspectorLock = false;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            closeTab = EditorPrefs.GetBool(closeTabKey, false);
            Menu.SetChecked("Tools/CHG/Shortcuts/Enable Tab Close %&W", closeTab);

            inspectorLock = EditorPrefs.GetBool(inspectorLockKey, true);
            Menu.SetChecked("Tools/CHG/Shortcuts/Enable Lock Inspector %&L", inspectorLock);
        }

        [MenuItem("Tools/CHG/Shortcuts/Enable Tab Close %&W")]
        private static void ToggleTabCloseShortcut()
        {
            closeTab = !closeTab;
            EditorPrefs.SetBool(closeTabKey, closeTab);
            Menu.SetChecked("Tools/CHG/Shortcuts/Enable Tab Close %&W", closeTab);

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss") + " Tab-Close Shortcut " + (closeTab ? "ON" : "OFF"));
        }
        [MenuItem("Tools/CHG/Shortcuts/Enable Lock Inspector %&L")]
        private static void ToggleInspectorLockShortcut()
        {
            inspectorLock = !inspectorLock;
            EditorPrefs.SetBool(inspectorLockKey, inspectorLock);
            Menu.SetChecked("Tools/CHG/Shortcuts/Enable Lock Inspector %&L", inspectorLock);

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss") + " Inspector Lock Shortcut " + (inspectorLock ? "ON" : "OFF"));
        }


        [Shortcut("CHG/Window/Close", KeyCode.W, ShortcutModifiers.Control)]
        private static void CloseTab(ShortcutArguments args)
        {
            EditorWindow focused = EditorWindow.focusedWindow;
            if(focused == null || !closeTab)
                return;

            focused.Close();
        }
        [Shortcut("CHG/Inspector/Lock", KeyCode.L, ShortcutModifiers.Control)]
        private static void LockInspector(ShortcutArguments args)
        {  
            if(inspectorLock)
            {
                GameObject selected = Selection.activeGameObject;

                ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
                ActiveEditorTracker.sharedTracker.ForceRebuild();

                if(selected != null)
                {
                    Selection.activeGameObject = selected;
                }
            }
        }
    }
}