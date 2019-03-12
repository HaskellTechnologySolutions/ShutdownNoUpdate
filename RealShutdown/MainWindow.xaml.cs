using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealShutdown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ExitWindowsEx(ExitWindowsFlags uFlags, ShutdownReason dwReason);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NTShutdownSystem(NTShutdownFlags uFlags);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NTSetSystemPowerState(NTSetPowerState_PowerActionFlags powerActionFlags,
            NTSetPowerState_SystemPowerStateFlags systemPowerStateFlags,
            ShutdownReason reason);


        public MainWindow()
        {
            InitializeComponent();

            if (!TokenAdjuster.EnablePrivilege("SeShutdownPrivilege", true))
            {
                MessageBox.Show("Failed to request SeShutdownPrivilege token. You lack permission to shut the systemd down.");
                Application.Current.Shutdown();
            }
        }

        private void ExitWindowsEx_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int val = ExitWindowsEx(ExitWindowsFlags.PowerOff | ExitWindowsFlags.Force, ShutdownReason.MajorOther | ShutdownReason.MinorOther | ShutdownReason.FlagPlanned);
                if (val != 0)
                {
                    MessageBox.Show("Shutdown Failed, error code: " + val.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Application.Current.Shutdown();
        }

        private void NTShutdownSystem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int val = NTShutdownSystem(NTShutdownFlags.ShutdownPowerOff);
                if (val != 0)
                {
                    MessageBox.Show("Shutdown Failed, error code: " + val.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Application.Current.Shutdown();
        }

        private void NTSetSystemPowerState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int val = NTSetSystemPowerState(NTSetPowerState_PowerActionFlags.PowerActionShutdownOff, NTSetPowerState_SystemPowerStateFlags.PowerSystemShutdown, ShutdownReason.MajorOther | ShutdownReason.MinorOther | ShutdownReason.FlagPlanned);
                if (val != 0)
                {
                    MessageBox.Show("Shutdown Failed, error code: " + val.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Application.Current.Shutdown();
        }
    }
}
