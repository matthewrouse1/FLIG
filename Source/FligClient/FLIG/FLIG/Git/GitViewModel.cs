using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FligClient.FileBrowsing;
using LibGit2Sharp;

namespace FligClient.Git
{
    public class GitViewModel
    {
        private GitModel _gitModel;

        public GitViewModel() : this (new GitModel(UserInfo.Username, UserInfo.EmailAddress, UserInfo.Password, UserInfo.RepoDir, UserInfo.RepoUrl))
        { }

        public GitViewModel(GitModel gitModel)
        {
            _gitModel = gitModel;
            FilesToStage = new List<string>();
        }

        public List<string> FilesToStage;

        public string CommitMessage;

        public void Add()
        {
            if (FilesToStage.Count > 0)
            {
                _gitModel.Add(FilesToStage);
                FilesToStage = new List<string>();
            }
        }

        public void Commit()
        {
            if (string.IsNullOrEmpty(CommitMessage))
                return;
            _gitModel.Commit(CommitMessage);
        }

        public void Pull()
        {
            PullHasNewFiles();
        }

        public bool PullHasNewFiles()
        {
            var mergeResult = _gitModel.Pull();
            if (mergeResult.Status != MergeStatus.UpToDate)
                return false;
            return true;
        }

        public void Push()
        {
            _gitModel.Push();
        }

        public void ResetFiles(IList selectedItemsList)
        {
            _gitModel.Reset((from object file in selectedItemsList select ((FligFile) file).Name).ToList());
        }
    }
}
