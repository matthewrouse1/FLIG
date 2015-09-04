using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Security.RightsManagement;
using System.Windows.Documents;
using RestSharp;
using RestSharp.Extensions.MonoHttp;

namespace FligClient
{
    public class LockedFilesModel : ILockedfilesModel
    {

        public IRestClient _apiClient;

        private string LockApiRequest = @"/flig/lock/{user}/{file}";
        private string OverrideApiRequest = @"/flig/override/{user}/{file}";
        private string UnlockApiRequest = @"/flig/unlock/{user}/{file}";
        private string CheckApiRequest = @"/flig/check/{file}";

        public LockedFilesModel() : this(new RestClient())
        { }

        public LockedFilesModel(IRestClient apiClient)
        {
            _apiClient = apiClient;
            _apiClient.BaseUrl = new Uri(UserInfo.WebApiPath);
        }

        public bool LockFile(string filename)
        {
            return ExecuteWebRequest(LockApiRequest, filename).ResponseStatus == ResponseStatus.Completed;
        }

        public bool OverrideLockOnFile(string filename)
        {
            return ExecuteWebRequest(OverrideApiRequest, filename).ResponseStatus == ResponseStatus.Completed;
        }

        private string EncodeFilename(string filename)
        {
            return filename.Replace(UserInfo.RepoDir, "").Replace(@"\", "");
        }

        // Special case so the restclient request has to go straight to the implementation
        public LockedFileInfo CheckLockOnFile(string filename)
        {
            var response = _apiClient.Execute<List<LockObject>>(new RestRequest(CheckApiRequest, Method.GET) { Parameters = {  new Parameter() { Name = "file", Value = EncodeFilename(filename), Type = ParameterType.UrlSegment} }});
            return new LockedFileInfo() { Locks = response.Data };
        }

        public bool UnlockFile(string filename)
        {
            return ExecuteWebRequest(UnlockApiRequest, filename).ResponseStatus == ResponseStatus.Completed;
        }

        private IRestResponse ExecuteWebRequest(string apiLocation, string filename)
        {
            var request = new RestRequest(apiLocation, Method.GET);
            request.AddParameter("user", UserInfo.Username, ParameterType.UrlSegment);
            request.AddParameter("file", EncodeFilename(filename), ParameterType.UrlSegment);
            return _apiClient.Execute(request);            
        }
    }

    public class LockObject
    {
        public string Username { get; set; }
        public DateTime LockedDateTime { get; set; }
    }
}
