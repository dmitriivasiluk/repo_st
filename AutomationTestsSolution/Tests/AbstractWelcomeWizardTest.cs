using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AutomationTestsSolution.Helpers;
using LibGit2Sharp;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;

namespace AutomationTestsSolution.Tests
{
    public abstract class AbstractWelcomeWizardTest : AbstractUITest
    {
        private bool _glogalIgnoreSet = false;
        

        private string pathToClonedGitRepo = Environment.ExpandEnvironmentVariables(ConstantsList.pathToClonedGitRepo);


        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            RemoveTestFolder();
            UnsetGlobalIgnore();
        }

        protected override void PreConfigureSourceTree()
        {
            PreConfigureSourceTreeCore();
            PreConfigureSourceTreeCheckedForOlderInstalls();

            RemoveTestFolder();
            CreateTestFolder();
            Repository.Init(pathToClonedGitRepo);

            SetGlobalIgnore();
        }

        private void CreateTestFolder()
        {
            Directory.CreateDirectory(pathToClonedGitRepo);
        }
        private void RemoveTestFolder()
        {
            Utils.RemoveDirectory(pathToClonedGitRepo);
        }

        protected override void RunAndAttach()
        {
            RunSourceTree(SourceTreeExePath);
            AttachToWelcomeWizard();
        }

        protected void AttachToWelcomeWizard()
        {
            MainWindow = null;
            MainWindow = Utils.FindNewWindow("Welcome");
        }

        private void SetGlobalIgnore()
        {
            if (IsGlobalIgnoreSet())
            {
                return;
            }

            var gitExePath = Path.Combine(SourceTreeDownloadPath, "git_local", "cmd", "git.exe");
            var globalIgnore = Path.Combine(SourceTreeAppPath, "extras", "gitignore_global_default.txt");
            var processHelper = new ProcessHelper(gitExePath, "config --global core.excludesfile  " + globalIgnore);
            if (processHelper.Run())
            {
                IsGlobalIgnoreSet();
            }
        }

        private bool IsGlobalIgnoreSet()
        {
            var gitExePath = Path.Combine(SourceTreeDownloadPath, "git_local", "cmd", "git.exe");
            var processHelper = new ProcessHelper(gitExePath, "config --global --get-all core.excludesfile");
            if (processHelper.Run())
            {
                return processHelper.Output.Length > 0;
            }
            
            // failed
            return false;
        }

        private void UnsetGlobalIgnore()
        {
            if (_glogalIgnoreSet)
            {
                using (var repo = new Repository(pathToClonedGitRepo))
                {
                    repo.Config.Unset("core.excludesfile");
                }
            }
        }
    }
}