using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace ScreenObjectsHelpers.Helpers
{
    public class GitWrapper
    {
        public string Username { get; set; }
        public string Email { get; set; }
        private readonly Repository _gitRepository;
        private bool _disposed;
        private Dictionary<string, UsernamePasswordCredentials> _remoteUrlCredentials = new Dictionary<string, UsernamePasswordCredentials>();

        public GitWrapper(string pathToRepository, string usernameCommiter, string email)
        {
            Username = usernameCommiter;
            Email = email;
            if (!Directory.Exists(pathToRepository))
            {
                Directory.CreateDirectory(pathToRepository);
            }
            if (!Utils.IsFolderGit(pathToRepository))
            {
                Repository.Init(pathToRepository);
            }
            _gitRepository = new Repository(pathToRepository);
        }

        public static GitWrapper GetRepositoryByPath(string path)
        {
            return Repository.IsValid(path) ? new GitWrapper(path, null, null) : null;
        }

        public string BranchName => _gitRepository.Head.RemoteName;
        public bool HasUnpushedCommits => _gitRepository.Head.TrackingDetails.AheadBy > 0;
        public bool HasUncommittedChanges => _gitRepository.RetrieveStatus().Any(s => s.State != FileStatus.Ignored);
        public IEnumerable<Commit> Log => _gitRepository.Head.Commits;

        public void AddRemote(string remoteUrl, string username, string password, string name = "origin")
        {
            _gitRepository.Network.Remotes.Update(name, r => { r.Url = remoteUrl; });
            var userCreds = new UsernamePasswordCredentials { Username = username, Password = password };
            _remoteUrlCredentials.Add(name, userCreds);
        }

        public void StageChanges()
        {
            try
            {
                RepositoryStatus status = _gitRepository.RetrieveStatus(); 
                List<string> filePaths = status.Modified.Select(mods => mods.FilePath).ToList();
                Commands.Stage(_gitRepository, filePaths);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:RepoActions:StageChanges " + ex.Message);
            }
        }

        public void CommitChanges(string message = "updating files..")
        {
            try
            {
                _gitRepository.Commit(message, new Signature(Username, Email, DateTimeOffset.Now),
                    new Signature(Username, Email, DateTimeOffset.Now));
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:RepoActions:CommitChanges " + e.Message);
            }
        }

        public void PushChanges(string remoteName = "origin", string branchName = "master")
        {
            try
            {
                Remote remote = _gitRepository.Network.Remotes[remoteName];
                Branch localBranch = _gitRepository.Head;
                _gitRepository.Branches.Update(localBranch, b => b.Remote = remote.Name, b => b.UpstreamBranch = localBranch.CanonicalName);
                var options = new PushOptions();
                options.CredentialsProvider = (url, user, cred) => _remoteUrlCredentials[remoteName];
                _gitRepository.Network.Push(_gitRepository.Branches[branchName], options);
 
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:RepoActions:PushChanges " + e.Message);
            }
        }

        public void PullChanges(string remoteName = "origin")
        {
            PullOptions options = new PullOptions();
            options.FetchOptions = new FetchOptions();
            options.FetchOptions.CredentialsProvider = (url, user, cred) => _remoteUrlCredentials[remoteName];
            Commands.Pull(_gitRepository, new Signature(Username, Email, new DateTimeOffset(DateTime.Now)), options);

        }

        public void AddFileWithRandomText()
        {
            var content = "Commit this!";
            var fileName = "newfileToCommit.txt";
            File.WriteAllText(Path.Combine(_gitRepository.Info.WorkingDirectory, fileName), content);
            Commands.Stage(_gitRepository, fileName);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _gitRepository.Dispose();
        }
    }
}
