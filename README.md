# MyDrawing - 流程圖繪製應用程式

## 專案概述

MyDrawing 是一個基於 Windows Forms 的流程圖繪製應用程式，採用 MVC (Model-View-Controller) 架構設計，具備完整的單元測試和 GUI 測試。本應用程式提供直觀的使用者介面，讓使用者能夠輕鬆建立、編輯和管理流程圖。

## 主要功能

### 圖形繪製
- 支援四種基本流程圖形狀：
  - **Start** (橢圓形)：流程起始點
  - **Process** (矩形)：處理程序
  - **Decision** (菱形)：決策點
  - **Terminator** (圓角矩形)：流程終點
- 支援 **Line** 工具連接各個圖形

### 編輯功能
- **拖曳移動**：點選並拖曳圖形到新位置
- **文字編輯**：雙擊圖形的文字點可編輯內部文字
- **文字位置調整**：可單獨調整文字在圖形內的位置
- **Undo/Redo**：支援復原與重做操作

### 資料管理
- **即時預覽**：使用 DataGridView 顯示所有圖形的詳細資訊
- **手動新增**：可透過輸入座標和尺寸直接新增圖形
- **刪除功能**：可從 DataGridView 或直接選取圖形刪除

### 檔案操作
- **儲存/載入**：支援自訂格式 (.mydrawing) 的檔案儲存與載入
- **自動儲存**：每 30 秒自動備份，保留最近 5 個備份檔
- **UI 鎖定**：檔案操作期間會顯示提示並鎖定介面

## 技術架構

### MVC 架構
- **Model 層** (`MyDrawing.Model`)：
  - 圖形資料模型 (Shape 類別及其子類)
  - 狀態管理 (IState 介面及實作)
  - 命令模式實作 (ICommand 介面)
  
- **View 層** (`Form1.cs`)：
  - Windows Forms 使用者介面
  - 工具列、畫布、資料格檢視

- **Controller 層**：
  - `ShapeModel`：管理圖形集合和業務邏輯
  - `PresentationModel`：協調 Model 和 View 的互動

### 設計模式
- **Command Pattern**：用於 Undo/Redo 功能
- **State Pattern**：管理不同的繪圖狀態（繪圖、選取、連線）
- **Factory Pattern**：建立不同類型的圖形物件

## 專案結構

```
MyDrawing/
├── Model/                    # 資料模型和商業邏輯
│   ├── Shapes/              # 圖形類別
│   │   ├── Shape.cs         # 抽象基底類別
│   │   ├── Start.cs
│   │   ├── Process.cs
│   │   ├── Decision.cs
│   │   ├── Terminator.cs
│   │   └── Line.cs
│   ├── States/              # 狀態模式實作
│   │   ├── IState.cs
│   │   ├── DrawingState.cs
│   │   ├── PointerState.cs
│   │   └── DrawLineState.cs
│   ├── Commands/            # 命令模式實作
│   │   ├── ICommand.cs
│   │   ├── DrawCommand.cs
│   │   ├── MoveCommand.cs
│   │   └── ...
│   └── ShapeModel.cs        # 主要模型類別
├── PresentationModel/        # 展示層邏輯
│   └── PresentationModel.cs
├── View/                    # 使用者介面
│   ├── Form1.cs
│   └── Form1.Designer.cs
└── Tests/                   # 測試專案
    ├── Unit Tests/          # 單元測試
    └── GUI Tests/           # GUI 自動化測試
```

## 測試架構

### 單元測試
- 涵蓋所有核心功能的單元測試
- 使用 MSTest 框架
- Mock 物件用於隔離測試

### GUI 測試
- 使用 Appium 和 Windows Application Driver
- 自動化測試主要使用者操作流程
- 包含回歸測試和整合測試

### 測試範圍
- 圖形繪製和編輯功能
- Undo/Redo 操作
- 檔案儲存和載入
- 自動儲存功能
- UI 回應性測試

## 使用說明

### 基本操作
1. **繪製圖形**：選擇工具列上的圖形按鈕，在畫布上拖曳繪製
2. **移動圖形**：使用指標工具選取並拖曳圖形
3. **編輯文字**：雙擊橘色文字點開啟編輯對話框
4. **連接圖形**：使用 Line 工具點擊圖形的連接點
5. **儲存檔案**：點擊儲存按鈕選擇檔案位置

### 快捷操作
- 復原：點擊 Undo 按鈕
- 重做：點擊 Redo 按鈕
- 刪除：選取圖形後按 Delete 鍵或使用 DataGridView 的刪除按鈕

## 開發環境需求

- Visual Studio 2019 或更新版本
- .NET Framework 4.7.2
- Windows Application Driver (用於 GUI 測試)
- MSTest 測試框架

## 安裝與執行

1. 複製專案到本地端
2. 使用 Visual Studio 開啟 `MyDrawing.sln`
3. 建置專案 (Build Solution)
4. 執行 `MyDrawing.exe`

## 授權

本專案採用 MIT 授權條款 - 詳見 LICENSE 檔案
