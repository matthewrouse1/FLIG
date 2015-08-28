using System;
using System.Security.RightsManagement;
using RestSharp;

namespace FligClient
{
    public class LockedFilesModel : ILockedfilesModel
    {
        public IRestClient _apiClient;
        private UserInfo _userInfo;

        public string webAPIAddress = @"http://localhost:18777/";

        private string LockApiRequest = @"/flig/lock/{user}/{file}";
        private string OverrideApiRequest = @"/flig/override/{user}/{file}";
        private string UnlockApiRequest = @"/flig/unlock/{user}/{file}";
        private string CheckApiRequest = @"/flig/check/{file}";

        public LockedFilesModel() : this(new RestClient(), new UserInfo())
        { }

        public LockedFilesModel(IRestClient apiClient, UserInfo userInfo)
        {
            _userInfo = userInfo;
            _apiClient = apiClient;
            _apiClient.BaseUrl = new Uri(webAPIAddress);
        }

        public bool LockFile(string filename)
        {
            return ExecuteWebRequest(LockApiRequest, filename).ResponseStatus == ResponseStatus.Completed;
        }

        public bool OverrideLockOnFile(string filename)
        {
            return ExecuteWebRequest(OverrideApiRequest, filename).ResponseStatus == ResponseStatus.Completed;
        }

        public LockedFileInfo CheckLockOnFile(string filename)
        {
            return new LockedFileInfo();
        }

        public bool UnlockFile(string filename)
        {
            return ExecuteWebRequest(UnlockApiRequest, filename).ResponseStatus == ResponseStatus.Completed;
        }

        private IRestResponse ExecuteWebRequest(string apiLocation, string filename)
        {
            var request = new RestRequest(OverrideApiRequest, Method.GET);
            request.AddParameter("user", _userInfo.Username, ParameterType.UrlSegment);
            request.AddParameter("file", filename, ParameterType.UrlSegment);
            return _apiClient.Execute(request);            
        }
    }
}
