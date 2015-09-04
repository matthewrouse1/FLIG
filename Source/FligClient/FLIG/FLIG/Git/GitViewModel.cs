using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace FligClient.Git
{
    public class GitViewModel
    {
        private GitModel _gitModel;

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
    }
}
