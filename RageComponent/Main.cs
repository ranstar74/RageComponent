using GTA;
using System;

namespace RageComponent
{
    public class Main : Script
    {
        private Version Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Invokes every tick.
        /// </summary>
        public static Action OnTick { get; set; }

        /// <summary>
        /// Invokes on abort.
        /// </summary>
        public static Action OnAbort { get; set; }

        public Main()
        {
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(Version.Build).AddSeconds(Version.Revision * 2);
            System.IO.File.AppendAllText($"./ScriptHookVDotNet.log", $"RageComponent - {Version} ({buildDate})" + Environment.NewLine);
            
            Tick += MainOnTick;
            Aborted += MainOnAbort;
        }

        private void MainOnAbort(object sender, EventArgs e)
        {
            OnAbort?.Invoke();
        }

        private void MainOnTick(object sender, System.EventArgs e)
        {
            OnTick?.Invoke();
        }
    }
}
