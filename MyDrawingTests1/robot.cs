using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;
using System.Windows.Automation;
using System.Windows;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace MyDrawingGUITest
{
    public class Robot
    {
        private WindowsDriver<WindowsElement> _driver;
        private Dictionary<string, string> _windowHandles;
        private string _root;
        private const string CONTROL_NOT_FOUND_EXCEPTION = "The specific control is not found!!";
        private const string WIN_APP_DRIVER_URI = "http://127.0.0.1:4723";
        private double windowrate;

        // constructor
        public Robot(string targetAppPath, string root)
        {
            Initialize(targetAppPath, root);
        }

        // initialize
        public void Initialize(string targetAppPath, string root)
        {
            _root = root;
            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", targetAppPath);
            options.AddAdditionalCapability("deviceName", "WindowsPC");

            _driver = new WindowsDriver<WindowsElement>(new Uri(WIN_APP_DRIVER_URI), options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _windowHandles = new Dictionary<string, string>
            {
                { _root, _driver.CurrentWindowHandle }
            };
        }

        // clean up
        public void CleanUp()
        {
            SwitchTo(_root);
            _driver.CloseApp();
            _driver.Dispose();
        }

        // switch to specific window
        public void SwitchTo(string formName)
        {
            if (_windowHandles.ContainsKey(formName))
            {
                _driver.SwitchTo().Window(_windowHandles[formName]);
            }
        }

        // click toolbar button by automation id
        public void ClickToolBarButton(string automationId)
        {
            _driver.FindElementByName(automationId).Click();
        }

        // get button state
        public bool GetToolBarButtonState(string automationId)
        {
            return _driver.FindElementByName(automationId).Selected;
        }

        private AutomationElement GetCanvas()
        {
            var windows = _driver.WindowHandles;
            AutomationElement mainWindow = null;

            foreach (var handle in windows)
            {
                try
                {
                    string cleanHandle = handle.Replace("0x", "");
                    IntPtr hwnd = (IntPtr)Int64.Parse(cleanHandle, System.Globalization.NumberStyles.HexNumber);
                    mainWindow = AutomationElement.FromHandle(hwnd);
                    if (mainWindow != null)
                        break;
                }
                catch
                {
                    continue;
                }
            }

            if (mainWindow == null)
                return null;

            var condition = new PropertyCondition(
                AutomationElement.ControlTypeProperty,
                ControlType.Pane
            );

            return mainWindow.FindFirst(TreeScope.Descendants, condition);
        }
        public double CalculateRate()
        {
            WindowsElement canvas = _driver.FindElementByAccessibilityId("canvas");
            var width = canvas.Rect.Width;
            return width / 940.0;
        }

        public void MouseDown(int x, int y)
        {
            windowrate = CalculateRate();
            var canvas = GetCanvas();
            if (canvas != null)
            {
                System.Drawing.Point clickPoint = new System.Drawing.Point(
                    (int)canvas.Current.BoundingRectangle.Left + (int)(x * windowrate),
                    (int)canvas.Current.BoundingRectangle.Top + (int)(y * windowrate)
                );
                System.Windows.Forms.Cursor.Position = clickPoint;
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            }
        }

        public void MouseMove(int x, int y)
        {
            windowrate = CalculateRate();
            var canvas = GetCanvas();
            if (canvas != null)
            {
                System.Drawing.Point movePoint = new System.Drawing.Point(
                    (int)canvas.Current.BoundingRectangle.Left + (int)(x * windowrate),
                    (int)canvas.Current.BoundingRectangle.Top + (int)(y * windowrate)
                );
                System.Windows.Forms.Cursor.Position = movePoint;
            }
        }

        public void MouseUp(int x, int y)
        {
            windowrate = CalculateRate();
            var canvas = GetCanvas();
            if (canvas != null)
            {
                System.Drawing.Point releasePoint = new System.Drawing.Point(
                    (int)canvas.Current.BoundingRectangle.Left + (int)(x * windowrate),
                    (int)canvas.Current.BoundingRectangle.Top + (int)(y * windowrate)
                );
                System.Windows.Forms.Cursor.Position = releasePoint;
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
        }

        // Windows API for mouse simulation
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;

        public void DoubleClickPoint(int x, int y)
        {
            windowrate = CalculateRate();
            var canvas = GetCanvas();
            if (canvas != null)
            {
                System.Drawing.Point clickPoint = new System.Drawing.Point(
                    (int)canvas.Current.BoundingRectangle.Left + (int)(x * windowrate),
                    (int)canvas.Current.BoundingRectangle.Top + (int)(y * windowrate)
                );
                System.Windows.Forms.Cursor.Position = clickPoint;
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Thread.Sleep(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
        }

        public void InputText(string text)
        {
            // 使用 UIA_EditControlTypeId 找到編輯框
            var condition = new PropertyCondition(
                AutomationElement.ControlTypeProperty,
                ControlType.Edit
            );

            // 從桌面開始找尋
            AutomationElement desktop = AutomationElement.RootElement;
            AutomationElement editBox = desktop.FindFirst(TreeScope.Descendants, condition);

            if (editBox != null)
            {
                var valuePattern = (ValuePattern)editBox.GetCurrentPattern(ValuePattern.Pattern);
                valuePattern.SetValue(text);
            }
        }

        // DataGridView相關操作
        public void SelectShapeType(string shapeType)
        {
            // 點擊下拉按鈕
            _driver.FindElementByName("開啟").Click();
            Sleep(1);  // 等待選單開啟

            // 選擇形狀
            _driver.FindElementByName(shapeType).Click();
        }

        public void InputDataGridViewData(Dictionary<string, string> data)
        {
            foreach (var item in data)
            {
                var textBox = _driver.FindElementByAccessibilityId(item.Key);
                if (textBox != null)
                {
                    textBox.Clear();
                    textBox.SendKeys(item.Value);
                }
            }
        }

        public void ClickAddButton()
        {
            _driver.FindElementByAccessibilityId("btn_add").Click();
        }

        public void ClickDataGridViewDelete(int rowIndex)
        {
            var deleteButton = _driver.FindElementByName($"刪 資料列 {rowIndex}");
            deleteButton.Click();
        }

        // For Undo/Redo testing
        public void ClickUndoButton()
        {
            _driver.FindElementByName("toolStripbtn_undo").Click();
        }

        public void ClickRedoButton()
        {
            _driver.FindElementByName("toolStripbtn_redo").Click();
        }
        // Save and Load related methods
        public void ClickSaveButton()
        {
            _driver.FindElementByName("toolStripbtn_save").Click();
        }

        public void ClickLoadButton()
        {
            _driver.FindElementByName("toolStripbtn_load").Click();
        }

        public void HandleSaveDialog(string filePath)
        {
            Sleep(1);

            // 切換到另存新檔對話框
            var saveDialog = _driver.FindElementByName("另存新檔");
            var fileNameEdit = saveDialog.FindElementByAccessibilityId("1001");
            fileNameEdit.SendKeys("test.mydrawing");

            // 按下存檔按鈕
            var saveButton = _driver.FindElementByName("存檔(S)");
            saveButton.Click();

            Sleep(1);
            try
            {
                var confirmDialog = _driver.FindElementByName("確認另存新檔");
                if (confirmDialog != null)
                {
                    var yesButton = confirmDialog.FindElementByName("是(Y)");
                    yesButton.Click();
                }
            }
            catch { }
        }

        public void HandleLoadDialog(string filePath)
        {
            Sleep(1);

            // 切換到開啟對話框
            var openDialog = _driver.FindElementByName("開啟");
            openDialog.SendKeys("test.mydrawing");

            // 按下開啟按鈕
            var openButton = _driver.FindElementByName("開啟(O)");
            openButton.Click();
        }

        public bool IsUIResponsive()
        {
            var pointer = _driver.FindElementByName("toolStripbtn_pointer");
            return pointer.GetAttribute("HasKeyboardFocus") == "true";
        }


        // 驗證按鈕啟用狀態
        public void AssertButtonEnabledAID(string buttonId, bool expectedState)
        {
            var button = _driver.FindElementByAccessibilityId(buttonId);
            Assert.AreEqual(expectedState, button.Enabled);
        }
        // 驗證按鈕啟用狀態
        public void AssertButtonEnabledName(string name, bool expectedState)
        {
            var button = _driver.FindElementByName(name);
            Assert.AreEqual(expectedState, button.Enabled);
        }

        // 驗證DataGridView的列數
        public void AssertDataGridViewRowCount(string gridId, int expectedCount)
        {
            var dataGridView = _driver.FindElementByAccessibilityId(gridId);
            var rows = dataGridView.FindElementsByName("資料列");
            Assert.AreEqual(expectedCount, rows.Count);
        }

        // Assert methods
        public void AssertToolBarButtonChecked(string name, bool expectedState)
        {
            var element = _driver.FindElementByName(name);
            System.Windows.Point point = new System.Windows.Point(element.Location.X, element.Location.Y);
            AutomationElement automationElement = AutomationElement.FromPoint(point);

            if (automationElement != null)
            {
                object pattern;
                if (automationElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out pattern))
                {
                    var selectionPattern = (SelectionItemPattern)pattern;
                    Assert.AreEqual(expectedState, selectionPattern.Current.IsSelected);
                }
            }
        }
        public void AssertDataGridViewContent(string gridId, int rowIndex, string[] expectedData, bool ignore = true)
        {
            var dataGridView = _driver.FindElementByAccessibilityId(gridId);
            var row = dataGridView.FindElementByName($"資料列 {rowIndex}");
            var cells = row.FindElementsByXPath("//*");

            for (int i = 1; i < cells.Count; i++)
            {
                if (i == 4 && ignore) Assert.AreEqual(expectedData[i - 1], null);
                else Assert.AreEqual(expectedData[i - 1], cells[i].Text.Replace("(null)", ""));
            }
        }

        public void AssertShapePosition(string gridId, int rowIndex, int expectedX, int expectedY)
        {
            var dataGridView = _driver.FindElementByAccessibilityId(gridId);
            var row = dataGridView.FindElementByName($"資料列 {rowIndex}");
            var cells = row.FindElementsByXPath("//*");

            // Assuming X and Y are at specific indices in the grid
            Assert.AreEqual(expectedX.ToString(), cells[5].Text);
            Assert.AreEqual(expectedY.ToString(), cells[6].Text);
        }


        // 取得 DataGridView 特定欄位的值
        public string GetDataGridViewCellText(string gridId, int rowIndex, int columnIndex)
        {
            var dataGridView = _driver.FindElementByAccessibilityId(gridId);
            var row = dataGridView.FindElementByName($"資料列 {rowIndex}");
            var cells = row.FindElementsByXPath("//*");
            return cells[columnIndex + 1].Text.Replace("(null)", "");
        }

        // Utility methods
        public void Sleep(double seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }

        public bool CheckWindowTitle(string expectedTitle)
        {
            var windows = _driver.WindowHandles;
            bool found = false;
            foreach (var handle in windows)
            {
                _driver.SwitchTo().Window(handle);
                if (_driver.Title == expectedTitle)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public bool IsBackupFileNameValid(string backupPath)
        {
            if (!Directory.Exists(backupPath))
                return false;

            var files = Directory.GetFiles(backupPath, "*_bak.mydrawing");
            if (files.Length == 0)
                return false;

            string fileName = Path.GetFileName(files[0]);
            return System.Text.RegularExpressions.Regex.IsMatch(fileName, @"^\d{14}_bak\.mydrawing$");
        }
    }
}