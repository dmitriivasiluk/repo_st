﻿
namespace SourceTree.AutomationTests.Utils.Helpers
{
    public class ConstantsList
    {
        //Constants for Help Menu (about window) tests
        public const string appVersion = "Version ";
        public const string copyrightCaption = "Copyright Atlassian 2012-2017. All Rights Reserved.";
        public const string aboutWindowHeader = "About SourceTree";

        //Constants for custom actions tests
        public const string addCustomActionName = "test1";
        public const string editedCustomActionName = "editedCustomAction";
        public const string customActionToBeDeleted = "customActionToBeEdited";

        //Constants for BasicTest
        public const string pathToLocalappdata = @"%localappdata%\";
        public const string pathToUserprofile = @"%userprofile%\";
        public const string userConfig = "user.config";
        public const string bookmarksXml = "bookmarks.xml";
        public const string opentabsXml = "opentabs.xml";
        public const string accountsJson = "accounts.json";
        public const string currentUserProfile = "%userprofile%";

        //Constants for Clone Tab tests
        public const string gitRepoType = "Git";
        public const string mercurialRepoType = "Mercurial";

            //CloneTab: link validation tests
        public const string gitRepoLink = @"https://github.com/GitHubNoTwoStepVerification/test_gh_git_publ_che.git";
        public const string mercurialRepoLink = @"https://UserAccountNo2FA@bitbucket.org/UserAccountNo2FA/test_bb_hg_publ_che";
        public const string notValidRepoLink = @"https://vcxzwsx@bitbucket.org/vcxzwsx/vcxzwsx.jit";

            //CloneTab: clone tests
        public const string gitRepoToClone = @"https://UserAccountNo2FA@bitbucket.org/UserAccountNo2FA/test_bbgitrepotoclone.git";
        public const string testGitRepoBookmarkName = "test_bbgitrepotoclone";

        public const string mercurialRepoToClone = @"https://UserAccountNo2FA@bitbucket.org/UserAccountNo2FA/test_bbmercurialrepotoclone";
        public const string testHgRepoBookmarkName = "test_bbmercurialrepotoclone";        

        //Constants for Clone tests (clone tab and welcome wizard)
        public const string dotGitFolder = ".git";
        public const string dotHgFolder = ".hg";
        public const string invalidFolder = "notValid";
        public const string emptyPath = "emptyPath";

        //Constants for Add Tab tests
        public const string emptyFolderForAddTest = @"TEST_Empty_AddTab";
        public const string gitInitFolderForAddTest = @"TEST_GitInit_AddTab";
        public const string hgInitFolderForAddTest = @"TEST_HgInit_AddTab";
        
        //Constants for Create Tab tests
        public const string fileForNotEmptyFolder = "SomeFile.txt";
        public const string mercurialExistRepoWarnTitle = "Failed to create local repository";

        //Constants for DialogSelectDestination
        public const string dialogSelectDestinationTitle = "Select Destination Path";

        //Constants for GitFlow Initialise window
        public const string defaultProductionBranch = "master";
        public const string defaultDevelopmentBranch = "develop";
        public const string defaultFeatureBranch = "feature/";
        public const string defaultReleaseBranch = "release/";
        public const string defaultHotfixBranch = "hotfix/";

        //Validation messages for Submodules and Subtrees
        public const string noSourcePathEntered = "No path / URL supplied";
        public const string wrongSourcePathEntered = "This is not a valid source path / URL";
        public const string correctSourcePathEntered = "This is a Git repository";
        public const string errorMessageForMercurialRepo = "Submodules can only be git repositories.";
    }
}