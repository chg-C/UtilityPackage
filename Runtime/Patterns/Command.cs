

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CHG.Utilities.Patterns
{
    /// <summary>
    /// Command Pattern 인터페이스
    /// </summary>
    public interface ICommand
    {
        void Execute();
        bool CanExecute();
    }
    /// <summary>
    /// 확장된 Command Pattern 인터페이스. Undo / Redo 기능을 사용하려면 이쪽으로 사용
    /// </summary>
    public interface IUndoableCommand : ICommand
    {
        void Undo();
    }

    public class CommandStack<T> where T : IUndoableCommand
    {
        Stack<T> undoStack = new Stack<T>();
        Stack<T> redoStack = new Stack<T>();

        public bool ExecuteCommand(T command)
        {
            if(!command.CanExecute())
            {
                return false;
            }
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();

            return true;
        }
        public bool UndoCommand()
        {
            if(undoStack.Count > 0)
            {
                T topCommand = undoStack.Peek();
                if(!topCommand.CanExecute())
                    return false;

                topCommand.Undo();
                
                undoStack.Pop();
                redoStack.Push(topCommand);
                
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool RedoCommand()
        {
            if(redoStack.Count > 0)
            {
                T topCommand = redoStack.Peek();
                if(!topCommand.CanExecute())
                    return false;

                topCommand.Execute();
                redoStack.Pop();
                undoStack.Push(topCommand);                

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
        