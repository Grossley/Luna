﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Luna.Runner;

namespace Luna {
    class Program {
        public static string[] Arguments;
        public static string GameLocation = null;

        static void Main(string[] args) {
            // Initalize definitions
            InstructionDefinition.Initalize();
            FunctionDefinition.Initalize();

            // Parse arguments
            Program.Arguments = GetArguments(args);
            for(int i = 0; i < Program.Arguments.Length; i++) {
                switch (Program.Arguments[i]) {
                    case "-game": {
                        Program.GameLocation = Program.Arguments[++i];
                        break;
                    }
                }
            }
            if (Program.GameLocation == null) Program.GameLocation = Path.GetDirectoryName(Program.Arguments[0]) + "\\data.win";

            // Check existence
            if (File.Exists(Program.GameLocation) == false) {
                MessageBox.Show("Could not find specified game file: \"" + Program.GameLocation + "\"", "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load game
            IFF _wad = new IFF(Program.GameLocation, new Game());
            _wad.Parse(delegate (Game _game) {
                _game.Initalize(false);
            });
            Console.ReadKey();
        }

        static string[] GetArguments(string[] args) {
            string[] _argumentList = new string[args.Length + 1];
            _argumentList[0] = System.Reflection.Assembly.GetEntryAssembly().Location; // first argument is always runner
            for (int i = 0; i < args.Length; i++) _argumentList[i + 1] = args[i];
            return _argumentList;
        }
    }

    public static class Extensions {
        public static bool ReadLBoolean(this BinaryReader _reader) {
            return (_reader.ReadInt32() == 1 ? true : false);
        }
    }
}
