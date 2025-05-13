using MyDrawing.Model.command;
using MyDrawing.Model;
using MyDrawing.shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace MyDrawing.PresentationModel
{
    public class PresentationModel : INotifyPropertyChanged
    {
        private readonly ShapeModel _model;
        private readonly Dictionary<string, bool> _buttonStates;
        private bool _isAddButtonEnabled;
        private bool _comboboxcheck = false;
        private Dictionary<string, bool> _inputErrors;
        private Dictionary<string, string> _inputValues;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<string> ButtonStateChanged;
        public event EventHandler<Dictionary<string, bool>> InputErrorsChanged;
        public event EventHandler<List<Shape>> DataGridViewChanged;

        public PresentationModel(ShapeModel model, Control canvas)
        {
            this._model = model;
            _buttonStates = new Dictionary<string, bool>
            {
                { "Start", false },
                { "Terminator", false },
                { "Process", false },
                { "Decision", false },
                { "Pointer", false },
                { "Line", false }
            };
            _inputErrors = new Dictionary<string, bool>
            {
                { "Word", true },
                { "X", true },
                { "Y", true },
                { "Width", true },
                { "Height", true }
            };
            _inputValues = new Dictionary<string, string>
            {
                { "Word", "" },
                { "X", "" },
                { "Y", "" },
                { "Width", "" },
                { "Height", "" }
            };
            IsAddButtonEnabled = false;
        }

        public bool IsAddButtonEnabled
        {
            get => _isAddButtonEnabled;
            private set
            {
                if (_isAddButtonEnabled != value)
                {
                    _isAddButtonEnabled = value;
                    OnPropertyChanged(nameof(IsAddButtonEnabled));
                }
            }
        }

        public void ComboBoxCheck(int selectedIndex)
        {
            _comboboxcheck = selectedIndex >= 0;
            UpdateAddButtonState();
        }

        public void ValidateInput(string fieldName, string value)
        {
            _inputValues[fieldName] = value;
            bool isValid = true;

            if (fieldName != "Word" && (!int.TryParse(value, out int number) || number <= 0))
            {
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                isValid = false;
            }

            _inputErrors[fieldName] = !isValid;
            InputErrorsChanged?.Invoke(this, _inputErrors);
            UpdateAddButtonState();
        }

        public Cursor GetCursorType()
        {
            return IsAnyButtonChecked() && !GetButtonState("Pointer") ?
                   Cursors.Cross : Cursors.Default;
        }

        public void HandleCanvasReleased()
        {
            ResetAllButtonStates();
            UpdateDataGridView();
        }

        public void DeleteShape(int shapeId)
        {
            var command = new DeleteCommand(_model, shapeId);
            _model.ExecuteCommand(command);
            UpdateDataGridView();
        }

        public (bool success, Shape shape) AddShape(string shapeType, string text)
        {
            if (!IsAddButtonEnabled || string.IsNullOrWhiteSpace(shapeType))
            {
                return (false, null);
            }

            int.TryParse(_inputValues["X"], out int x);
            int.TryParse(_inputValues["Y"], out int y);
            int.TryParse(_inputValues["Width"], out int width);
            int.TryParse(_inputValues["Height"], out int height);

            Shape newShape = ShapeFactory.CreateShape(shapeType);
            if (newShape == null)
            {
                return (false, null);
            }

            newShape.ID = _model.GetNextId();
            newShape.Text = text;
            newShape.X = x;
            newShape.Y = y;
            newShape.Width = width;
            newShape.Height = height;

            var command = new AddCommand(_model, newShape);
            _model.ExecuteCommand(command);

            UpdateDataGridView();
            return (true, newShape);
        }

        public bool IsAnyButtonChecked()
        {
            return _buttonStates.Values.Any(state => state);
        }

        public void ResetAllButtonStates()
        {
            foreach (var key in _buttonStates.Keys.ToList())
            {
                _buttonStates[key] = false;
            }
            NotifyButtonStateChanged("");
            _model.SetSelectedShapeType("");
        }

        public void SetButtonState(string buttonName)
        {
            if (!_buttonStates.ContainsKey(buttonName))
                return;

            foreach (var key in _buttonStates.Keys.ToList())
            {
                _buttonStates[key] = false;
            }

            _buttonStates[buttonName] = true;
            _model.SetSelectedShapeType(buttonName);
            NotifyButtonStateChanged(buttonName);
        }

        public bool GetButtonState(string buttonName)
        {
            return _buttonStates.ContainsKey(buttonName) && _buttonStates[buttonName];
        }
        public void Draw(System.Drawing.Graphics graphics)
        {
            _model.Draw(new WindowsFormsGraphicsAdaptor(graphics));
        }

        public string GetSelectedShapeText()
        {
            var shape = _model.GetSelectedShape();
            return shape?.Text ?? "";
        }

        public bool HandleDragPointDoubleClick(int x, int y)
        {
            return _model.IsPointOnDragPoint(x, y);
        }

        public void ShowTextEditDialog(int x, int y)
        {
            if (HandleDragPointDoubleClick(x, y))
            {
                Shape shape = _model.GetSelectedShape();
                using (var dialog = new Form1.TextEditDialog(GetSelectedShapeText()))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        UpdateShapeText(dialog.EditedText);
                    }
                }
            }
        }
        public void UpdateShapeText(string newText)
        {
            Shape shape = _model.GetSelectedShape();
            if (shape != null && shape.Text != newText)  // 只在文字真的改變時才創建命令
            {
                var command = new TextChangedCommand(_model, shape, shape.Text, newText);
                _model.ExecuteCommand(command);
            }
        }
        private void UpdateDataGridView()
        {
            DataGridViewChanged?.Invoke(this, _model.GetShapes());
        }
        private void UpdateAddButtonState()
        {
            IsAddButtonEnabled = !_inputErrors.Values.Any(error => error) &&
                               !_inputValues.Values.Any(string.IsNullOrWhiteSpace) &&
                               _comboboxcheck;
        }

        private void NotifyButtonStateChanged(string buttonName)
        {
            ButtonStateChanged?.Invoke(this, buttonName);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SaveAsync()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "MyDrawing files (*.mydrawing)|*.mydrawing";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await _model.SaveToFileAsync(saveFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "存檔錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void Load()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "MyDrawing files (*.mydrawing)|*.mydrawing";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _model.LoadFromFile(openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "讀取錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}