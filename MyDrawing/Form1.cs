using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MyDrawing.shapes;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading.Tasks;

namespace MyDrawing
{
    public partial class Form1 : Form
    {
        private ShapeModel _shapeModel;
        private PresentationModel.PresentationModel _presentationModel;
        Panel _canvas = new DoubleBufferedPanel();
        private string _originalTitle;
        private const string FORM_TITLE = "MyDrawing";
        private const string AUTO_SAVING_TITLE = "MyDrawing (Auto saving...)";

        public Form1()
        {
            InitializeComponent();
            this.Text = FORM_TITLE;
            toolStripbtn_start.Click += (s, e) => _presentationModel.SetButtonState("Start");
            toolStripbtn_terminator.Click += (s, e) => _presentationModel.SetButtonState("Terminator");
            toolStripbtn_process.Click += (s, e) => _presentationModel.SetButtonState("Process");
            toolStripbtn_decision.Click += (s, e) => _presentationModel.SetButtonState("Decision");
            toolStripbtn_pointer.Click += (s, e) => _presentationModel.SetButtonState("Pointer");
            toolStripbtn_line.Click += (s, e) => _presentationModel.SetButtonState("Line");

            toolStripbtn_undo.Click += HandleUndo;
            toolStripbtn_redo.Click += HandleRedo;
            toolStripbtn_undo.Enabled = false;
            toolStripbtn_redo.Enabled = false;

            toolStripbtn_save.Click += async (s, e) => await HandleSave();
            toolStripbtn_load.Click += (s, e) => HandleLoad();

            _canvas.Location = new Point(100, 58);
            _canvas.Size = new Size(940, 660);
            _canvas.Name = "canvas";
            _canvas.BackColor = System.Drawing.Color.LightYellow;
            _canvas.MouseDown += HandleCanvasPressed;
            _canvas.MouseUp += HandleCanvasReleased;
            _canvas.MouseMove += HandleCanvasMoved;
            _canvas.Paint += HandleCanvasPaint;
            _canvas.MouseEnter += HandleCanvasMouseEnter;
            _canvas.MouseLeave += HandleCanvasMouseLeave;
            _canvas.MouseDoubleClick += (s, e) => HandleDragPointDoubleClick(e.X, e.Y);
            Controls.Add(_canvas);
            
            comboBox_shapes.SelectedIndexChanged += (s, e) => _presentationModel.ComboBoxCheck(comboBox_shapes.SelectedIndex);
            tb_word.TextChanged += (s, e) => _presentationModel.ValidateInput("Word", tb_word.Text);
            tb_x.TextChanged += (s, e) => _presentationModel.ValidateInput("X", tb_x.Text);
            tb_y.TextChanged += (s, e) => _presentationModel.ValidateInput("Y", tb_y.Text);
            tb_w.TextChanged += (s, e) => _presentationModel.ValidateInput("Width", tb_w.Text);
            tb_h.TextChanged += (s, e) => _presentationModel.ValidateInput("Height", tb_h.Text);

            _shapeModel = new ShapeModel();
            _presentationModel = new PresentationModel.PresentationModel(_shapeModel, _canvas);
            _presentationModel.ButtonStateChanged += HandleButtonStateChanged;
            _presentationModel.InputErrorsChanged += HandleInputErrorsChanged;
            _shapeModel.ShapeChanged += HandleModelChanged;

            _shapeModel.InitializeAutoSave();
            _shapeModel.AutoSaveStarted += HandleAutoSaveStarted;
            _shapeModel.AutoSaveCompleted += HandleAutoSaveCompleted;
            btn_add.DataBindings.Add("Enabled", _presentationModel, "IsAddButtonEnabled", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void HandleAutoSaveStarted(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleAutoSaveStarted(sender, e)));
                return;
            }
            this.Text = AUTO_SAVING_TITLE;
        }

        private void HandleAutoSaveCompleted(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    this.Text = FORM_TITLE;
                }));
            }
            else
            {
                this.Text = FORM_TITLE;
            }
        }
        private async Task HandleSave()
        {
            toolStripbtn_save.Enabled = false;
            try
            {
                await _presentationModel.SaveAsync();
            }
            finally
            {
                toolStripbtn_save.Enabled = true;
            }
        }

        private void HandleLoad()
        {
            _presentationModel.Load();
            UpdateDataGridView();
        }
        private void HandleUndo(object sender, EventArgs e)
        {
            _shapeModel.Undo();
            UpdateDataGridView();
        }
        private void HandleRedo(object sender, EventArgs e)
        {
            _shapeModel.Redo();
            UpdateDataGridView();
        }
        private void HandleInputErrorsChanged(object sender, Dictionary<string, bool> errors)
        {
            label_word.ForeColor = errors["Word"] ? Color.Red : Color.Black;
            label_x.ForeColor = errors["X"] ? Color.Red : Color.Black;
            label_y.ForeColor = errors["Y"] ? Color.Red : Color.Black;
            label_h.ForeColor = errors["Height"] ? Color.Red : Color.Black;
            label_w.ForeColor = errors["Width"] ? Color.Red : Color.Black; 
        }

        // 處理按鈕狀態改變事件
        private void HandleButtonStateChanged(object sender, string buttonName)
        {
            toolStripbtn_start.Checked = _presentationModel.GetButtonState("Start");
            toolStripbtn_terminator.Checked = _presentationModel.GetButtonState("Terminator");
            toolStripbtn_process.Checked = _presentationModel.GetButtonState("Process");
            toolStripbtn_decision.Checked = _presentationModel.GetButtonState("Decision");
            toolStripbtn_pointer.Checked = _presentationModel.GetButtonState("Pointer");
            toolStripbtn_line.Checked = _presentationModel.GetButtonState("Line");
        }
        private void HandleDragPointDoubleClick(int x, int y)
        {
            _presentationModel.ShowTextEditDialog(x, y);
        }

        private void HandleCanvasMouseEnter(object sender, EventArgs e)
        {
            _canvas.Cursor = _presentationModel.GetCursorType();
        }

        private void HandleCanvasMouseLeave(object sender, EventArgs e)
        {
            _canvas.Cursor = Cursors.Default;
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            string shapeType = comboBox_shapes.SelectedItem?.ToString() ?? "";
            _presentationModel.AddShape(shapeType, tb_word.Text);
            UpdateDataGridView();
        }

        private void UpdateDataGridView()
        {
            shapeDataGrid.Rows.Clear();
            foreach (var shape in _shapeModel.GetShapes())
            {
                shapeDataGrid.Rows.Add("Delete", shape.ID, shape.ShapeType, shape.Text, shape.X, shape.Y, shape.Height, shape.Width);
            }
        }

        private void ShapeDataGridCellClick(object sender, DataGridViewCellEventArgs e)
        {
            int shapeId = (int)shapeDataGrid.Rows[e.RowIndex].Cells["id_number"].Value;
            _presentationModel.DeleteShape(shapeId);
            UpdateDataGridView();
        }

        private void HandleCanvasPressed(object sender, MouseEventArgs e)
        {
            _shapeModel.PointerPressed(e.X, e.Y);
        }

        private void HandleCanvasReleased(object sender, MouseEventArgs e)
        {
            _canvas.Cursor = Cursors.Default;
            _shapeModel.PointerReleased(e.X, e.Y);
            _presentationModel.HandleCanvasReleased();
            UpdateDataGridView();
        }

        private void HandleCanvasMoved(object sender, MouseEventArgs e)
        {
            _shapeModel.PointerMoved(e.X, e.Y);
        }

        private void HandleCanvasPaint(object sender, PaintEventArgs e)
        {
            _presentationModel.Draw(e.Graphics);
        }

        private void HandleModelChanged()
        {
            toolStripbtn_redo.Enabled = _shapeModel.IsRedoEnabled;
            toolStripbtn_undo.Enabled = _shapeModel.IsUndoEnabled;
            Invalidate(true);
            _canvas.Update();
        }

    
        public class TextEditDialog : Form
        {
            private System.Windows.Forms.TextBox textBox;
            private System.Windows.Forms.Button okButton;
            private System.Windows.Forms.Button cancelButton;
            private string originalText;

            public string EditedText => textBox.Text;

            public TextEditDialog(string currentText)
            {
                InitializeComponents();
                originalText = currentText;
                textBox.Text = currentText;
                UpdateOkButtonState();
                textBox.TextChanged += HandleTextChanged;
            }

            private void HandleTextChanged(object sender, EventArgs e)
            {
                UpdateOkButtonState();
            }

            private void UpdateOkButtonState()
            {
                string newText = textBox.Text;
                okButton.Enabled = !string.IsNullOrWhiteSpace(newText) && newText != originalText;
            }

            private void InitializeComponents()
            {
                this.Text = "文字編輯方塊";
                this.Size = new System.Drawing.Size(300, 150);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                textBox = new System.Windows.Forms.TextBox
                {
                    Location = new System.Drawing.Point(10, 10),
                    Size = new System.Drawing.Size(260, 20)
                };

                okButton = new System.Windows.Forms.Button
                {
                    Text = "確定",
                    DialogResult = DialogResult.OK,
                    Location = new System.Drawing.Point(100, 70)
                };

                cancelButton = new System.Windows.Forms.Button
                {
                    Text = "取消",
                    DialogResult = DialogResult.Cancel,
                    Location = new System.Drawing.Point(190, 70)
                };

                this.Controls.AddRange(new Control[] { textBox, okButton, cancelButton });
                this.AcceptButton = okButton;
                this.CancelButton = cancelButton;
            }
        }
    }
}