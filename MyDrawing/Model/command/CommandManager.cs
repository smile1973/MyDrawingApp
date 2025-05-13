using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawing.Model.command
{
    public class CommandManager
    {
        private Stack<ICommand> _undoStack;
        private Stack<ICommand> _redoStack;

        public CommandManager()
        {
            _undoStack = new Stack<ICommand>();
            _redoStack = new Stack<ICommand>();
        }

        public void Execute(ICommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                ICommand command = _undoStack.Pop();
                _redoStack.Push(command);
                command.UnExecute();
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                ICommand command = _redoStack.Pop();
                _undoStack.Push(command);
                command.Execute();
            }
        }

        public bool IsRedoEnabled
        {
            get
            {
                return _redoStack.Count > 0;
            }
        }

        public bool IsUndoEnabled
        {
            get
            {
                return _undoStack.Count > 0;
            }
        }
    }
}
