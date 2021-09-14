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

        public Main()
        {
            Tick += MainOnTick;
        }

        private void MainOnTick(object sender, System.EventArgs e)
        {
            OnTick?.Invoke();
        }
    }
}
