using System;
using RestSharp;

namespace FligClient
{
    public class LockedFilesModel : ILockedfilesModel
    {
        public IRestClient _apiClient;
        public string webAPIAddress = @"http://localhost:18777/";

        public string LockApiRequest = @"/flig/lock/{user}/{file}";
        public string OverrideApiRequest = @"/flig/override/{user}/{file}";
        public string UnlockApiRequest = @"/flig/unlock/{user}/{file}";
        public string CheckApiRequest = @"/flig/check/{file}";

        public LockedFilesModel() : this(new RestClient())
        { }

        public LockedFilesModel(IRestClient apiClient)
        {
            _apiClient = apiClient;
            _apiClient.BaseUrl = new Uri(webAPIAddress);
        }

        public bool LockFile(string filename)
        {
            var request = new RestRequest(LockApiRequest, Method.GET);
            var response = _apiClient.Execute(request);
            return response.ResponseStatus == ResponseStatus.Completed;
        }

        public bool OverrideLockOnFile(string filename)
        {
            return true;
        }

        public LockedFileInfo CheckLockOnFile(string filename)
        {
            return new LockedFileInfo();
        }

        public bool UnlockFile(string filename)
        {
            return true;
        }
    }
}
