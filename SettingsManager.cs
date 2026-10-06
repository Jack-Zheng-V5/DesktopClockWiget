using System;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace DesktopClockWidget
{
    public class AppSettings
    {
        public int ClockSize { get; set; } = 200;
        public Point Location { get; set; } = Point.Empty;
        public int VibrationDuration { get; set; } = 5; // 默认震动时长为5秒
        public string? CustomClockFacePath { get; set; } = null; // 自定义表盘图片路径，null表示使用默认表盘
        public string? CustomAlarmSoundPath { get; set; } = null; // 自定义闹铃声音路径，null表示使用默认闹铃
        public string Language { get; set; } = "zh-CN"; // 默认使用中文
        public bool EnableHourlyChime { get; set; } = true; // 默认启用整点报时功能
        public bool EnableAlarmAd { get; set; } = true; // 默认启用闹铃广告功能
    }

    public class SettingsManager
    {
        private string settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DesktopClockWidget", "settings.json");

        public SettingsManager()
        {
            EnsureDataDirectoryExists();
        }

        private void EnsureDataDirectoryExists()
        {
            string directory = Path.GetDirectoryName(settingsFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public AppSettings LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                try
                {
                    string json = File.ReadAllText(settingsFilePath);
                    return JsonSerializer.Deserialize<AppSettings>(json);
                }
                catch { }
            }
            return null;
        }

        public void SaveSettings(AppSettings settings)
        {
            try
            {
                string json = JsonSerializer.Serialize(settings);
                File.WriteAllText(settingsFilePath, json);
            }
            catch { }
        }
    }
}