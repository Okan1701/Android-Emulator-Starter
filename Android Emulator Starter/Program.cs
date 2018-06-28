/*
 * Written by Okan Emeni
 * Date: 28-06-2018
 * Status: WIP
 * Repo: https://github.com/OkanEmeni/Android-Emulator-Starter
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Android_Emulator_Starter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            #region Config
            // Below fields shouldn't have to be edited unless path is incorrect
            // emulatorPath is the path to the emulator executable found within the Android SDK
            // Modify if needed
            string emulatorPath = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Android\\Sdk\\emulator";
            string cmdListDevices = "/C emulator -list-avds"; // Command that lists all devices
            string cmdStartDevice = "/C emulator -avd "; // Command used to start specific device
            #endregion
            
            #region Main
            List<string> deviceList = new List<string>(); // Used at runtime to store names of all available devices

            // Print the emulator sdk path that the program will use to console
            Console.WriteLine("Emulator SDK Path: " + emulatorPath);
            Console.WriteLine("");
            
            // Configure a new Process, it will be a instance of the command prompt
            // Inside the prompt, we will run the two commands
            Process proc = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    Arguments = cmdListDevices, // We will first run the command that lists all devices
                    WorkingDirectory = emulatorPath,
                   
                }
            };
            // TODO: Ask user for SDK path if specified one is invalid
            proc.Exited += new EventHandler(ProcessTerminated); // Event for when the process terminates
            proc.Start();
            
            // The command will output all device names (1 per line)
            // We will loop through all the output lines and add each line (which contains device name)
            // to the list
            while (!proc.StandardOutput.EndOfStream)
            {
                deviceList.Add(proc.StandardOutput.ReadLine());
            }

            // Print all devices to console prefixed with a number
            Console.WriteLine("Device list:");
            int i = 1;
            foreach (var t in deviceList)
            {
                Console.WriteLine(i + ": " + t);
            }
            
            // Get the number that user inputted
            Console.WriteLine("");
            Console.Write("Enter the number of the device you wish to start: ");
            string input = Console.ReadLine();

            // See if input is actually a number,
            // Figure out if it is tied to a device name,
            // Reconfigure the previous process to run the required command and start it
            string device;
            if (int.TryParse(input, out int number))
            {
                // TODO: Add a check that sees if number is linked to a device
                device = deviceList[number - 1];
                Console.WriteLine("");
                Console.WriteLine("Starting " + device + "...");
                cmdStartDevice += device;
                proc.StartInfo.Arguments = cmdStartDevice;
                proc.Start();
            }
            else
            {
                // If user entered invalid character, display this
                Console.WriteLine("ERROR: Invalid input entered!");
            }
            #endregion
        }

        public static void ProcessTerminated(object sender, EventArgs e)
        {
            // BUG: This never displays
            Console.WriteLine("Emulator has terminated. Press any key to quit.");
            Console.ReadKey();
        }
    }
}