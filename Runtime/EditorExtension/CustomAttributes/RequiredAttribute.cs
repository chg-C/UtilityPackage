using System;
using UnityEngine;

namespace CHG.Utilities.Attribute
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class RequiredAttribute : PropertyAttribute
    {
        /// <summary>
        /// Debug.Warning을 사용해 경고 표시
        /// </summary>
        public bool showWarning;
        
        public RequiredAttribute(bool showWarning = false)
        {
            this.showWarning = showWarning;
        }
        public bool isSafe;
    }
}