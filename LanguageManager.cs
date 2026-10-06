using System.Collections.Generic;

namespace DesktopClockWidget
{
    public class LanguageManager
    {
        // 当前语言
        public string CurrentLanguage { get; private set; } = "zh-CN";
        
        // 语言资源字典
        private Dictionary<string, Dictionary<string, string>> languageResources;
        
        // 单例实例
        private static LanguageManager instance;
        public static LanguageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LanguageManager();
                }
                return instance;
            }
        }
        
        private LanguageManager()
        {
            InitializeLanguageResources();
        }
        
        /// <summary>
        /// 初始化语言资源
        /// </summary>
        private void InitializeLanguageResources()
        {
            languageResources = new Dictionary<string, Dictionary<string, string>>();
            
            // 添加中文资源
            languageResources["zh-CN"] = new Dictionary<string, string>
            {
                { "SettingsTitle", "时钟设置" },
                { "LanguageSelectionLabel", "选择语言:" },
                { "ClockSizeGroupBox", "时钟大小" },
                { "SystemSettingsGroupBox", "系统设置" },
                { "ClockFaceGroupBox", "表盘样式" },
                { "ClockSizeLabel", "时钟大小: {0}px" },
                { "VibrationDurationLabel", "震动时长 (秒):" },
                { "CustomClockFaceLabel", "自定义表盘:" },
                { "UploadClockFaceButton", "上传表盘图片" },
                { "PreviewClockFaceButton", "预览表盘" },
                { "RemoveClockFaceButton", "恢复默认表盘" },
                { "CustomAlarmSoundLabel", "自定义闹铃:" },
                { "UploadAlarmSoundButton", "上传闹铃声音" },
                { "PreviewAlarmSoundButton", "预览闹铃" },
                { "RemoveAlarmSoundButton", "恢复默认闹铃" },
                { "AutoStartCheckBox", "开机自启动" },
                { "AlarmsSectionTitle", "闹钟设置" },
                { "AddAlarmButton", "添加闹钟" },
                { "AddTestAlarmButton", "添加测试闹钟" },
                { "PreviewVibrationButton", "预览震动" },
                { "PreviewAlarmButton", "预览闹铃声音" },
                { "SaveButton", "保存" },
                { "CurrentDefaultClockFace", "当前使用默认表盘样式" },
                { "CurrentCustomClockFace", "当前使用自定义表盘: {0}" },
                { "CurrentDefaultAlarmSound", "当前使用默认闹铃声音" },
                { "CurrentCustomAlarmSound", "当前使用自定义闹铃: {0}" },
                { "ClockFaceUploadSuccess", "表盘图片选择成功！点击预览按钮查看效果。" },
                { "ClockFaceUploadError", "选择的文件不是有效的图片格式。" },
                { "AlarmSoundUploadSuccess", "闹铃声音选择成功！点击预览按钮试听效果。" },
                { "AlarmSoundUploadError", "选择的文件不是有效的音频格式。" },
                { "ClockFacePreviewSuccess", "表盘样式已应用，点击保存按钮永久保存此设置。" },
                { "ClockFacePreviewError", "请先上传一个有效的表盘图片。" },
                { "ClockFaceResetSuccess", "已恢复默认表盘样式。" },
                { "AlarmSoundResetSuccess", "已恢复默认闹铃声音。" },
                { "DefaultAlarmPreviewMessage", "播放默认闹铃声音。" },
                { "CustomAlarmPreviewMessage", "正在播放自定义闹铃声音..." },
                { "AlarmSoundPreviewError", "播放闹铃声音时出错: {0}" },
                { "TestAlarmAdded", "已添加测试闹钟，将在1分钟后触发（时间：{0}:{1}）" },
                { "AlarmNotExists", "声音文件不存在，已播放系统提示音。" },
                { "FormatNotSupported", "不支持的音频格式：{0}。\n支持的格式有：{1}" },
                { "SuccessTitle", "成功" },
                { "ErrorTitle", "错误" },
                { "WarningTitle", "警告" },
                { "InformationTitle", "提示" },
                { "AlarmEnabledText", "启用" },
                { "AlarmDeleteButton", "删除" },
                { "AlarmIsPlayingText", "正在播放..." },
                { "SettingsMenuItem", "设置" },
                { "ExitMenuItem", "退出" },
                { "EnableHourlyChimeText", "启动整点报时功能" },
                { "TestAdMenuItem", "测试广告" }
            };
            
            // 添加英文资源
            languageResources["en-US"] = new Dictionary<string, string>
            {
                { "SettingsTitle", "Clock Settings" },
                { "LanguageSelectionLabel", "Select Language:" },
                { "ClockSizeGroupBox", "Clock Size" },
                { "SystemSettingsGroupBox", "System Settings" },
                { "ClockFaceGroupBox", "Clock Face Style" },
                { "ClockSizeLabel", "Clock Size: {0}px" },
                { "VibrationDurationLabel", "Vibration Duration (seconds):" },
                { "CustomClockFaceLabel", "Custom Clock Face:" },
                { "UploadClockFaceButton", "Upload Clock Face" },
                { "PreviewClockFaceButton", "Preview Clock Face" },
                { "RemoveClockFaceButton", "Restore Default Clock Face" },
                { "CustomAlarmSoundLabel", "Custom Alarm Sound:" },
                { "UploadAlarmSoundButton", "Upload Alarm Sound" },
                { "PreviewAlarmSoundButton", "Preview Alarm Sound" },
                { "RemoveAlarmSoundButton", "Restore Default Alarm Sound" },
                { "AutoStartCheckBox", "Auto Start on Boot" },
                { "AlarmsSectionTitle", "Alarm Settings" },
                { "AddAlarmButton", "Add Alarm" },
                { "AddTestAlarmButton", "Add Test Alarm" },
                { "PreviewVibrationButton", "Preview Vibration" },
                { "PreviewAlarmButton", "Preview Alarm Sound" },
                { "SaveButton", "Save" },
                { "CurrentDefaultClockFace", "Using default clock face" },
                { "CurrentCustomClockFace", "Using custom clock face: {0}" },
                { "CurrentDefaultAlarmSound", "Using default alarm sound" },
                { "CurrentCustomAlarmSound", "Using custom alarm sound: {0}" },
                { "ClockFaceUploadSuccess", "Clock face image selected successfully! Click preview to see the effect." },
                { "ClockFaceUploadError", "The selected file is not a valid image format." },
                { "AlarmSoundUploadSuccess", "Alarm sound selected successfully! Click preview to hear the effect." },
                { "AlarmSoundUploadError", "The selected file is not a valid audio format." },
                { "ClockFacePreviewSuccess", "Clock face style has been applied. Click save to keep this setting permanently." },
                { "ClockFacePreviewError", "Please upload a valid clock face image first." },
                { "ClockFaceResetSuccess", "Default clock face style has been restored." },
                { "AlarmSoundResetSuccess", "Default alarm sound has been restored." },
                { "DefaultAlarmPreviewMessage", "Playing default alarm sound." },
                { "CustomAlarmPreviewMessage", "Playing custom alarm sound..." },
                { "AlarmSoundPreviewError", "Error playing alarm sound: {0}" },
                { "TestAlarmAdded", "Test alarm added, will trigger in 1 minute (Time: {0}:{1})" },
                { "AlarmNotExists", "Sound file does not exist, played system beep instead." },
                { "FormatNotSupported", "Unsupported audio format: {0}.\nSupported formats: {1}" },
                { "SuccessTitle", "Success" },
                { "ErrorTitle", "Error" },
                { "WarningTitle", "Warning" },
                { "InformationTitle", "Information" },
                { "AlarmEnabledText", "Enabled" },
                { "AlarmDeleteButton", "Delete" },
                { "AlarmIsPlayingText", "Playing..." },
                { "SettingsMenuItem", "Settings" },
                { "ExitMenuItem", "Exit" },
                { "EnableHourlyChimeText", "Enable Hourly Chime" },
                { "TestAdMenuItem", "Test Ad" }
            };
        }
        
        /// <summary>
        /// 设置当前语言
        /// </summary>
        /// <param name="languageCode">语言代码，如 "zh-CN" 或 "en-US"</param>
        public void SetLanguage(string languageCode)
        {
            if (languageResources.ContainsKey(languageCode))
            {
                CurrentLanguage = languageCode;
            }
        }
        
        /// <summary>
        /// 初始化语言管理器
        /// </summary>
        public void Init()
        {
            // 初始化操作，目前不需要特殊处理
        }
        
        /// <summary>
        /// 获取翻译文本
        /// </summary>
        /// <param name="key">文本键</param>
        /// <param name="args">格式化参数</param>
        /// <returns>翻译后的文本</returns>
        public string GetText(string key, params object[] args)
        {
            if (languageResources.TryGetValue(CurrentLanguage, out var languageDict))
            {
                if (languageDict.TryGetValue(key, out var text))
                {
                    if (args.Length > 0)
                    {
                        return string.Format(text, args);
                    }
                    return text;
                }
            }
            
            // 如果找不到对应语言的文本，返回键名
            return key;
        }
    }
}