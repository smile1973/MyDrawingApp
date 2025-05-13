using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyDrawing;
using MyDrawing.Model.command;
using MyDrawing.Model;
using MyDrawing.shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MyDrawing.Tests
{
    public class MockGraphics : IGraphics
    {
        public bool WasCleared { get; private set; }
        public bool LineDrawn { get; private set; }
        public bool RectangleDrawn { get; private set; }
        public bool EllipseDrawn { get; private set; }
        public bool StringDrawn { get; private set; }
        public bool ArcDrawn { get; private set; }
        public bool ColorSet { get; private set; }
        public bool PenSet { get; private set; }

        public void ClearAll() => WasCleared = true;
        public void DrawLine(double x1, double y1, double x2, double y2) => LineDrawn = true;
        public void DrawRectangle(double x, double y, double width, double height) => RectangleDrawn = true;
        public void DrawEllipse(double x, double y, double width, double height) => EllipseDrawn = true;
        public void DrawString(string text, double x, double y) => StringDrawn = true;
        public void DrawArc(int x, int y, int width, int height, float startAngle, float sweepAngle) => ArcDrawn = true;
        public void SetPenColor(string color) => ColorSet = true;
        public void SetPen(int x) => PenSet = true;
    }
    public class MonkState : IState
    {
        public bool ISPress { get; private set; }
        public bool ISMove { get; private set; }
        public bool ISRelease { get; private set; }
        public bool ISDraw { get; private set; }

        public void HandlePointerPressed(double x, double y) => ISPress = true;
        public void HandlePointerMoved(double x, double y) => ISMove = true;
        public void HandlePointerReleased(double x, double y) => ISRelease = true;
        public void Draw(IGraphics graphics) => ISDraw = true;
    }

    [TestClass]
    public class ShapeModelTests
    {
        private ShapeModel _model;
        private MockGraphics _mockGraphics;
        private MonkState _monkState;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _mockGraphics = new MockGraphics();
            _monkState = new MonkState();
        }
        [TestMethod]
        public void TestInitialState()
        {
            Assert.IsNotNull(_model);
            Assert.AreEqual(0, _model.GetShapes().Count);
        }

        [TestMethod]
        public void TestSetState_WhenValidState_ShouldUpdateCurrentState()
        {
            // Arrange
            bool isModelChanged = false;
            _model.ShapeChanged += () => isModelChanged = true;

            // Act
            _model.SetState(_monkState);

            // Assert
            Assert.IsTrue(isModelChanged, "Model should notify when state changes");
        }

        [TestMethod]
        public void TestSetState_WhenNullState_ShouldNotUpdateState()
        {
            // Arrange
            bool isModelChanged = false;
            _model.SetState(_monkState); // Set initial state
            _model.ShapeChanged += () => isModelChanged = true;

            // Act
            _model.SetState(null);

            // Assert
            Assert.IsFalse(isModelChanged, "Model should not change state when null state is provided");
        }

        [TestMethod]
        public void TestEnterDrawingState_WhenValidShapeType_ShouldChangeToDrawingState()
        {
            // Arrange
            bool isModelChanged = false;
            _model.ShapeChanged += () => isModelChanged = true;

            // Act
            _model.EnterDrawingState("Rectangle");

            // Assert
            Assert.IsTrue(isModelChanged, "Model should notify when entering drawing state");
        }

        [TestMethod]
        public void TestEnterDrawingState_WhenEmptyShapeType_ShouldNotChangeState()
        {
            // Arrange
            bool isModelChanged = false;
            _model.ShapeChanged += () => isModelChanged = true;

            // Act
            _model.EnterDrawingState(null);

            // Assert
            Assert.IsFalse(isModelChanged, "Model should not change state when shape type is empty");
        }

        [TestMethod]
        public void TestEnterPointerState_ShouldChangeToPointerState()
        {
            // Arrange
            bool isModelChanged = false;
            _model.ShapeChanged += () => isModelChanged = true;

            // Act
            _model.EnterPointerState();

            // Assert
            Assert.IsTrue(isModelChanged, "Model should notify when entering pointer state");
        }
        [TestMethod]
        public void TestSetSelectedShapeType_WhenPointer_ShouldEnterPointerState()
        {
            // Arrange
            bool isModelChanged = false;
            _model.ShapeChanged += () => isModelChanged = true;

            // Act
            _model.SetSelectedShapeType("Pointer");

            // Assert
            Assert.IsTrue(isModelChanged, "Model should notify when entering pointer state via SetSelectedShapeType");
        }

        [TestMethod]
        public void TestSetSelectedShapeType_WhenShapeType_ShouldEnterDrawingState()
        {
            // Arrange
            bool isModelChanged = false;
            _model.ShapeChanged += () => isModelChanged = true;

            // Act
            _model.SetSelectedShapeType("Start");

            // Assert
            Assert.IsTrue(isModelChanged, "Model should notify when entering drawing state via SetSelectedShapeType");
        }

        [TestMethod]
        public void TestSetState_ShouldTriggerStateEvents()
        {
            // Arrange
            _model.SetState(_monkState);

            // Act
            _model.PointerPressed(10, 10);
            _model.PointerMoved(20, 20);
            _model.PointerReleased(30, 30);

            // Assert
            Assert.IsTrue(_monkState.ISPress, "HandlePointerPressed should be called");
            Assert.IsTrue(_monkState.ISMove, "HandlePointerMoved should be called");
            Assert.IsTrue(_monkState.ISRelease, "HandlePointerReleased should be called");
        }

        [TestMethod]
        public void TestDraw_ShouldTriggerStateDraw()
        {
            // Arrange
            _model.SetState(_monkState);
            var mockGraphics = new MockGraphics();

            // Act
            _model.Draw(mockGraphics);

            // Assert
            Assert.IsTrue(_monkState.ISDraw, "Draw method should be called on current state");
            Assert.IsTrue(mockGraphics.WasCleared, "ClearAll should be called before drawing");
        }

        [TestMethod]
        public void TestGetNextId()
        {
            // First ID should be 1
            Assert.AreEqual(1, _model.GetNextId());
            // Second ID should be 2
            Assert.AreEqual(2, _model.GetNextId());
            // Third ID should be 3
            Assert.AreEqual(3, _model.GetNextId());
        }

        [TestMethod]
        public void TestAddNewShape()
        {
            Shape shape = ShapeFactory.CreateShape("Process");
            shape.X = 100;
            shape.Y = 100;
            shape.Width = 50;
            shape.Height = 50;

            _model.AddNewShape(shape);
            List<Shape> shapes = _model.GetShapes();

            Assert.AreEqual(1, shapes.Count);
            Assert.AreEqual(shape, shapes[0]);
        }

        [TestMethod]
        public void TestGetShapeAt()
        {
            // Add a shape at specific coordinates
            Shape shape1 = ShapeFactory.CreateShape("Process");
            shape1.X = 100;
            shape1.Y = 100;
            shape1.Width = 50;
            shape1.Height = 50;
            _model.AddNewShape(shape1);

            Shape shape2 = ShapeFactory.CreateShape("Start");
            shape2.X = 10;
            shape2.Y = 10;
            shape2.Width = 100;
            shape2.Height = 100;
            _model.AddNewShape(shape2);

            // Test point inside shape
            Shape foundShape = _model.GetShapeAt(125, 125);
            Assert.AreEqual(shape1, foundShape);
            Assert.AreNotEqual(shape2, foundShape);

            // Test point outside shape
            Shape notFound = _model.GetShapeAt(200, 200);
            Assert.IsNull(notFound);
        }

        [TestMethod]
        public void TestDeleteShape()
        {
            // Add a shape
            Shape shape = ShapeFactory.CreateShape("Process");
            shape.ID = _model.GetNextId();
            _model.AddNewShape(shape);

            // Verify shape was added
            Assert.AreEqual(1, _model.GetShapes().Count);

            // Delete the shape
            bool result = _model.DeleteShape(shape.ID);
            Assert.IsTrue(result);
            Assert.AreEqual(0, _model.GetShapes().Count);

            // Try to delete non-existent shape
            result = _model.DeleteShape(999);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestModelChanged()
        {
            bool eventRaised = false;
            _model.ShapeChanged += () => eventRaised = true;

            // Add shape should trigger event
            Shape shape = ShapeFactory.CreateShape("Process");
            _model.AddNewShape(shape);
            Assert.IsTrue(eventRaised);

            // Reset flag and test delete
            eventRaised = false;
            _model.DeleteShape(shape.ID);
            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void TestGenerateRandomText()
        {
            string text1 = _model.GenerateRandomText();
            string text2 = _model.GenerateRandomText();

            // Verify length constraints (3-10 characters)
            Assert.IsTrue(text1.Length >= 3 && text1.Length <= 10);
            Assert.IsTrue(text2.Length >= 3 && text2.Length <= 10);

            // Verify two generated texts are different
            Assert.AreNotEqual(text1, text2);
        }

        [TestMethod]
        public void TestSetSelectedShapeType()
        {
            // Test Pointer state
            _model.SetSelectedShapeType("Pointer");
            var shapes = _model.GetShapes();
            Assert.AreEqual(0, shapes.Count); // Should not create any shapes

            // Test drawing state
            _model.SetSelectedShapeType("Process");
            shapes = _model.GetShapes();
            Assert.AreEqual(0, shapes.Count); // Should not create shapes until drawing
        }
        [TestMethod]
        public void TestDraw()
        {
            //process
            var shape_process = ShapeFactory.CreateShape("Process");
            shape_process.X = 10;
            shape_process.Y = 10;
            shape_process.Width = 100;
            shape_process.Height = 100;
            shape_process.Text = "test";
            _model.AddNewShape(shape_process);
            _model.Draw(_mockGraphics);
            Assert.IsTrue(_mockGraphics.WasCleared);
            Assert.IsTrue(_mockGraphics.RectangleDrawn);
            Assert.IsTrue(_mockGraphics.StringDrawn);

            //process
            var shape_start = ShapeFactory.CreateShape("Start");
            shape_start.X = 10;
            shape_start.Y = 10;
            shape_start.Width = 100;
            shape_start.Height = 100;
            shape_start.Text = "test";
            _model.AddNewShape(shape_start);
            _model.Draw(_mockGraphics);
            Assert.IsTrue(_mockGraphics.WasCleared);
            Assert.IsTrue(_mockGraphics.EllipseDrawn);
            Assert.IsTrue(_mockGraphics.StringDrawn);

            //terminator
            var shape_terminator = ShapeFactory.CreateShape("Terminator");
            shape_terminator.X = 10;
            shape_terminator.Y = 10;
            shape_terminator.Width = 100;
            shape_terminator.Height = 100;
            shape_terminator.Text = "test";
            _model.AddNewShape(shape_terminator);
            _model.Draw(_mockGraphics);
            Assert.IsTrue(_mockGraphics.WasCleared);
            Assert.IsTrue(_mockGraphics.ArcDrawn);
            Assert.IsTrue(_mockGraphics.LineDrawn);
            Assert.IsTrue(_mockGraphics.StringDrawn);

            //decision
            var shape_decision = ShapeFactory.CreateShape("Decision");
            shape_decision.X = 10;
            shape_decision.Y = 10;
            shape_decision.Width = 100;
            shape_decision.Height = 100;
            shape_decision.Text = "test";
            _model.AddNewShape(shape_decision);
            _model.Draw(_mockGraphics);
            Assert.IsTrue(_mockGraphics.WasCleared);
            Assert.IsTrue(_mockGraphics.LineDrawn);
            Assert.IsTrue(_mockGraphics.StringDrawn);
        }
        [TestMethod]
        public void TestDrawingState_HandlePointerPressed_NegativeCoordinates()
        {
            // Arrange
            var drawingState = new DrawingState(_model, "Process");

            // Act
            drawingState.HandlePointerPressed(-10, -10);

            // Assert - No shape should be created
            Assert.AreEqual(0, _model.GetShapes().Count);
        }

        // Test for shape creation with reverse coordinates
        [TestMethod]
        public void TestDrawingState_ReverseCoordinates()
        {
            // Arrange
            var drawingState = new DrawingState(_model, "Process");

            // Act - Draw from right to left and bottom to top
            drawingState.HandlePointerPressed(100, 100);
            drawingState.HandlePointerMoved(50, 50);
            drawingState.HandlePointerReleased(50, 50);

            // Assert
            var shape = _model.GetShapes()[0];
            Assert.AreEqual(50, shape.X); // Should adjust to smaller X
            Assert.AreEqual(50, shape.Y); // Should adjust to smaller Y
            Assert.AreEqual(50, shape.Width);
            Assert.AreEqual(50, shape.Height);
        }

        // Test when ShapeFactory returns null
        [TestMethod]
        public void TestDrawingState_NullShapeType()
        {
            // Arrange
            var drawingState = new DrawingState(_model, "InvalidShape");

            // Act
            drawingState.HandlePointerPressed(10, 10);
            drawingState.HandlePointerMoved(20, 20);
            drawingState.HandlePointerReleased(20, 20);

            // Assert
            Assert.AreEqual(0, _model.GetShapes().Count);
        }

        // Test Terminator shape with different coordinate scenarios
        [TestMethod]
        public void TestTerminator_DifferentCoordinateScenarios()
        {
            // Arrange
            var drawingState = new DrawingState(_model, "Terminator");

            // Test 1: Right to left
            drawingState.HandlePointerPressed(100, 100);
            drawingState.HandlePointerMoved(50, 100);
            drawingState.HandlePointerReleased(50, 100);

            var shape1 = _model.GetShapes()[0];
            Assert.AreEqual(50, shape1.X);

            // Test 2: Bottom to top
            drawingState = new DrawingState(_model, "Terminator");
            drawingState.HandlePointerPressed(100, 100);
            drawingState.HandlePointerMoved(100, 50);
            drawingState.HandlePointerReleased(100, 50);

            var shape2 = _model.GetShapes()[1];
            Assert.AreEqual(50, shape2.Y);
        }

        // Test PointerState drag functionality with null shape
        [TestMethod]
        public void TestPointerState_DragWithNullShape()
        {
            // Arrange
            var pointerState = new PointerState(_model);

            // Act
            pointerState.HandlePointerPressed(10, 10);
            pointerState.HandlePointerMoved(20, 20);
            pointerState.HandlePointerReleased(20, 20);

            // Draw should not throw exception when no shape is selected
            pointerState.Draw(_mockGraphics);

            Assert.IsFalse(_mockGraphics.RectangleDrawn);
        }

        // Test multiple event handlers
        [TestMethod]
        public void TestModelChanged_MultipleHandlers()
        {
            // Arrange
            int eventCount = 0;
            void Handler1() => eventCount++;
            void Handler2() => eventCount++;

            _model.ShapeChanged += Handler1;
            _model.ShapeChanged += Handler2;

            // Act
            _model.NotifyModelChanged();

            // Assert
            Assert.AreEqual(2, eventCount);

            // Cleanup
            _model.ShapeChanged -= Handler1;
            _model.ShapeChanged -= Handler2;
        }

        // Test drawing state with null preview shape
        [TestMethod]
        public void TestDrawingState_NullPreviewShape()
        {
            // Arrange
            var drawingState = new DrawingState(_model, "InvalidShape");

            // Act
            drawingState.HandlePointerPressed(10, 10);
            drawingState.HandlePointerMoved(20, 20);
            drawingState.Draw(_mockGraphics);

            // Assert - Should not throw exception
            Assert.IsFalse(_mockGraphics.RectangleDrawn);
        }
    }

    [TestClass]
    public class PointerStateTests
    {
        private ShapeModel _model;
        private PointerState _pointerState;
        private MockGraphics _mockGraphics;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _pointerState = new PointerState(_model);
            _mockGraphics = new MockGraphics();
        }
        private Shape CreateTestShape()
        {
            var shape = ShapeFactory.CreateShape("Start");
            shape.X = 100;
            shape.Y = 100;
            shape.Width = 200;
            shape.Height = 100;
            shape.Text = "Test Shape";
            shape.TextX = 120;
            shape.TextY = 130;
            shape.DragPointX = 170;
            shape.DragPointY = 126;
            return shape;
        }

        [TestMethod]
        public void TestHandlePointerPressedOutsideShape()
        {
            // Arrange
            var shape = ShapeFactory.CreateShape("Process");
            shape.X = 10;
            shape.Y = 10;
            shape.Width = 100;
            shape.Height = 100;
            _model.AddNewShape(shape);

            // Act
            _pointerState.HandlePointerPressed(200, 200);
            _pointerState.Draw(_mockGraphics);

            // Assert
            Assert.IsFalse(_mockGraphics.RectangleDrawn);
        }

        [TestMethod]
        public void TestHandlePointerPressed()
        {
            var shape = CreateTestShape();
            _model.AddNewShape(shape);
            // Act
            _pointerState.HandlePointerPressed(150, 150);
            _pointerState.Draw(_mockGraphics);

            var selectedShape = _model.GetShapeAt(150, 150);
            Assert.IsNotNull(selectedShape, "Shape should be selected when drag point is clicked");

            Assert.IsTrue(_mockGraphics.PenSet);
            Assert.IsTrue(_mockGraphics.ColorSet);
        }

        [TestMethod]
        public void TestHandlePointerMoved()
        {
            var shape = CreateTestShape();
            _model.AddNewShape(shape);

            _pointerState.HandlePointerPressed(150, 150);
            _pointerState.HandlePointerMoved(200, 150);

            var selectedShape = _model.GetShapeAt(200, 150);
            Assert.IsNotNull(selectedShape);
        }
    }

    [TestClass]
    public class PointerStateTextMovementTests
    {
        private ShapeModel _model;
        private PointerState _pointerState;
        private MockGraphics _mockGraphics;
        private PrivateObject _privatePointerState;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _pointerState = new PointerState(_model);
            _privatePointerState = new PrivateObject(_pointerState);
            _mockGraphics = new MockGraphics();
        }

        private Shape CreateTestShape()
        {
            var shape = ShapeFactory.CreateShape("Start");
            shape.X = 100;
            shape.Y = 100;
            shape.Width = 200;
            shape.Height = 100;
            shape.Text = "Test Shape";
            shape.TextX = 120;
            shape.TextY = 130;
            shape.DragPointX = 170;
            shape.DragPointY = 126;
            return shape;
        }

        [TestMethod]
        public void TestTextDragInitiation()
        {
            // Arrange
            var shape = CreateTestShape();
            _model.AddNewShape(shape);

            // Act
            _pointerState.HandlePointerPressed(170, 126);
            _pointerState.Draw(_mockGraphics);

            // Assert
            // Check if text drag is initiated when clicking on drag point
            var isDraggingText = (bool)_privatePointerState.GetField("_isDraggingText");
            Assert.IsTrue(isDraggingText);
            Assert.IsTrue(_mockGraphics.PenSet);
            Assert.IsTrue(_mockGraphics.ColorSet);
        }

        [TestMethod]
        public void TestTextDragMovement()
        {
            // Arrange
            var shape = CreateTestShape();
            _model.AddNewShape(shape);

            _pointerState.HandlePointerPressed(170, 126);

            _pointerState.HandlePointerMoved(200, 150);

            int expectedTextX = 120 + (200 - 170);
            int expectedTextY = 130 + (150 - 126);
            int expectedDragPointX = expectedTextX + shape.Text.Length * 5;
            int expectedDragPointY = expectedTextY - 4;

            // Assert
            Assert.AreEqual(expectedTextX, shape.TextX);
            Assert.AreEqual(expectedTextY, shape.TextY);
            Assert.AreEqual(expectedDragPointX, shape.DragPointX);
            Assert.AreEqual(expectedDragPointY, shape.DragPointY);
        }

        [TestMethod]
        public void TestTextDragRelease_InShape()
        {
            var shape = CreateTestShape();
            _model.AddNewShape(shape);

            _pointerState.HandlePointerPressed(170, 126);
            _pointerState.HandlePointerMoved(200, 150);
            _pointerState.HandlePointerReleased(200, 150);

            int expectedTextX = 120 + (200 - 170);
            int expectedTextY = 130 + (150 - 126);
            int expectedDragPointX = expectedTextX + shape.Text.Length * 5;
            int expectedDragPointY = expectedTextY - 4;

            Assert.AreEqual(expectedTextX, shape.TextX);
            Assert.AreEqual(expectedTextY, shape.TextY);
            Assert.AreEqual(expectedDragPointX, shape.DragPointX);
            Assert.AreEqual(expectedDragPointY, shape.DragPointY);
        }
        [TestMethod]
        public void TestTextDragRelease_OutsideShape_Right()
        {
            var shape = CreateTestShape();
            _model.AddNewShape(shape);

            _pointerState.HandlePointerPressed(170, 126);
            _pointerState.HandlePointerMoved(350, 150);
            _pointerState.HandlePointerReleased(350, 150);

            int expectedTextX = shape.X + shape.Width - 50;
            int expectedTextY = 130 + (150 - 126);
            int expectedDragPointX = expectedTextX + shape.Text.Length * 5;
            int expectedDragPointY = expectedTextY - 4;

            Assert.AreEqual(expectedTextX, shape.TextX);
            Assert.AreEqual(expectedTextY, shape.TextY);
            Assert.AreEqual(expectedDragPointX, shape.DragPointX);
            Assert.AreEqual(expectedDragPointY, shape.DragPointY);
        }
        [TestMethod]
        public void TestTextDragRelease_OutsideShape_Up()
        {
            var shape = CreateTestShape();
            _model.AddNewShape(shape);

            _pointerState.HandlePointerPressed(170, 126);
            _pointerState.HandlePointerMoved(200, 10);
            _pointerState.HandlePointerReleased(200, 10);

            int expectedTextX = 120 + (200 - 170);
            int expectedTextY = shape.Y + 10;
            int expectedDragPointX = expectedTextX + shape.Text.Length * 5;
            int expectedDragPointY = expectedTextY - 4;

            Assert.AreEqual(expectedTextX, shape.TextX);
            Assert.AreEqual(expectedTextY, shape.TextY);
            Assert.AreEqual(expectedDragPointX, shape.DragPointX);
            Assert.AreEqual(expectedDragPointY, shape.DragPointY);
        }
        [TestMethod]
        public void TestTextDragRelease_OutsideShape_Left()
        {
            var shape = CreateTestShape();
            _model.AddNewShape(shape);

            _pointerState.HandlePointerPressed(170, 126);
            _pointerState.HandlePointerMoved(50, 150);
            _pointerState.HandlePointerReleased(50, 150);

            int expectedTextX = shape.X + 10;
            int expectedTextY = 130 + (150 - 126);
            int expectedDragPointX = expectedTextX + shape.Text.Length * 5;
            int expectedDragPointY = expectedTextY - 4;

            Assert.AreEqual(expectedTextX, shape.TextX);
            Assert.AreEqual(expectedTextY, shape.TextY);
            Assert.AreEqual(expectedDragPointX, shape.DragPointX);
            Assert.AreEqual(expectedDragPointY, shape.DragPointY);
        }
        [TestMethod]
        public void TestTextDragRelease_OutsideShape_Down()
        {
            var shape = CreateTestShape();
            _model.AddNewShape(shape);

            _pointerState.HandlePointerPressed(170, 126);
            _pointerState.HandlePointerMoved(200, 350);
            _pointerState.HandlePointerReleased(200, 350);

            int expectedTextX = 120 + (200 - 170);
            int expectedTextY = shape.Y + shape.Height - 10;
            int expectedDragPointX = expectedTextX + shape.Text.Length * 5;
            int expectedDragPointY = expectedTextY - 4;

            Assert.AreEqual(expectedTextX, shape.TextX);
            Assert.AreEqual(expectedTextY, shape.TextY);
            Assert.AreEqual(expectedDragPointX, shape.DragPointX);
            Assert.AreEqual(expectedDragPointY, shape.DragPointY);
        }

        [TestMethod]
        public void TestTextClikOutsideShape()
        {
            // Arrange
            var shape = CreateTestShape();
            _model.AddNewShape(shape);

            // Act
            _pointerState.HandlePointerPressed(50, 50);

            // Assert
            var isDraggingText = (bool)_privatePointerState.GetField("_isDraggingText");
            Assert.IsFalse(isDraggingText, "Text dragging should not initiate outside drag point");
        }
    }

    [TestClass]
    public class DrawingStateTests
    {
        private ShapeModel _model;
        private DrawingState _drawingState;
        private MockGraphics _mockGraphics;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _drawingState = new DrawingState(_model, "Process");
            _mockGraphics = new MockGraphics();
        }


        [TestMethod]
        public void TestHandlePointerPressedAndReleased()
        {
            // Act
            _drawingState.HandlePointerPressed(10, 10);
            _drawingState.HandlePointerMoved(110, 110);
            _drawingState.HandlePointerReleased(110, 110);

            // Assert
            Assert.AreEqual(1, _model.GetShapes().Count);
            var shape = _model.GetShapes()[0];
            Assert.AreEqual(100, shape.Width);
            Assert.AreEqual(100, shape.Height);
        }

        [TestMethod]
        public void TestDrawPreview()
        {
            // Act
            _drawingState.HandlePointerPressed(10, 10);
            _drawingState.HandlePointerMoved(110, 110);
            _drawingState.Draw(_mockGraphics);

            // Assert
            Assert.IsTrue(_mockGraphics.RectangleDrawn);
        }

        [TestMethod]
        public void TestTerminatorMinimumSize()
        {
            // Arrange
            _drawingState = new DrawingState(_model, "Terminator");

            // Act
            _drawingState.HandlePointerPressed(10, 10);
            _drawingState.HandlePointerMoved(20, 20);
            _drawingState.HandlePointerReleased(20, 20);

            // Assert
            var shape = _model.GetShapes()[0];
            Assert.IsTrue(shape.Height >= 30);
            Assert.IsTrue(shape.Width >= shape.Height * 3 / 2);
        }
    }

    [TestClass]
    public class DrawingStatePrivateMethodTests
    {
        private ShapeModel _model;
        private PrivateObject _drawingState;
        private Shape _testShape;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _drawingState = new PrivateObject(new DrawingState(_model, "Process"));
            _testShape = ShapeFactory.CreateShape("Process");
            _testShape.X = 50;
            _testShape.Y = 50;
            _testShape.Width = 0;
            _testShape.Height = 0;
        }

        [TestMethod]
        public void TestUpdateShapeSize_DrawingRightAndDown()
        {
            var firstPoint = new Coordinate(50, 50);
            _drawingState.SetFieldOrProperty("_firstPoint", firstPoint);

            _drawingState.Invoke("UpdateShapeSize", _testShape, 100.0, 100.0);

            Assert.AreEqual(50, _testShape.Width);
            Assert.AreEqual(50, _testShape.Height);
            Assert.AreEqual(50, _testShape.X);
            Assert.AreEqual(50, _testShape.Y);
        }

        [TestMethod]
        public void TestUpdateShapeSize_DrawingLeftAndUp()
        {
            var firstPoint = new Coordinate(100, 100);
            _drawingState.SetFieldOrProperty("_firstPoint", firstPoint);

            _drawingState.Invoke("UpdateShapeSize", _testShape, 50.0, 50.0);

            Assert.AreEqual(50, _testShape.Width);
            Assert.AreEqual(50, _testShape.Height);
            Assert.AreEqual(50, _testShape.X);
            Assert.AreEqual(50, _testShape.Y);
        }

        [TestMethod]
        public void TestUpdateShapeSize_DrawingLeftAndDown()
        {
            var firstPoint = new Coordinate(100, 50);
            _drawingState.SetFieldOrProperty("_firstPoint", firstPoint);

            _drawingState.Invoke("UpdateShapeSize", _testShape, 50.0, 100.0);

            Assert.AreEqual(50, _testShape.Width);
            Assert.AreEqual(50, _testShape.Height);
            Assert.AreEqual(50, _testShape.X);
            Assert.AreEqual(50, _testShape.Y);
        }

        [TestMethod]
        public void TestUpdateShapeSize_DrawingRightAndUp()
        {
            var firstPoint = new Coordinate(50, 100);
            _drawingState.SetFieldOrProperty("_firstPoint", firstPoint);

            _drawingState.Invoke("UpdateShapeSize", _testShape, 100.0, 50.0);

            Assert.AreEqual(50, _testShape.Width);
            Assert.AreEqual(50, _testShape.Height);
            Assert.AreEqual(50, _testShape.X);
            Assert.AreEqual(50, _testShape.Y);
        }

        [TestMethod]
        public void TestUpdateTerminatorSize_BelowMinimumSize()
        {
            var firstPoint = new Coordinate(50, 50);
            _drawingState.SetFieldOrProperty("_firstPoint", firstPoint);
            var terminator = ShapeFactory.CreateShape("Terminator");
            terminator.X = 50;
            terminator.Y = 50;

            _drawingState.Invoke("UpdateTerminatorSize", terminator, 60.0, 60.0);

            Assert.AreEqual(30, terminator.Height);
            Assert.AreEqual(45, terminator.Width);  // 1.5 * height
            Assert.AreEqual(50, terminator.X);
            Assert.AreEqual(50, terminator.Y);
        }

        [TestMethod]
        public void TestUpdateTerminatorSize_AboveMinimumSize()
        {
            var firstPoint = new Coordinate(50, 50);
            _drawingState.SetFieldOrProperty("_firstPoint", firstPoint);
            var terminator = ShapeFactory.CreateShape("Terminator");
            terminator.X = 50;
            terminator.Y = 50;

            _drawingState.Invoke("UpdateTerminatorSize", terminator, 200.0, 100.0);

            Assert.AreEqual(50, terminator.Height);
            Assert.AreEqual(150, terminator.Width);
            Assert.AreEqual(50, terminator.X);
            Assert.AreEqual(50, terminator.Y);
        }

        [TestMethod]
        public void TestUpdateTerminatorSize_DrawingLeftAndUp()
        {
            var firstPoint = new Coordinate(200, 200);
            _drawingState.SetFieldOrProperty("_firstPoint", firstPoint);
            var terminator = ShapeFactory.CreateShape("Terminator");
            terminator.X = 200;
            terminator.Y = 200;

            _drawingState.Invoke("UpdateTerminatorSize", terminator, 100.0, 100.0);

            Assert.AreEqual(100, terminator.Height);
            Assert.AreEqual(150, terminator.Width); // 1.5 * height
            Assert.AreEqual(100, terminator.X); // smaller X
            Assert.AreEqual(100, terminator.Y); // smaller Y
        }
    }

    [TestClass]
    public class UndoRedoTests
    {
        private ShapeModel _model;
        private MockGraphics _mockGraphics;
        private Shape _testShape;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _mockGraphics = new MockGraphics();
            _testShape = ShapeFactory.CreateShape("Process");
            _testShape.ID = _model.GetNextId();
            _testShape.X = 100;
            _testShape.Y = 100;
            _testShape.Width = 50;
            _testShape.Height = 50;
            _testShape.Text = "Test";
            _model.AddNewShape(_testShape);
        }

        // Text Editing Tests
        [TestMethod]
        public void TestUpdateSelectedShapeText()
        {
            // Arrange
            _model.SetState(new PointerState(_model));
            _model.PointerPressed(_testShape.X + 10, _testShape.Y + 10);
            string newText = "Updated Text";

            // Act
            _model.UpdateSelectedShapeText(_testShape, newText);

            // Assert
            Assert.AreEqual(newText, _testShape.Text);
        }

        [TestMethod]
        public void TestIsPointOnDragPoint()
        {
            // Arrange
            _model.SetState(new PointerState(_model));
            _model.PointerPressed(_testShape.X + 10, _testShape.Y + 10);

            // Act & Assert
            bool isOnDragPoint = _model.IsPointOnDragPoint(_testShape.DragPointX, _testShape.DragPointY);
            Assert.IsTrue(isOnDragPoint);

            bool isNotOnDragPoint = _model.IsPointOnDragPoint(_testShape.DragPointX + 20, _testShape.DragPointY + 20);
            Assert.IsFalse(isNotOnDragPoint);
        }

        // Command Pattern Tests

        [TestMethod]
        public void TestUndoRedoMove()
        {
            // Arrange
            int originalX = _testShape.X;
            int originalY = _testShape.Y;
            List<int> originalPos = new List<int> { originalX, originalY, _testShape.TextX, _testShape.TextY };
            _testShape.X = originalX + 50;
            _testShape.Y = originalY + 50;
            var command = new MoveCommand(_testShape, originalPos, _model.GetShapes());

            // Act & Assert
            command.Execute();
            Assert.AreEqual(originalX + 50, _testShape.X);
            Assert.AreEqual(originalY + 50, _testShape.Y);

            command.UnExecute();
            Assert.AreEqual(originalX, _testShape.X);
            Assert.AreEqual(originalY, _testShape.Y);
        }

        [TestMethod]
        public void TestCommandManagerUndoRedo()
        {
            // Arrange
            string originalText = _testShape.Text;
            string newText = "New Text";

            // Act
            _model.UpdateSelectedShapeText(_testShape, newText);
            string afterChange = _testShape.Text;

            _model.Undo();
            _model.Redo();
            string afterRedo = _testShape.Text;

            // Assert
            Assert.AreEqual(newText, afterChange);
            Assert.AreEqual(newText, afterRedo);
        }
        [TestMethod]
        public void TestUndoRedoAdd()
        {
            // Arrange
            var newShape = ShapeFactory.CreateShape("Process");
            newShape.ID = _model.GetNextId();
            int initialCount = _model.GetShapes().Count;
            var command = new AddCommand(_model, newShape);

            // Act & Assert
            command.Execute();
            Assert.AreEqual(initialCount + 1, _model.GetShapes().Count);
            Assert.IsTrue(_model.GetShapes().Contains(newShape));

            command.UnExecute();
            Assert.AreEqual(initialCount, _model.GetShapes().Count);
            Assert.IsFalse(_model.GetShapes().Contains(newShape));
        }

        [TestMethod]
        public void TestUndoRedoDelete()
        {
            // Arrange
            int initialCount = _model.GetShapes().Count;
            var command = new DeleteCommand(_model, _testShape.ID);

            // Act & Assert
            command.Execute();
            Assert.AreEqual(initialCount - 1, _model.GetShapes().Count);
            Assert.IsFalse(_model.GetShapes().Contains(_testShape));

            command.UnExecute();
            Assert.AreEqual(initialCount, _model.GetShapes().Count);
            Assert.IsTrue(_model.GetShapes().Contains(_testShape));
        }

        [TestMethod]
        public void TestUndoRedoMultipleOperations()
        {
            // Arrange
            int initialCount = _model.GetShapes().Count;
            var shape1 = ShapeFactory.CreateShape("Process");
            var shape2 = ShapeFactory.CreateShape("Decision");
            shape1.ID = _model.GetNextId();
            shape2.ID = _model.GetNextId();

            // Act - Add two shapes
            _model.ExecuteCommand(new AddCommand(_model, shape1));
            _model.ExecuteCommand(new AddCommand(_model, shape2));
            Assert.AreEqual(initialCount + 2, _model.GetShapes().Count);

            // Undo both additions
            _model.Undo();
            _model.Undo();
            Assert.AreEqual(initialCount, _model.GetShapes().Count);

            // Redo one addition
            _model.Redo();
            Assert.AreEqual(initialCount + 1, _model.GetShapes().Count);
        }

        [TestMethod]
        public void TestRedoClearedAfterNewOperation()
        {
            // Arrange
            var firstShape = ShapeFactory.CreateShape("Process");
            firstShape.ID = _model.GetNextId();
            _model.ExecuteCommand(new AddCommand(_model, firstShape));
            _model.Undo();

            // Act
            var secondShape = ShapeFactory.CreateShape("Decision");
            secondShape.ID = _model.GetNextId();
            _model.ExecuteCommand(new AddCommand(_model, secondShape));

            // Try to redo the first operation
            _model.Redo();

            // Assert
            Assert.IsFalse(_model.GetShapes().Contains(firstShape));
            Assert.IsTrue(_model.GetShapes().Contains(secondShape));
        }

        [TestMethod]
        public void TestUndoRedoTextMove()
        {
            // Arrange
            List<int> originalPos = new List<int> { _testShape.X, _testShape.Y, _testShape.TextX, _testShape.TextY };
            _testShape.TextX += 50;
            _testShape.TextY += 50;
            var command = new TextMoveCommand(_testShape, originalPos);

            // Act & Assert
            int newTextX = _testShape.TextX;
            int newTextY = _testShape.TextY;

            command.UnExecute();
            Assert.AreEqual(originalPos[2], _testShape.TextX);
            Assert.AreEqual(originalPos[3], _testShape.TextY);

            command.Execute();
            Assert.AreEqual(newTextX, _testShape.TextX);
            Assert.AreEqual(newTextY, _testShape.TextY);
        }


    }

    [TestClass]
    public class CommandTests
    {
        private ShapeModel _model;
        private MockGraphics _mockGraphics;
        private Shape _testShape;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _mockGraphics = new MockGraphics();
            _testShape = ShapeFactory.CreateShape("Process");
            _testShape.ID = _model.GetNextId();
            _testShape.X = 100;
            _testShape.Y = 100;
            _testShape.Width = 50;
            _testShape.Height = 50;
            _testShape.Text = "Test";
            _model.AddNewShape(_testShape);
        }

        [TestMethod]
        public void TestDrawCommand_Execute()
        {
            // Arrange
            var shape = ShapeFactory.CreateShape("Process");
            shape.ID = _model.GetNextId();
            var command = new DrawCommand(_model, shape);
            int initialCount = _model.GetShapes().Count;

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(initialCount + 1, _model.GetShapes().Count);
            Assert.IsTrue(_model.GetShapes().Contains(shape));
        }

        [TestMethod]
        public void TestDrawCommand_UnExecute()
        {
            // Arrange
            var shape = ShapeFactory.CreateShape("Process");
            shape.ID = _model.GetNextId();
            var command = new DrawCommand(_model, shape);
            command.Execute();
            int countAfterExecute = _model.GetShapes().Count;

            // Act
            command.UnExecute();

            // Assert
            Assert.AreEqual(countAfterExecute - 1, _model.GetShapes().Count);
            Assert.IsFalse(_model.GetShapes().Contains(shape));
        }

        [TestMethod]
        public void TestDeleteCommand_Execute()
        {
            // Arrange
            var command = new DeleteCommand(_model, _testShape.ID);
            int initialCount = _model.GetShapes().Count;

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(initialCount - 1, _model.GetShapes().Count);
            Assert.IsFalse(_model.GetShapes().Contains(_testShape));
        }

        [TestMethod]
        public void TestDeleteCommand_UnExecute()
        {
            // Arrange
            var command = new DeleteCommand(_model, _testShape.ID);
            command.Execute();
            int countAfterDelete = _model.GetShapes().Count;

            // Act
            command.UnExecute();

            // Assert
            Assert.AreEqual(countAfterDelete + 1, _model.GetShapes().Count);
            Assert.IsTrue(_model.GetShapes().Any(s => s.ID == _testShape.ID));
        }

        [TestMethod]
        public void TestTextMoveCommand_Execute()
        {
            // Arrange
            int originalTextX = _testShape.TextX;
            int originalTextY = _testShape.TextY;
            _testShape.TextX = originalTextX + 50;
            _testShape.TextY = originalTextY + 50;
            List<int> originalPos = new List<int> { _testShape.X, _testShape.Y, originalTextX, originalTextY };
            var command = new TextMoveCommand(_testShape, originalPos);

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(originalTextX + 50, _testShape.TextX);
            Assert.AreEqual(originalTextY + 50, _testShape.TextY);
            Assert.AreEqual(_testShape.TextX + _testShape.Text.Length * 5, _testShape.DragPointX);
            Assert.AreEqual(_testShape.TextY - 4, _testShape.DragPointY);
        }

        [TestMethod]
        public void TestTextMoveCommand_UnExecute()
        {
            // Arrange
            int originalTextX = _testShape.TextX;
            int originalTextY = _testShape.TextY;
            _testShape.TextX = originalTextX + 50;
            _testShape.TextY = originalTextY + 50;
            List<int> originalPos = new List<int> { _testShape.X, _testShape.Y, originalTextX, originalTextY };
            var command = new TextMoveCommand(_testShape, originalPos);
            command.Execute();

            // Act
            command.UnExecute();

            // Assert
            Assert.AreEqual(originalTextX, _testShape.TextX);
            Assert.AreEqual(originalTextY, _testShape.TextY);
            Assert.AreEqual(_testShape.TextX + _testShape.Text.Length * 5, _testShape.DragPointX);
            Assert.AreEqual(_testShape.TextY - 4, _testShape.DragPointY);
        }

        [TestMethod]
        public void TestAddCommand_Execute()
        {
            // Arrange
            var shape = ShapeFactory.CreateShape("Decision");
            shape.ID = _model.GetNextId();
            var command = new AddCommand(_model, shape);
            int initialCount = _model.GetShapes().Count;

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(initialCount + 1, _model.GetShapes().Count);
            Assert.IsTrue(_model.GetShapes().Contains(shape));
        }

        [TestMethod]
        public void TestAddCommand_UnExecute()
        {
            // Arrange
            var shape = ShapeFactory.CreateShape("Decision");
            shape.ID = _model.GetNextId();
            var command = new AddCommand(_model, shape);
            command.Execute();
            int countAfterAdd = _model.GetShapes().Count;

            // Act
            command.UnExecute();

            // Assert
            Assert.AreEqual(countAfterAdd - 1, _model.GetShapes().Count);
            Assert.IsFalse(_model.GetShapes().Contains(shape));
        }


        [TestMethod]
        public void TestCommandManager_MultipleUndoRedo()
        {
            // Arrange
            var shape1 = ShapeFactory.CreateShape("Process");
            var shape2 = ShapeFactory.CreateShape("Decision");
            var shape3 = ShapeFactory.CreateShape("Terminator");

            // Act - Execute multiple commands
            _model.ExecuteCommand(new AddCommand(_model, shape1));
            _model.ExecuteCommand(new AddCommand(_model, shape2));
            _model.ExecuteCommand(new AddCommand(_model, shape3));

            int countAfterAdd = _model.GetShapes().Count;

            // Undo all
            _model.Undo();
            _model.Undo();
            _model.Undo();

            int countAfterUndo = _model.GetShapes().Count;

            // Redo all
            _model.Redo();
            _model.Redo();
            _model.Redo();

            // Assert
            Assert.AreEqual(4, countAfterAdd); // Including _testShape
            Assert.AreEqual(1, countAfterUndo); // Only _testShape remains
            Assert.AreEqual(4, _model.GetShapes().Count); // Back to all shapes
        }
    }

    [TestClass]
    public class LineDrawingTests
    {
        private ShapeModel _model;
        private MockGraphics _mockGraphics;
        private Shape _startShape;
        private Shape _endShape;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _mockGraphics = new MockGraphics();

            // Create two shapes for line connection
            _startShape = ShapeFactory.CreateShape("Process");
            _startShape.ID = _model.GetNextId();
            _startShape.X = 100;
            _startShape.Y = 100;
            _startShape.Width = 100;
            _startShape.Height = 50;
            _model.AddNewShape(_startShape);

            _endShape = ShapeFactory.CreateShape("Process");
            _endShape.ID = _model.GetNextId();
            _endShape.X = 300;
            _endShape.Y = 300;
            _endShape.Width = 100;
            _endShape.Height = 50;
            _model.AddNewShape(_endShape);
        }

        [TestMethod]
        public void TestDrawLine_BetweenTwoShapes()
        {
            // Arrange
            var drawLineState = new DrawLineState(_model);
            int initialShapeCount = _model.GetShapes().Count;

            // Act - Simulate drawing line from start shape to end shape
            // Click on start shape's right connect point
            drawLineState.HandlePointerPressed(_startShape.X + _startShape.Width, _startShape.Y + _startShape.Height / 2);
            // Move to end shape's left connect point
            drawLineState.HandlePointerMoved(_endShape.X, _endShape.Y + _endShape.Height / 2);
            // Release on end shape
            drawLineState.HandlePointerReleased(_endShape.X, _endShape.Y + _endShape.Height / 2);

            // Assert
            Assert.AreEqual(initialShapeCount + 1, _model.GetShapes().Count);
            var addedLine = _model.GetShapes().Last() as Line;
            Assert.IsNotNull(addedLine);
            Assert.AreEqual(_startShape, addedLine.StartPoint.Shape);
            Assert.AreEqual(_endShape, addedLine.EndPoint.Shape);
        }

        [TestMethod]
        public void TestDrawLine_SameShape()
        {
            // Arrange
            var drawLineState = new DrawLineState(_model);
            int initialShapeCount = _model.GetShapes().Count;

            // Act - Try to connect shape to itself
            drawLineState.HandlePointerPressed(_startShape.X + _startShape.Width, _startShape.Y + _startShape.Height / 2);
            drawLineState.HandlePointerMoved(_startShape.X, _startShape.Y + _startShape.Height / 2);
            drawLineState.HandlePointerReleased(_startShape.X, _startShape.Y + _startShape.Height / 2);

            // Assert
            Assert.AreEqual(initialShapeCount + 1, _model.GetShapes().Count);
        }

        [TestMethod]
        public void TestDrawLine_DifferentConnectPoints()
        {
            // Arrange
            var drawLineState = new DrawLineState(_model);

            // Test connecting different points
            var testCases = new[]
            {
            // Top to Bottom
            (startX: _startShape.X + _startShape.Width/2, startY: _startShape.Y,
             endX: _endShape.X + _endShape.Width/2, endY: _endShape.Y + _endShape.Height),
            
            // Left to Right
            (startX: _startShape.X, startY: _startShape.Y + _startShape.Height/2,
             endX: _endShape.X + _endShape.Width, endY: _endShape.Y + _endShape.Height/2),
            
            // Bottom to Top
            (startX: _startShape.X + _startShape.Width/2, startY: _startShape.Y + _startShape.Height,
             endX: _endShape.X + _endShape.Width/2, endY: _endShape.Y)
        };

            foreach (var testCase in testCases)
            {
                // Act
                drawLineState.HandlePointerPressed(testCase.startX, testCase.startY);
                drawLineState.HandlePointerReleased(testCase.endX, testCase.endY);

                // Assert
                var line = _model.GetShapes().Last() as Line;
                Assert.IsNotNull(line);
                Assert.AreEqual(_startShape, line.StartPoint.Shape);
                Assert.AreEqual(_endShape, line.EndPoint.Shape);
            }
        }

        [TestMethod]
        public void TestUndoRedoLine_ShouldWorkCorrectly()
        {
            // Arrange
            var drawLineState = new DrawLineState(_model);
            int initialCount = _model.GetShapes().Count;

            // Act 1 - Draw line
            drawLineState.HandlePointerPressed(_startShape.X + _startShape.Width, _startShape.Y + _startShape.Height / 2);
            drawLineState.HandlePointerReleased(_endShape.X, _endShape.Y + _endShape.Height / 2);
            int countAfterDraw = _model.GetShapes().Count;

            // Act 2 - Undo
            _model.Undo();
            int countAfterUndo = _model.GetShapes().Count;

            // Act 3 - Redo
            _model.Redo();
            int countAfterRedo = _model.GetShapes().Count;

            // Assert
            Assert.AreEqual(initialCount + 1, countAfterDraw); // Line added
            Assert.AreEqual(initialCount, countAfterUndo);     // Line removed
            Assert.AreEqual(initialCount + 1, countAfterRedo); // Line restored
        }

        // Line Drawing Tests
        [TestMethod]
        public void TestDrawLineState_HandlePointerPressed()
        {
            // Arrange
            var drawLineState = new DrawLineState(_model);
            Shape startShape = ShapeFactory.CreateShape("Process");
            startShape.X = 50;
            startShape.Y = 50;
            startShape.Width = 100;
            startShape.Height = 100;
            _model.AddNewShape(startShape);

            // Act
            drawLineState.HandlePointerPressed(100, 100);
            drawLineState.HandlePointerMoved(200, 200);
            drawLineState.HandlePointerReleased(200, 200);

            // Assert
            Assert.IsFalse(_mockGraphics.LineDrawn);
        }

        [TestMethod]
        public void TestDrawLineState_NoLineCreatedWhenSamePoint()
        {
            // Arrange
            var drawLineState = new DrawLineState(_model);
            var initialShapeCount = _model.GetShapes().Count;

            // Act
            drawLineState.HandlePointerPressed(100, 100);
            drawLineState.HandlePointerReleased(100, 100);

            // Assert
            Assert.AreEqual(initialShapeCount, _model.GetShapes().Count);
        }
    }

    [TestClass]
    public class SaveLoadTests
    {
        private ShapeModel _model;
        private MockGraphics _mockGraphics;
        private string _testPath;
        private string _backupFolder;

        [TestInitialize]
        public void Initialize()
        {
            _model = new ShapeModel();
            _mockGraphics = new MockGraphics();
            _testPath = "test.mydrawing";
            _backupFolder = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "drawing_backup"
            );

            if (Directory.Exists(_backupFolder))
                Directory.Delete(_backupFolder, true);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testPath))
                File.Delete(_testPath);
            if (Directory.Exists(_backupFolder))
                Directory.Delete(_backupFolder, true);
        }

        [TestMethod]
        public async Task TestSaveLoadShape()
        {
            // Arrange
            bool modelChangedCalled = false;
            _model.ShapeChanged += () => modelChangedCalled = true;

            var shape = ShapeFactory.CreateShape("Process");
            shape.ID = _model.GetNextId();
            shape.Text = "Test";
            shape.X = 100;
            shape.Y = 100;
            _model.AddNewShape(shape);

            // Act
            await _model.SaveToFileAsync(_testPath);
            _model.GetShapes().Clear();
            _model.LoadFromFile(_testPath);

            // Assert
            Assert.IsTrue(modelChangedCalled, "Model應該觸發變更事件");
            var shapes = _model.GetShapes();
            Assert.AreEqual(1, shapes.Count, "應該只有一個形狀");
            Assert.AreEqual("Test", shapes[0].Text, "文字應該相同");
            Assert.AreEqual(100, shapes[0].X, "X座標應該相同");
            Assert.AreEqual(100, shapes[0].Y, "Y座標應該相同");
        }

        [TestMethod]
        public async Task TestSaveLoadLine()
        {
            // Arrange
            var startShape = ShapeFactory.CreateShape("Start");
            startShape.ID = _model.GetNextId();
            var endShape = ShapeFactory.CreateShape("Process");
            endShape.ID = _model.GetNextId();
            _model.AddNewShape(startShape);
            _model.AddNewShape(endShape);

            var line = new Line();
            line.ID = _model.GetNextId();
            line.StartPoint = new ConnectPoint(100, 100, startShape);
            line.EndPoint = new ConnectPoint(200, 200, endShape);
            _model.AddNewShape(line);

            // Act
            await _model.SaveToFileAsync(_testPath);
            _model.GetShapes().Clear();
            _model.LoadFromFile(_testPath);

            // Assert
            var shapes = _model.GetShapes();
            Assert.AreEqual(3, shapes.Count, "應該有三個形狀");

            var loadedLine = shapes.FirstOrDefault(s => s is Line) as Line;
            Assert.IsNotNull(loadedLine, "應該包含一條線");
            Assert.AreEqual(100, loadedLine.StartPoint.X, "起點X座標應該相同");
            Assert.AreEqual(100, loadedLine.StartPoint.Y, "起點Y座標應該相同");
            Assert.AreEqual(200, loadedLine.EndPoint.X, "終點X座標應該相同");
            Assert.AreEqual(200, loadedLine.EndPoint.Y, "終點Y座標應該相同");
        }

        [TestMethod]
        public void TestAutoSaveInitialization()
        {
            // Arrange
            bool autoSaveStartedCalled = false;
            bool autoSaveCompletedCalled = false;
            _model.AutoSaveStarted += (s, e) => autoSaveStartedCalled = true;
            _model.AutoSaveCompleted += (s, e) => autoSaveCompletedCalled = true;

            // Act
            _model.InitializeAutoSave();
            var shape = ShapeFactory.CreateShape("Process");
            shape.ID = _model.GetNextId();
            _model.AddNewShape(shape);

            // Assert
            Assert.IsFalse(autoSaveStartedCalled, "自動存檔還未開始");
            Assert.IsFalse(autoSaveCompletedCalled, "自動存檔還未完成");
        }

        [TestMethod]
        public void TestAutoSaveBackupLimit()
        {
            // Arrange
            _model.InitializeAutoSave();
            if (!Directory.Exists(_backupFolder))
                Directory.CreateDirectory(_backupFolder);

            // 創建6個測試備份檔案
            for (int i = 1; i <= 6; i++)
            {
                string backupPath = Path.Combine(_backupFolder, $"test{i}_bak.mydrawing");
                File.WriteAllText(backupPath, $"test{i}");
            }

            // Act
            _model.CleanupBackupFiles(_backupFolder);
            var backupFiles = Directory.GetFiles(_backupFolder, "*_bak.mydrawing");

            // Assert
            Assert.AreEqual(5, backupFiles.Length, "備份檔案數量應該是5個");
        }

        [TestMethod]
        public void TestLoadInvalidFile()
        {
            // Arrange
            string invalidPath = "invalid.mydrawing";

            // Act & Assert
            Assert.ThrowsException<Exception>(() => _model.LoadFromFile(invalidPath));
        }

        [TestMethod]
        public async Task TestSaveToInvalidPath()
        {
            // Arrange
            string invalidPath = Path.Combine("Z:", "nonexistent", "test.mydrawing");

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _model.SaveToFileAsync(invalidPath));
        }

        [TestMethod]
        public async Task TestSaveWithEmptyModel()
        {
            // Arrange
            _model.GetShapes().Clear();

            // Act
            await _model.SaveToFileAsync(_testPath);

            // Assert
            Assert.IsTrue(File.Exists(_testPath), "即使沒有形狀也應該創建檔案");
            _model.LoadFromFile(_testPath);
            Assert.AreEqual(0, _model.GetShapes().Count, "讀取空檔案應該不會有任何形狀");
        }

        [TestMethod]
        public void TestLoadCorruptedFile()
        {
            // Arrange
            File.WriteAllText(_testPath, "This is not a valid file content");

            // Act & Assert
            Assert.ThrowsException<Exception>(() => _model.LoadFromFile(_testPath),
                "載入損壞的檔案應該拋出異常");
        }

        [TestMethod]
        public async Task TestMultipleShapesWithLines()
        {
            // Arrange
            var start = new Start { ID = 1 };
            var process = new Processes { ID = 2 };
            var decision = new Decision { ID = 3 };

            var line1 = new Line
            {
                ID = 4,
                StartPoint = new ConnectPoint(0, 0, start),
                EndPoint = new ConnectPoint(100, 100, process)
            };

            var line2 = new Line
            {
                ID = 5,
                StartPoint = new ConnectPoint(100, 100, process),
                EndPoint = new ConnectPoint(200, 200, decision)
            };

            _model.AddNewShape(start);
            _model.AddNewShape(process);
            _model.AddNewShape(decision);
            _model.AddNewShape(line1);
            _model.AddNewShape(line2);

            // Act
            await _model.SaveToFileAsync(_testPath);
            _model.GetShapes().Clear();
            _model.LoadFromFile(_testPath);

            // Assert
            var shapes = _model.GetShapes();
            Assert.AreEqual(5, shapes.Count, "應該有5個形狀（3個基本形狀和2條線）");

            var lines = shapes.OfType<Line>().ToList();
            Assert.AreEqual(2, lines.Count, "應該有2條線");

            var line1Loaded = lines.First();
            Assert.AreEqual(start.ID, line1Loaded.StartPoint.Shape.ID, "第一條線的起點應該連接到start");
            Assert.AreEqual(process.ID, line1Loaded.EndPoint.Shape.ID, "第一條線的終點應該連接到process");
        }

        [TestMethod]
        public void TestCleanupBackupFilesWithEmptyFolder()
        {
            // Arrange
            if (!Directory.Exists(_backupFolder))
                Directory.CreateDirectory(_backupFolder);

            // Act
            _model.CleanupBackupFiles(_backupFolder);
            var backupFiles = Directory.GetFiles(_backupFolder, "*_bak.mydrawing");

            // Assert
            Assert.AreEqual(0, backupFiles.Length, "空資料夾不應該有任何備份檔案");
        }

        [TestMethod]
        public void TestCleanupBackupFilesWithLessThan5Files()
        {
            // Arrange
            if (!Directory.Exists(_backupFolder))
                Directory.CreateDirectory(_backupFolder);

            // 只創建3個測試備份檔案
            for (int i = 1; i <= 3; i++)
            {
                string backupPath = Path.Combine(_backupFolder, $"test{i}_bak.mydrawing");
                File.WriteAllText(backupPath, $"test{i}");
            }

            // Act
            _model.CleanupBackupFiles(_backupFolder);
            var backupFiles = Directory.GetFiles(_backupFolder, "*_bak.mydrawing");

            // Assert
            Assert.AreEqual(3, backupFiles.Length, "當檔案少於5個時，不應該刪除任何檔案");
        }

        [TestMethod]
        public async Task TestLoadWithLineButMissingShape()
        {
            // Arrange
            var start = new Start { ID = 1 };
            var process = new Processes { ID = 2 };
            var line = new Line
            {
                ID = 3,
                StartPoint = new ConnectPoint(0, 0, start),
                EndPoint = new ConnectPoint(100, 100, process)
            };

            _model.AddNewShape(start);
            _model.AddNewShape(line);  // 故意不加入 process

            // Act
            await _model.SaveToFileAsync(_testPath);
            _model.GetShapes().Clear();

            // Assert
            // 讀取時應該會處理缺少連接形狀的情況
            Assert.ThrowsException<Exception>(() => _model.LoadFromFile(_testPath),
                "當Line的連接形狀不完整時，應該拋出異常");
        }
    }
}