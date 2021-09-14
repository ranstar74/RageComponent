using GTA;
using System;

namespace RageComponent
{
    internal class Main : Script
    {
        private bool _firstTick = true;

        internal Main()
        {
            Tick += OnTick;
            Aborted += OnAbort;
        }

        private void OnAbort(object sender, EventArgs e)
        {

        }

        private void OnTick(object sender, EventArgs e)
        {

        }
    }
}
