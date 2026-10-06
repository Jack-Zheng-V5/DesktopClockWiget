using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using NAudio.Wave; // 用于NAudio音频播放
using System.Threading.Tasks; // 用于异步任务
using System.Diagnostics; // 用于Process类

namespace DesktopClockWidget
{
    public class Alarm
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool IsEnabled { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }

    public class AlarmManager
    {
        private ClockForm parentForm;
        private List<Alarm> alarms;
        private string alarmsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DesktopClockWidget", "alarms.json");
        private int vibrationDuration = 5; // 默认震动时长为5秒
        
        // 添加成员变量用于跟踪正在播放的闹铃和震动
        private WaveOutEvent? currentSoundPlayer = null;
        private AudioFileReader? currentAudioFileReader = null;
        private System.Windows.Forms.Timer? currentVibrateTimer = null;
        private AdManager? adManager = null;

        // 震动时长属性（范围1-100秒）
        public int VibrationDuration
        {
            get { return vibrationDuration; }
            set 
            { 
                // 确保时长在有效范围内
                vibrationDuration = Math.Max(1, Math.Min(100, value)); 
            }
        }

        public AlarmManager(ClockForm form)
        {
            parentForm = form;
            alarms = new List<Alarm>();
            EnsureDataDirectoryExists();
            adManager = new AdManager(form);
        }

        private void EnsureDataDirectoryExists()
        {
            string directory = Path.GetDirectoryName(alarmsFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void AddAlarm(Alarm alarm)
        {
            // 确保闹钟有唯一ID
            if (string.IsNullOrEmpty(alarm.Id))
            {
                alarm.Id = Guid.NewGuid().ToString();
            }
            
            alarms.Add(alarm);
            SaveAlarms();
            OnAlarmsChanged();
        }

        public void RemoveAlarm(Alarm alarm)
        {
            alarms.Remove(alarm);
            SaveAlarms();
            OnAlarmsChanged();
        }

        public void UpdateAlarm(Alarm updatedAlarm)
        {
            Alarm existingAlarm = alarms.FirstOrDefault(a => a.Id == updatedAlarm.Id);
            if (existingAlarm != null)
            {
                // 更新闹钟属性
                existingAlarm.Hour = updatedAlarm.Hour;
                existingAlarm.Minute = updatedAlarm.Minute;
                existingAlarm.IsEnabled = updatedAlarm.IsEnabled;
                
                SaveAlarms();
                OnAlarmsChanged();
            }
        }

        public List<Alarm> LoadAlarms()
        {
            if (File.Exists(alarmsFilePath))
            {
                try
                {
                    string json = File.ReadAllText(alarmsFilePath);
                    alarms = JsonSerializer.Deserialize<List<Alarm>>(json) ?? new List<Alarm>();
                    // 确保每个闹钟都有唯一ID
                    foreach (var alarm in alarms)
                    {
                        if (string.IsNullOrEmpty(alarm.Id))
                        {
                            alarm.Id = Guid.NewGuid().ToString();
                        }
                    }
                    SaveAlarms(); // 保存更新后的闹钟列表（包含ID）
                }
                catch (Exception _)
                {
                    // 发生异常时创建空列表
                    alarms = new List<Alarm>();
                }
            }
            else
            {
                // 文件不存在时创建空列表
                alarms = new List<Alarm>();
            }
            return alarms;
        }

        private void SaveAlarms()
        {
            try
            {
                string json = JsonSerializer.Serialize(alarms);
                File.WriteAllText(alarmsFilePath, json);
            }
            catch { }
        }

        // 用于跟踪已经触发过的闹钟，防止在同一分钟内重复触发
        private Dictionary<string, DateTime> triggeredAlarms = new Dictionary<string, DateTime>();
        
        // 用于跟踪已经触发过的整点报时，防止在同一分钟内重复触发
        private Dictionary<string, DateTime> triggeredHourlyChimes = new Dictionary<string, DateTime>();
        
        public event EventHandler AlarmsChanged;
        
        // 整点报时功能开关
        public bool EnableHourlyChime { get; set; } = false;

        // 获取所有闹钟
        public List<Alarm> GetAllAlarms()
        {
            return new List<Alarm>(alarms);
        }

        // 触发AlarmsChanged事件
        private void OnAlarmsChanged()
        {
            AlarmsChanged?.Invoke(this, EventArgs.Empty);
        }

        // 优化后的RemoveAlarm方法，使用ID进行移除
        public void RemoveAlarm(string alarmId)
        {
            Alarm? alarmToRemove = alarms.FirstOrDefault(a => a.Id == alarmId);
            if (alarmToRemove != null)
            {
                RemoveAlarm(alarmToRemove);
            }
        }

        public void CheckAlarms()
        {
            // 清理5分钟前触发的闹钟记录
            DateTime fiveMinutesAgo = DateTime.Now.AddMinutes(-5);
            var expiredAlarms = triggeredAlarms.Where(kv => kv.Value < fiveMinutesAgo).ToList();
            foreach (var kv in expiredAlarms)
            {
                triggeredAlarms.Remove(kv.Key);
            }

            DateTime now = DateTime.Now;
            foreach (Alarm alarm in alarms)
            {
                // 只检查启用的闹钟
                if (alarm.IsEnabled)
                {
                    // 检查当前时间是否与闹钟时间匹配（忽略秒）
                    if (now.Hour == alarm.Hour && now.Minute == alarm.Minute)
                    {
                        // 检查这个闹钟在当前分钟是否已经触发过
                        string alarmKey = $"{alarm.Hour}:{alarm.Minute}:{alarm.Id}";
                        if (!triggeredAlarms.ContainsKey(alarmKey))
                        {
                            TriggerAlarm(alarm);
                            // 记录这个闹钟在当前分钟已经触发
                            triggeredAlarms[alarmKey] = now;
                        }
                    }
                }
            }
        }

        private void TriggerAlarm(Alarm alarm)
        {
            // 在主UI线程中执行震动效果
            parentForm.Invoke((MethodInvoker)delegate
            {
                // 保存原始TopMost状态
                bool originalTopMost = parentForm.TopMost;
                
                // 设置窗口为置顶，确保用户能看到震动
                parentForm.TopMost = true;
                parentForm.TopMost = false; // 先设为false再设为true，确保能正确置顶
                parentForm.TopMost = true;
                
                // 执行震动效果
                VibrateWindow(parentForm);
                PlayAlarmSound();
                
                // 检查是否启用了闹铃广告功能
                // 默认启用广告功能，即使设置文件不存在或格式错误
                bool enableAlarmAd = true;
                try
                {
                    SettingsManager settingsManager = new SettingsManager();
                    var settings = settingsManager.LoadSettings();
                    if (settings != null)
                    {
                        enableAlarmAd = settings.EnableAlarmAd;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("加载设置时出错: " + ex.Message);
                    // 出错时仍默认启用广告
                }
                
                // 如果启用了闹铃广告，则显示广告
                if (enableAlarmAd && adManager != null)
                {
                    // 直接调用ShowAd方法，不再检查IsAdDisplaying()
                    adManager.ShowAd();
                }
                
                // 闹铃播放结束后恢复原始TopMost状态
                Task.Run(async () =>
                {
                    // 等待闹铃播放完成（闹铃播放时间为10秒+1秒缓冲）
                    await Task.Delay(10000 + 1000);
                    parentForm.Invoke((MethodInvoker)delegate
                    {
                        parentForm.TopMost = false;
                    });
                });
            });
        }
        
        /// <summary>
        /// 停止闹铃播放和震动效果
        /// </summary>
        public void StopAlarm()
        {
            parentForm.Invoke((MethodInvoker)delegate
            {
                // 停止声音播放
                if (currentSoundPlayer != null)
                {
                    currentSoundPlayer.Stop();
                    currentSoundPlayer.Dispose();
                    currentSoundPlayer = null;
                }
                
                if (currentAudioFileReader != null)
                {
                    currentAudioFileReader.Dispose();
                    currentAudioFileReader = null;
                }
                
                // 停止震动效果
                if (currentVibrateTimer != null)
                {
                    currentVibrateTimer.Stop();
                    currentVibrateTimer.Dispose();
                    currentVibrateTimer = null;
                    // 恢复窗口原始位置
                    if (parentForm != null)
                    {
                        // 由于我们不知道原始位置，这里简单处理为停止震动
                        // 在实际使用中，可能需要额外保存原始位置
                    }
                }
                
                // 立即恢复窗口正常层级
                if (parentForm != null)
                {
                    parentForm.TopMost = false;
                }
            });
        }
        
        /// <summary>
        /// 触发整点报时
        /// </summary>
        public void TriggerHourlyChime(Form form)
        {
            DateTime now = DateTime.Now;
            
            // 清理5分钟前触发的整点报时记录
            DateTime fiveMinutesAgo = DateTime.Now.AddMinutes(-5);
            var expiredChimes = triggeredHourlyChimes.Where(kv => kv.Value < fiveMinutesAgo).ToList();
            foreach (var kv in expiredChimes)
            {
                triggeredHourlyChimes.Remove(kv.Key);
            }
            
            // 检查这个整点是否已经触发过
            string chimeKey = $"{now.Hour}:00";
            if (!triggeredHourlyChimes.ContainsKey(chimeKey) && EnableHourlyChime)
            {
                // 在主UI线程中执行震动效果
                form.Invoke((MethodInvoker)delegate
                {
                    // 保存原始TopMost状态
                    bool originalTopMost = form.TopMost;
                    
                    // 设置窗口为置顶，确保用户能看到震动
                    form.TopMost = true;
                    form.TopMost = false; // 先设为false再设为true，确保能正确置顶
                    form.TopMost = true;
                    
                    // 执行短时间震动效果（2秒）
                    VibrateWindow(form);
                    
                    // 播放系统提示音作为整点报时
                    PlaySystemBeep();
                    
                    // 播放预设铃声
                    PlayAlarmSound();
                    
                    // 报时结束后恢复原始TopMost状态
                    Task.Run(async () =>
                    {
                        // 等待铃声和震动完成（铃声播放时间为10秒）
                        await Task.Delay(10000 + 1000);
                        form.Invoke((MethodInvoker)delegate
                        {
                            form.TopMost = originalTopMost;
                        });
                    });
                });
                
                // 记录这个整点已经触发过报时
                triggeredHourlyChimes[chimeKey] = now;
            }
        }
        
        /// <summary>
        /// 播放闹铃声音
        /// </summary>
        private void PlayAlarmSound()
        {
            // 优先使用用户自定义的闹铃声音路径
            string? soundFilePath = null;
            
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
            
            // 先清理之前可能存在的播放器
            if (currentSoundPlayer != null)
            {
                currentSoundPlayer.Dispose();
                currentSoundPlayer = null;
            }
            
            if (currentAudioFileReader != null)
            {
                currentAudioFileReader.Dispose();
                currentAudioFileReader = null;
            }
            
            // 创建新的播放器
            currentSoundPlayer = new WaveOutEvent();
            currentAudioFileReader = null;
            
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
                        try
                        {
                            currentAudioFileReader = new AudioFileReader(soundFilePath);
                            currentSoundPlayer.Init(currentAudioFileReader);
                            currentSoundPlayer.Play();
                            
                            // 播放10秒钟后停止
                            Task.Run(async () =>
                            {
                                await Task.Delay(10000);
                                parentForm.Invoke((MethodInvoker)delegate
                                {
                                    if (currentSoundPlayer != null)
                                    {
                                        currentSoundPlayer.Stop();
                                        currentSoundPlayer.Dispose();
                                        currentSoundPlayer = null;
                                    }
                                    
                                    if (currentAudioFileReader != null)
                                    {
                                        currentAudioFileReader.Dispose();
                                        currentAudioFileReader = null;
                                    }
                                });
                            });
                        }
                        catch
                        {
                            // 发生异常时回退到系统提示音
                            PlaySystemBeep();
                        }
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
            catch
            {
                // 发生异常时回退到系统提示音
                PlaySystemBeep();
                
                // 清理资源 - 注意：这里不再需要手动清理，因为我们现在使用成员变量并在StopAlarm方法中处理
            }
        }
        
        // 公共方法：播放默认闹铃声音
        public void PlayDefaultAlarmSound()
        {
            try
            {
                string defaultSoundPath = Path.Combine(Application.StartupPath, "Resource", "relaxing-guitar-loop.mp3");

                if (File.Exists(defaultSoundPath))
                {
                    using (var audioFileReader = new AudioFileReader(defaultSoundPath))
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFileReader);
                        outputDevice.Play();

                        // 播放3秒后停止
                        Task.Delay(3000).Wait();
                        outputDevice.Stop();
                    }
                }
                else
                {
                    // 如果默认闹铃文件不存在，使用系统提示音
                    PlaySystemBeep();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("播放默认闹铃声音时出错: " + ex.Message);
                PlaySystemBeep();
            }
        }

        // 公共方法：播放自定义闹铃声音
        public void PlayCustomAlarmSound(string soundPath)
        {
            try
            {
                if (string.IsNullOrEmpty(soundPath) || !File.Exists(soundPath))
                {
                    throw new FileNotFoundException("指定的闹铃声音文件不存在。");
                }

                using (var audioFileReader = new AudioFileReader(soundPath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFileReader);
                    outputDevice.Play();

                    // 播放3秒后停止
                    Task.Delay(3000).Wait();
                    outputDevice.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("播放自定义闹铃声音时出错: " + ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// 播放系统提示音
        /// </summary>
        private void PlaySystemBeep()
        {
            // 为了确保声音能被听到，连续播放多次
            parentForm.Invoke((MethodInvoker)delegate
            {
                for (int i = 0; i < 3; i++)
                {
                    System.Media.SystemSounds.Beep.Play();
                    System.Threading.Thread.Sleep(300);
                }
            });
        }
        
        // 供手动触发震动效果的公共方法（用于测试）
        public void TriggerVibration(Form form)
        {
            // 直接调用震动方法
            VibrateWindow(form);
        }

        private void VibrateWindow(Form form)
        {
            // 保存原始位置
            Point originalLocation = form.Location;
            int interval = 30; // 震动间隔（毫秒）
            
            // 计算总震动次数，确保震动总时长严格等于设置的时长
            int totalVibrations = Math.Max(1, (vibrationDuration * 1000) / interval);
            int vibrationsLeft = totalVibrations;
            
            // 初始化震动参数
            int maxOffset = 8; // 最大震动幅度
            int currentOffset = maxOffset;
            int direction = 1; // 震动方向
            
            // 计算振幅衰减步长，确保在震动结束时振幅平滑减小
            int amplitudeSteps = totalVibrations;
            
            // 先停止之前可能存在的震动计时器
            if (currentVibrateTimer != null)
            {
                currentVibrateTimer.Stop();
                currentVibrateTimer.Dispose();
            }
            
            // 使用Timer实现平滑震动效果
            currentVibrateTimer = new System.Windows.Forms.Timer
            {
                Interval = interval
            };
            
            currentVibrateTimer.Tick += (sender, e) =>
            {
                if (vibrationsLeft > 0)
                {
                    // 计算新位置（交替上下左右震动）
                    int xOffset = (vibrationsLeft % 2 == 0) ? currentOffset * direction : 0;
                    int yOffset = (vibrationsLeft % 2 == 1) ? currentOffset * direction : 0;
                    
                    form.Location = new Point(
                        originalLocation.X + xOffset,
                        originalLocation.Y + yOffset
                    );
                    
                    // 切换方向
                    if (vibrationsLeft % 2 == 0)
                    {
                        direction *= -1;
                    }
                    
                    // 平滑衰减振幅，根据剩余震动次数动态调整
                    int remainingSteps = totalVibrations - vibrationsLeft;
                    if (amplitudeSteps > 0)
                    {
                        // 使用比例衰减，确保震动结束时振幅自然减小到1
                        currentOffset = maxOffset - (remainingSteps * (maxOffset - 1) / amplitudeSteps);
                        currentOffset = Math.Max(1, currentOffset); // 确保振幅至少为1
                    }
                    
                    vibrationsLeft--;
                }
                else
                {
                    // 震动结束，恢复原始位置并停止计时器
                    form.Location = originalLocation;
                    currentVibrateTimer.Stop();
                    currentVibrateTimer.Dispose();
                    currentVibrateTimer = null; // 重置为null表示没有活跃的震动计时器
                }
            };
            
            // 启动震动计时器
            currentVibrateTimer.Start();
        }
    }
}