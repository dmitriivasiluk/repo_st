### SourceTree QA Automation project ###

[![Build status](https://ci.appveyor.com/api/projects/status/2awompskpxxxh2fs?svg=true)](https://ci.appveyor.com/project/icherednyk/sourcetreeqaautomation)

[![Bitbucket open pull requests](https://img.shields.io/bitbucket/pr/atlassian/sourcetreeqaautomation.svg)](https://bitbucket.org/atlassian/sourcetreeqaautomation/pull-requests/)

## Quick Start

### clone git repository
$ `git clone "git@bitbucket.org:atlassian/sourcetreeqaautomation.git"`
### Upload Solution File to Visual Studio



## Project Description

### SourceTree.AutomationTests.Complex used for creation and running flaky and complex automated test scripts
### SourceTree.AutomationTests.General is used for creation and running stable automated test scripts
### ScreenObjectsHelpers is used for creations of windows and helpers



## Branching

### Qa Engineer should create F/SRCTREE-XXX branch name, where XXX is number of the JDog task 

### QA Engineer adds some tests there(F/SRCTREE-XXX )

### QA Engineer creates a PR to merge feature branch into dev

### Merge approved PR into dev

### QA team uses two branches - master and dev. Dev branch is used for work, where all data is mergered of aproved pull requests

### Master branch consists of well tested, stable and refactored code, that comes from dev branch
 


### Automation ID should be created for All windows avaliable on the app, if there are no required Ids -issues should be run in jira 

# commit to force a build