namespace MyDrawing
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.shapeDataGrid = new System.Windows.Forms.DataGridView();
            this.btn_del = new System.Windows.Forms.DataGridViewButtonColumn();
            this.id_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shapes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pos_x = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pos_y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.height = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.label_w = new System.Windows.Forms.Label();
            this.label_h = new System.Windows.Forms.Label();
            this.label_y = new System.Windows.Forms.Label();
            this.label_x = new System.Windows.Forms.Label();
            this.tb_w = new System.Windows.Forms.TextBox();
            this.tb_y = new System.Windows.Forms.TextBox();
            this.tb_h = new System.Windows.Forms.TextBox();
            this.tb_x = new System.Windows.Forms.TextBox();
            this.label_word = new System.Windows.Forms.Label();
            this.tb_word = new System.Windows.Forms.TextBox();
            this.comboBox_shapes = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.說明ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.關於ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripbtn_start = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtn_decision = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtn_terminator = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtn_process = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtn_pointer = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtn_undo = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtn_redo = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtn_line = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtn_save = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtn_load = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.shapeDataGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // shapeDataGrid
            // 
            this.shapeDataGrid.AllowUserToAddRows = false;
            this.shapeDataGrid.AllowUserToDeleteRows = false;
            this.shapeDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.shapeDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.shapeDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.shapeDataGrid.ColumnHeadersHeight = 29;
            this.shapeDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btn_del,
            this.id_number,
            this.shapes,
            this.description,
            this.pos_x,
            this.pos_y,
            this.height,
            this.weight});
            this.shapeDataGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.shapeDataGrid.Location = new System.Drawing.Point(5, 54);
            this.shapeDataGrid.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.shapeDataGrid.Name = "shapeDataGrid";
            this.shapeDataGrid.ReadOnly = true;
            this.shapeDataGrid.RowHeadersVisible = false;
            this.shapeDataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.shapeDataGrid.Size = new System.Drawing.Size(306, 469);
            this.shapeDataGrid.TabIndex = 0;
            this.shapeDataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ShapeDataGridCellClick);
            // 
            // btn_del
            // 
            this.btn_del.FillWeight = 50F;
            this.btn_del.HeaderText = "刪";
            this.btn_del.MinimumWidth = 6;
            this.btn_del.Name = "btn_del";
            this.btn_del.ReadOnly = true;
            this.btn_del.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btn_del.Text = "刪";
            this.btn_del.UseColumnTextForButtonValue = true;
            // 
            // id_number
            // 
            this.id_number.FillWeight = 69F;
            this.id_number.HeaderText = "ID";
            this.id_number.MinimumWidth = 6;
            this.id_number.Name = "id_number";
            this.id_number.ReadOnly = true;
            // 
            // shapes
            // 
            this.shapes.HeaderText = "形狀";
            this.shapes.MinimumWidth = 6;
            this.shapes.Name = "shapes";
            this.shapes.ReadOnly = true;
            this.shapes.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.shapes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // description
            // 
            this.description.HeaderText = "文字";
            this.description.MinimumWidth = 6;
            this.description.Name = "description";
            this.description.ReadOnly = true;
            // 
            // pos_x
            // 
            this.pos_x.FillWeight = 75F;
            this.pos_x.HeaderText = "X";
            this.pos_x.MinimumWidth = 6;
            this.pos_x.Name = "pos_x";
            this.pos_x.ReadOnly = true;
            // 
            // pos_y
            // 
            this.pos_y.FillWeight = 75F;
            this.pos_y.HeaderText = "Y";
            this.pos_y.MinimumWidth = 6;
            this.pos_y.Name = "pos_y";
            this.pos_y.ReadOnly = true;
            // 
            // height
            // 
            this.height.FillWeight = 75F;
            this.height.HeaderText = "H";
            this.height.MinimumWidth = 6;
            this.height.Name = "height";
            this.height.ReadOnly = true;
            // 
            // weight
            // 
            this.weight.FillWeight = 75F;
            this.weight.HeaderText = "W";
            this.weight.MinimumWidth = 6;
            this.weight.Name = "weight";
            this.weight.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_add);
            this.groupBox1.Controls.Add(this.label_w);
            this.groupBox1.Controls.Add(this.label_h);
            this.groupBox1.Controls.Add(this.label_y);
            this.groupBox1.Controls.Add(this.label_x);
            this.groupBox1.Controls.Add(this.tb_w);
            this.groupBox1.Controls.Add(this.tb_y);
            this.groupBox1.Controls.Add(this.tb_h);
            this.groupBox1.Controls.Add(this.tb_x);
            this.groupBox1.Controls.Add(this.label_word);
            this.groupBox1.Controls.Add(this.tb_word);
            this.groupBox1.Controls.Add(this.comboBox_shapes);
            this.groupBox1.Controls.Add(this.shapeDataGrid);
            this.groupBox1.Location = new System.Drawing.Point(808, 52);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(316, 528);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "資料顯示";
            // 
            // btn_add
            // 
            this.btn_add.Enabled = false;
            this.btn_add.Location = new System.Drawing.Point(5, 20);
            this.btn_add.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(50, 30);
            this.btn_add.TabIndex = 13;
            this.btn_add.Text = "新增";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.BtnAddClick);
            // 
            // label_w
            // 
            this.label_w.AutoSize = true;
            this.label_w.ForeColor = System.Drawing.Color.Red;
            this.label_w.Location = new System.Drawing.Point(290, 15);
            this.label_w.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_w.Name = "label_w";
            this.label_w.Size = new System.Drawing.Size(16, 12);
            this.label_w.TabIndex = 12;
            this.label_w.Text = "W";
            // 
            // label_h
            // 
            this.label_h.AutoSize = true;
            this.label_h.ForeColor = System.Drawing.Color.Red;
            this.label_h.Location = new System.Drawing.Point(256, 15);
            this.label_h.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_h.Name = "label_h";
            this.label_h.Size = new System.Drawing.Size(13, 12);
            this.label_h.TabIndex = 11;
            this.label_h.Text = "H";
            // 
            // label_y
            // 
            this.label_y.AutoSize = true;
            this.label_y.ForeColor = System.Drawing.Color.Red;
            this.label_y.Location = new System.Drawing.Point(225, 15);
            this.label_y.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_y.Name = "label_y";
            this.label_y.Size = new System.Drawing.Size(13, 12);
            this.label_y.TabIndex = 10;
            this.label_y.Text = "Y";
            // 
            // label_x
            // 
            this.label_x.AutoSize = true;
            this.label_x.ForeColor = System.Drawing.Color.Red;
            this.label_x.Location = new System.Drawing.Point(189, 15);
            this.label_x.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_x.Name = "label_x";
            this.label_x.Size = new System.Drawing.Size(13, 12);
            this.label_x.TabIndex = 9;
            this.label_x.Text = "X";
            // 
            // tb_w
            // 
            this.tb_w.Location = new System.Drawing.Point(282, 30);
            this.tb_w.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_w.Name = "tb_w";
            this.tb_w.Size = new System.Drawing.Size(30, 22);
            this.tb_w.TabIndex = 8;
            // 
            // tb_y
            // 
            this.tb_y.Location = new System.Drawing.Point(215, 30);
            this.tb_y.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_y.Name = "tb_y";
            this.tb_y.Size = new System.Drawing.Size(30, 22);
            this.tb_y.TabIndex = 7;
            // 
            // tb_h
            // 
            this.tb_h.Location = new System.Drawing.Point(249, 30);
            this.tb_h.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_h.Name = "tb_h";
            this.tb_h.Size = new System.Drawing.Size(30, 22);
            this.tb_h.TabIndex = 6;
            // 
            // tb_x
            // 
            this.tb_x.Location = new System.Drawing.Point(182, 30);
            this.tb_x.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_x.Name = "tb_x";
            this.tb_x.Size = new System.Drawing.Size(30, 22);
            this.tb_x.TabIndex = 5;
            // 
            // label_word
            // 
            this.label_word.AutoSize = true;
            this.label_word.ForeColor = System.Drawing.Color.Red;
            this.label_word.Location = new System.Drawing.Point(141, 15);
            this.label_word.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_word.Name = "label_word";
            this.label_word.Size = new System.Drawing.Size(29, 12);
            this.label_word.TabIndex = 4;
            this.label_word.Text = "文字";
            // 
            // tb_word
            // 
            this.tb_word.Location = new System.Drawing.Point(133, 30);
            this.tb_word.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_word.Name = "tb_word";
            this.tb_word.Size = new System.Drawing.Size(45, 22);
            this.tb_word.TabIndex = 3;
            // 
            // comboBox_shapes
            // 
            this.comboBox_shapes.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.comboBox_shapes.FormattingEnabled = true;
            this.comboBox_shapes.Items.AddRange(new object[] {
            "Start",
            "Terminator",
            "Process",
            "Decision"});
            this.comboBox_shapes.Location = new System.Drawing.Point(59, 31);
            this.comboBox_shapes.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox_shapes.Name = "comboBox_shapes";
            this.comboBox_shapes.Size = new System.Drawing.Size(70, 20);
            this.comboBox_shapes.TabIndex = 2;
            this.comboBox_shapes.Text = "形狀";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 6);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 67);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(4, 78);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 72);
            this.button2.TabIndex = 3;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Location = new System.Drawing.Point(0, 46);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(98, 534);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.說明ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1124, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 說明ToolStripMenuItem
            // 
            this.說明ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.關於ToolStripMenuItem});
            this.說明ToolStripMenuItem.Name = "說明ToolStripMenuItem";
            this.說明ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.說明ToolStripMenuItem.Text = "說明";
            // 
            // 關於ToolStripMenuItem
            // 
            this.關於ToolStripMenuItem.Name = "關於ToolStripMenuItem";
            this.關於ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.關於ToolStripMenuItem.Text = "關於";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripbtn_start,
            this.toolStripbtn_decision,
            this.toolStripbtn_terminator,
            this.toolStripbtn_process,
            this.toolStripbtn_pointer,
            this.toolStripbtn_undo,
            this.toolStripbtn_redo,
            this.toolStripbtn_line,
            this.toolStripbtn_save,
            this.toolStripbtn_load});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1124, 27);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripbtn_start
            // 
            this.toolStripbtn_start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripbtn_start.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtn_start.Image")));
            this.toolStripbtn_start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtn_start.Name = "toolStripbtn_start";
            this.toolStripbtn_start.Size = new System.Drawing.Size(24, 24);
            this.toolStripbtn_start.Text = "toolStripbtn_start";
            this.toolStripbtn_start.ToolTipText = "toolStripbtn_start";
            // 
            // toolStripbtn_decision
            // 
            this.toolStripbtn_decision.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripbtn_decision.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtn_decision.Image")));
            this.toolStripbtn_decision.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtn_decision.Name = "toolStripbtn_decision";
            this.toolStripbtn_decision.Size = new System.Drawing.Size(24, 24);
            this.toolStripbtn_decision.Text = "toolStripbtn_decision";
            // 
            // toolStripbtn_terminator
            // 
            this.toolStripbtn_terminator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripbtn_terminator.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtn_terminator.Image")));
            this.toolStripbtn_terminator.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtn_terminator.Name = "toolStripbtn_terminator";
            this.toolStripbtn_terminator.Size = new System.Drawing.Size(24, 24);
            this.toolStripbtn_terminator.Text = "toolStripbtn_terminator";
            this.toolStripbtn_terminator.ToolTipText = "toolStripbtn_terminator";
            // 
            // toolStripbtn_process
            // 
            this.toolStripbtn_process.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripbtn_process.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtn_process.Image")));
            this.toolStripbtn_process.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtn_process.Name = "toolStripbtn_process";
            this.toolStripbtn_process.Size = new System.Drawing.Size(24, 24);
            this.toolStripbtn_process.Text = "toolStripbtn_process";
            this.toolStripbtn_process.ToolTipText = "toolStripbtn_process";
            // 
            // toolStripbtn_pointer
            // 
            this.toolStripbtn_pointer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripbtn_pointer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtn_pointer.Image")));
            this.toolStripbtn_pointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtn_pointer.Name = "toolStripbtn_pointer";
            this.toolStripbtn_pointer.Size = new System.Drawing.Size(24, 24);
            this.toolStripbtn_pointer.Text = "toolStripbtn_pointer";
            this.toolStripbtn_pointer.ToolTipText = "toolStripbtn_pointer";
            // 
            // toolStripbtn_undo
            // 
            this.toolStripbtn_undo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripbtn_undo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtn_undo.Image")));
            this.toolStripbtn_undo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtn_undo.Name = "toolStripbtn_undo";
            this.toolStripbtn_undo.Size = new System.Drawing.Size(24, 24);
            this.toolStripbtn_undo.Text = "toolStripbtn_undo";
            this.toolStripbtn_undo.ToolTipText = "toolStripbtn_undo";
            // 
            // toolStripbtn_redo
            // 
            this.toolStripbtn_redo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripbtn_redo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtn_redo.Image")));
            this.toolStripbtn_redo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtn_redo.Name = "toolStripbtn_redo";
            this.toolStripbtn_redo.Size = new System.Drawing.Size(24, 24);
            this.toolStripbtn_redo.Text = "toolStripbtn_redo";
            // 
            // toolStripbtn_line
            // 
            this.toolStripbtn_line.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripbtn_line.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtn_line.Image")));
            this.toolStripbtn_line.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtn_line.Name = "toolStripbtn_line";
            this.toolStripbtn_line.Size = new System.Drawing.Size(24, 24);
            this.toolStripbtn_line.Text = "toolStripbtn_line";
            // 
            // toolStripbtn_save
            // 
            this.toolStripbtn_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripbtn_save.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtn_save.Image")));
            this.toolStripbtn_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtn_save.Name = "toolStripbtn_save";
            this.toolStripbtn_save.Size = new System.Drawing.Size(24, 24);
            this.toolStripbtn_save.Text = "toolStripbtn_save";
            // 
            // toolStripbtn_load
            // 
            this.toolStripbtn_load.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripbtn_load.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtn_load.Image")));
            this.toolStripbtn_load.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtn_load.Name = "toolStripbtn_load";
            this.toolStripbtn_load.Size = new System.Drawing.Size(24, 24);
            this.toolStripbtn_load.Text = "toolStripbtn_load";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 580);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.shapeDataGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView shapeDataGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_word;
        private System.Windows.Forms.ComboBox comboBox_shapes;
        private System.Windows.Forms.Label label_w;
        private System.Windows.Forms.Label label_h;
        private System.Windows.Forms.Label label_y;
        private System.Windows.Forms.Label label_x;
        private System.Windows.Forms.TextBox tb_w;
        private System.Windows.Forms.TextBox tb_y;
        private System.Windows.Forms.TextBox tb_h;
        private System.Windows.Forms.TextBox tb_x;
        private System.Windows.Forms.Label label_word;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 說明ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 關於ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewButtonColumn btn_del;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn shapes;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn pos_x;
        private System.Windows.Forms.DataGridViewTextBoxColumn pos_y;
        private System.Windows.Forms.DataGridViewTextBoxColumn height;
        private System.Windows.Forms.DataGridViewTextBoxColumn weight;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripbtn_decision;
        private System.Windows.Forms.ToolStripButton toolStripbtn_terminator;
        private System.Windows.Forms.ToolStripButton toolStripbtn_process;
        private System.Windows.Forms.ToolStripButton toolStripbtn_pointer;
        private System.Windows.Forms.ToolStripButton toolStripbtn_undo;
        private System.Windows.Forms.ToolStripButton toolStripbtn_redo;
        private System.Windows.Forms.ToolStripButton toolStripbtn_line;
        private System.Windows.Forms.ToolStripButton toolStripbtn_save;
        private System.Windows.Forms.ToolStripButton toolStripbtn_load;
        private System.Windows.Forms.ToolStripButton toolStripbtn_start;
    }
}

