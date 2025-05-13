using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Threading;
using System.IO;
using System;
using System.Collections.Generic;

namespace MyDrawingGUITest
{
    [TestClass]
    public class UndoRedoTest
    {
        private Robot _robot;
        private const string CANVAS_NAME = "_canvas";
        private const string SHAPE_GRID = "shapeDataGrid";
        private string targetAppPath;
        private const string DRAWING_FORM = "Form1";

        [TestInitialize]
        public void Initialize()
        {
            var projectName = "MyDrawing";
            string solutionPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            targetAppPath = Path.Combine(solutionPath, projectName, "bin", "Debug", "MyDrawing.exe");
            _robot = new Robot(targetAppPath, DRAWING_FORM);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _robot.CleanUp();
        }

        // 測試圖形移動的Undo/Redo
        [TestMethod]
        public void TestUndoRedoShapeMove()
        {
            // 1. 先畫一個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);

            // 記錄原始位置
            string[] initialPosition = new string[] { "刪", "1", "Process", null, "100", "100", "100", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, initialPosition);

            _robot.MouseDown(120, 120);
            _robot.MouseMove(300, 300);
            _robot.MouseUp(300, 300);
            _robot.Sleep(1);

            // 確認移動後的位置
            string[] movedPosition = new string[] { "刪", "1", "Process", null, "280", "280", "100", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, movedPosition);

            // 3. 執行Undo
            _robot.ClickUndoButton();
            _robot.Sleep(1);
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, initialPosition);

            // 4. 執行Redo
            _robot.ClickRedoButton();
            _robot.Sleep(1);
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, movedPosition);

            _robot.AssertButtonEnabledName("toolStripbtn_undo", true);
            _robot.AssertButtonEnabledName("toolStripbtn_redo", false);
        }

        // 測試文字移動的Undo/Redo
        [TestMethod]
        public void TestUndoRedoTextMove()
        {
            // 1. 先畫一個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);

            // 2. 切換到 Pointer 模式
            _robot.ClickToolBarButton("toolStripbtn_pointer");
            _robot.Sleep(1);

            // 3. 取得實際的文字內容並計算橘色點位置
            string actualText = _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3);
            int dragPointX = 120 + (actualText.Length * 5);
            int dragPointY = 146;

            // 4. 拖曳文字
            _robot.MouseDown(dragPointX, dragPointY);
            _robot.MouseMove(dragPointX + 50, dragPointY);
            _robot.MouseUp(dragPointX + 50, dragPointY);
            _robot.Sleep(1);

            // 5. 執行Undo
            _robot.ClickUndoButton();
            _robot.Sleep(1);

            // 6. 執行Redo
            _robot.ClickRedoButton();
            _robot.Sleep(1);

            // 7. 檢查按鈕狀態
            _robot.AssertButtonEnabledName("toolStripbtn_undo", true);
            _robot.AssertButtonEnabledName("toolStripbtn_redo", false);
        }

        // 測試文字修改的Undo/Redo
        [TestMethod]
        public void TestUndoRedoTextEdit()
        {
            // 1. 先畫一個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);

            // 2. 取得原始文字
            string originalText = _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3);

            // 3. 切換到 Pointer 模式並點擊文字點
            _robot.ClickToolBarButton("toolStripbtn_pointer");
            int dragPointX = 120 + (originalText.Length * 5);
            int dragPointY = 146;
            _robot.DoubleClickPoint(dragPointX, dragPointY);
            _robot.Sleep(1);

            // 4. 修改文字
            _robot.SwitchTo("文字編輯方塊");
            _robot.InputText("NewText");
            _robot.ClickToolBarButton("確定");
            _robot.Sleep(1);

            // 5. 執行Undo
            _robot.ClickUndoButton();
            _robot.Sleep(1);

            // 驗證文字恢復原狀
            Assert.AreEqual(originalText, _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3));

            // 6. 執行Redo
            _robot.ClickRedoButton();
            _robot.Sleep(1);

            // 驗證文字恢復修改後狀態
            Assert.AreEqual("NewText", _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3));

            // 7. 檢查按鈕狀態
            _robot.AssertButtonEnabledName("toolStripbtn_undo", true);
            _robot.AssertButtonEnabledName("toolStripbtn_redo", false);
        }

        // 測試連線的Undo/Redo
        [TestMethod]
        public void TestUndoRedoLine()
        {
            // 1. 先畫兩個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);
            string[] shape1State = new string[] { "刪", "1", "Process", null, "100", "100", "100", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, shape1State);

            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(300, 100);
            _robot.MouseMove(400, 200);
            _robot.MouseUp(400, 200);
            _robot.Sleep(1);
            string[] shape2State = new string[] { "刪", "2", "Process", null, "300", "100", "100", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 1, shape2State);

            // 2. 畫線連接兩個圖形
            _robot.ClickToolBarButton("toolStripbtn_line");
            _robot.MouseDown(200, 150);
            _robot.MouseMove(300, 150);
            _robot.MouseUp(300, 150);
            _robot.Sleep(1);
            string[] lineState = new string[] { "刪", "3", "Line", null, "200", "150", "0", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 2, lineState);

            // 3. 執行Undo
            _robot.ClickUndoButton();
            _robot.Sleep(1);

            // 4. 執行Redo
            _robot.ClickRedoButton();
            _robot.Sleep(1);
            _robot.AssertDataGridViewContent(SHAPE_GRID, 2, lineState);

            // 5. 檢查按鈕狀態
            _robot.AssertButtonEnabledName  ("toolStripbtn_undo", true);
            _robot.AssertButtonEnabledName("toolStripbtn_redo", false);
        }

        // 測試DataGridView新增刪除的Undo/Redo
        [TestMethod]
        public void TestUndoRedoDataGridView()
        {
            // 1. 新增圖形
            _robot.SelectShapeType("Process");
            var data = new Dictionary<string, string>
            {
                { "tb_word", "Test" },
                { "tb_x", "100" },
                { "tb_y", "100" },
                { "tb_w", "100" },
                { "tb_h", "100" }
            };
            _robot.InputDataGridViewData(data);
            _robot.ClickAddButton();
            _robot.Sleep(1);
            string[] shape1 = new string[] { "刪", "1", "Process", null, "100", "100", "100", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, shape1);
            
            // 2. 刪除圖形
            _robot.ClickDataGridViewDelete(0);
            _robot.Sleep(1);
            _robot.AssertDataGridViewRowCount(SHAPE_GRID, 0);

            // 3. 執行Undo
            _robot.ClickUndoButton();
            _robot.Sleep(1);
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, shape1);

            // 4. 執行Redo
            _robot.ClickRedoButton();
            _robot.Sleep(1);

            // 5. 檢查按鈕狀態
            _robot.AssertButtonEnabledName("toolStripbtn_undo", true);
            _robot.AssertButtonEnabledName("toolStripbtn_redo", false);
        }


        // 測試連線後刪除圖形的Undo/Redo
        [TestMethod]
        public void TestUndoRedoDeleteShapeWithLine()
        {
            // 1. 畫兩個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);

            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(300, 100);
            _robot.MouseMove(400, 200);
            _robot.MouseUp(400, 200);
            _robot.Sleep(1);

            // 2. 連接兩個圖形
            _robot.ClickToolBarButton("toolStripbtn_line");
            _robot.MouseDown(200, 150);
            _robot.MouseMove(300, 150);
            _robot.MouseUp(300, 150);
            _robot.Sleep(1);

            // 3. 刪除一個圖形
            _robot.ClickDataGridViewDelete(0);
            _robot.Sleep(1);

            // 4. Undo
            _robot.ClickUndoButton();
            _robot.Sleep(1);

            // 5. Redo
            _robot.ClickRedoButton();
            _robot.Sleep(1);

            // 檢查按鈕狀態
            _robot.AssertButtonEnabledName("toolStripbtn_undo", true);
            _robot.AssertButtonEnabledName("toolStripbtn_redo", false);
        }

        // 測試文字超出圖形邊界的Undo/Redo
        [TestMethod]
        public void TestUndoRedoTextOutOfBounds()
        {
            // 1. 畫一個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);

            // 3. 取得文字位置
            string currentText = _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3);
            int dragPointX = 120 + (currentText.Length * 5);
            int dragPointY = 146;

            // 4. 嘗試將文字拖曳到圖形外
            _robot.MouseDown(dragPointX, dragPointY);
            _robot.MouseMove(300, dragPointY);
            _robot.MouseUp(300, dragPointY);
            _robot.Sleep(1);

            // 5. Undo
            _robot.ClickUndoButton();
            _robot.Sleep(1);

            // 6. Redo
            _robot.ClickRedoButton();
            _robot.Sleep(1);

            // 檢查按鈕狀態
            _robot.AssertButtonEnabledName("toolStripbtn_undo", true);
            _robot.AssertButtonEnabledName("toolStripbtn_redo", false);
        }
    }
}