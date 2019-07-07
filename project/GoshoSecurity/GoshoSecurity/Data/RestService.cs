namespace GoshoSecurity.Data
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using GoshoSecurity.Models;
    using Newtonsoft.Json;
    using Plugin.Media.Abstractions;

    public class RestService
    {
        public LoggedUser LoggedUser { get; private set; }
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
            LoggedUser = new LoggedUser();
        }

        public async Task<LoggedUser> Authenticate(UserLoginModel user)
        {

            var uri = new Uri(string.Format(Constants.UsersUrl, "authenticate"));

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                var respondedContent = await response.Content.ReadAsStringAsync();
                LoggedUser = JsonConvert.DeserializeObject<LoggedUser>(respondedContent);

                return LoggedUser;
            }

            return null;
        }

        public async Task<string> ChangePassword(PasswordChangeModel model)
        {
            var uri = new Uri(string.Format(Constants.UsersUrl, "passwordchange"));

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", LoggedUser.Token);

            var response = await _client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> ChangeUserInformation(UserEditModel model)
        {
            var uri = new Uri(string.Format(Constants.UsersUrl, "update"));

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", LoggedUser.Token);

            var response = await _client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                LoggedUser.Name = model.Name;
                LoggedUser.Email = model.Email;
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<List<Photo>> GetUserPhoto()
        {
            List<Photo> photos = new List<Photo>();

            var uri = new Uri(string.Format(Constants.PhotosUrl, $"get/{LoggedUser.Id}"));

            _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", LoggedUser.Token);

            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var d = await response.Content.ReadAsStringAsync();
                photos = JsonConvert.DeserializeObject<List<Photo>>(d);
            }

            return photos;

        }

        public async Task<bool> DeletePhoto(string photoId)
        {
            var uri = new Uri(string.Format(Constants.PhotosUrl, $"delete/{LoggedUser.Id}/{photoId}"));

            var response = await _client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;

        }

        public async Task<bool> UploadPhoto(MediaFile file)
        {
            var uri = new Uri(string.Format(Constants.PhotosUrl, $"upload/{LoggedUser.Id}"));

            _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", LoggedUser.Token);

            var requestContent = new MultipartFormDataContent();
            var imageContent = new StreamContent(file.GetStream());
            imageContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");
  
            requestContent.Add(imageContent, "file", "file.jpg");

            var result = await _client.PostAsync(uri, requestContent);

            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;

        }

        public void Logout()
        {
            this.LoggedUser = null;
        }
    }
}
