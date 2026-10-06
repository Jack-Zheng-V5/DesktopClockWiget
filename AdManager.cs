using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

// 添加MG广告SDK命名空间引用
using MiracleGamesWin32;
using Newtonsoft.Json.Linq;

namespace DesktopClockWidget
{
    public class AdManager
    {
        private readonly ClockForm _parentForm;
        private bool _isAdDisplaying; // 是否正在显示广告

        // 构造函数
        public AdManager(ClockForm parentForm)
        {
            _parentForm = parentForm;
            _isAdDisplaying = false;

            Debug.WriteLine("AdManager constructor called");
            Debug.WriteLine("Parent form: " + parentForm.Name);
            Console.WriteLine("AdManager constructor called");
            Console.WriteLine("Parent form: " + parentForm.Name);
        }

        // 显示广告（外部调用接口）
        public async Task ShowAdAsync()
        {
            // 调用插屏广告显示方法
            await ShowInterstitialAd();
        }

        // 显示广告（同步接口，用于非异步调用场景）
        public void ShowAd()
        {
            Debug.WriteLine("ShowAd() called");
            Debug.WriteLine($"_isAdDisplaying: {_isAdDisplaying}");
            
            try
            {
                // 直接调用同步广告显示方法，该方法内部已经包含了所有必要的检查
                ShowInterstitialAdSync();
            } catch (Exception ex)
            {
                Debug.WriteLine($"Error in ShowAd: {ex.Message}");
                Debug.WriteLine($"Exception details: {ex.ToString()}");
                _isAdDisplaying = false;
            }
        }

        // 同步显示插屏广告 - 内部使用异步处理
        private void ShowInterstitialAdSync()
        {
            // 使用Task.Run将异步操作包装在同步方法中
            Task.Run(async () => await ShowInterstitialAdAsync()).Wait();
        }
        
        // 异步显示插屏广告
        private async Task ShowInterstitialAdAsync()
        {
            // 检查是否可以显示广告
            if (_isAdDisplaying)
            {
                string message = "Ad not shown: already displaying";
                Debug.WriteLine(message);
                LogToFile(message);
                return;
            }

            try
            {
                _isAdDisplaying = true;
                
                string showMessage = $"Showing interstitial ad with ID: {SdkManager.InterstitialAdId}";
                Debug.WriteLine(showMessage);
                LogToFile(showMessage);
                
                try
                {
                    // 检查网络连接状态
                    if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    {
                        string networkMessage = "Network not available. Showing simulated ad instead.";
                        Debug.WriteLine(networkMessage);
                        LogToFile(networkMessage);
                        _isAdDisplaying = false;
                        SimulateInterstitialAd();
                        return;
                    }
                    
                    // 检查_parentForm是否为null
                    if (_parentForm == null)
                    {
                        string nullParentMessage = "Parent form is null. Showing simulated ad instead.";
                        Debug.WriteLine(nullParentMessage);
                        LogToFile(nullParentMessage);
                        _isAdDisplaying = false;
                        SimulateInterstitialAd();
                        return;
                    }
                    
                    // 调用MG广告SDK显示插屏广告
                    string sdkCallMessage = "Calling AdvertManager.OpenAdvert()";
                    Debug.WriteLine(sdkCallMessage);
                    LogToFile(sdkCallMessage);
                    
                    try
                    {
                        // 根据异常堆栈，OpenAdvert内部有异步操作，使用异步方式调用
                        await Task.Run(() => AdvertManager.OpenAdvert(_parentForm, SdkManager.InterstitialAdId));

                        string successMessage = "AdvertManager.OpenAdvert called successfully";
                        Debug.WriteLine(successMessage);
                        LogToFile(successMessage);
                    }
                    catch (NullReferenceException ex)
                    {
                        string sdkNullRefMessage = "NullReferenceException in AdvertManager.OpenAdvert - likely SDK internal issue with network request. Using simulated ad.";
                        Debug.WriteLine(sdkNullRefMessage);
                        Debug.WriteLine(ex.ToString());
                        LogToFile(sdkNullRefMessage);
                        LogToFile(ex.ToString());
                        
                        // SDK内部出错，使用模拟广告
                        _isAdDisplaying = false;
                        SimulateInterstitialAd();
                        return;
                    }
                    
                    // 由于注释了广告关闭事件，设置一个定时器在30秒后自动重置广告显示状态
                    System.Threading.Timer timer = new System.Threading.Timer(
                        (state) => 
                        {
                            if (_isAdDisplaying)
                            {
                                string timerMessage = "Ad display timer expired. Resetting ad display state.";
                                Debug.WriteLine(timerMessage);
                                LogToFile(timerMessage);
                                _isAdDisplaying = false;
                            }
                        },
                        null,
                        30000, // 30秒后执行
                        System.Threading.Timeout.Infinite);
                }
                catch (NullReferenceException ex)
                {
                    string errorMessage = $"NullReferenceException when showing ad: {ex.Message}";
                    string detailsMessage = $"Exception details: {ex.ToString()}";
                    string contextMessage = "This error likely occurs in the SDK's network request. Showing simulated ad instead.";
                    
                    Debug.WriteLine(errorMessage);
                    Debug.WriteLine(detailsMessage);
                    Debug.WriteLine(contextMessage);
                    
                    LogToFile(errorMessage);
                    LogToFile(detailsMessage);
                    LogToFile(contextMessage);
                    
                    // 如果SDK调用失败，回退到模拟广告
                    _isAdDisplaying = false;
                    SimulateInterstitialAd();
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Failed to show ad via SDK: {ex.Message}";
                    string detailsMessage = $"Exception details: {ex.ToString()}";
                    string innerExceptionMessage = $"Inner exception: {ex.InnerException?.Message}";
                    
                    Debug.WriteLine(errorMessage);
                    Debug.WriteLine(detailsMessage);
                    Debug.WriteLine(innerExceptionMessage);
                    
                    LogToFile(errorMessage);
                    LogToFile(detailsMessage);
                    LogToFile(innerExceptionMessage);
                    
                    // 如果SDK调用失败，回退到模拟广告
                    _isAdDisplaying = false;
                    SimulateInterstitialAd();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error in ShowInterstitialAdSync: {ex.Message}";
                string detailsMessage = $"Exception details: {ex.ToString()}";
                
                Debug.WriteLine(errorMessage);
                Debug.WriteLine(detailsMessage);
                
                LogToFile(errorMessage);
                LogToFile(detailsMessage);
                
                _isAdDisplaying = false;
            }
        }

        // 显示插屏广告（异步版本，用于需要异步调用的场景）
        public async Task ShowInterstitialAd()
        {
            await Task.Run(() => ShowInterstitialAdSync());
        }

        // 模拟插屏广告（SDK不可用时的回退方案）
        private void SimulateInterstitialAd()
        {
            try
            {
                _isAdDisplaying = true;

                // 创建广告窗口
                Form adForm = new Form
                {
                    Text = "广告",
                    Size = new System.Drawing.Size(320, 480),
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.FixedSingle,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    ShowIcon = false
                };

                // 添加广告内容
                Label adLabel = new Label
                {
                    Text = $"插屏广告示例\n广告ID: {SdkManager.InterstitialAdId}\nAppKey: {SdkManager.AppKey}",
                    Dock = DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold)
                };
                adForm.Controls.Add(adLabel);

                // 添加关闭按钮
                Button closeButton = new Button
                {
                    Text = "关闭广告",
                    Size = new System.Drawing.Size(120, 40),
                    Location = new System.Drawing.Point((adForm.Width - 120) / 2, adForm.Height - 80)
                };
                closeButton.Click += (sender, e) => adForm.Close();
                adForm.Controls.Add(closeButton);

                // 广告窗口关闭事件
                adForm.FormClosed += (sender, e) =>
                {
                    _isAdDisplaying = false;
                    Debug.WriteLine("Interstitial ad closed");
                };

                // 30秒后自动关闭广告
                Task.Delay(30000).ContinueWith(t =>
                {
                    if (adForm != null && !adForm.IsDisposed)
                    {
                        adForm.Invoke((MethodInvoker)delegate
                        {
                            if (!adForm.IsDisposed)
                            {
                                adForm.Close();
                            }
                        });
                    }
                });

                // 显示广告
                adForm.ShowDialog();
            } catch (Exception ex)
            {
                Debug.WriteLine($"Error in SimulateInterstitialAd: {ex.Message}");
                _isAdDisplaying = false;
            }
        }

        // 检查是否可以显示广告
        public bool CanShowAd()
        {
            return !_isAdDisplaying;
        }

        // 显示退屏广告
        public void ShowExitAd()
        {
            try
            {
                AdvertManager.openExitAdvert();
            } catch (Exception ex)
            {
                Debug.WriteLine($"Failed to show exit ad: {ex.Message}");
            }
        }

        // 记录日志到文件
        private void LogToFile(string message)
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
}