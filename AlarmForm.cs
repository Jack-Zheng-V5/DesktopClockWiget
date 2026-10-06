using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Threading;
using NAudio.Wave; // 用于NAudio音频播放
using System.Runtime.InteropServices;  // 用于DllImport

namespace DesktopClockWidget
{
    public partial class AlarmForm : Form
    {
        private Alarm alarm;

        public AlarmForm(Alarm alarm)
        {
            InitializeComponent();
            this.alarm = alarm;
            InitializeAlarmForm();
            
            // 设置圆角样式
            SetWindowRoundedCorners();
        }

        private WaveOutEvent soundPlayer; // 音频播放器
        private AudioFileReader audioFileReader; // 音频文件读取器

        private void InitializeAlarmForm()
        {
            // 设置窗口属性，确保在最上层显示
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(300, 200);
            this.Text = "闹钟响了！";

            // 设置时间显示
            timeLabel.Text = $"{alarm.Hour:D2}:{alarm.Minute:D2}";
            timeLabel.Font = new Font("Arial", 36, FontStyle.Bold);
            timeLabel.TextAlign = ContentAlignment.MiddleCenter;
            timeLabel.Dock = DockStyle.Top;
            timeLabel.Height = 100;

            // 设置关闭按钮
            closeButton.Text = "关闭";
            closeButton.Size = new Size(100, 40);
            closeButton.Font = new Font("Arial", 12);
            closeButton.Click += CloseButton_Click;

            // 设置按钮容器
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.Controls.Add(closeButton);
            buttonPanel.Padding = new Padding(10);
            closeButton.Anchor = AnchorStyles.None;

            // 添加控件到表单
            this.Controls.Add(timeLabel);
            this.Controls.Add(buttonPanel);

            // 设置FormClosed事件处理程序
            this.FormClosed += AlarmForm_FormClosed;

            // 启动窗口抖动效果
            // 直接在UI线程中启动抖动，不需要等待Shown事件
            StartShakeEffect();

            // 播放闹铃声音
            PlayAlarmSound();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 播放闹铃声音
        /// </summary>
        private void PlayAlarmSound()
        {
            // 优先使用用户自定义的闹铃声音路径
            string soundFilePath = null;
            
            // 使用SettingsManager获取自定义闹铃声音路径
            try
            {
                SettingsManager settingsManager = new SettingsManager();
                var settings = settingsManager.LoadSettings();
                if (settings != null && !string.IsNullOrEmpty(settings.CustomAlarmSoundPath) && File.Exists(settings.CustomAlarmSoundPath))
                {
                    soundFilePath = settings.CustomAlarmSoundPath;
                }
            }
            catch { }
            
            // 如果没有自定义路径或自定义路径不存在，则使用默认路径
            if (string.IsNullOrEmpty(soundFilePath) || !File.Exists(soundFilePath))
            {
                soundFilePath = Path.Combine(Application.StartupPath, "Resource", "relaxing-guitar-loop.mp3");
            }
            
            try
            {
                if (File.Exists(soundFilePath))
                {
                    // 支持的音频格式列表
                    string[] supportedFormats = { ".mp3", ".wav", ".flac", ".aac", ".ogg" };
                    string extension = Path.GetExtension(soundFilePath).ToLower();
                    
                    if (supportedFormats.Contains(extension))
                    {
                        // 使用NAudio播放各种格式的音频文件
                        soundPlayer = new WaveOutEvent();
                        audioFileReader = new AudioFileReader(soundFilePath);
                        soundPlayer.Init(audioFileReader);
                        soundPlayer.Play();
                    }
                    else
                    {
                        // 不支持的音频格式，播放系统提示音
                        PlaySystemBeep();
                    }
                }
                else
                {
                    // 如果文件不存在，播放系统提示音
                    PlaySystemBeep();
                }
            }
            catch (Exception ex)
            {
                // 发生异常时回退到系统提示音
                PlaySystemBeep();
            }
        }
        
        /// <summary>
        /// 播放系统提示音
        /// </summary>
        private void PlaySystemBeep()
        {
            // 为了确保声音能被听到，连续播放多次
            for (int i = 0; i < 3; i++)
            {
                System.Media.SystemSounds.Beep.Play();
                System.Threading.Thread.Sleep(300);
            }
        }
        
        /// <summary>
        /// 窗口关闭事件处理程序，清理音频资源
        /// </summary>
        private void AlarmForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (soundPlayer != null)
                {
                    soundPlayer.Stop();
                    soundPlayer.Dispose();
                    soundPlayer = null;
                }
                
                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                    audioFileReader = null;
                }
            }
            catch { }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 允许用户按ESC键关闭窗口
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 启动窗口抖动效果
        /// </summary>
        private void StartShakeEffect()
        {
            // 使用Timer来实现抖动效果
            System.Windows.Forms.Timer shakeTimer = new System.Windows.Forms.Timer();
            int shakeCount = 0;
            int maxShakes = 40; // 增加抖动次数，使其更明显
            int shakeDistance = 8; // 增加抖动距离
            Point originalLocation = this.Location;

            shakeTimer.Interval = 30; // 抖动频率（毫秒）
            shakeTimer.Tick += (sender, e) =>
            {
                if (shakeCount < maxShakes)
                {
                    // 根据奇偶次抖动决定方向，使用更复杂的抖动模式
                    int offsetX = (shakeCount % 2 == 0) ? shakeDistance : -shakeDistance;
                    int offsetY = (shakeCount % 4 < 2) ? shakeDistance : -shakeDistance;
                    
                    // 应用抖动偏移
                    this.Location = new Point(
                        originalLocation.X + offsetX,
                        originalLocation.Y + offsetY
                    );
                    
                    shakeCount++;
                }
                else
                {
                    // 抖动结束，恢复原位置并停止计时器
                    this.Location = originalLocation;
                    shakeTimer.Stop();
                    shakeTimer.Dispose();
                }
            };

            // 在UI线程中立即启动抖动效果
            shakeTimer.Start();
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