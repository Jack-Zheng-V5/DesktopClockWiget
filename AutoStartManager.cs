using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace DesktopClockWidget
{
    public static class AutoStartManager
    {
        private const string RegistryKeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string AppName = "DesktopClockWidget";

        /// <summary>
        /// 设置应用程序是否开机自动启动
        /// </summary>
        /// <param name="isAutoStart">是否启用开机自启动</param>
        public static void SetAutoStart(bool isAutoStart)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key == null)
                    {
                        // 如果注册表项不存在，尝试创建
                        using (RegistryKey newKey = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
                        {
                            if (isAutoStart)
                            {
                                newKey.SetValue(AppName, Process.GetCurrentProcess().MainModule.FileName);
                            }
                        }
                    }
                    else
                    {
                        if (isAutoStart)
                        {
                            // 启用自启动，设置注册表项
                            key.SetValue(AppName, Process.GetCurrentProcess().MainModule.FileName);
                        }
                        else
                        {
                            // 禁用自启动，删除注册表项
                            if (key.GetValue(AppName) != null)
                            {
                                key.DeleteValue(AppName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 在实际应用中应该添加日志记录
                // 这里简化处理，忽略异常
            }
        }

        /// <summary>
        /// 检查应用程序是否设置了开机自启动
        /// </summary>
        /// <returns>是否已设置开机自启动</returns>
        public static bool IsAutoStartEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
                {
                    if (key != null)
                    {
                        return key.GetValue(AppName) != null;
                    }
                }
            }
            catch (Exception ex)
            {
                // 在实际应用中应该添加日志记录
                // 这里简化处理，忽略异常
            }
            return false;
        }
    }
}