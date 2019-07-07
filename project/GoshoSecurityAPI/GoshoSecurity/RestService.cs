using GoshoSecurityAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GoshoSecurity
{
    public class RestService
    {
        public static LoggedUser LoggedUser;
        private const string usersUrl = "https://localhost:5001/api/users/{0}";

        HttpClient client;

        public RestService()
        {
            client = new HttpClient();
        }

        public async System.Threading.Tasks.Task<string> LoginAsync(UserLoginModel user)
        {
            var uri = new Uri(string.Format(usersUrl, "authenticate"));

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, content);

            var t = await response.Content.ReadAsStringAsync();

            LoggedUser = JsonConvert.DeserializeObject<LoggedUser>(t);

            return LoggedUser.Email;
        }
    }
}
