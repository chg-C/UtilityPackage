///스크립트 생성 일자 - 2025 - 03 - 07
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using UnityEngine;

namespace CHG.Utilities.ColliderEvents
{
    /// <summary>
    /// OnTriggerEnter / OnTriggerExit 이벤트 핸들러
    /// </summary>
	public class OnTriggerEventHandler : ColliderEventHandler
	{
        void OnTriggerEnter(Collider other)
        {            
            if(!CheckCondition(other.gameObject))
                return;

            _onEnter?.Invoke();
        }
        void OnTriggerExit(Collider other)
        {
            if(!CheckCondition(other.gameObject))
                return;

            _onExit?.Invoke();
        }
    }
}