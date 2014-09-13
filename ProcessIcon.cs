using KeyboardSpeedReset.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KeyboardSpeedReset
{
    class ProcessIcon : IDisposable
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

        private NotifyIcon notifyIcon = new NotifyIcon();

        public ProcessIcon()
        {
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
        }

        /// <summary>
        /// Displays the icon in the system tray.
        /// </summary>
        public void Display()
        {
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            notifyIcon.Icon = Resources.Icon;
            notifyIcon.Text = "Keyboard Speed Fixer";
            notifyIcon.Visible = true;
        }

        void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Resume)
            {
                ResetKeyboardSpeed();
            }
        }

        void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            ResetKeyboardSpeed();
            notifyIcon.ShowBalloonTip(1000, "Success", "The keyboard settings have been successfully reset.", ToolTipIcon.Info);
        }

        void ResetKeyboardSpeed()
        {
            uint SPI_SETKEYBOARDSPEED = 0x000B;
            uint SPI_SETKEYBOARDDELAY = 0x0017;
            uint ReptSpeed = 31;
            uint Delay = 0;
            uint NotUsed = 0;

            SystemParametersInfo(SPI_SETKEYBOARDSPEED, ReptSpeed, ref NotUsed, 0);
            SystemParametersInfo(SPI_SETKEYBOARDDELAY, Delay, ref NotUsed, 0);
        }

        public void Dispose()
        {
            notifyIcon.Dispose();
        }
    }
}
