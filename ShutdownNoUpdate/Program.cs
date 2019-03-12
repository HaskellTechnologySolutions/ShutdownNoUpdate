using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ShutdownNoUpdate
{
    static class Program
    {
		[DllImport("ntdll.dll", SetLastError = true)]
		private static extern int NTShutdownSystem(NTShutdownFlags uFlags);

		[STAThread]
        static void Main()
        {
            if (!TokenAdjuster.EnablePrivilege("SeShutdownPrivilege", true))
            {
                MessageBox.Show("Failed to request SeShutdownPrivilege token. You lack permission to shut the systemd down.");
                return;
            }

            try
            {
				NTShutdownSystem(NTShutdownFlags.ShutdownPowerOff);
			}
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return;
        }
    }
}
