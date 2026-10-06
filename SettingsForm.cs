using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave; // 用于NAudio音频播放
using System.Runtime.InteropServices;  // 用于DllImport

namespace DesktopClockWidget
{
    public partial class SettingsForm : Form
    {
        private int clockSize;
        private ClockForm parentForm;
        private AlarmManager alarmManager;
        private SettingsManager settingsManager;
        private string? customClockFacePath = null; // 存储自定义表盘图片路径
        private string? customAlarmSoundPath = null; // 存储自定义闹铃声音路径


        public SettingsForm(int currentSize, ClockForm parent)
        {
            // 使用ClockForm中已有的AlarmManager实例
            alarmManager = parent.AlarmManager;
            settingsManager = new SettingsManager();


            InitializeComponent();
            clockSize = currentSize;
            parentForm = parent;

            InitializeSettings();

            UpdateUIForCurrentLanguage();
            
            // 设置圆角样式
            SetWindowRoundedCorners();
        }
        

        
        /// <summary>
        /// 语言变更事件处理
        /// </summary>
        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 获取选中的语言代码
            string languageCode = languageComboBox.SelectedIndex == 1 ? "en-US" : "zh-CN";
            
            // 更新语言管理器的当前语言
            LanguageManager.Instance.SetLanguage(languageCode);
            
            // 更新UI文本
            UpdateUIForCurrentLanguage();
            
            // 强制刷新表单
            this.Refresh();
            
            // 保存语言设置到配置文件
            var settings = settingsManager.LoadSettings() ?? new AppSettings();
            settings.Language = languageCode;
            settingsManager.SaveSettings(settings);
            
            // 通知父窗口更新语言
            if (parentForm != null)
            {
                parentForm.UpdateLanguageUI();
            }
        }
        
        /// <summary>
        /// 调整其他控件的位置以适应语言选择控件
        /// </summary>
        private void AdjustControlPositions()
        {
            // 这里需要根据实际的控件布局进行调整
            // 为了简化，我们假设其他控件的位置可以整体向下移动
            int offset = 40; // 移动的像素数
            
            // 遍历所有控件，除了语言选择相关的控件
            foreach (Control control in this.Controls)
            {
                if (control != languageLabel && control != languageComboBox && control.Top >= 20)
                {
                    control.Top += offset;
                }
            }
            
            // 调整表单大小
            this.Height += offset;
        }
        
        /// <summary>
        /// 更新UI文本为当前语言
        /// </summary>
        private void UpdateUIForCurrentLanguage()
        {
            // 更新表单标题
            this.Text = LanguageManager.Instance.GetText("SettingsTitle");
            
            // 更新语言选择标签
            if (languageLabel != null)
            {
                languageLabel.Text = LanguageManager.Instance.GetText("LanguageSelectionLabel");
            }
            
            // 更新大小组框
            if (sizeGroupBox != null)
            {
                sizeGroupBox.Text = LanguageManager.Instance.GetText("ClockSizeGroupBox");
                sizeGroupBox.Refresh();
            }
            
            // 更新系统设置组框
            if (systemGroupBox != null)
            {
                systemGroupBox.Text = LanguageManager.Instance.GetText("SystemSettingsGroupBox");
                systemGroupBox.Refresh();
            }
            
            // 更新表盘样式组框
            if (clockFaceGroupBox != null)
            {
                clockFaceGroupBox.Text = LanguageManager.Instance.GetText("ClockFaceGroupBox");
                clockFaceGroupBox.Refresh();
            }
            
            // 更新大小标签
            if (sizeLabel != null)
            {
                sizeLabel.Text = LanguageManager.Instance.GetText("ClockSizeLabel", clockSize);
            }
            
            // 更新震动时长标签
            if (vibrationDurationLabel != null)
            {
                vibrationDurationLabel.Text = LanguageManager.Instance.GetText("VibrationDurationLabel");
            }
            
            // 更新上传表盘按钮
            if (uploadClockFaceButton != null)
            {
                uploadClockFaceButton.Text = LanguageManager.Instance.GetText("UploadClockFaceButton");
            }
            
            // 更新预览表盘按钮
            if (previewClockFaceButton != null)
            {
                previewClockFaceButton.Text = LanguageManager.Instance.GetText("PreviewClockFaceButton");
            }
            
            // 更新恢复默认表盘按钮
            if (removeClockFaceButton != null)
            {
                removeClockFaceButton.Text = LanguageManager.Instance.GetText("RemoveClockFaceButton");
            }
            
            // 更新上传闹铃按钮
            if (uploadAlarmSoundButton != null)
            {
                uploadAlarmSoundButton.Text = LanguageManager.Instance.GetText("UploadAlarmSoundButton");
            }
            
            // 更新预览闹铃按钮
            if (previewCustomAlarmButton != null)
            {
                previewCustomAlarmButton.Text = LanguageManager.Instance.GetText("PreviewAlarmSoundButton");
            }
            
            // 更新恢复默认闹铃按钮
            if (removeAlarmSoundButton != null)
            {
                removeAlarmSoundButton.Text = LanguageManager.Instance.GetText("RemoveAlarmSoundButton");
            }
            
            // 更新开机自启动复选框
            if (autostartCheckBox != null)
            {
                autostartCheckBox.Text = LanguageManager.Instance.GetText("AutoStartCheckBox");
            }
            
            // 更新闹钟设置标题
            if (alarmGroupBox != null)
            {
                alarmGroupBox.Text = LanguageManager.Instance.GetText("AlarmsSectionTitle");
            }
            
            // 更新添加闹钟按钮
            if (addAlarmButton != null)
            {
                addAlarmButton.Text = LanguageManager.Instance.GetText("AddAlarmButton");
            }
            
            // 更新添加测试闹钟按钮
            if (addTestAlarmButton != null)
            {
                addTestAlarmButton.Text = LanguageManager.Instance.GetText("AddTestAlarmButton");
            }
            
            // 更新预览震动按钮
            if (previewVibrationButton != null)
            {
                previewVibrationButton.Text = LanguageManager.Instance.GetText("PreviewVibrationButton");
            }
            
            // 更新预览闹铃声音按钮
            if (previewAlarmButton != null)
            {
                previewAlarmButton.Text = LanguageManager.Instance.GetText("PreviewAlarmButton");
            }
            
            // 更新保存按钮
            if (saveButton != null)
            {
                saveButton.Text = LanguageManager.Instance.GetText("SaveButton");
            }
            
            // 更新整点报时复选框
            if (hourlyChimeCheckBox != null)
            {
                hourlyChimeCheckBox.Text = LanguageManager.Instance.GetText("EnableHourlyChimeText");
            }
            
            // 更新表盘路径标签
            UpdateClockFacePathLabel();
            
            // 更新闹铃路径标签
            UpdateAlarmSoundPathLabel();
            
            // 更新现有闹钟的UI文本
            UpdateAlarmUIElements();
        }
        
        /// <summary>
        /// 更新现有闹钟的UI元素文本
        /// </summary>
        private void UpdateAlarmUIElements()
        {
            if (alarmsFlowLayoutPanel != null)
            {
                foreach (Control control in alarmsFlowLayoutPanel.Controls)
                {
                    if (control is Panel alarmPanel)
                    {
                        foreach (Control childControl in alarmPanel.Controls)
                        {
                            if (childControl is CheckBox checkBox && checkBox.Text == "启用")
                            {
                                checkBox.Text = LanguageManager.Instance.GetText("AlarmEnabledText");
                            }
                            else if (childControl is Button button && button.Text == "删除")
                            {
                                button.Text = LanguageManager.Instance.GetText("AlarmDeleteButton");
                            }
                        }
                    }
                }
            }
        }

        private void InitializeSettings()
        {
            // 设置大小滑块
            sizeTrackBar.Minimum = 100;
            sizeTrackBar.Maximum = 400;
            sizeTrackBar.Value = clockSize;
            sizeLabel.Text = LanguageManager.Instance.GetText("ClockSizeLabel", clockSize);
            
            // 初始化组框文本
            if (sizeGroupBox != null)
            {
                sizeGroupBox.Text = LanguageManager.Instance.GetText("ClockSizeGroupBox");
            }
            
            if (systemGroupBox != null)
            {
                systemGroupBox.Text = LanguageManager.Instance.GetText("SystemSettingsGroupBox");
            }
            
            if (clockFaceGroupBox != null)
            {
                clockFaceGroupBox.Text = LanguageManager.Instance.GetText("ClockFaceGroupBox");
            }

            // 设置开机自启动复选框的初始状态
            autostartCheckBox.Checked = AutoStartManager.IsAutoStartEnabled();

            // 加载震动时长和表盘样式设置
            var settings = settingsManager.LoadSettings();
            if (settings != null)
            {
                vibrationDurationNumericUpDown.Value = settings.VibrationDuration;
                alarmManager.VibrationDuration = settings.VibrationDuration;
                

                
                // 加载自定义表盘图片路径
                customClockFacePath = settings.CustomClockFacePath;
                UpdateClockFacePathLabel();
                
                // 加载自定义闹铃声音路径
                customAlarmSoundPath = settings.CustomAlarmSoundPath;
                UpdateAlarmSoundPathLabel();

                // 加载整点报时设置
                hourlyChimeCheckBox.Checked = settings.EnableHourlyChime;
                
                // 加载闹铃广告设置
                if (alarmAdCheckBox != null)
                {
                    alarmAdCheckBox.Checked = settings.EnableAlarmAd;
                }
            }

            // 加载闹钟设置
            LoadAlarmSettings();
        }

        private void UpdateClockFacePathLabel()
        {
            if (!string.IsNullOrEmpty(customClockFacePath) && File.Exists(customClockFacePath))
            {
                string fileName = Path.GetFileName(customClockFacePath);
                clockFacePathLabel.Text = LanguageManager.Instance.GetText("CurrentCustomClockFace", fileName);
            }
            else
            {
                clockFacePathLabel.Text = LanguageManager.Instance.GetText("CurrentDefaultClockFace");
            }
        }

        private void UpdateAlarmSoundPathLabel()
        {
            if (!string.IsNullOrEmpty(customAlarmSoundPath) && File.Exists(customAlarmSoundPath))
            {
                string fileName = Path.GetFileName(customAlarmSoundPath);
                alarmSoundPathLabel.Text = LanguageManager.Instance.GetText("CurrentCustomAlarmSound", fileName);
            }
            else
            {
                alarmSoundPathLabel.Text = LanguageManager.Instance.GetText("CurrentDefaultAlarmSound");
            }
        }

        private void uploadClockFaceButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp;*.gif|所有文件|*.*";
                openFileDialog.Title = "选择表盘图片";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 验证选择的文件是否为有效的图片
                        using (Image testImage = Image.FromFile(openFileDialog.FileName))
                        {
                            customClockFacePath = openFileDialog.FileName;
                            UpdateClockFacePathLabel();
                            MessageBox.Show("表盘图片选择成功！点击预览按钮查看效果。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("选择的文件不是有效的图片格式。" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void uploadAlarmSoundButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "音频文件|*.mp3;*.wav;*.ogg;*.flac|所有文件|*.*";
                openFileDialog.Title = "选择闹铃声音";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 保存选中的音频路径
                        customAlarmSoundPath = openFileDialog.FileName;
                        UpdateAlarmSoundPathLabel();
                        MessageBox.Show("闹铃声音选择成功！点击预览按钮试听效果。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("选择的文件不是有效的音频格式。" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void previewClockFaceButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(customClockFacePath) && File.Exists(customClockFacePath))
            {
                // 临时设置自定义表盘路径并刷新时钟显示
                parentForm.SetCustomClockFacePath(customClockFacePath);
                parentForm.Invalidate();
                MessageBox.Show("表盘样式已应用，点击保存按钮永久保存此设置。", "预览成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先上传一个有效的表盘图片。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void removeClockFaceButton_Click(object sender, EventArgs e)
        {
            customClockFacePath = null;
            UpdateClockFacePathLabel();
            parentForm.SetCustomClockFacePath(null);
            parentForm.Invalidate();
            MessageBox.Show("已恢复默认表盘样式。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void previewCustomAlarmButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(customAlarmSoundPath))
                {
                    // 如果没有自定义闹铃，则使用默认闹铃预览
                    System.Media.SystemSounds.Beep.Play();
                    MessageBox.Show("播放默认闹铃声音。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // 预览自定义闹铃声音
                    WaveOutEvent player = new WaveOutEvent();
                    AudioFileReader audioFileReader = new AudioFileReader(customAlarmSoundPath);
                    player.Init(audioFileReader);
                    player.Play();
                    
                    // 播放3秒钟后停止
                    Task.Delay(3000).ContinueWith(t =>
                    {
                        player.Stop();
                        player.Dispose();
                        audioFileReader.Dispose();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    
                    MessageBox.Show("正在播放自定义闹铃声音...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("播放闹铃声音时出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void removeAlarmSoundButton_Click(object sender, EventArgs e)
        {
            customAlarmSoundPath = null;
            UpdateAlarmSoundPathLabel();
            MessageBox.Show("已恢复默认闹铃声音。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadAlarmSettings()
        {
            // 加载已保存的闹钟设置
            var alarms = alarmManager.LoadAlarms();
            if (alarms != null)
            {
                foreach (var alarm in alarms)
                {
                    AddAlarmToUI(alarm);
                }
            }
        }

        private void sizeTrackBar_Scroll(object sender, EventArgs e)
        {
            clockSize = sizeTrackBar.Value;
            sizeLabel.Text = LanguageManager.Instance.GetText("ClockSizeLabel", clockSize);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // 保存震动时长、表盘样式和闹铃声音设置
            var settings = settingsManager.LoadSettings() ?? new AppSettings();

            settings.VibrationDuration = (int)vibrationDurationNumericUpDown.Value;
            settings.CustomClockFacePath = customClockFacePath;
            settings.CustomAlarmSoundPath = customAlarmSoundPath;
            settings.Language = languageComboBox.SelectedIndex == 1 ? "en-US" : "zh-CN"; // 保存用户选择的语言
            settings.EnableHourlyChime = hourlyChimeCheckBox.Checked; // 保存整点报时设置
            settings.EnableAlarmAd = alarmAdCheckBox.Checked; // 保存闹铃广告设置
            settingsManager.SaveSettings(settings);


            // 应用设置
            parentForm.UpdateClockSize(clockSize);
            parentForm.SetCustomClockFacePath(customClockFacePath);
            
            // 将整点报时设置传递给AlarmManager
            alarmManager.EnableHourlyChime = hourlyChimeCheckBox.Checked;
            
            this.Close();
        }

        private void addAlarmButton_Click(object sender, EventArgs e)
        {
            int hour = (int)alarmHourNumericUpDown.Value;
            int minute = (int)alarmMinuteNumericUpDown.Value;
            bool isEnabled = true;

            Alarm alarm = new Alarm
            {
                Hour = hour,
                Minute = minute,
                IsEnabled = isEnabled
            };

            alarmManager.AddAlarm(alarm);
            AddAlarmToUI(alarm);
        }

        private void AddAlarmToUI(Alarm alarm)
        {
            Panel alarmPanel = new Panel
            {
                Width = alarmsFlowLayoutPanel.Width - 20,
                Height = 30,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label timeLabel = new Label
            {
                Text = $"{alarm.Hour:D2}:{alarm.Minute:D2}",
                Location = new Point(5, 5),
                Width = 80
            };

            CheckBox enableCheckBox = new CheckBox
            {
                Checked = alarm.IsEnabled,
                Location = new Point(90, 5),
                Text = LanguageManager.Instance.GetText("AlarmEnabledText")
            };
            enableCheckBox.CheckedChanged += (s, e) =>
            {
                alarm.IsEnabled = enableCheckBox.Checked;
                alarmManager.UpdateAlarm(alarm);
            };

            Button deleteButton = new Button
            {
                Text = LanguageManager.Instance.GetText("AlarmDeleteButton"),
                Location = new Point(alarmPanel.Width - 85, 3),
                Width = 80,
                Height = 24
            };
            deleteButton.Click += (s, e) =>
            {
                alarmManager.RemoveAlarm(alarm);
                alarmsFlowLayoutPanel.Controls.Remove(alarmPanel);
            };

            alarmPanel.Controls.Add(timeLabel);
            alarmPanel.Controls.Add(enableCheckBox);
            alarmPanel.Controls.Add(deleteButton);
            alarmsFlowLayoutPanel.Controls.Add(alarmPanel);
        }

        private void autostartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AutoStartManager.SetAutoStart(autostartCheckBox.Checked);
        }

        // 添加一个测试闹钟的方法，设置1分钟后触发，方便用户测试
        private void addTestAlarmButton_Click(object sender, EventArgs e)
        {
            // 获取当前时间并加1分钟
            DateTime now = DateTime.Now;
            DateTime testAlarmTime = now.AddMinutes(1);
            
            Alarm testAlarm = new Alarm
            {
                Hour = testAlarmTime.Hour,
                Minute = testAlarmTime.Minute,
                IsEnabled = true
            };
            
            alarmManager.AddAlarm(testAlarm);
            AddAlarmToUI(testAlarm);
            
            MessageBox.Show($"已添加测试闹钟，将在1分钟后触发（时间：{testAlarmTime.Hour:D2}:{testAlarmTime.Minute:D2}）", "测试闹钟", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 预览时钟窗口震动效果的方法
        private void previewVibrationButton_Click(object sender, EventArgs e)
        {
            // 检查父窗口（ClockForm）是否存在
            if (parentForm != null)
            {
                // 获取用户设置的震动时长并设置到AlarmManager
                int duration = (int)vibrationDurationNumericUpDown.Value;
                alarmManager.VibrationDuration = duration;
                
                // 触发时钟窗口震动效果
                parentForm.TriggerVibration();
            }
        }
        
        /// <summary>
        /// 应用窗口震动效果
        /// </summary>

        
        // 预览闹铃声音的方法
        private async void previewAlarmButton_Click(object sender, EventArgs e)
        {
            string soundFilePath = @"c:\Users\ZZZ\Documents\MyProjects\DesktopClockWiget\Resource\relaxing-guitar-loop.mp3";
            WaveOutEvent? player = null;
            AudioFileReader? audioFileReader = null;
            
            try
            {
                // 更改按钮文本，表示正在播放
                var originalText = previewAlarmButton.Text;
                previewAlarmButton.Text = "正在播放...";
                previewAlarmButton.Enabled = false;
                
                if (File.Exists(soundFilePath))
                {
                    // 获取文件扩展名
                    string extension = Path.GetExtension(soundFilePath).ToLower();
                    
                    // 支持的音频格式列表
                    string[] supportedFormats = { ".mp3", ".wav", ".flac", ".aac", ".ogg" };
                    
                    if (supportedFormats.Contains(extension))
                    {
                        // 使用NAudio播放各种格式的音频文件
                        player = new WaveOutEvent();
                        audioFileReader = new AudioFileReader(soundFilePath);
                        player.Init(audioFileReader);
                        player.Play();
                        
                        // 显示当前播放的文件信息
                        string fileName = Path.GetFileName(soundFilePath);
                        // 播放10秒钟后停止
                        await Task.Delay(10000);
                    }
                    else
                    {
                        // 不支持的音频格式
                        MessageBox.Show($"不支持的音频格式：{extension}。\n支持的格式有：{string.Join(", ", supportedFormats)}", 
                                    "格式不支持", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        await Task.Delay(1000);
                    }
                }
                else
                {
                    // 如果文件不存在，播放系统提示音
                    System.Media.SystemSounds.Beep.Play();
                    MessageBox.Show("声音文件不存在，已播放系统提示音。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                // 发生异常时显示错误消息
                MessageBox.Show($"播放闹铃声音时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 停止播放并清理资源
                if (player != null)
                {
                    try
                    {
                        player.Stop();
                        player.Dispose();
                    }
                    catch { }
                }
                
                if (audioFileReader != null)
                {
                    try
                    {
                        audioFileReader.Dispose();
                    }
                    catch { }
                }
                
                // 恢复按钮状态
                previewAlarmButton.Text = "预览闹铃声音";
                previewAlarmButton.Enabled = true;
            }
        }

        // Windows API 导入
        [DllImport("user32.dll")]
        private static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 设置窗口圆角样式
        /// </summary>
        private void SetWindowRoundedCorners()
        {
            // 计算圆角区域，圆角半径为20px
            int radius = 20;
            IntPtr hRgn = CreateRoundRectRgn(0, 0, this.Width, this.Height, radius, radius);
            SetWindowRgn(this.Handle, hRgn, true);
            DeleteObject(hRgn); // 释放资源
        }
    }
}