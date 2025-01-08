        
using UnityEngine;
namespace CHG.Utilities.Patterns
{
    /// <summary>
    /// State Pattern 인터페이스
    /// </summary>
    public interface IBaseState
    {
        void Enter();
        void Update();
        void Exit();
        bool CanTransitionTo(IBaseState newState);
    }

    public interface IStateHandler<T> where T : IBaseState
    {
        void ChangeState(T newState);
    }
}
        