namespace DesktopClockWidget
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Label vibrationDurationLabel;
        private System.Windows.Forms.NumericUpDown vibrationDurationNumericUpDown;
        private System.Windows.Forms.Label vibrationDurationUnitLabel;
        private System.Windows.Forms.Label alarmSoundPathLabel;
        private System.Windows.Forms.Button uploadAlarmSoundButton;
        private System.Windows.Forms.Button previewCustomAlarmButton;
        private System.Windows.Forms.Button removeAlarmSoundButton;

        private System.Windows.Forms.GroupBox sizeGroupBox;
        private System.Windows.Forms.Button addTestAlarmButton;
        private System.Windows.Forms.Label sizeLabel;
        private System.Windows.Forms.TrackBar sizeTrackBar;
        private System.Windows.Forms.GroupBox alarmGroupBox;
        private System.Windows.Forms.FlowLayoutPanel alarmsFlowLayoutPanel;
        private System.Windows.Forms.Panel addAlarmPanel;
        private System.Windows.Forms.NumericUpDown alarmMinuteNumericUpDown;
        private System.Windows.Forms.Label colonLabel;
        private System.Windows.Forms.NumericUpDown alarmHourNumericUpDown;
        private System.Windows.Forms.Button addAlarmButton;
        private System.Windows.Forms.GroupBox systemGroupBox;
        private System.Windows.Forms.CheckBox autostartCheckBox;
        private System.Windows.Forms.Button previewAlarmButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button previewVibrationButton;
        // 表盘样式相关控件
        private System.Windows.Forms.GroupBox clockFaceGroupBox;
        private System.Windows.Forms.Button uploadClockFaceButton;
        private System.Windows.Forms.Button previewClockFaceButton;
        private System.Windows.Forms.Button removeClockFaceButton;
        private System.Windows.Forms.Label clockFacePathLabel;

        private ComboBox languageComboBox; // 语言选择下拉框
        private Label languageLabel; // 语言选择标签
        private CheckBox hourlyChimeCheckBox; // 整点报时复选框
        private CheckBox alarmAdCheckBox; // 闹铃广告复选框

        #region Windows Form Designer generated code


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 创建语言选择标签
            languageLabel = new Label
            {
                Text = LanguageManager.Instance.GetText("LanguageSelectionLabel"),
                Location = new Point(12, 20),
                AutoSize = true
            };

            // 创建语言选择下拉框
            languageComboBox = new ComboBox
            {
                Location = new Point(languageLabel.Right +10, 17),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 120
            };

            // 添加语言选项
            languageComboBox.Items.AddRange(new object[] { "中文", "English" });




            this.sizeGroupBox = new System.Windows.Forms.GroupBox();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.sizeTrackBar = new System.Windows.Forms.TrackBar();
            this.alarmGroupBox = new System.Windows.Forms.GroupBox();
            this.alarmsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.addAlarmPanel = new System.Windows.Forms.Panel();
            this.alarmMinuteNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.colonLabel = new System.Windows.Forms.Label();
            this.alarmHourNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.addAlarmButton = new System.Windows.Forms.Button();
            this.addTestAlarmButton = new System.Windows.Forms.Button();
            this.systemGroupBox = new System.Windows.Forms.GroupBox();
            this.vibrationDurationLabel = new System.Windows.Forms.Label();
            this.vibrationDurationNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.vibrationDurationUnitLabel = new System.Windows.Forms.Label();
            this.autostartCheckBox = new System.Windows.Forms.CheckBox();
            this.previewAlarmButton = new System.Windows.Forms.Button();
            this.previewVibrationButton = new System.Windows.Forms.Button();
            this.alarmSoundPathLabel = new System.Windows.Forms.Label();
            this.uploadAlarmSoundButton = new System.Windows.Forms.Button();
            this.previewCustomAlarmButton = new System.Windows.Forms.Button();
            this.removeAlarmSoundButton = new System.Windows.Forms.Button();

            this.saveButton = new System.Windows.Forms.Button();


            this.sizeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeTrackBar)).BeginInit();
            this.alarmGroupBox.SuspendLayout();
            this.addAlarmPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alarmMinuteNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alarmHourNumericUpDown)).BeginInit();
            this.systemGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // sizeGroupBox
            // 
            this.sizeGroupBox.Controls.Add(this.sizeLabel);
            this.sizeGroupBox.Controls.Add(this.sizeTrackBar);
            this.sizeGroupBox.Location = new System.Drawing.Point(12, 42);
            this.sizeGroupBox.Name = "sizeGroupBox";
            this.sizeGroupBox.Size = new System.Drawing.Size(327, 86);
            this.sizeGroupBox.TabIndex = 0;
            this.sizeGroupBox.TabStop = false;
            this.sizeGroupBox.Text = "时钟大小";
            // 
            // sizeLabel
            // 
            this.sizeLabel.AutoSize = true;
            this.sizeLabel.Location = new System.Drawing.Point(10, 63);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(77, 15);
            this.sizeLabel.TabIndex = 1;
            this.sizeLabel.Text = "时钟大小: 200px";
            // 
            // sizeTrackBar
            // 
            this.sizeTrackBar.Location = new System.Drawing.Point(10, 22);
            this.sizeTrackBar.Name = "sizeTrackBar";
            this.sizeTrackBar.Size = new System.Drawing.Size(308, 45);
            this.sizeTrackBar.TabIndex = 0;
            this.sizeTrackBar.Scroll += new System.EventHandler(this.sizeTrackBar_Scroll);
            // 
            // hourlyChimeCheckBox
            // 
            this.hourlyChimeCheckBox = new System.Windows.Forms.CheckBox();
            this.hourlyChimeCheckBox.AutoSize = true;
            this.hourlyChimeCheckBox.Location = new System.Drawing.Point(10, 20);
            this.hourlyChimeCheckBox.Name = "hourlyChimeCheckBox";
            this.hourlyChimeCheckBox.Size = new System.Drawing.Size(150, 19);
            this.hourlyChimeCheckBox.TabIndex = 10;
            this.hourlyChimeCheckBox.Text = "启用整点报时功能";
            this.hourlyChimeCheckBox.UseVisualStyleBackColor = true;
            this.hourlyChimeCheckBox.Checked = true;


            // 创建闹铃广告复选框
            this.alarmAdCheckBox = new System.Windows.Forms.CheckBox();
            this.alarmAdCheckBox.AutoSize = true;
            this.alarmAdCheckBox.Location = new System.Drawing.Point(160, 20);
            this.alarmAdCheckBox.Name = "alarmAdCheckBox";
            this.alarmAdCheckBox.Size = new System.Drawing.Size(150, 19);
            this.alarmAdCheckBox.TabIndex = 11;
            this.alarmAdCheckBox.Text = "启用闹铃广告功能";
            this.alarmAdCheckBox.UseVisualStyleBackColor = true;
            this.alarmAdCheckBox.Checked = true;
            this.alarmAdCheckBox.Enabled = false;
            // 
            // addAlarmPanel
            // 
            this.addAlarmPanel.Controls.Add(this.alarmMinuteNumericUpDown);
            this.addAlarmPanel.Controls.Add(this.colonLabel);
            this.addAlarmPanel.Controls.Add(this.alarmHourNumericUpDown);
            this.addAlarmPanel.Controls.Add(this.addAlarmButton);
            this.addAlarmPanel.Controls.Add(this.addTestAlarmButton);
            this.addAlarmPanel.Location = new System.Drawing.Point(10, 42);
            this.addAlarmPanel.Name = "addAlarmPanel";
            this.addAlarmPanel.Size = new System.Drawing.Size(308, 35);
            this.addAlarmPanel.TabIndex = 1;

            // 
            // alarmsFlowLayoutPanel
            // 
            this.alarmsFlowLayoutPanel.AutoScroll = true;
            this.alarmsFlowLayoutPanel.Location = new System.Drawing.Point(10, 75);
            this.alarmsFlowLayoutPanel.Name = "alarmsFlowLayoutPanel";
            this.alarmsFlowLayoutPanel.Size = new System.Drawing.Size(308, 160);
            this.alarmsFlowLayoutPanel.TabIndex = 2;

            // alarmGroupBox
            // 
            this.alarmGroupBox.Controls.Add(this.alarmsFlowLayoutPanel);
            this.alarmGroupBox.Controls.Add(this.alarmAdCheckBox);
            this.alarmGroupBox.Controls.Add(this.hourlyChimeCheckBox);
            this.alarmGroupBox.Controls.Add(this.addAlarmPanel);
            this.alarmGroupBox.Location = new System.Drawing.Point(12, 144);
            this.alarmGroupBox.Name = "alarmGroupBox";
            this.alarmGroupBox.Size = new System.Drawing.Size(327, 240);
            this.alarmGroupBox.TabIndex = 1;
            this.alarmGroupBox.TabStop = false;
            this.alarmGroupBox.Text = "闹钟设置";


            // 
            // alarmMinuteNumericUpDown
            // 
            this.alarmMinuteNumericUpDown.Location = new System.Drawing.Point(65, 6);
            this.alarmMinuteNumericUpDown.Maximum = new decimal(new int[] {
            59, 0, 0, 0});
            this.alarmMinuteNumericUpDown.Name = "alarmMinuteNumericUpDown";
            this.alarmMinuteNumericUpDown.Size = new System.Drawing.Size(44, 23);
            this.alarmMinuteNumericUpDown.TabIndex = 3;
            // 
            // colonLabel
            // 
            this.colonLabel.AutoSize = true;
            this.colonLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.colonLabel.Location = new System.Drawing.Point(50, 7);
            this.colonLabel.Name = "colonLabel";
            this.colonLabel.Size = new System.Drawing.Size(18, 21);
            this.colonLabel.TabIndex = 2;
            this.colonLabel.Text = ":";
            // 
            // alarmHourNumericUpDown
            // 
            this.alarmHourNumericUpDown.Location = new System.Drawing.Point(3, 6);
            this.alarmHourNumericUpDown.Maximum = new decimal(new int[] {
            23, 0, 0, 0});
            this.alarmHourNumericUpDown.Name = "alarmHourNumericUpDown";
            this.alarmHourNumericUpDown.Size = new System.Drawing.Size(44, 23);
            this.alarmHourNumericUpDown.TabIndex = 1;
            // 
            // addAlarmButton
            // 
            this.addAlarmButton.Location = new System.Drawing.Point(115, 6);
            this.addAlarmButton.Name = "addAlarmButton";
            this.addAlarmButton.Size = new System.Drawing.Size(75, 23);
            this.addAlarmButton.TabIndex = 0;
            this.addAlarmButton.Text = "添加闹钟";
            this.addAlarmButton.UseVisualStyleBackColor = true;
            this.addAlarmButton.Click += new System.EventHandler(this.addAlarmButton_Click);
            // 
            // addTestAlarmButton
            // 
            //this.addTestAlarmButton.Location = new System.Drawing.Point(200, 6);
            //this.addTestAlarmButton.Name = "addTestAlarmButton";
            //this.addTestAlarmButton.Size = new System.Drawing.Size(90, 23);
            //this.addTestAlarmButton.TabIndex = 4;
            //this.addTestAlarmButton.Text = "添加测试闹钟";
            //this.addTestAlarmButton.UseVisualStyleBackColor = true;
            //this.addTestAlarmButton.Click += new System.EventHandler(this.addTestAlarmButton_Click);

            // 
            // autostartCheckBox
            this.autostartCheckBox.AutoSize = true;
            this.autostartCheckBox.Location = new System.Drawing.Point(10, 22);
            this.autostartCheckBox.Name = "autostartCheckBox";
            this.autostartCheckBox.Size = new System.Drawing.Size(102, 19);
            this.autostartCheckBox.TabIndex = 0;
            this.autostartCheckBox.Text = "开机自动启动";
            this.autostartCheckBox.UseVisualStyleBackColor = true;
            this.autostartCheckBox.CheckedChanged += new System.EventHandler(this.autostartCheckBox_CheckedChanged);
            // 
            // previewAlarmButton
            // 
            this.previewAlarmButton.Location = new System.Drawing.Point(10, 47);
            this.previewAlarmButton.Name = "previewAlarmButton";
            this.previewAlarmButton.Size = new System.Drawing.Size(120, 23);
            this.previewAlarmButton.TabIndex = 1;
            this.previewAlarmButton.Text = "预览闹铃声音";
            this.previewAlarmButton.UseVisualStyleBackColor = true;
            this.previewAlarmButton.Click += new System.EventHandler(this.previewAlarmButton_Click);
            // 
            // vibrationDurationLabel
            // 
            this.vibrationDurationLabel.AutoSize = true;
            this.vibrationDurationLabel.Location = new System.Drawing.Point(10, 76);
            this.vibrationDurationLabel.Name = "vibrationDurationLabel";
            this.vibrationDurationLabel.Size = new System.Drawing.Size(140, 15);
            this.vibrationDurationLabel.TabIndex = 2;
            this.vibrationDurationLabel.Text = "震动效果时长（秒）：";
            // 
            // vibrationDurationNumericUpDown
            // 
            this.vibrationDurationNumericUpDown.Location = new System.Drawing.Point(156, 74);
            this.vibrationDurationNumericUpDown.Maximum = new decimal(new int[] {100, 0, 0, 0});
            this.vibrationDurationNumericUpDown.Minimum = new decimal(new int[] {1, 0, 0, 0});
            this.vibrationDurationNumericUpDown.Name = "vibrationDurationNumericUpDown";
            this.vibrationDurationNumericUpDown.Size = new System.Drawing.Size(50, 23);
            this.vibrationDurationNumericUpDown.TabIndex = 4;
            this.vibrationDurationNumericUpDown.Value = new decimal(new int[] {5, 0, 0, 0});
            // 
            // vibrationDurationUnitLabel
            // 
            this.vibrationDurationUnitLabel.AutoSize = true;
            this.vibrationDurationUnitLabel.Location = new System.Drawing.Point(212, 76);
            this.vibrationDurationUnitLabel.Name = "vibrationDurationUnitLabel";
            this.vibrationDurationUnitLabel.Size = new System.Drawing.Size(14, 15);
            this.vibrationDurationUnitLabel.TabIndex = 5;
            this.vibrationDurationUnitLabel.Text = "秒";
            // 
            // previewVibrationButton
            // 
            this.previewVibrationButton.Location = new System.Drawing.Point(10, 105);
            this.previewVibrationButton.Name = "previewVibrationButton";
            this.previewVibrationButton.Size = new System.Drawing.Size(140, 23);
            this.previewVibrationButton.TabIndex = 3;
            this.previewVibrationButton.Text = "预览时钟窗口震动效果";
            this.previewVibrationButton.UseVisualStyleBackColor = true;
            this.previewVibrationButton.Click += new System.EventHandler(this.previewVibrationButton_Click);
          
            // alarmSoundPathLabel
            // 
            this.alarmSoundPathLabel.AutoSize = false;
            this.alarmSoundPathLabel.Location = new System.Drawing.Point(10, 134);
            this.alarmSoundPathLabel.Name = "alarmSoundPathLabel";
            this.alarmSoundPathLabel.Size = new System.Drawing.Size(300, 20);
            this.alarmSoundPathLabel.TabIndex = 6;
            this.alarmSoundPathLabel.Text = "当前使用默认闹铃声音";
            // 
            // uploadAlarmSoundButton
            // 
            this.uploadAlarmSoundButton.Location = new System.Drawing.Point(10, 155);
            this.uploadAlarmSoundButton.Name = "uploadAlarmSoundButton";
            this.uploadAlarmSoundButton.Size = new System.Drawing.Size(100, 23);
            this.uploadAlarmSoundButton.TabIndex = 7;
            this.uploadAlarmSoundButton.Text = "上传闹铃声音";
            this.uploadAlarmSoundButton.UseVisualStyleBackColor = true;
            this.uploadAlarmSoundButton.Click += new System.EventHandler(this.uploadAlarmSoundButton_Click);
            // 
            // previewCustomAlarmButton
            // 
            this.previewCustomAlarmButton.Location = new System.Drawing.Point(116, 155);
            this.previewCustomAlarmButton.Name = "previewCustomAlarmButton";
            this.previewCustomAlarmButton.Size = new System.Drawing.Size(100, 23);
            this.previewCustomAlarmButton.TabIndex = 8;
            this.previewCustomAlarmButton.Text = "预览闹铃声音";
            this.previewCustomAlarmButton.UseVisualStyleBackColor = true;
            this.previewCustomAlarmButton.Click += new System.EventHandler(this.previewCustomAlarmButton_Click);
            // 
            // removeAlarmSoundButton
            // 
            this.removeAlarmSoundButton.Location = new System.Drawing.Point(222, 155);
            this.removeAlarmSoundButton.Name = "removeAlarmSoundButton";
            this.removeAlarmSoundButton.Size = new System.Drawing.Size(100, 23);
            this.removeAlarmSoundButton.TabIndex = 9;
            this.removeAlarmSoundButton.Text = "恢复默认";
            this.removeAlarmSoundButton.UseVisualStyleBackColor = true;
            this.removeAlarmSoundButton.Click += new System.EventHandler(this.removeAlarmSoundButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(264, 680);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "保存";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // systemGroupBox
            // 
            this.systemGroupBox.Controls.Add(this.removeAlarmSoundButton);
            this.systemGroupBox.Controls.Add(this.previewCustomAlarmButton);
            this.systemGroupBox.Controls.Add(this.uploadAlarmSoundButton);
            this.systemGroupBox.Controls.Add(this.alarmSoundPathLabel);
            this.systemGroupBox.Controls.Add(this.vibrationDurationUnitLabel);
            this.systemGroupBox.Controls.Add(this.vibrationDurationNumericUpDown);
            this.systemGroupBox.Controls.Add(this.vibrationDurationLabel);
            this.systemGroupBox.Controls.Add(this.previewVibrationButton);
            this.systemGroupBox.Controls.Add(this.previewAlarmButton);
            this.systemGroupBox.Controls.Add(this.autostartCheckBox);
            this.systemGroupBox.Location = new System.Drawing.Point(12, 390);
            this.systemGroupBox.Name = "systemGroupBox";
            this.systemGroupBox.Size = new System.Drawing.Size(327,190);
            this.systemGroupBox.TabIndex = 2;
            this.systemGroupBox.TabStop = false;
            this.systemGroupBox.Text = "系统设置";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 640);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ControlBox = true;
            this.MinimizeBox = true;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            
            this.clockFaceGroupBox = new System.Windows.Forms.GroupBox();
            this.removeClockFaceButton = new System.Windows.Forms.Button();
            this.previewClockFaceButton = new System.Windows.Forms.Button();
            this.uploadClockFaceButton = new System.Windows.Forms.Button();
            this.clockFacePathLabel = new System.Windows.Forms.Label();
            
            this.clockFaceGroupBox.SuspendLayout();
            
            // 
            // clockFaceGroupBox
            // 
            this.clockFaceGroupBox.Controls.Add(this.clockFacePathLabel);
            this.clockFaceGroupBox.Controls.Add(this.uploadClockFaceButton);
            this.clockFaceGroupBox.Controls.Add(this.previewClockFaceButton);
            this.clockFaceGroupBox.Controls.Add(this.removeClockFaceButton);
            this.clockFaceGroupBox.Location = new System.Drawing.Point(12, 580);
            this.clockFaceGroupBox.Name = "clockFaceGroupBox";
            this.clockFaceGroupBox.Size = new System.Drawing.Size(327, 80);
            this.clockFaceGroupBox.TabIndex = 4;
            this.clockFaceGroupBox.TabStop = false;
            this.clockFaceGroupBox.Text = "表盘样式";
            // 
            // clockFacePathLabel
            // 
            this.clockFacePathLabel.AutoSize = true;
            this.clockFacePathLabel.Location = new System.Drawing.Point(10, 22);
            this.clockFacePathLabel.Name = "clockFacePathLabel";
            this.clockFacePathLabel.Size = new System.Drawing.Size(126, 15);
            this.clockFacePathLabel.TabIndex = 3;
            this.clockFacePathLabel.Text = "当前使用默认表盘样式";
            // 
            // uploadClockFaceButton
            // 
            this.uploadClockFaceButton.Location = new System.Drawing.Point(10, 47);
            this.uploadClockFaceButton.Name = "uploadClockFaceButton";
            this.uploadClockFaceButton.Size = new System.Drawing.Size(100, 23);
            this.uploadClockFaceButton.TabIndex = 0;
            this.uploadClockFaceButton.Text = "上传表盘图片";
            this.uploadClockFaceButton.UseVisualStyleBackColor = true;
            this.uploadClockFaceButton.Click += new System.EventHandler(this.uploadClockFaceButton_Click);
            // 
            // previewClockFaceButton
            // 
            this.previewClockFaceButton.Location = new System.Drawing.Point(116, 47);
            this.previewClockFaceButton.Name = "previewClockFaceButton";
            this.previewClockFaceButton.Size = new System.Drawing.Size(100, 23);
            this.previewClockFaceButton.TabIndex = 1;
            this.previewClockFaceButton.Text = "预览表盘样式";
            this.previewClockFaceButton.UseVisualStyleBackColor = true;
            this.previewClockFaceButton.Click += new System.EventHandler(this.previewClockFaceButton_Click);
            // 
            // removeClockFaceButton
            // 
            this.removeClockFaceButton.Location = new System.Drawing.Point(222, 47);
            this.removeClockFaceButton.Name = "removeClockFaceButton";
            this.removeClockFaceButton.Size = new System.Drawing.Size(95, 23);
            this.removeClockFaceButton.TabIndex = 2;
            this.removeClockFaceButton.Text = "恢复默认样式";
            this.removeClockFaceButton.UseVisualStyleBackColor = true;
            this.removeClockFaceButton.Click += new System.EventHandler(this.removeClockFaceButton_Click);

            // 从设置中加载当前语言
            var settings = settingsManager.LoadSettings();
            string currentLanguage = settings?.Language ?? "zh-CN";

            // 设置初始选中的语言
            languageComboBox.SelectedIndex = currentLanguage == "en-US" ? 1 : 0;

            // 添加语言变更事件处理
            languageComboBox.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;

            // 将控件添加到表单
            this.Controls.Add(languageLabel);
            this.Controls.Add(languageComboBox);

            // 调整其他控件的位置
            AdjustControlPositions();

            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.sizeGroupBox);
            this.Controls.Add(this.alarmGroupBox);
            this.Controls.Add(this.systemGroupBox);
            this.Controls.Add(this.clockFaceGroupBox);
            this.Name = "SettingsForm";
            this.Text = "时钟设置";
            this.ClientSize = new System.Drawing.Size(351, 720);
            this.sizeGroupBox.ResumeLayout(false);
            this.sizeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeTrackBar)).EndInit();
            this.alarmGroupBox.ResumeLayout(false);
            this.addAlarmPanel.ResumeLayout(false);
            this.addAlarmPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alarmMinuteNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alarmHourNumericUpDown)).EndInit();
            this.systemGroupBox.ResumeLayout(false);
            this.systemGroupBox.PerformLayout();
            this.clockFaceGroupBox.ResumeLayout(false);
            this.clockFaceGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        

    }
}