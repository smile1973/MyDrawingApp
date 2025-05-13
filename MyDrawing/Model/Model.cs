using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MyDrawing.Model;
using MyDrawing.Model.command;
using MyDrawing.shapes;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyDrawing
{
    public class ShapeModel
    {
        private List<Shape> shapes;
        int idCounter = 1;
        IState currentState;
        public event ModelChangedEventHandler ShapeChanged;
        public delegate void ModelChangedEventHandler();
        private readonly CommandManager commandManager;
        private bool _isModified = false;

        [Serializable]
        public class ShapeData
        {
            public int ID { get; set; }
            public string ShapeType { get; set; }
            public string Text { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int TextX { get; set; }
            public int TextY { get; set; }
            public ConnectPointData StartPoint { get; set; }
            public ConnectPointData EndPoint { get; set; }
        }

        [Serializable]
        public class ConnectPointData
        {
            public double X { get; set; }
            public double Y { get; set; }
            public int ShapeID { get; set; }  // 儲存關聯形狀的ID而不是形狀本身
        }

        public ShapeModel()
        {
            shapes = new List<Shape>();
            currentState = new PointerState(this);
            commandManager = new CommandManager();
        }
        public void Undo()
        {
            commandManager.Undo();
            NotifyModelChanged();
        }
        public void Redo()
        {
            commandManager.Redo();
            NotifyModelChanged();
        }

        public void ExecuteCommand(ICommand command)
        {
            commandManager.Execute(command);
        }

        public void SetState(IState state)
        {
            if (state != null)  // 確保新狀態不為空
            {
                currentState = state;
                NotifyModelChanged();
            }
        }


        public void EnterDrawingState(string shapeType)
        {
            if (!string.IsNullOrEmpty(shapeType))  // 確保形狀類型有效
            {
                SetState(new DrawingState(this, shapeType));
            }
        }

        public void EnterPointerState()
        {
            var pointerState = new PointerState(this);
            SetState(pointerState);
        }

        public int GetNextId()
        {
            return idCounter++;
        }

        public void AddNewShape(Shape shape)
        {
            shapes.Add(shape);
            _isModified = true;
            NotifyModelChanged();
        }

        public Shape GetShapeAt(int x, int y)
        {
            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                Shape shape = shapes[i];
                if (x >= shape.X && x <= shape.X + shape.Width &&
                    y >= shape.Y && y <= shape.Y + shape.Height)
                {
                    return shape;
                }
            }
            return null;
        }

        public void PointerPressed(double x, double y)
        {
            currentState.HandlePointerPressed(x, y);
        }

        public void PointerMoved(double x, double y)
        {
            currentState.HandlePointerMoved(x, y);
        }

        public void PointerReleased(double x, double y)
        {
            currentState.HandlePointerReleased(x, y);
        }

        public void Draw(IGraphics graphics)
        {
            graphics.ClearAll();
            foreach (Shape shape in shapes)
            {
                shape.Draw(graphics);
            }
            currentState.Draw(graphics);
        }

        public void NotifyModelChanged()
        {
            ShapeChanged?.Invoke();
        }

        public Random random = new Random();
        public string GenerateRandomText()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int length = random.Next(3, 11); // 3-10
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void SetSelectedShapeType(string shapeType)
        {
            if (shapeType == "Pointer")
            {
                EnterPointerState();
            }
            else if (shapeType == "Line")
            {
                SetState(new DrawLineState(this));
            }
            else
            {
                EnterDrawingState(shapeType);
            }
        }

        public List<Shape> GetShapes()
        {
            return shapes;
        }

        public bool DeleteShape(int id)
        {
            var shape = shapes.FirstOrDefault(s => s.ID == id);
            if (shape != null)
            {
                if (currentState is PointerState pointerState)
                {
                    pointerState.HandleShapeDeleted(id);
                }
                shapes.Remove(shape);
                _isModified = true;
                NotifyModelChanged();
                return true;
            }
            return false;
        }

        public bool IsPointOnDragPoint(int x, int y)
        {
            var shape = GetSelectedShape();
            if (shape != null)
            {
                const int dragPointSize = 6;
                return x >= shape.DragPointX - dragPointSize &&
                       x <= shape.DragPointX + dragPointSize &&
                       y >= shape.DragPointY - dragPointSize &&
                       y <= shape.DragPointY + dragPointSize;
            }
            return false;
        }

        public Shape GetSelectedShape()
        {
            if (currentState is PointerState pointerState)
            {
                return pointerState.GetSelectedShape();
            }
            return null;
        }
        
        public void UpdateSelectedShapeText(Shape shape, string newText)
        {
            if (shape != null)
            {
                shape.Text = newText;
                shape.DragPointX = shape.TextX + shape.Text.Length * 5;
                shape.DragPointY = shape.TextY - 4;
                NotifyModelChanged();
            }
        }
        public void CleanupBackupFiles(string backupFolder)
        {
            var files = Directory.GetFiles(backupFolder, "*_bak.mydrawing")
                .OrderByDescending(f => f)
                .Skip(5);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
        public bool IsRedoEnabled
        {
            get
            {
                return commandManager.IsRedoEnabled;
            }
        }

        public bool IsUndoEnabled
        {
            get
            {
                return commandManager.IsUndoEnabled;
            }
        }

        

        public async Task SaveToFileAsync(string filePath, bool isAutoSave = false)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    var formatter = new BinaryFormatter();
                    var saveData = shapes.Select(shape =>
                    {
                        var shapeData = new ShapeData
                        {
                            ID = shape.ID,
                            ShapeType = shape.ShapeType,
                            Text = shape.Text,
                            X = shape.X,
                            Y = shape.Y,
                            Width = shape.Width,
                            Height = shape.Height,
                            TextX = shape.TextX,
                            TextY = shape.TextY
                        };

                        if (shape is Line line)
                        {
                            shapeData.StartPoint = new ConnectPointData
                            {
                                X = line.StartPoint.X,
                                Y = line.StartPoint.Y,
                                ShapeID = line.StartPoint.Shape.ID
                            };
                            shapeData.EndPoint = new ConnectPointData
                            {
                                X = line.EndPoint.X,
                                Y = line.EndPoint.Y,
                                ShapeID = line.EndPoint.Shape.ID
                            };
                        }

                        return shapeData;
                    }).ToList();

                    if (isAutoSave)
                    {
                        AutoSaveStarted?.Invoke(this, EventArgs.Empty);
                    }

                    await Task.Run(() =>
                    {
                        formatter.Serialize(fileStream, saveData);
                        Thread.Sleep(3000);
                    });

                    if (isAutoSave)
                    {
                        string backupFolder = Path.GetDirectoryName(filePath);
                        CleanupBackupFiles(backupFolder);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("儲存失敗：" + ex.Message);
            }
        }

        public void LoadFromFile(string filePath)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    var loadedData = (List<ShapeData>)formatter.Deserialize(fileStream);

                    shapes.Clear();
                    // 第一次遍歷：創建所有基本形狀
                    Dictionary<int, Shape> idToShape = new Dictionary<int, Shape>();
                    foreach (var data in loadedData)
                    {
                        if (data.ShapeType != "Line")
                        {
                            var shape = ShapeFactory.CreateShape(data.ShapeType);
                            if (shape != null)
                            {
                                shape.ID = data.ID;
                                shape.Text = data.Text;
                                shape.X = data.X;
                                shape.Y = data.Y;
                                shape.Width = data.Width;
                                shape.Height = data.Height;
                                shape.TextX = data.TextX;
                                shape.TextY = data.TextY;
                                shapes.Add(shape);
                                idToShape[shape.ID] = shape;
                            }
                        }
                    }

                    // 第二次遍歷：處理所有Line
                    foreach (var data in loadedData)
                    {
                        if (data.ShapeType == "Line")
                        {
                            var startShape = idToShape[data.StartPoint.ShapeID];
                            var endShape = idToShape[data.EndPoint.ShapeID];

                            var line = new Line
                            {
                                ID = data.ID,
                                StartPoint = new ConnectPoint(data.StartPoint.X, data.StartPoint.Y, startShape),
                                EndPoint = new ConnectPoint(data.EndPoint.X, data.EndPoint.Y, endShape)
                            };
                            shapes.Add(line);
                        }
                    }

                    NotifyModelChanged();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("讀取失敗：" + ex.Message);
            }
        }

        public event EventHandler AutoSaveStarted;
        public event EventHandler AutoSaveCompleted;
        private System.Timers.Timer autoSaveTimer;

        public void InitializeAutoSave()
        {
            autoSaveTimer = new System.Timers.Timer(30000); // 30秒
            autoSaveTimer.Elapsed += AutoSaveTimer_Elapsed;
            autoSaveTimer.Start();
        }

        private async void AutoSaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_isModified) return;

            string backupFolder = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "drawing_backup"
            );

            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string backupPath = Path.Combine(backupFolder, $"{timestamp}_bak.mydrawing");

            await SaveToFileAsync(backupPath, true);

            // 保持最多5個備份
            var files = Directory.GetFiles(backupFolder, "*_bak.mydrawing")
                .OrderByDescending(f => f)
                .Skip(5);
            foreach (var file in files)
            {
                File.Delete(file);
            }
            AutoSaveCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

}
