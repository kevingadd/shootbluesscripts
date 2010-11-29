﻿using System;
using ShootBlues;
using System.Collections.Generic;
using System.Diagnostics;
using Squared.Task;
using System.Reflection;
using System.IO;

namespace ShootBlues.Profile {
    public class EVE : SimpleExecutableProfile {
        public EVE ()
            : base("exefile.exe") {

            AddDependency("Common.Script.dll");
        }

        public override string ProfileName {
            get {
                return "EVE Online";
            }
        }

        private IEnumerator<object> BaseOnNewProcess (Process process) {
            return base.OnNewProcess(process);
        }

        protected override IEnumerator<object> OnNewProcess (Process process) {
            Console.WriteLine("Waiting for EVE to start...");

            IntPtr hWnd = IntPtr.Zero;
            while ((hWnd == IntPtr.Zero) && (!process.HasExited)) {
                yield return new Sleep(1);

                hWnd = Win32.FindWindow("triuiScreen", "EVE");
            }

            if (hWnd == IntPtr.Zero) {
                Console.WriteLine("EVE exited without starting (crashed?)");
                yield break;
            }

            Console.WriteLine("EVE started.");

            yield return new Sleep(0.1);

            yield return BaseOnNewProcess(process);
        }
    }
}
