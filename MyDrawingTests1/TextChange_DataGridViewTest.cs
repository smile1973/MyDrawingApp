using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Threading;
using System.IO;
using System;
using System.Collections.Generic;


namespace MyDrawingGUITest
{
    [TestClass]
    public class TextChange_DataGridViewControl_Test
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
        public void TestModifyText()
        {
            // 1. 先畫一個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);

            // 2. 切換到 Pointer 模式
            _robot.ClickToolBarButton("toolStripbtn_pointer");

            // 3. 取得目前文字
            string originalText = _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3);

            // 4. 計算橘色點位置並雙擊
            int textX = 100 + (100 / 5);
            int textY = 100 + (100 / 2);
            int dragPointX = textX + (originalText.Length * 5);
            int dragPointY = textY - 4;
            _robot.DoubleClickPoint(dragPointX, dragPointY);
            _robot.Sleep(1);

            // 5. 在對話框中輸入新文字
            _robot.InputText("New Text");
            _robot.ClickToolBarButton("確定");

            // 6. 驗證文字已更新
            _robot.Sleep(1);
            string newText = _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3);
            Assert.AreEqual("New Text", newText);
        }

        // 透過 DataGridView 新增圖形測試
        [TestMethod]
        public void TestAddProcessByDataGridView()
        {
            // 1. 選擇圖形類型
            _robot.SelectShapeType("Process");

            // 2. 輸入資料
            var data = new Dictionary<string, string>
            {
                { "tb_word", "Test Shape" },
                { "tb_x", "100" },
                { "tb_y", "100" },
                { "tb_h", "80" },
                { "tb_w", "150" }
                
            };
            _robot.InputDataGridViewData(data);

            // 3. 點擊新增按鈕
            _robot.ClickAddButton();

            // 4. 驗證圖形已新增且資料正確
            _robot.Sleep(1);
            string[] expectedData = new string[] { "刪", "1", "Process", "Test Shape", "100", "100", "80", "150" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedData, false);
        }

        [TestMethod]
        public void TestAddStartByDataGridView()
        {
            // 1. 選擇圖形類型
            _robot.SelectShapeType("Start");

            // 2. 輸入資料
            var data = new Dictionary<string, string>
            {
                { "tb_word", "Test Shape" },
                { "tb_x", "100" },
                { "tb_y", "100" },
                { "tb_h", "80" },
                { "tb_w", "150" }

            };
            _robot.InputDataGridViewData(data);

            // 3. 點擊新增按鈕
            _robot.ClickAddButton();

            // 4. 驗證圖形已新增且資料正確
            _robot.Sleep(1);
            string[] expectedData = new string[] { "刪", "1", "Start", "Test Shape", "100", "100", "80", "150" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedData, false);
        }

        [TestMethod]
        public void TestAddDecisionByDataGridView()
        {
            // 1. 選擇圖形類型
            _robot.SelectShapeType("Decision");

            // 2. 輸入資料
            var data = new Dictionary<string, string>
            {
                { "tb_word", "Test Shape" },
                { "tb_x", "100" },
                { "tb_y", "100" },
                { "tb_h", "80" },
                { "tb_w", "150" }

            };
            _robot.InputDataGridViewData(data);

            // 3. 點擊新增按鈕
            _robot.ClickAddButton();

            // 4. 驗證圖形已新增且資料正確
            _robot.Sleep(1);
            string[] expectedData = new string[] { "刪", "1", "Decision", "Test Shape", "100", "100", "80", "150" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedData, false);
        }

        [TestMethod]
        public void TestAddTerminatorByDataGridView()
        {
            // 1. 選擇圖形類型
            _robot.SelectShapeType("Terminator");

            // 2. 輸入資料
            var data = new Dictionary<string, string>
            {
                { "tb_word", "Test Shape" },
                { "tb_x", "100" },
                { "tb_y", "100" },
                { "tb_h", "80" },
                { "tb_w", "150" }

            };
            _robot.InputDataGridViewData(data);

            // 3. 點擊新增按鈕
            _robot.ClickAddButton();

            // 4. 驗證圖形已新增且資料正確
            _robot.Sleep(1);
            string[] expectedData = new string[] { "刪", "1", "Terminator", "Test Shape", "100", "100", "80", "150" };
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, expectedData, false);
        }

        // 透過 DataGridView 刪除圖形測試
        [TestMethod]
        public void TestDeleteShapeByDataGridView()
        {
            _robot.SelectShapeType("Process");
            var data = new Dictionary<string, string>
            {
                { "tb_word", "Test Shape" },
                { "tb_x", "100" },
                { "tb_y", "100" },
                { "tb_h", "80" },
                { "tb_w", "150" }
            };
            _robot.InputDataGridViewData(data);
            _robot.ClickAddButton();
            _robot.Sleep(1);

            _robot.ClickDataGridViewDelete(0);
            _robot.Sleep(1);

            _robot.AssertDataGridViewRowCount(SHAPE_GRID, 0);
        }

        [TestMethod]
        public void TestCancelTextModification()
        {
            // 1. 先畫一個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);

            // 2. 切換到 Pointer 模式
            _robot.ClickToolBarButton("toolStripbtn_pointer");

            // 3. 記錄原始文字
            string originalText = _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3);

            // 4. 計算橘色點位置並雙擊
            int textX = 100 + (100 / 5);
            int textY = 100 + (100 / 2);
            int dragPointX = textX + (originalText.Length * 5);
            int dragPointY = textY - 4;
            _robot.DoubleClickPoint(dragPointX, dragPointY);

            // 5. 在對話框中輸入新文字但按取消
            _robot.InputText("New Text");
            _robot.ClickToolBarButton("取消");

            // 6. 驗證文字沒有改變
            _robot.Sleep(1);
            string newText = _robot.GetDataGridViewCellText(SHAPE_GRID, 0, 3);
            Assert.AreEqual(originalText, newText);
        }


        // 新增資料時驗證按鈕的啟用狀態
        [TestMethod]
        public void TestAddButtonEnableState()
        {
            // 1. 初始狀態下，新增按鈕應該是禁用的
            _robot.AssertButtonEnabledAID("btn_add", false);

            // 2. 選擇圖形類型
            _robot.SelectShapeType("Process");

            // 3. 輸入不完整的資料 (缺少高度)
            var data = new Dictionary<string, string>
            {
                { "tb_word", "Test Shape" },
                { "tb_x", "100" },
                { "tb_y", "100" },
                { "tb_h", "80" }
            };
            _robot.InputDataGridViewData(data);

            // 4. 驗證按鈕仍應該是禁用的
            _robot.AssertButtonEnabledAID("btn_add", false);

            // 5. 補完所有資料
            _robot.InputDataGridViewData(new Dictionary<string, string> { { "tb_w", "150" } });

            // 6. 驗證按鈕變為啟用
            _robot.AssertButtonEnabledAID("btn_add", true);
        }
    }
}