using System.IO;
using AutomationTestsSolution.Helpers;
using AutomationTestsSolution.Tests;
using LibGit2Sharp;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;

namespace ScreenObjectsHelpers.Tests
{
    public abstract class AbstractWelcomeWizardTest : BasicTest
    {
        private bool _glogalIgnoreSet = false;

        public string PathToClonedGitRepo { get { return Path.Combine(SourceTreeTestDataPath, ConstantsList.testGitRepoBookmarkName); } }


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
            Repository.Init(PathToClonedGitRepo);

            SetGlobalIgnore();
        }

        private void CreateTestFolder()
        {
            Directory.CreateDirectory(PathToClonedGitRepo);
        }
        private void RemoveTestFolder()
        {
            Utils.RemoveDirectory(PathToClonedGitRepo);
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
                using (var repo = new Repository(PathToClonedGitRepo))
                {
                    repo.Config.Unset("core.excludesfile");
                }
            }
        }
    }
}