
        
using UnityEngine;
namespace CHG.Utilities.Patterns
{
    public interface ICommand
    {
        void Execute();
        bool CanExecute();
        string GetDescription();
    }
    public interface IUndoableCommand : ICommand
    {
        void Undo();
        void Redo();
    }
}
        