        
using UnityEngine;
namespace CHG.Utilities.Patterns
{
    public interface IBaseState
    {
        void Enter();
        void Update();
        void Exit();
        bool CanTransitionTo(IBaseState newState);
    }
}
        