using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyDrawingGUITest;
using System.IO;
using System;

namespace MyDrawingGUITest {
    [TestClass]
    public class AutoSaveTest
    {
        private Robot _robot;
        private const string CANVAS_NAME = "_canvas";
        private const string SHAPE_GRID = "shapeDataGrid";
        private const string DRAWING_FORM = "Form1";
        private string targetAppPath;
        private string backupPath;

        [TestInitialize]
        public void Initialize()
        {
            string solutionPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            targetAppPath = Path.Combine(solutionPath, "MyDrawing", "bin", "Debug", "MyDrawing.exe");
            backupPath = Path.Combine(Path.GetDirectoryName(targetAppPath), "drawing_backup");

            if (Directory.Exists(backupPath))
                Directory.Delete(backupPath, true);

            _robot = new Robot(targetAppPath, DRAWING_FORM);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _robot.CleanUp();
            if (Directory.Exists(backupPath))
                Directory.Delete(backupPath, true);
        }

        [TestMethod]
        public void TestAutoSave()
        {
            bool check = false;
            // 1. 繪製圖形
            _robot.ClickToolBarButton("toolStripbtn_process");
            _robot.MouseDown(100, 100);
            _robot.MouseMove(200, 200);
            _robot.MouseUp(200, 200);

            // 2. 等待30秒後確認"Auto Saving"文字
            _robot.Sleep(22);
            while (!check) 
            {
                check = _robot.CheckWindowTitle("MyDrawing (Auto saving...)");
                _robot.Sleep(1);
            } 

            // 3. 確認存檔過程中UI仍可操作
            _robot.ClickToolBarButton("toolStripbtn_pointer");
            _robot.AssertToolBarButtonChecked("toolStripbtn_pointer", true);

            Assert.IsTrue(_robot.IsBackupFileNameValid(backupPath));
        }
    }
}