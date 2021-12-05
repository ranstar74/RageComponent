﻿// Copyright (c) 2021 All Rights Reserved
// Author: Homer
// Date: Today
/*
▓▓▓▓
▒▒▒▓▓
▒▒▒▒▒▓
▒▒▒▒▒▒▓
▒▒▒▒▒▒▓
▒▒▒▒▒▒▒▓
▒▒▒▒▒▒▒▓▓▓
▒▓▓▓▓▓▓░░░▓
▒▓░░░░▓░░░░▓
▓░░░░░░▓░▓░▓
▓░░░░░░▓░░░▓
▓░░▓░░░▓▓▓▓
▒▓░░░░▓▒▒▒▒▓
▒▒▓▓▓▓▒▒▒▒▒▓
▒▒▒▒▒▒▒▒▓▓▓▓
▒▒▒▒▒▓▓▓▒▒▒▒▓
▒▒▒▒▓▒▒▒▒▒▒▒▒▓
▒▒▒▓▒▒▒▒▒▒▒▒▒▓
▒▒▓▒▒▒▒▒▒▒▒▒▒▒▓
▒▓▒▓▒▒▒▒▒▒▒▒▒▓
▒▓▒▓▓▓▓▓▓▓▓▓▓
▒▓▒▒▒▒▒▒▒▓
▒▒▓▒▒▒▒▒▓
*/

using GTA;
using System;

namespace RageComponent
{
    /// <summary>
    /// Main class of the script.
    /// </summary>
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

        /// <summary>
        /// Creates a new instance of script <see cref="RageComponent"/>.
        /// </summary>
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
