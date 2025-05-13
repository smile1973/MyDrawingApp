using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Threading;
using System.IO;
using System;
using System.Collections.Generic;


namespace MyDrawingGUITest
{
    [TestClass]
    public class Draw_Drag_Test
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

        [TestMethod]
        public void TestDrawStart()
        {
            _robot.ClickToolBarButton("toolStripbtn_start");
            _robot.AssertToolBarButtonChecked("toolStripbtn_start", true);
            _robot.AssertToolBarButtonChecked("toolStripbtn_pointer", false);

            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);

            _robot.Sleep(1);
            string[] expectedData = new string[] { "刪", "1", "Start", null, "100", "100", "100", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedData);
        }

        [TestMethod]
        public void TestDrawTerminator()
        {
            _robot.ClickToolBarButton("toolStripbtn_terminator");
            _robot.AssertToolBarButtonChecked("toolStripbtn_terminator", true);
            _robot.AssertToolBarButtonChecked("toolStripbtn_pointer", false);

            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);

            _robot.Sleep(1);
            string[] expectedData = new string[] { "刪", "1", "Terminator", null, "100", "100", "100", "150" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedData);
        }

        [TestMethod]
        public void TestDrawProcess()
        {
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.AssertToolBarButtonChecked("toolStripbtn_process", true);
            _robot.AssertToolBarButtonChecked("toolStripbtn_pointer", false);

            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);

            _robot.Sleep(1);
            string[] expectedData = new string[] { "刪", "1", "Process", null, "100", "100", "100", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedData);
        }

        [TestMethod]
        public void TestDrawDecision()
        {
            _robot.ClickToolBarButton("toolStripbtn_decision");
            _robot.AssertToolBarButtonChecked("toolStripbtn_decision", true);
            _robot.AssertToolBarButtonChecked("toolStripbtn_pointer", false);

            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);

            _robot.Sleep(1);
            string[] expectedData = new string[] { "刪", "1", "Decision", null, "100", "100", "100", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedData);
        }

        [TestMethod]
        public void TestDrawLine()
        {
            // 先畫兩個圖形以便連線
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

            // 切換到 Line 模式
            _robot.ClickToolBarButton("toolStripbtn_line");
            _robot.AssertToolBarButtonChecked("toolStripbtn_line", true);
            _robot.AssertToolBarButtonChecked("toolStripbtn_pointer", false);

            // 從第一個圖形連接到第二個圖形
            // 點擊第一個圖形的右側連接點 (位於右側中點)
            _robot.MouseDown(200, 150);  // 第一個圖形的右側中點
            _robot.MouseMove(300, 150);  // 移動到第二個圖形的左側中點
            _robot.MouseUp(300, 150);

            _robot.Sleep(1);
            string[] expectedData = new string[] { "刪", "3", "Line", null, "200", "150", "0", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 2, expectedData);  // 檢查第三個物件(線)的資料
        }

        // 圖形拖曳測試
        [TestMethod]
        public void TestDragShape()
        {
            // 1. 先畫一個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);

            // 2. 切換到 Pointer 模式
            _robot.ClickToolBarButton("toolStripbtn_pointer");
            _robot.AssertToolBarButtonChecked("toolStripbtn_pointer", true);

            // 3. 拖曳圖形
            _robot.MouseDown(120, 120);  // 點擊圖形中心位置
            _robot.MouseMove(300, 300);  // 移動到新位置
            _robot.MouseUp(300, 300);    // 放開滑鼠

            // 4. 驗證新位置
            _robot.Sleep(1);
            _robot.AssertShapePosition(SHAPE_GRID, 0, 280, 280);  // 預期的新座標
        }

        // 文字拖曳測試
        [TestMethod]
        public void TestDragText()
        {
            // 1. 先畫一個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);

            // 2. 切換到 Pointer 模式
            _robot.ClickToolBarButton("toolStripbtn_pointer");

            // 3. 取得實際的文字內容並計算橘色點位置
            string actualText = _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3); // 第4欄是文字欄位
            // 文字X = 100 + (100/5) = 120
            // 文字Y = 100 + (100/2) = 150
            // 橘色點X = 120 + (實際文字長度*5)
            // 橘色點Y = 150 - 4 = 146
            int dragPointX = 120 + (actualText.Length * 5);
            int dragPointY = 146;

            // 4. 拖曳文字點
            _robot.MouseDown(dragPointX, dragPointY);
            _robot.MouseMove(dragPointX + 20, dragPointY);  // 向右移動20個單位
            _robot.MouseUp(dragPointX + 20, dragPointY);

            // 4. 驗證圖形位置沒有改變
            _robot.Sleep(1);
            string[] expectedData = new string[] { "刪", "1", "Process", null, "100", "100", "100", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedData);
        }

        // 文字超出框外測試
        [TestMethod]
        public void TestDragTextOutOfBounds()
        {
            // 1. 先畫一個圖形 (100,100) -> (200,200)
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);

            // 2. 切換到 Pointer 模式並點選圖形
            _robot.ClickToolBarButton("toolStripbtn_pointer");
            _robot.MouseDown(150, 150);
            _robot.MouseUp(150, 150);

            // 3. 取得目前文字計算橘色點位置
            string currentText = _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3);

            // 計算初始文字位置 (根據 PointerState 邏輯)
            int shapeX = 100;
            int shapeY = 100;
            int shapeWidth = 100;
            int shapeHeight = 100;
            int textX = shapeX + (shapeWidth / 5);  // Shape.X + (Shape.Width / 5)
            int textY = shapeY + (shapeHeight / 2); // Shape.Y + (Shape.Height / 2)
            int dragPointX = textX + (currentText.Length * 5);
            int dragPointY = textY - 4;

            // 4. 拖曳文字到框外(超出圖形右邊界)
            _robot.MouseDown(dragPointX, dragPointY);
            _robot.MouseMove(300, dragPointY);  // 拖到圖形右邊界之外
            _robot.MouseUp(300, dragPointY);
            _robot.Sleep(1);

            // 5. 確認圖形位置不變且文字回到右邊界內
            string[] expectedData = new string[] { "刪", "1", "Process", currentText, "100", "100", "100", "100" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedData, false);

            int expectedTextX = shapeX + shapeWidth - 50;  // 預期的文字X座標
            dragPointX = expectedTextX + (currentText.Length * 5);  // 新的橘色點X座標
            _robot.DoubleClickPoint(dragPointX, dragPointY);
            _robot.Sleep(1);

            // 7. 驗證文字編輯視窗是否跳出
            _robot.SwitchTo("文字編輯方塊");
            _robot.ClickToolBarButton("取消");
            _robot.Sleep(1);

            // 8. 確認文字仍維持原樣
            string textAfterCancel = _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3);
            Assert.AreEqual(currentText, textAfterCancel, "Text should not change after canceling edit");
        }
    }
}