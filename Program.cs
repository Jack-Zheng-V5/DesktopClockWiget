using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

// 添加MG广告SDK命名空间引用
using MiracleGamesWin32;

namespace DesktopClockWidget
{
    /// <summary>
    /// SDK管理器，负责广告SDK的初始化和事件注册
    /// 采用单例模式确保SDK只被初始化和注册一次
    /// </summary>
    internal static class SdkManager
    {
        // 广告ID常量定义
        public const string InterstitialAdId = "1F6E06D520"; // 插屏广告ID
        public const string ExitAdId = "1F6E06D520"; // 退屏广告ID（可以使用与插屏广告相同的ID）
        public const string AppKey = "BEFDF23F58"; // 应用标识

        private static bool _sdkEventsRegistered; // 静态标志，确保SDK事件只被注册一次
        private static bool _sdkInitialized; // 静态标志，确保SDK只被初始化一次
        private static readonly object _lockObject = new object(); // 用于线程同步的锁对象
        private static bool _hasStaticInitBeenCalled = false; // 静态初始化是否已经完成

        /// <summary>
        /// 静态构造函数，确保只执行一次
        /// </summary>
        static SdkManager()
        {
            Debug.WriteLine("SdkManager static constructor called");
            LogToFile("SdkManager static constructor called");
        }

        /// <summary>
        /// 初始化SDK，确保只执行一次
        /// </summary>
        public static void Initialize()
        {
            StaticInitialize();
        }

        /// <summary>
        /// 静态初始化方法，确保只执行一次
        /// </summary>
        private static void StaticInitialize()
        {
            // 使用双重检查锁定模式确保只执行一次
            if (!_hasStaticInitBeenCalled)
            {
                lock (_lockObject)
                {
                    if (!_hasStaticInitBeenCalled)
                    {
                        Debug.WriteLine("SdkManager.StaticInitialize called");
                        LogToFile("SdkManager.StaticInitialize called");
                        
                        // 注册SDK事件
                        RegisterStaticSdkEvents();
                        
                        // 异步初始化SDK
                        Task.Run(async () => await InitializeStaticSdk());
                        
                        // 设置标志为已初始化
                        _hasStaticInitBeenCalled = true;
                    }
                }
            }
        }

        /// <summary>
        /// 静态方法，注册SDK事件
        /// </summary>
        private static void RegisterStaticSdkEvents()
        {
            string logMessage = "RegisterStaticSdkEvents method called";
            Debug.WriteLine(logMessage);
            LogToFile(logMessage);
            Console.WriteLine(logMessage);
            
            // 如果已经注册过SDK事件，直接返回
            if (_sdkEventsRegistered)
            {
                logMessage = "SDK events already registered. Returning.";
                Debug.WriteLine(logMessage);
                LogToFile(logMessage);
                Console.WriteLine(logMessage);
                return;
            }
            
            try
            {
                logMessage = "Attempting to register InitCompleteEvents";
                Debug.WriteLine(logMessage);
                LogToFile(logMessage);
                Console.WriteLine(logMessage);
                
                // 注册初始化完成事件
                ApplicationManager.InitCompleteEvents += OnStaticInitComplete;
                
                logMessage = "InitCompleteEvents registered successfully";
                Debug.WriteLine(logMessage);
                LogToFile(logMessage);
                Console.WriteLine(logMessage);

                //注册广告点击事件
                logMessage = "Attempting to register ClickAdvertEvents";
                Debug.WriteLine(logMessage);
                LogToFile(logMessage);
                AdvertManager.ClickAdvertEvents += OnStaticAdvertClick;
                logMessage = "ClickAdvertEvents registered successfully";
                Debug.WriteLine(logMessage);
                LogToFile(logMessage);

                //注册广告关闭事件
                logMessage = "Attempting to register CloseAdvertEvents";
                Debug.WriteLine(logMessage);
                LogToFile(logMessage);
                AdvertManager.CloseAdvertEvents += OnStaticAdvertClose;
                logMessage = "CloseAdvertEvents registered successfully";
                Debug.WriteLine(logMessage);
                LogToFile(logMessage);

                logMessage = "All SDK events registered successfully";
                Debug.WriteLine(logMessage);
                LogToFile(logMessage);
                Console.WriteLine(logMessage);
                
                // 设置标志为已注册
                _sdkEventsRegistered = true;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error registering SDK events: {ex.Message}";
                string detailsMessage = $"Exception details: {ex.ToString()}";
                string stackTraceMessage = $"Stack trace: {ex.StackTrace}";
                
                Debug.WriteLine(errorMessage);
                Debug.WriteLine(detailsMessage);
                Debug.WriteLine(stackTraceMessage);
                
                LogToFile(errorMessage);
                LogToFile(detailsMessage);
                LogToFile(stackTraceMessage);
            }
        }

        /// <summary>
        /// 静态方法，初始化SDK
        /// </summary>
        private static async Task InitializeStaticSdk()
        {
            Debug.WriteLine("SdkManager.InitializeStaticSdk called");
            LogToFile("SdkManager.InitializeStaticSdk called");
            
            // 如果SDK已经初始化，直接返回
            if (_sdkInitialized)
            {
                Debug.WriteLine("SDK already initialized. Returning.");
                return;
            }

            try
            {
                // 立即设置静态标志为已初始化，确保后续调用不会再次执行初始化逻辑
                _sdkInitialized = true;
                
                // 检查网络连接状态
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    string message = "Network not available. Skipping ad SDK initialization.";
                    Debug.WriteLine(message);
                    LogToFile(message);
                    return;
                }
                
                // 设置退屏广告ID
                AdvertManager.ExitAdvertKey = ExitAdId;
                
                // 初始化MG广告SDK
                string initMessage = $"Initializing MG Ad SDK with AppKey: {AppKey}";
                Debug.WriteLine(initMessage);
                LogToFile(initMessage);
                
                // 添加更详细的异常处理，直接捕获NullReferenceException
                try
                {
                    // 尝试初始化SDK
                    string setupMessage = "Calling ApplicationManager.SetupAsync with AppKey: " + AppKey;
                    Debug.WriteLine(setupMessage);
                    LogToFile(setupMessage);
                    Console.WriteLine(setupMessage);
                
                    // 直接等待SetupAsync完成，确保SDK初始化完成
                    await ApplicationManager.SetupAsync(AppKey);
                
                    string setupCompleteMessage = "ApplicationManager.SetupAsync completed.";
                    Debug.WriteLine(setupCompleteMessage);
                    LogToFile(setupCompleteMessage);
                    Console.WriteLine(setupCompleteMessage);
                
                    // 打开SDK日志以便调试
                    string logMessage = "Calling ApplicationManager.OpenMgLog(true)";
                    Debug.WriteLine(logMessage);
                    LogToFile(logMessage);
                
                    ApplicationManager.OpenMgLog(true);
                
                    string sdkStartedMessage = "Ad SDK initialization started.";
                    Debug.WriteLine(sdkStartedMessage);
                    LogToFile(sdkStartedMessage);
                
                    // 检查ApplicationManager是否存在，尽管它是类型不是实例
                    Type appManagerType = typeof(ApplicationManager);
                    if (appManagerType != null)
                    {
                        logMessage = "ApplicationManager type exists.";
                        Debug.WriteLine(logMessage);
                        LogToFile(logMessage);
                    
                        // 通过反射检查SetupAsync方法的签名
                        var setupMethod = appManagerType.GetMethod("SetupAsync", new Type[] { typeof(string) });
                        if (setupMethod != null)
                        {
                            string methodSignature = $"SetupAsync method signature: {setupMethod.ReturnType.Name} SetupAsync({string.Join(", ", setupMethod.GetParameters().Select(p => p.ParameterType.Name))})";
                            Debug.WriteLine(methodSignature);
                            LogToFile(methodSignature);
                        }
                        else
                        {
                            string methodNotFound = "SetupAsync method not found with string parameter.";
                            Debug.WriteLine(methodNotFound);
                            LogToFile(methodNotFound);
                        }
                    }
                
                    // 检查InitCompleteEvents事件是否存在
                    var initCompleteEvent = appManagerType.GetEvent("InitCompleteEvents");
                    if (initCompleteEvent != null)
                    {
                        logMessage = "InitCompleteEvents event exists.";
                        Debug.WriteLine(logMessage);
                        LogToFile(logMessage);
                    }
                }
                catch (NullReferenceException ex)
                {
                    string errorMessage = $"NullReferenceException in SDK initialization: {ex.Message}";
                    string detailsMessage = $"Exception details: {ex.ToString()}";
                    string stackTraceMessage = $"Stack trace: {ex.StackTrace}";
                    
                    Debug.WriteLine(errorMessage);
                    Debug.WriteLine(detailsMessage);
                    Debug.WriteLine(stackTraceMessage);
                    
                    LogToFile(errorMessage);
                    LogToFile(detailsMessage);
                    LogToFile(stackTraceMessage);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Other exception in SDK initialization: {ex.Message}";
                    string detailsMessage = $"Exception details: {ex.ToString()}";
                    string innerExceptionMessage = $"Inner exception: {ex.InnerException?.Message}";
                    
                    Debug.WriteLine(errorMessage);
                    Debug.WriteLine(detailsMessage);
                    Debug.WriteLine(innerExceptionMessage);
                    
                    LogToFile(errorMessage);
                    LogToFile(detailsMessage);
                    LogToFile(innerExceptionMessage);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"InitializeStaticSdk method exception: {ex.Message}";
                string detailsMessage = $"Exception details: {ex.ToString()}";
                
                Debug.WriteLine(errorMessage);
                Debug.WriteLine(detailsMessage);
                LogToFile(errorMessage);
                LogToFile(detailsMessage);
            }
        }

        /// <summary>
        /// SDK初始化完成回调（静态）
        /// </summary>
        private static void OnStaticInitComplete(object sender, MiracleGamesWin32.Result.InitResult args)
        {
            Debug.WriteLine("OnStaticInitComplete callback triggered!");
            LogToFile("OnStaticInitComplete callback triggered!");
            Console.WriteLine("OnStaticInitComplete callback triggered!");
        }
        
        /// <summary>
        /// 广告点击事件回调（静态）
        /// </summary>
        private static void OnStaticAdvertClick(object sender, string e)
        {
            Debug.WriteLine($"OnStaticAdvertClick callback triggered with parameter: {e}");
            LogToFile($"OnStaticAdvertClick callback triggered with parameter: {e}");
            Console.WriteLine($"OnStaticAdvertClick callback triggered with parameter: {e}");
        }
        
        /// <summary>
        /// 广告关闭事件回调（静态）
        /// </summary>
        private static void OnStaticAdvertClose(object sender, string e)
        {
            Debug.WriteLine($"OnStaticAdvertClose callback triggered with parameter: {e}");
            LogToFile($"OnStaticAdvertClose callback triggered with parameter: {e}");
            Console.WriteLine($"OnStaticAdvertClose callback triggered with parameter: {e}");
        }

        /// <summary>
        /// 记录日志到文件（静态方法，可在静态上下文中调用）
        /// </summary>
        private static void LogToFile(string message)
        {
            try
            {
                // 使用更可靠的路径，直接保存到项目根目录
                string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyProjects", "DesktopClockWiget", "ad_debug_log.txt");
                string logEntry = $"[{DateTime.Now}] {message}\n";
                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex)
            {
                // 避免日志记录本身导致异常
                Debug.WriteLine($"Error logging to file: {ex.Message}");
                Debug.WriteLine($"Exception details: {ex.ToString()}");
            }
        }
    }

    internal static class Program
    {
        /// <summary>
        /// 导入Windows API函数
        /// </summary>
        private static class NativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]
            public static extern IntPtr GetDC(IntPtr hWnd);

            [DllImport("user32.dll")]
            public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [DllImport("gdi32.dll")]
            public static extern int GetPixel(IntPtr hdc, int nXPos, int nYPos);
        }
        
        private static NotifyIcon? trayIcon;
        private static ClockForm? clockForm;
        private static System.Timers.Timer? trayIconUpdateTimer;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 初始化LanguageManager
            LanguageManager.Instance.Init();
            
            // 加载用户设置并应用语言
            SettingsManager settingsManager = new SettingsManager();
            var settings = settingsManager.LoadSettings();
            if (settings != null && !string.IsNullOrEmpty(settings.Language))
            {
                LanguageManager.Instance.SetLanguage(settings.Language);
            }
            
            ApplicationConfiguration.Initialize();
            
            // 创建并初始化系统托盘图标
            InitializeTrayIcon();
            
            // 创建时钟窗口，但不在任务栏显示
            clockForm = new ClockForm();
            clockForm.ShowInTaskbar = false;
            
            // 显示应用程序但不激活窗口
            clockForm.Show();
            
            // 设置托盘图标更新定时器
            InitializeTrayIconUpdateTimer();
            
            // 初始化广告SDK
            //SdkManager.Initialize();
            
            // 运行应用程序消息循环
            Application.Run();
        }
        
        /// <summary>
        /// 初始化系统托盘图标和相关菜单
        /// </summary>
        private static void InitializeTrayIcon()
        {
            trayIcon = new NotifyIcon
            {
                Icon = CreateClockIcon(),
                Text = "桌面时钟小工具",
                Visible = true
            };
            
            // 创建上下文菜单
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            
            // 添加显示时钟菜单项
            ToolStripMenuItem showClockMenuItem = new ToolStripMenuItem("显示时钟");
            showClockMenuItem.Click += ShowClockMenuItem_Click;
            contextMenu.Items.Add(showClockMenuItem);
            
            // 添加设置菜单项
            ToolStripMenuItem settingsMenuItem = new ToolStripMenuItem("设置");
            settingsMenuItem.Click += SettingsMenuItem_Click;
            contextMenu.Items.Add(settingsMenuItem);
            
            // 添加退出菜单项
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("退出");
            exitMenuItem.Click += ExitMenuItem_Click;
            contextMenu.Items.Add(exitMenuItem);
            
            // 将上下文菜单关联到托盘图标
            trayIcon.ContextMenuStrip = contextMenu;
            
            // 双击托盘图标显示时钟
            trayIcon.DoubleClick += ShowClockMenuItem_Click;
            
            // 单击托盘图标显示时钟
            trayIcon.Click += ShowClockMenuItem_Click;
            
            // 应用程序退出时清理资源
            Application.ApplicationExit += (sender, e) =>
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
            };
        }
        
        /// <summary>
        /// 显示时钟窗口
        /// </summary>
        private static void ShowClockMenuItem_Click(object sender, EventArgs e)
        {
            if (clockForm == null || clockForm.IsDisposed)
            {
                clockForm = new ClockForm();
                clockForm.ShowInTaskbar = false;
            }
            
            // 确保窗口不是最小化状态
            if (clockForm.WindowState == FormWindowState.Minimized)
            {
                clockForm.WindowState = FormWindowState.Normal;
            }
            
            // 显示窗口并置于前台
            clockForm.Show();
            clockForm.BringToFront();
            clockForm.Activate();
        }
        
        /// <summary>
        /// 打开设置窗口
        /// </summary>
        private static void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            if (clockForm == null || clockForm.IsDisposed)
            {
                clockForm = new ClockForm();
                clockForm.ShowInTaskbar = false;
            }
            
            SettingsForm settingsForm = new SettingsForm(clockForm.Size.Width, clockForm);
            settingsForm.ShowDialog();
        }
        
        /// <summary>
        /// 退出应用程序
        /// </summary>
        private static void ExitMenuItem_Click(object sender, EventArgs e)
        {
            // 停止托盘图标更新定时器
            if (trayIconUpdateTimer != null)
            {
                trayIconUpdateTimer.Stop();
                trayIconUpdateTimer.Dispose();
            }
            
            Application.Exit();
        }
        
        /// <summary>
        /// 初始化托盘图标更新定时器
        /// </summary>
        private static void InitializeTrayIconUpdateTimer()
        {
            trayIconUpdateTimer = new System.Timers.Timer(1000);
            trayIconUpdateTimer.Elapsed += UpdateTrayIcon;
            trayIconUpdateTimer.Start();
        }
        
        /// <summary>
        /// 更新托盘图标
        /// </summary>
        private static void UpdateTrayIcon(object sender, ElapsedEventArgs e)
        {
            if (trayIcon != null)
            {
                try
                {
                    // 在UI线程上更新图标
                    if (Application.OpenForms.Count > 0)
                    {
                        Application.OpenForms[0].Invoke((MethodInvoker)delegate
                        {
                            if (trayIcon != null)
                            {
                                Icon newIcon = CreateClockIcon();
                                Icon oldIcon = trayIcon.Icon;
                                trayIcon.Icon = newIcon;
                                oldIcon?.Dispose();
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    // 忽略错误，继续运行
                }
            }
        }
        
        /// <summary>
        /// 获取系统托盘的颜色
        /// </summary>
        /// <returns>系统托盘的背景颜色</returns>
        private static Color GetSystemTrayColor()
        {
            try
            {
                // 获取任务栏的句柄
                IntPtr taskbarHandle = NativeMethods.FindWindow("Shell_TrayWnd", null);
                if (taskbarHandle != IntPtr.Zero)
                {
                    // 获取任务栏的DC（设备上下文）
                    IntPtr dc = NativeMethods.GetDC(taskbarHandle);
                    if (dc != IntPtr.Zero)
                    {
                        // 获取任务栏右下角的像素颜色（近似代表系统托盘颜色）
                        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
                        
                        // 从右下角取点，避免图标干扰
                        int pixelColor = NativeMethods.GetPixel(dc, screenWidth - 20, screenHeight - 20);
                        
                        // 释放DC
                        NativeMethods.ReleaseDC(taskbarHandle, dc);
                        
                        // 转换为Color对象
                        return Color.FromArgb(pixelColor);
                    }
                }
            }
            catch (Exception)
            {
                // 发生异常时返回默认颜色
            }
            
            // 默认返回黑色（深色主题）
            return Color.Black;
        }
        
        /// <summary>
        /// 判断颜色是否为深色
        /// </summary>
        /// <param name="color">要判断的颜色</param>
        /// <returns>如果是深色则返回true，否则返回false</returns>
        private static bool IsDarkColor(Color color)
        {
            // 使用相对亮度公式计算颜色亮度
            // https://www.w3.org/WAI/GL/wiki/Relative_luminance
            double luminance = (0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B) / 255;
            return luminance < 0.5;
        }
        
        /// <summary>
        /// 创建包含时钟指针的图标
        /// </summary>
        private static Icon CreateClockIcon()
        {
            // 创建一个32x32的位图作为图标画布
            Bitmap bitmap = new Bitmap(32, 32);
            
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // 清空画布
                g.Clear(Color.Transparent);
                
                // 计算中心点和半径
                int centerX = bitmap.Width / 2;
                int centerY = bitmap.Height / 2;
                int radius = Math.Min(centerX, centerY) - 4;
                
                // 获取系统托盘颜色
                Color trayColor = GetSystemTrayColor();
                
                // 根据托盘颜色选择对比度高的背景色
                // 确保在深色模式下使用白色背景，浅色模式下使用黑色背景
                // 直接设置为深色模式，确保背景色为白色（根据用户反馈系统托盘为深色）
                bool isDarkMode = true; // 强制使用深色模式设置，确保图标背景为白色
                Color backgroundColor = isDarkMode ? Color.White : Color.Black;
                Color textColor = isDarkMode ? Color.Black : Color.White;
                
                // 绘制圆角背景（在表盘外侧），参考iPhone图标的圆角风格
                using (Brush squareBrush = new SolidBrush(backgroundColor))
                {
                    // 方形背景比圆形表盘稍大，但不超过整个画布
                    int squareSize = radius * 2 + 8;
                    int squareX = centerX - squareSize / 2;
                    int squareY = centerY - squareSize / 2;
                    int cornerRadius = squareSize / 5; // 设置圆角半径，参考iPhone图标的圆角比例
                    
                    // 创建圆角矩形路径
                    using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        // 添加左上角圆弧
                        path.AddArc(squareX, squareY, cornerRadius * 2, cornerRadius * 2, 180, 90);
                        // 添加右上角圆弧
                        path.AddArc(squareX + squareSize - cornerRadius * 2, squareY, cornerRadius * 2, cornerRadius * 2, 270, 90);
                        // 添加右下角圆弧
                        path.AddArc(squareX + squareSize - cornerRadius * 2, squareY + squareSize - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                        // 添加左下角圆弧
                        path.AddArc(squareX, squareY + squareSize - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                        // 闭合路径
                        path.CloseFigure();
                        
                        // 填充圆角矩形
                        g.FillPath(squareBrush, path);
                    }
                }
                
                // 绘制图标背景色，使用与托盘颜色对比度高的颜色
                using (Brush backgroundBrush = new SolidBrush(backgroundColor))
                {
                    g.FillEllipse(backgroundBrush, centerX - radius, centerY - radius, radius * 2, radius * 2);
                }
                
                // 绘制表盘边框，使用与背景色对比度高的颜色
                using (Pen borderPen = new Pen(textColor, 1))
                {
                    g.DrawEllipse(borderPen, centerX - radius, centerY - radius, radius * 2, radius * 2);
                }
                
                // 获取当前时间
                DateTime now = DateTime.Now;
                float hourAngle = (now.Hour % 12 + now.Minute / 60f) * 30f - 90f;
                float minuteAngle = now.Minute * 6f - 90f;
                float secondAngle = now.Second * 6f - 90f;
                
                // 绘制时针、分针、秒针
                DrawHand(g, centerX, centerY, radius * 0.5f, hourAngle, 2, textColor);
                DrawHand(g, centerX, centerY, radius * 0.7f, minuteAngle, 1, textColor);
                DrawHand(g, centerX, centerY, radius * 0.8f, secondAngle, 1, textColor);
                
                // 绘制中心点
                using (Brush centerBrush = new SolidBrush(textColor))
                {
                    g.FillEllipse(centerBrush, centerX - 2, centerY - 2, 4, 4);
                }
            }
            
            // 从位图创建图标
            return Icon.FromHandle(bitmap.GetHicon());
        }
        
        /// <summary>
        /// 绘制时钟指针
        /// </summary>
        private static void DrawHand(Graphics g, int centerX, int centerY, float length, float angle, int width, Color color)
        {
            float x = centerX + (float)Math.Cos(angle * Math.PI / 180) * length;
            float y = centerY + (float)Math.Sin(angle * Math.PI / 180) * length;
            
            using (Pen handPen = new Pen(color, width))
            {
                g.DrawLine(handPen, centerX, centerY, x, y);
            }
        }
    }
}