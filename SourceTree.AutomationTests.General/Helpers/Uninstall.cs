using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Win32;

namespace AutomationTestsSolution.Helpers
{
    public class Uninstall
    {
        private const string NameOfProgram = "SourceTree";
        private const string AcoountJsonPath = @"%localappdata%\SourceTree\accounts.json";
        private const string SourceTreeSettingsPath = @"%localappdata%\SourceTree-Settings";
        private const string SquirrelTempPath = @"%localappdata%\SquirrelTemp";
        private const string AtlassianPath = @"%localappdata%\Atlassian";
        private const string SourceTreePath = @"%localappdata%\SourceTree";
        private const string SourceTreeBetaPath = @"%localappdata%\SourceTreeBeta";
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        public void CompletelyUninstallSourceTree()
        {
            string uninstallCommandSourceTree = GetUninstallCommandFor(NameOfProgram);
            if (!string.IsNullOrEmpty(uninstallCommandSourceTree))
            {
                ExecuteWindowsCommand(uninstallCommandSourceTree);
                // Give a time for removing SourceTree
                Debug.WriteLine("Waiting 5 seconds for complete uninstall...");
                Thread.Sleep(5000);
            }
            RemoveFoldersSourceTree();
            Debug.WriteLine("SourceTree was successfully removed from computer!");
        }

        public void ClearAccountSettings()
        {
            string pathToAccontSetting = Environment.ExpandEnvironmentVariables(AcoountJsonPath);
            if (File.Exists(pathToAccontSetting))
            {
                File.Delete(pathToAccontSetting);
            }
        }

        public static void ExecuteWindowsCommand(string сommandForExecution)
        {
            string windowProgram = "cmd";
            Debug.WriteLine("Executing uninstall command in cmd.exe...");
            Process.Start(windowProgram, "/C " + сommandForExecution);
            Debug.WriteLine("Executing is finished!");
        }

        /// <summary>
        /// This method remove all user config data folders to return SourceTree in configuration state. Also it find dynamic folders in 
        /// C:\Users\%USERNAME%\AppData\Local\Atlassian like "SourceTree.exe_Url_qsuikde1gcnj3eovksjtmq0msq50grno" and romove it.
        /// </summary>
        public void ResetToCleanInstallState()
        {
            // Static folders SourceTree
            List<string> pathsForSourceTree = new List<string>();
            pathsForSourceTree.Add(Environment.ExpandEnvironmentVariables(SourceTreeSettingsPath));
            pathsForSourceTree.Add(Environment.ExpandEnvironmentVariables(SquirrelTempPath));

            // Need to find out dynamic name of folders which owned SourceTree in Atlassian folder
            string pathAtlassian = Environment.ExpandEnvironmentVariables(AtlassianPath);
            string[] fileArray = Directory.GetDirectories(pathAtlassian);
            for (int i = 0; i < fileArray.Length; i++)
            {
                if (fileArray[i].Contains(NameOfProgram))
                {
                    pathsForSourceTree.Add(fileArray[i]);
                }
            }

            foreach (string pathToSourceTree in pathsForSourceTree)
            {
                try
                {
                    Directory.Delete(pathToSourceTree, true);
                    Debug.WriteLine($"Directory {pathToSourceTree} was removed!");
                }
                catch (UnauthorizedAccessException)
                {
                    Debug.WriteLine($"You don't have access to remove {pathToSourceTree}");
                }
                catch (DirectoryNotFoundException)
                {
                    Debug.WriteLine($"Directory {pathToSourceTree} is not found.");
                }
                catch (InvalidOperationException)
                {
                    Debug.WriteLine($"InvalidOperationException when trying to delete - {pathToSourceTree}.");
                }
                catch (IOException)
                {
                    Debug.WriteLine($"IOException - {pathToSourceTree}.");
                }
            }
        }

        /// <summary>
        /// This method remove all necessary folders owned to SourceTree for completely uninstall SourceTree from computer.
        /// </summary>
        public void RemoveFoldersSourceTree()
        {
            ResetToCleanInstallState();
            List<string> pathsForSourceTree = new List<string>();
            pathsForSourceTree.Add(Environment.ExpandEnvironmentVariables(SourceTreePath));
            pathsForSourceTree.Add(Environment.ExpandEnvironmentVariables(SourceTreeBetaPath));

            foreach (string pathToSourceTree in pathsForSourceTree)
            {
                try
                {
                    Directory.Delete(pathToSourceTree, true);
                    Debug.WriteLine($"Directory {pathToSourceTree} was removed!");
                }
                catch (UnauthorizedAccessException)
                {
                    Debug.WriteLine($"You don't have access to remove {pathToSourceTree}");
                }
                catch (DirectoryNotFoundException)
                {
                    Debug.WriteLine($"Directory {pathToSourceTree} is not found.");
                }
            }
        }

        /// <summary>
        /// This method finds DisplayName in registries with path 
        /// HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrectVersion\Uninstall and
        /// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrectVersion\Uninstall
        /// When it finds the current program, it gets UninstallString or QuietUninstallString uninstall command for executing it from command line.
        /// </summary>
        /// <param name="productDisplayName">A part of name program which want to uninstall</param>
        /// <returns>Command for uninstall current program using cmd.exe</returns>
        public static string GetUninstallCommandFor(string productDisplayName)
        {
            string uninstallCommand = "";

            List<RegistryKey> differentRegisteryFolder = new List<RegistryKey>();
            differentRegisteryFolder.Add(Registry.LocalMachine.OpenSubKey(RegistryKeyPath));
            differentRegisteryFolder.Add(Registry.CurrentUser.OpenSubKey(RegistryKeyPath));

            // loop for different registries
            foreach (RegistryKey openSubKeys in differentRegisteryFolder)
            {
                using (RegistryKey key = openSubKeys)
                {
                    Debug.WriteLine($"Start looking for {productDisplayName} in registry - {key.Name}");
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                            string displayName = (string)subkey.GetValue("DisplayName");
                            if (displayName != null && displayName.Contains(productDisplayName))
                            {
                                var QuiteUninstallCommand = subkey.GetValue("QuietUninstallString");
                                if (QuiteUninstallCommand != null)
                                {
                                    uninstallCommand = (string)QuiteUninstallCommand;
                                    Debug.WriteLine("Quite uninstall is present!");
                                    Debug.WriteLine($"Uninstall command is {uninstallCommand}");
                                    return uninstallCommand;
                                }
                                uninstallCommand = (string)subkey.GetValue("UninstallString") + " /S"; //Additional key for silence Uninstall;
                                Debug.WriteLine($"Uninstall command is {uninstallCommand}");
                                return uninstallCommand;
                            }
                        }
                    }
                }
            }
            Debug.WriteLine($"Program {productDisplayName} is not found.");
            return uninstallCommand;
        }

        public bool IsExist()
        {
            string uninstallCommandSourceTree = GetUninstallCommandFor(NameOfProgram);
            if (!string.IsNullOrEmpty(uninstallCommandSourceTree))
            {
                return true;
            }
            return false;
        }
    }
}
