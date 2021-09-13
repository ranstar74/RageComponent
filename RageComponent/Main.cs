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
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Initialize all components on first frame
            if(_firstTick)
            {
                Components.OnInit();

                _firstTick = false;
            }
            Components.OnTick();
        }
    }
}
