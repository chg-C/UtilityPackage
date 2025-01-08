using System;
using UnityEngine;

namespace CHG.Utilities.EditorExpansion
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        bool onEditor;
        bool onRuntime;
        
        public bool OnEditor {get {return onEditor;}}
        public bool OnRuntime {get {return onRuntime;}}
        public ReadOnlyAttribute(bool onEditor = false, bool onRuntime = true)
        {
            this.onEditor = onEditor;
            this.onRuntime = onRuntime;
        }
    }
}