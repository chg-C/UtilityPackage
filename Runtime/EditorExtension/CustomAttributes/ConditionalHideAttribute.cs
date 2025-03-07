using System;
using UnityEngine;

namespace CHG.Utilities.Attribute
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ConditionalHideAttribute : PropertyAttribute
    {   
        ///연결된 속성이 유효할 경우에만 Attribute 출력

        /// <summary>
        /// Show/Hide 계산 값 저장용 변수
        /// </summary>
        public bool hideProperty;
        /// <summary>
        /// 연결된 속성의 이름
        /// </summary>
        public string refProperty;
        /// <summary>
        /// inverted일 경우 연결된 값이 false일 때 보이고 / true일 때 안 보이게 반전
        /// </summary>
        public bool inverted;

        /// <summary>
        /// 최종적으로 Hide할 필요 체크
        /// </summary>
        public bool NeedHide => hideProperty == inverted;

        public ConditionalHideAttribute(string refProperty, bool inverted = false)
        {
            this.refProperty = refProperty;
            this.inverted = inverted;
        }

    }
}