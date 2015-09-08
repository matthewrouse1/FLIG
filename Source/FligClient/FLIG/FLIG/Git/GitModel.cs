using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace FligClient.Git
{
    public class GitModel
    {
        public PullOptions pullOptions { get; private set; }
        public PushOptions pushOptions { get; private set; }
        public FetchOptions fetchOptions { get; private set; }
        public CloneOptions cloneOptions { get; private set; }
        public StatusOptions statusOptions { get; private set; }

        public SecureUsernamePasswordCredentials credentials { get; private set; }
        public LibGit2Sharp.Repository repository { get; private set; }
        public Signature signature { get; private set; }

        private void setupOptions(string Username, SecureString SecurePassword, string RepoPath, string RepoUrl)
        {
            credentials = new SecureUsernamePasswordCredentials() {Username = Username, Password = SecurePassword};
            var credentialsProvider =
                new CredentialsHandler((_url, _user, _cred) => new SecureUsernamePasswordCredentials
                {
                    Username = Username,
                    Password = SecurePassword
                });
            fetchOptions = new FetchOptions() {CredentialsProvider = credentialsProvider};
            pullOptions = new PullOptions() {FetchOptions = fetchOptions};
            pushOptions = new PushOptions() {CredentialsProvider = credentialsProvider};
            cloneOptions = new CloneOptions() {CredentialsProvider = credentialsProvider};
            statusOptions = new StatusOptions() {ExcludeSubmodules = true, IncludeUnaltered = false};
        }

        private void setSignature(string Username, string EmailAddress)
        {
            signature = new Signature(Username, EmailAddress, DateTimeOffset.Now);
        }

        public string CloneRepo(string Url, string CloneToDirectory)
        {
            return LibGit2Sharp.Repository.Clone(Url, CloneToDirectory, cloneOptions);
        }

        public GitModel(string Username, string EmailAddress, SecureString Password,
            string RepoPath,
            string RepoUrl = "http://github.com/AdvancedLegal/FormsLibrary")
        {
            setupOptions(Username, Password, RepoPath, RepoUrl);

            setSignature(Username, EmailAddress);

            if (!Directory.Exists(RepoPath))
            {
                CloneRepo(RepoUrl, RepoPath);
            }

            if (string.IsNullOrEmpty(RepoPath))
                RepoPath = UserInfo.RepoDir;

            if (!RepoPath.EndsWith("\\.git\\"))
                RepoPath += "\\.git\\";

            repository = new Repository(RepoPath);

        }

        public MergeResult Pull()
        {
            return repository.Network.Pull(signature, pullOptions);
        }

        public void Push(Branch Branch = null)
        {
            Branch = Branch ?? repository.Branches["master"];
            repository.Network.Push(Branch, pushOptions);
        }

        public void Add(List<string> files)
        {
            files.ForEach((file => repository.Stage(file)));
        }

        private bool AnyChangedFiles()
        {
            var status = repository.RetrieveStatus(new StatusOptions());
            if (status.Staged.Any())
                return true;
            return false;
        }

        public void Commit(string message)
        {
            if (!AnyChangedFiles())
                return;

            repository.Commit(message);
        }

        public RepositoryStatus Status()
        {
            return repository.RetrieveStatus(statusOptions);
        }

        public void Reset(List<string> files)
        {
            repository.CheckoutPaths("master", files, new CheckoutOptions() { CheckoutModifiers = CheckoutModifiers.Force } );
        }
    }
}
