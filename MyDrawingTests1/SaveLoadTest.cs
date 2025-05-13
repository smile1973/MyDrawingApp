using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Threading;
using System.IO;
using System;
using System.Collections.Generic;

namespace MyDrawingGUITest
{
    [TestClass]
    public class SaveLoadTest
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

        [TestCleanup]
        public void Cleanup()
        {
            _robot.CleanUp();
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }

        // 測試基本的Save & Load功能
        [TestMethod]
        public void TestSaveAndLoad()
        {
            // 1. 繪製一些圖形
            string[] shape1State = new string[] { "刪", "1", "Process", null, "100", "100", "100", "100" };
            string[] shape2State = new string[] { "刪", "2", "Start", null, "300", "100", "100", "100" };
            string[] lineState = new string[] { "刪", "3", "Line", null, "200", "150", "0", "100" };

            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);
            _robot.AssertDataGridViewContent(SHAPE_GRID, 0, shape1State);

            _robot.ClickToolBarButton("toolStripbtn_start");
            _robot.MouseDown(300, 100);
            _robot.MouseMove(400, 200);
            _robot.MouseUp(400, 200);
            _robot.Sleep(1);
            _robot.AssertDataGridViewContent(SHAPE_GRID, 1, shape2State);

            _robot.ClickToolBarButton("toolStripbtn_line");
            _robot.MouseDown(200, 150);
            _robot.MouseMove(300, 150);
            _robot.MouseUp(300, 150);
            _robot.Sleep(1);
            _robot.AssertDataGridViewContent(SHAPE_GRID, 2, lineState);

            // 2. 儲存檔案
            _robot.ClickSaveButton();
            _robot.HandleSaveDialog(testFilePath);

            _robot.AssertButtonEnabledName("toolStripbtn_save", false);
            _robot.Sleep(4);
            _robot.AssertButtonEnabledName("toolStripbtn_save", true);

            _robot.ClickDataGridViewDelete(2);
            _robot.Sleep(1);
            _robot.ClickDataGridViewDelete(0);
            _robot.Sleep(1);

            _robot.ClickLoadButton();
            _robot.HandleLoadDialog(testFilePath);
            _robot.Sleep(1);  // 等待載入完成
        }

        // 測試儲存與UI響應
        [TestMethod]
        public void TestSaveUIResponsive()
        {
            // 1. 建立一個圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);

            // 2. 開始儲存
            _robot.ClickSaveButton();
            _robot.HandleSaveDialog(testFilePath);


            // 嘗試進行繪圖操作
            _robot.ClickToolBarButton("toolStripbtn_start");
            _robot.MouseDown(300, 100);
            _robot.MouseMove(400, 200);
            _robot.MouseUp(400, 200);
            _robot.Sleep(1);

            _robot.Sleep(4);
            _robot.AssertButtonEnabledName("toolStripbtn_save", true);
        }

        // 測試載入時UI鎖定
        [TestMethod]
        public void TestLoadUILocked()
        {
            // 1. 先建立一個檔案
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);
            _robot.Sleep(1);

            _robot.ClickSaveButton();
            _robot.HandleSaveDialog(testFilePath);
            _robot.Sleep(4);

            // 2. 載入檔案
            _robot.ClickLoadButton();

            // 3. 驗證載入期間UI被鎖定
            Assert.IsFalse(_robot.IsUIResponsive(), "UI should be locked during load");
            _robot.HandleLoadDialog(testFilePath);
        }
    }
}
