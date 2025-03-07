using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System;
using System.Collections.Generic;

namespace CHG.Editor.Settings
{
    public static class ShortcutsSetter
    {
        private const string ItemPath = "Tools/CHG/Shortcuts/";
        private const string closeTabKey = "CloseTabShortcut";
        private static bool closeTab = false;
        
        private const string inspectorLockKey = "InspectorLockShortcut";
        private static bool inspectorLock = false;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            closeTab = EditorPrefs.GetBool(closeTabKey, false);
            Menu.SetChecked(ItemPath + "Enable Tab Close %&W", closeTab);

            inspectorLock = EditorPrefs.GetBool(inspectorLockKey, true);
            Menu.SetChecked(ItemPath + "Enable Lock Inspector %&L", inspectorLock);
        }

        [MenuItem(ItemPath + "Enable Tab Close %&W", priority = 6)]
        private static void ToggleTabCloseShortcut()
        {
            closeTab = !closeTab;
            EditorPrefs.SetBool(closeTabKey, closeTab);
            Menu.SetChecked(ItemPath + "Enable Tab Close %&W", closeTab);

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss") + " Tab-Close Shortcut " + (closeTab ? "ON" : "OFF"));
        }
        [MenuItem(ItemPath + "Enable Lock Inspector %&L", priority = 6)]
        private static void ToggleInspectorLockShortcut()
        {
            inspectorLock = !inspectorLock;
            EditorPrefs.SetBool(inspectorLockKey, inspectorLock);
            Menu.SetChecked(ItemPath + "Enable Lock Inspector %&L", inspectorLock);

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