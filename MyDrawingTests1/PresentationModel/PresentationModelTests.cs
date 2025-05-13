using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using MyDrawing.shapes;
using System;
using System.Linq;

namespace MyDrawing.PresentationModel.Tests
{
    [TestClass]
    public class PresentationModelTests
    {
        private PresentationModel _presentationModel;
        private ShapeModel _model;
        private Control _canvas;
        private List<string> _propertyChangedList;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _canvas = new Control();
            _presentationModel = new PresentationModel(_model, _canvas);
            _propertyChangedList = new List<string>();
            _presentationModel.PropertyChanged += (sender, e) =>
            {
                _propertyChangedList.Add(e.PropertyName);
            };
        }

        [TestMethod]
        public void TestComboBoxCheck_SelectedIndexGreaterThanZero_ShouldUpdateState()
        {
            _presentationModel.ComboBoxCheck(1);
            _presentationModel.ValidateInput("Word", "Test");
            _presentationModel.ValidateInput("X", "10");
            _presentationModel.ValidateInput("Y", "10");
            _presentationModel.ValidateInput("Width", "100");
            _presentationModel.ValidateInput("Height", "100");

            Assert.IsTrue(_presentationModel.IsAddButtonEnabled);
        }

        [TestMethod]
        public void TestComboBoxCheck_SelectedIndexZero_ShouldNotEnableAddButton()
        {
            _presentationModel.ComboBoxCheck(0);

            Assert.IsFalse(_presentationModel.IsAddButtonEnabled);
        }

        // Input Validation
        [TestMethod]
        public void TestValidateInput_ValidNumericalInput_ShouldNotHaveError()
        {
            Dictionary<string, bool> errors = null;
            _presentationModel.InputErrorsChanged += (s, e) => errors = e;

            _presentationModel.ValidateInput("X", "100");

            Assert.IsNotNull(errors);
            Assert.IsFalse(errors["X"]);
        }

        [TestMethod]
        public void TestValidateInput_InvalidNumericalInput_ShouldHaveError()
        {
            Dictionary<string, bool> errors = null;
            _presentationModel.InputErrorsChanged += (s, e) => errors = e;

            _presentationModel.ValidateInput("X", "-1");

            Assert.IsNotNull(errors);
            Assert.IsTrue(errors["X"]);
        }

        [TestMethod]
        public void TestValidateInput_EmptyInput_ShouldHaveError()
        {
            Dictionary<string, bool> errors = null;
            _presentationModel.InputErrorsChanged += (s, e) => errors = e;

            _presentationModel.ValidateInput("Word", "");

            Assert.IsNotNull(errors);
            Assert.IsTrue(errors["Word"]);
        }

        // Cursor Type
        [TestMethod]
        public void TestGetCursorType_NoButtonChecked_ShouldReturnDefault()
        {
            var cursor = _presentationModel.GetCursorType();
            Assert.AreEqual(Cursors.Default, cursor);
        }

        [TestMethod]
        public void TestGetCursorType_NonPointerButtonChecked_ShouldReturnCross()
        {
            _presentationModel.SetButtonState("Process");
            var cursor = _presentationModel.GetCursorType();
            Assert.AreEqual(Cursors.Cross, cursor);
        }

        [TestMethod]
        public void TestGetCursorType_PointerButtonChecked_ShouldReturnDefault()
        {
            _presentationModel.SetButtonState("Pointer");
            var cursor = _presentationModel.GetCursorType();
            Assert.AreEqual(Cursors.Default, cursor);
        }

        // Button State
        [TestMethod]
        public void TestSetButtonState_ValidButton_ShouldUpdateState()
        {
            string changedButton = null;
            _presentationModel.ButtonStateChanged += (s, e) => changedButton = e;

            _presentationModel.SetButtonState("Process");

            Assert.AreEqual("Process", changedButton);
            Assert.IsTrue(_presentationModel.GetButtonState("Process"));
        }

        [TestMethod]
        public void TestSetButtonState_InvalidButton_ShouldNotUpdateState()
        {
            _presentationModel.SetButtonState("InvalidButton");
            Assert.IsFalse(_presentationModel.GetButtonState("InvalidButton"));
        }

        [TestMethod]
        public void TestResetAllButtonStates_ShouldResetAllButtons()
        {
            string changedButton = "test";
            _presentationModel.ButtonStateChanged += (s, e) => changedButton = e;

            _presentationModel.SetButtonState("Process");
            _presentationModel.ResetAllButtonStates();

            Assert.AreEqual("", changedButton);
            Assert.IsFalse(_presentationModel.GetButtonState("Process"));
        }

        [TestMethod]
        public void TestAddShape_ValidInput_ShouldAddShape()
        {
            _presentationModel.ComboBoxCheck(1);
            _presentationModel.ValidateInput("Word", "Test");
            _presentationModel.ValidateInput("X", "10");
            _presentationModel.ValidateInput("Y", "10");
            _presentationModel.ValidateInput("Width", "100");
            _presentationModel.ValidateInput("Height", "100");

            List<Shape> updatedShapes = null;
            _presentationModel.DataGridViewChanged += (s, e) => updatedShapes = e;

            var (success, shape) = _presentationModel.AddShape("Process", "Test");

            Assert.IsTrue(success);
            Assert.IsNotNull(shape);
            Assert.IsNotNull(updatedShapes);
            Assert.AreEqual(1, updatedShapes.Count);
        }

        [TestMethod]
        public void TestAddShape_InvalidInput_ShouldNotAddShape()
        {
            var (success, shape) = _presentationModel.AddShape("", "Test");
            Assert.IsFalse(success);
            Assert.IsNull(shape);
        }

        [TestMethod]
        public void TestDeleteShape_ExistingShape_ShouldDelete()
        {
            List<Shape> updatedShapes = null;
            _presentationModel.DataGridViewChanged += (s, e) => updatedShapes = e;

            _presentationModel.ComboBoxCheck(1);
            _presentationModel.ValidateInput("Word", "Test");
            _presentationModel.ValidateInput("X", "10");
            _presentationModel.ValidateInput("Y", "10");
            _presentationModel.ValidateInput("Width", "100");
            _presentationModel.ValidateInput("Height", "100");
            var (_, shape) = _presentationModel.AddShape("Process", "Test");

            _presentationModel.DeleteShape(shape.ID);

            Assert.IsNotNull(updatedShapes);
            Assert.AreEqual(0, updatedShapes.Count);
        }

        [TestMethod]
        public void TestHandleCanvasReleased_ShouldResetAndUpdate()
        {
            List<Shape> updatedShapes = null;
            _presentationModel.DataGridViewChanged += (s, e) => updatedShapes = e;

            _presentationModel.HandleCanvasReleased();

            Assert.IsNotNull(updatedShapes);
            Assert.IsFalse(_presentationModel.IsAnyButtonChecked());
        }

        [TestMethod]
        public void TestDraw_ShouldCreateGraphicsAdapter()
        {
            var panel = new Panel();
            var graphics = panel.CreateGraphics();

            try
            {
                _presentationModel.Draw(graphics);
                Assert.IsTrue(true);
            }
            finally
            {
                graphics.Dispose();
            }
        }
    }

    [TestClass]
    public class PresentationModelPrivateMethodTests
    {
        private PrivateObject _privateModel;
        private PresentationModel _presentationModel;
        private ShapeModel _model;
        private Control _canvas;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _canvas = new Control();
            _presentationModel = new PresentationModel(_model, _canvas);
            _privateModel = new PrivateObject(_presentationModel);
        }

        [TestMethod]
        public void TestUpdateAddButtonState_AllValid_ShouldEnableButton()
        {
            _presentationModel.ComboBoxCheck(1);
            _presentationModel.ValidateInput("Word", "Test");
            _presentationModel.ValidateInput("X", "10");
            _presentationModel.ValidateInput("Y", "10");
            _presentationModel.ValidateInput("Width", "100");
            _presentationModel.ValidateInput("Height", "100");

            _privateModel.Invoke("UpdateAddButtonState");

            Assert.IsTrue(_presentationModel.IsAddButtonEnabled);
        }

        [TestMethod]
        public void TestUpdateAddButtonState_SomeInvalid_ShouldDisableButton()
        {
            _presentationModel.ComboBoxCheck(1);
            _presentationModel.ValidateInput("Word", "");

            _privateModel.Invoke("UpdateAddButtonState");

            Assert.IsFalse(_presentationModel.IsAddButtonEnabled);
        }

        [TestMethod]
        public void TestNotifyButtonStateChanged_ShouldInvokeEvent()
        {
            string changedButton = null;
            _presentationModel.ButtonStateChanged += (s, e) => changedButton = e;

            _privateModel.Invoke("NotifyButtonStateChanged", "TestButton");

            Assert.AreEqual("TestButton", changedButton);
        }

        [TestMethod]
        public void TestOnPropertyChanged_ShouldInvokeEvent()
        {
            string changedProperty = null;
            _presentationModel.PropertyChanged += (s, e) => changedProperty = e.PropertyName;

            _privateModel.Invoke("OnPropertyChanged", "TestProperty");

            Assert.AreEqual("TestProperty", changedProperty);
        }

        [TestMethod]
        public void TestUpdateDataGridView_ShouldInvokeEvent()
        {
            List<Shape> updatedShapes = null;
            _presentationModel.DataGridViewChanged += (s, e) => updatedShapes = e;

            _privateModel.Invoke("UpdateDataGridView");

            Assert.IsNotNull(updatedShapes);
            CollectionAssert.AreEqual(_model.GetShapes(), updatedShapes);
        }
    }

    [TestClass]
    public class PresentationModelUndoRedoBottonStateTests
    {
        private PresentationModel _presentationModel;
        private ShapeModel _model;
        private Control _canvas;
        private Shape _testShape;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _canvas = new Control();
            _presentationModel = new PresentationModel(_model, _canvas);

            // Create and add a test shape
            _testShape = ShapeFactory.CreateShape("Process");
            _testShape.Text = "Initial Text";
            _testShape.X = 100;
            _testShape.Y = 100;
            _testShape.Width = 50;
            _testShape.Height = 50;
            _model.AddNewShape(_testShape);
        }

        [TestMethod]
        public void TestUndoRedo_AfterAddShape()
        {
            // Arrange
            _presentationModel.ComboBoxCheck(1);
            _presentationModel.ValidateInput("Word", "Test");
            _presentationModel.ValidateInput("X", "10");
            _presentationModel.ValidateInput("Y", "10");
            _presentationModel.ValidateInput("Width", "100");
            _presentationModel.ValidateInput("Height", "100");

            // Act - Add shape should enable undo
            var (success, shape) = _presentationModel.AddShape("Process", "Test");

            // Assert
            Assert.IsTrue(_model.IsUndoEnabled);
            Assert.IsFalse(_model.IsRedoEnabled);

            // Act - Undo should enable redo
            _model.Undo();

            // Assert
            Assert.IsFalse(_model.IsUndoEnabled);
            Assert.IsTrue(_model.IsRedoEnabled);
        }

        [TestMethod]
        public void TestUndoRedo_AfterDeleteShape()
        {
            // Arrange - Initial state
            Assert.IsFalse(_model.IsUndoEnabled);
            Assert.IsFalse(_model.IsRedoEnabled);

            // Act - Delete should enable undo
            _presentationModel.DeleteShape(_testShape.ID);

            // Assert
            Assert.IsTrue(_model.IsUndoEnabled);
            Assert.IsFalse(_model.IsRedoEnabled);

            // Act - Undo deletion
            _model.Undo();

            // Assert
            Assert.IsFalse(_model.IsUndoEnabled);
            Assert.IsTrue(_model.IsRedoEnabled);
        }

        [TestMethod]
        public void TestUndoRedo_AfterUpdateText()
        {
            // Arrange
            _model.SetState(new PointerState(_model));
            _model.PointerPressed(_testShape.X + 10, _testShape.Y + 10);

            // Act - Update text should enable undo
            _presentationModel.UpdateShapeText("New Text");

            // Assert
            Assert.IsTrue(_model.IsUndoEnabled);
            Assert.IsFalse(_model.IsRedoEnabled);

            // Act - Undo text change
            _model.Undo();

            // Assert
            Assert.IsFalse(_model.IsUndoEnabled);
            Assert.IsTrue(_model.IsRedoEnabled);
        }

        [TestMethod]
        public void TestUndoRedo_MultipleOperations()
        {
            // Act 1 - Add shape
            _presentationModel.ComboBoxCheck(1);
            _presentationModel.ValidateInput("Word", "Test");
            _presentationModel.ValidateInput("X", "10");
            _presentationModel.ValidateInput("Y", "10");
            _presentationModel.ValidateInput("Width", "100");
            _presentationModel.ValidateInput("Height", "100");
            _presentationModel.AddShape("Process", "Test");

            // Assert 1
            Assert.IsTrue(_model.IsUndoEnabled);
            Assert.IsFalse(_model.IsRedoEnabled);

            // Act 2 - Delete shape
            _presentationModel.DeleteShape(_testShape.ID);

            // Assert 2 - Should still be undo enabled after second operation
            Assert.IsTrue(_model.IsUndoEnabled);
            Assert.IsFalse(_model.IsRedoEnabled);

            // Act 3 - Undo twice
            _model.Undo();
            _model.Undo();

            // Assert 3 - After undoing all operations
            Assert.IsFalse(_model.IsUndoEnabled);
            Assert.IsTrue(_model.IsRedoEnabled);
        }

        [TestMethod]
        public void TestUndoRedo_RedoClearedAfterNewOperation()
        {
            // Arrange - Do operation and undo it
            _presentationModel.DeleteShape(_testShape.ID);
            _model.Undo();
            Assert.IsTrue(_model.IsRedoEnabled);

            // Additional setup - Select the shape first
            _model.SetState(new PointerState(_model));
            _model.PointerPressed(_testShape.X + 10, _testShape.Y + 10);

            // Act - Perform new operation
            _presentationModel.UpdateShapeText("New Text");

            // Assert - Redo should be cleared
            Assert.IsTrue(_model.IsUndoEnabled);
            Assert.IsFalse(_model.IsRedoEnabled);
        }
    }
}