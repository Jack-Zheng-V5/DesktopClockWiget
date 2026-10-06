using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;  // 用于DllImport

namespace DesktopClockWidget
{
    public partial class ClockForm : Form
    {
        // 时钟相关变量
        private int clockSize = 200;
        private Point mouseOffset;
        private bool isMouseDown = false;
        private System.Timers.Timer clockTimer;
        private AlarmManager alarmManager;
        private SettingsManager settingsManager;
        private AdManager adManager;
        
        // 供SettingsForm访问的AlarmManager属性
        public AlarmManager AlarmManager { get { return alarmManager; } }
        
        // 设置自定义表盘路径并立即刷新
        public void SetCustomClockFacePath(string? path)
        {
            // 保存设置
            var settings = settingsManager.LoadSettings() ?? new AppSettings();
            settings.CustomClockFacePath = path;
            settingsManager.SaveSettings(settings);
            
            // 立即刷新表盘
            this.Invalidate();
        }

        public ClockForm()
        {
            InitializeComponent();
            InitializeClock();
        }

        private void InitializeClock()
        {
            // 设置窗口样式
            this.FormBorderStyle = FormBorderStyle.None;
            // 使用特定颜色作为透明键
            this.BackColor = Color.Magenta;  // 使用洋红色作为透明色
            this.TransparencyKey = Color.Magenta;  // 设置透明键
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.Size = new Size(clockSize, clockSize);
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - clockSize / 2, 
                                     Screen.PrimaryScreen.Bounds.Height / 2 - clockSize / 2);
            
            // 设置圆角样式
            SetWindowRoundedCorners();

            // 初始化管理器
            settingsManager = new SettingsManager();
            alarmManager = new AlarmManager(this);
            adManager = new AdManager(this);

            // 加载设置
            LoadSettings();

            // 设置定时器
            clockTimer = new System.Timers.Timer(1000);
            clockTimer.Elapsed += TimerElapsed;
            clockTimer.Start();

            // 添加右键菜单
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem settingsMenuItem = new ToolStripMenuItem(LanguageManager.Instance.GetText("SettingsMenuItem"));
            settingsMenuItem.Click += SettingsMenuItem_Click;
            
            // 添加测试广告按钮菜单项
            //ToolStripMenuItem testAdMenuItem = new ToolStripMenuItem(LanguageManager.Instance.GetText("TestAdMenuItem"));
            //testAdMenuItem.Click += TestAdMenuItem_Click;
            
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem(LanguageManager.Instance.GetText("ExitMenuItem"));
            exitMenuItem.Click += ExitMenuItem_Click;
            
            contextMenu.Items.Add(settingsMenuItem);
            //contextMenu.Items.Add(testAdMenuItem);
            contextMenu.Items.Add(exitMenuItem);
            this.ContextMenuStrip = contextMenu;
        }

        private void LoadSettings()
        {
            var settings = settingsManager.LoadSettings();
            if (settings != null)
            {
                clockSize = settings.ClockSize;
                this.Size = new Size(clockSize, clockSize);
                if (settings.Location != Point.Empty)
                {
                    this.Location = settings.Location;
                }
                // 传递整点报时设置给AlarmManager
                alarmManager.EnableHourlyChime = settings.EnableHourlyChime;
            }
        }

        private void SaveSettings()
        {
            // 先加载已有的设置，保留其他设置项
            var settings = settingsManager.LoadSettings() ?? new AppSettings();
            settings.ClockSize = clockSize;
            settings.Location = this.Location;
            // 保存整点报时设置
            settings.EnableHourlyChime = alarmManager.EnableHourlyChime;
            settingsManager.SaveSettings(settings);
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                Invalidate();
                // 检查闹钟
                alarmManager.CheckAlarms();
                
                // 检查整点报时
                DateTime now = DateTime.Now;
                if (now.Minute == 0 && now.Second == 0 && alarmManager.EnableHourlyChime)
                {
                    // 整点时刻，触发闹铃提醒和震动反馈
                    alarmManager.TriggerHourlyChime(this);
                }
            });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawClock(e.Graphics);
        }

        private void DrawClock(Graphics g)
        {
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;
            int radius = Math.Min(centerX, centerY) - 10;
            int cornerRadius = 30; // 圆角半径

            // 使用圆角矩形作为表盘背景
            try
            {
                // 创建圆角矩形路径
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                int rectWidth = radius * 2;
                int rectHeight = radius * 2;
                int rectX = centerX - radius;
                int rectY = centerY - radius;
                
                // 添加圆角矩形到路径
                path.AddArc(rectX, rectY, cornerRadius * 2, cornerRadius * 2, 180, 90);
                path.AddArc(rectX + rectWidth - cornerRadius * 2, rectY, cornerRadius * 2, cornerRadius * 2, 270, 90);
                path.AddArc(rectX + rectWidth - cornerRadius * 2, rectY + rectHeight - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                path.AddArc(rectX, rectY + rectHeight - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                path.CloseFigure();

                // 优先使用用户自定义的表盘图片路径
                string? imagePath = settingsManager?.LoadSettings()?.CustomClockFacePath;
                
                // 如果没有自定义路径或自定义路径不存在，则使用默认路径
                if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                {
                    imagePath = Path.Combine(Application.StartupPath, "Resource", "ClockBackground.jpeg");
                }
                
                if (File.Exists(imagePath))
                {
                    using (Image clockImage = Image.FromFile(imagePath))
                    {
                        // 设置裁剪区域为圆角矩形
                        g.SetClip(path);
                        
                        // 计算图片的缩放比例并绘制
                        float scale = Math.Min((float)radius * 2 / clockImage.Width, (float)radius * 2 / clockImage.Height);
                        int scaledWidth = (int)(clockImage.Width * scale);
                        int scaledHeight = (int)(clockImage.Height * scale);
                        g.DrawImage(clockImage, 
                            centerX - scaledWidth / 2, centerY - scaledHeight / 2, 
                            scaledWidth, scaledHeight);
                        
                        // 恢复默认裁剪区域
                        g.ResetClip();
                    }
                }
                else
                {
                    // 如果图片不存在，绘制默认的圆角表盘
                    using (Brush faceBrush = new SolidBrush(Color.LightGray))
                    {
                        g.FillPath(faceBrush, path);
                    }
                    using (Pen borderPen = new Pen(Color.Black, 2))
                    {
                        g.DrawPath(borderPen, path);
                    }
                    DrawClockMarkings(g, centerX, centerY, radius);
                }
                
                // 释放路径资源
                path.Dispose();
            }
            catch (Exception _)
            {
                // 绘制默认圆角表盘作为备用
                int backupCornerRadius = 30; // 圆角半径
                System.Drawing.Drawing2D.GraphicsPath? path = null;
                try
                {
                    // 创建圆角矩形路径
                    path = new System.Drawing.Drawing2D.GraphicsPath();
                    int rectWidth = radius * 2;
                    int rectHeight = radius * 2;
                    int rectX = centerX - radius;
                    int rectY = centerY - radius;
                    
                    // 添加圆角矩形到路径
                    path.AddArc(rectX, rectY, backupCornerRadius * 2, backupCornerRadius * 2, 180, 90);
                    path.AddArc(rectX + rectWidth - backupCornerRadius * 2, rectY, backupCornerRadius * 2, backupCornerRadius * 2, 270, 90);
                    path.AddArc(rectX + rectWidth - backupCornerRadius * 2, rectY + rectHeight - backupCornerRadius * 2, backupCornerRadius * 2, backupCornerRadius * 2, 0, 90);
                    path.AddArc(rectX, rectY + rectHeight - backupCornerRadius * 2, backupCornerRadius * 2, backupCornerRadius * 2, 90, 90);
                    path.CloseFigure();

                    using (Brush faceBrush = new SolidBrush(Color.LightGray))
                    {
                        g.FillPath(faceBrush, path);
                    }
                    using (Pen borderPen = new Pen(Color.Black, 2))
                    {
                        g.DrawPath(borderPen, path);
                    }
                    DrawClockMarkings(g, centerX, centerY, radius);
                }
                finally
                {
                    // 确保释放路径资源
                    if (path != null)
                    {
                        path.Dispose();
                    }
                }
            }

            // 获取当前时间
            DateTime now = DateTime.Now;
            float hourAngle = (now.Hour % 12 + now.Minute / 60f) * 30f - 90f;
            float minuteAngle = now.Minute * 6f - 90f;
            float secondAngle = now.Second * 6f - 90f;

            // 绘制时针、分针、秒针
            DrawHand(g, centerX, centerY, radius * 0.5f, hourAngle, 4, Color.Black);
            DrawHand(g, centerX, centerY, radius * 0.7f, minuteAngle, 3, Color.DarkBlue);
            DrawHand(g, centerX, centerY, radius * 0.8f, secondAngle, 1, Color.Red);

            // 绘制中心点
            using (Brush centerBrush = new SolidBrush(Color.Black))
            {
                g.FillEllipse(centerBrush, centerX - 5, centerY - 5, 10, 10);
            }
        }

        private void DrawClockMarkings(Graphics g, int centerX, int centerY, int radius)
        {
            for (int i = 0; i < 12; i++)
            {
                float angle = i * 30f - 90f;
                float x1 = centerX + (float)Math.Cos(angle * Math.PI / 180) * radius;
                float y1 = centerY + (float)Math.Sin(angle * Math.PI / 180) * radius;
                float x2 = centerX + (float)Math.Cos(angle * Math.PI / 180) * (radius - 15);
                float y2 = centerY + (float)Math.Sin(angle * Math.PI / 180) * (radius - 15);

                using (Pen hourPen = new Pen(Color.Black, 3))
                {
                    g.DrawLine(hourPen, x1, y1, x2, y2);
                }

                // 绘制数字
                float numberX = centerX + (float)Math.Cos(angle * Math.PI / 180) * (radius - 30);
                float numberY = centerY + (float)Math.Sin(angle * Math.PI / 180) * (radius - 30);
                string hourNumber = (i == 0 ? 12 : i).ToString();
                SizeF textSize = g.MeasureString(hourNumber, new Font("Arial", 10, FontStyle.Bold));
                g.DrawString(hourNumber, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 
                            numberX - textSize.Width / 2, numberY - textSize.Height / 2);
            }
        }

        private void DrawHand(Graphics g, int centerX, int centerY, float length, float angle, int width, Color color)
        {
            float x = centerX + (float)Math.Cos(angle * Math.PI / 180) * length;
            float y = centerY + (float)Math.Sin(angle * Math.PI / 180) * length;

            using (Pen handPen = new Pen(color, width))
            {
                g.DrawLine(handPen, centerX, centerY, x, y);
            }
        }

        // 鼠标事件处理（拖动功能）
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                mouseOffset = new Point(e.X, e.Y);
                isMouseDown = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isMouseDown)
            {
                Point newLocation = this.Location;
                newLocation.X += e.X - mouseOffset.X;
                newLocation.Y += e.Y - mouseOffset.Y;
                this.Location = newLocation;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
                SaveSettings(); // 保存位置设置
            }
        }
        
        /// <summary>
        /// 处理鼠标点击事件，当闹铃响起时点击表盘可以停止闹铃
        /// </summary>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            // 当用户点击表盘时，尝试停止闹铃
            alarmManager.StopAlarm();
        }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(clockSize, this);
            settingsForm.ShowDialog();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Application.Exit();
        }
        
        /// <summary>
        /// 测试广告按钮点击事件处理程序
        /// </summary>
        private void TestAdMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 使用已初始化的AdManager实例
                if (adManager != null)
                {
                    adManager.ShowAd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("测试广告时出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 供设置界面调用的方法，用于更新时钟大小
        public void UpdateClockSize(int newSize)
        {
            clockSize = newSize;
            this.Size = new Size(clockSize, clockSize);
            SaveSettings();
        }
        
        // 供手动触发时钟窗口震动效果的方法（用于测试）
        public void TriggerVibration()
        {
            alarmManager.TriggerVibration(this);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            
            // 如果是用户关闭窗口，而不是应用程序退出
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // 保存设置
                SaveSettings();
                
                // 取消关闭事件
                e.Cancel = true;
                
                // 隐藏窗口而不是关闭
                this.Hide();
            }
            else
            {
                // 如果是应用程序退出，则释放资源
                SaveSettings();
                clockTimer.Stop();
                clockTimer.Dispose();
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

        // 当窗口大小改变时，重新设置圆角
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetWindowRoundedCorners();
        }
        
        /// <summary>
        /// 更新语言UI
        /// </summary>
        public void UpdateLanguageUI()
        {
            if (this.ContextMenuStrip != null)
            {
                foreach (ToolStripMenuItem item in this.ContextMenuStrip.Items)
                {
                    // 根据原文本确定对应的翻译键
                    if (item.Text == "Settings" || item.Text == "设置")
                    {
                        item.Text = LanguageManager.Instance.GetText("SettingsMenuItem");
                    }
                    else if (item.Text == "Exit" || item.Text == "退出")
                    {
                        item.Text = LanguageManager.Instance.GetText("ExitMenuItem");
                    }
                }
            }
        }
    }
}