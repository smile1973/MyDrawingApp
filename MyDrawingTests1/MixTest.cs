using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyDrawingGUITest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawingGUITest
{
    [TestClass]
    public class MixTest
    {
        private Robot _robot;
        private const string CANVAS_NAME = "_canvas";
        private const string SHAPE_GRID = "shapeDataGrid";
        private string targetAppPath;
        private const string DRAWING_FORM = "Form1";
        private string testFilePath;

        [TestInitialize]
        public void Initialize()
        {
            var projectName = "MyDrawing";
            string solutionPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            targetAppPath = Path.Combine(solutionPath, projectName, "bin", "Debug", "MyDrawing.exe");
            testFilePath = Path.Combine(solutionPath, projectName, "bin", "Debug", "test.mydrawing");
            _robot = new Robot(targetAppPath, DRAWING_FORM);
        }
        [TestMethod]
        public void TestIntegrationFlow()
        {
            // 1. 繪製流程起點
            _robot.ClickToolBarButton("toolStripbtn_start");
            _robot.MouseDown(200, 100);
            _robot.MouseMove(300, 150);
            _robot.MouseUp(300, 150);
            _robot.Sleep(0.5);

            // 2. 繪製處理方塊
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(200, 250);
            _robot.MouseMove(300, 300);
            _robot.MouseUp(300, 300);
            _robot.Sleep(0.5);

            // 3. 繪製判斷方塊
            _robot.ClickToolBarButton("toolStripbtn_decision");
            _robot.MouseDown(200, 400);
            _robot.MouseMove(300, 450);
            _robot.MouseUp(300, 450);
            _robot.Sleep(0.5);

            // 4. 繪製終止方塊
            _robot.ClickToolBarButton("toolStripbtn_terminator");
            _robot.MouseDown(400, 400);
            _robot.MouseMove(500, 450);
            _robot.MouseUp(500, 450);
            _robot.Sleep(0.5);

            _robot.MouseDown(210, 410);
            _robot.MouseMove(410, 260);
            _robot.MouseUp(410, 260);

            // 5. 連接圖形
            _robot.ClickToolBarButton("toolStripbtn_line");
            _robot.MouseDown(250, 150);  // Start下方
            _robot.MouseUp(250, 250);    // Process上方
            _robot.Sleep(0.5);

            _robot.ClickToolBarButton("toolStripbtn_line");
            _robot.MouseDown(300, 275);  // Process right
            _robot.MouseUp(400, 275);    // Decision left
            _robot.Sleep(0.5);

            _robot.ClickToolBarButton("toolStripbtn_line");
            _robot.MouseDown(450, 300);  // Decision down
            _robot.MouseUp(450, 400);    // Terminator top
            _robot.Sleep(0.5);

            // 6. 修改文字
            ModifyShapeText(0, "開始");
            ModifyShapeText(1, "處理資料");
            ModifyShapeText(2, "判斷條件");
            ModifyShapeText(3, "結束");

            // 7. 儲存檔案
            _robot.ClickToolBarButton("toolStripbtn_save");
            _robot.HandleSaveDialog("test.mydrawing");
            _robot.Sleep(0.5);

            // 8. 刪除全部圖形
            for (int i = 0; i < 4; i++)
            {
                _robot.ClickDataGridViewDelete(0);
                _robot.Sleep(0.5);
            }

            // 9. 載入檔案
            _robot.ClickToolBarButton("toolStripbtn_load");
            _robot.HandleLoadDialog("test.mydrawing");
            _robot.Sleep(0.5);

            // 10. 驗證
            string[] expectedDataStart = { "刪", "1", "Start", "開始", "200", "100", "50", "100" };
            string[] expectedDataProcess = { "刪", "2", "Process", "處理資料", "200", "250", "50", "100" };
            string[] expectedDataDecision = { "刪", "3", "Decision", "判斷條件", "400", "250", "50", "100" };
            string[] expectedDataTerminator = { "刪", "4", "Terminator", "結束", "400", "400", "50", "100" };

            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedDataStart, false);
            _robot.AssertDataGridViewContent(SHAPE_GRID, 1, expectedDataProcess, false);
            _robot.AssertDataGridViewContent(SHAPE_GRID, 2, expectedDataDecision, false);
            _robot.AssertDataGridViewContent(SHAPE_GRID, 3, expectedDataTerminator, false);

            _robot.MouseDown(410, 260);
            _robot.MouseMove(510, 110);
            _robot.MouseUp(510, 110);
            _robot.CleanUp();
        }
        private void ModifyShapeText(int shapeIndex, string newText)
        {
            // 計算橘色點位置並雙擊
            string currentText = _robot.GetDataGridViewCellText(SHAPE_GRID, shapeIndex, 3);
            int dragPointX = int.Parse(_robot.GetDataGridViewCellText(SHAPE_GRID, shapeIndex, 4)) +
                            (int.Parse(_robot.GetDataGridViewCellText(SHAPE_GRID, shapeIndex, 7)) / 5) +
                            (currentText.Length * 5);
            int dragPointY = int.Parse(_robot.GetDataGridViewCellText(SHAPE_GRID, shapeIndex, 5)) +
                            (int.Parse(_robot.GetDataGridViewCellText(SHAPE_GRID, shapeIndex, 6)) / 2) - 4;

            _robot.DoubleClickPoint(dragPointX, dragPointY);
            _robot.Sleep(0.5);
            _robot.InputText(newText);
            _robot.ClickToolBarButton("確定");
            _robot.Sleep(0.5);
        }
    }
}
