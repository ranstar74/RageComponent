using GTA;
using System;

namespace RageComponent
{
    public class Main : Script
    {
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
