using System;
using RestSharp;

namespace FligClient
{
    public class LockedFilesModel : ILockedfilesModel
    {
        public IRestClient _apiClient;
        public string webAPIAddress = @"http://localhost:18777/";

        private string LockApiRequest = @"/flig/lock/{user}/{file}";
        private string OverrideApiRequest = @"/flig/override/{user}/{file}";
        private string UnlockApiRequest = @"/flig/unlock/{user}/{file}";
        private string CheckApiRequest = @"/flig/check/{file}";

        public LockedFilesModel() : this(new RestClient())
        { }

        public LockedFilesModel(IRestClient apiClient)
        {
            _apiClient = apiClient;
            _apiClient.BaseUrl = new Uri(webAPIAddress);
        }

        public bool LockFile(string filename)
        {
            return ExecuteWebRequest(LockApiRequest).ResponseStatus == ResponseStatus.Completed;
        }

        public bool OverrideLockOnFile(string filename)
        {
            return ExecuteWebRequest(OverrideApiRequest).ResponseStatus == ResponseStatus.Completed;
        }

        public LockedFileInfo CheckLockOnFile(string filename)
        {
            return new LockedFileInfo();
        }

        public bool UnlockFile(string filename)
        {
            return true;
        }

        private IRestResponse ExecuteWebRequest(string apiLocation)
        {
            var request = new RestRequest(OverrideApiRequest, Method.GET);
            return _apiClient.Execute(request);            
        }
    }
}
