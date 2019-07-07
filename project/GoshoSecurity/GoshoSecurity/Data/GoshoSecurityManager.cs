namespace GoshoSecurity
{
    using GoshoSecurity.Data;
    using GoshoSecurity.Models;
    using GoshoSecurity.Views;
    using Plugin.Media.Abstractions;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GoshoSecurityManager
	{
        RestService restService;

        public LoggedUser LoggedUser { get => restService.LoggedUser; }

		public GoshoSecurityManager (RestService service)
		{
			restService = service;
		}

		public async Task Login(UserLoginModel model)
		{
            await restService.Authenticate(model);
		}

        public async Task<string> ChangePasswordAsync(PasswordChangeModel model)
        {
            return await restService.ChangePassword(model);
        }

        public async Task<List<Photo>> GetPhotosForUser()
        {
            return await restService.GetUserPhoto();
        }

        public async Task<string> ChangeUserInformation(UserEditModel model)
        {
            return await restService.ChangeUserInformation(model);
        }

        public void Logout()
        {
            restService.Logout();
        }

        public async Task<bool> DeletePhoto(Photo photo)
        {
            return await restService.DeletePhoto(photo.Id);
        }

        public async Task<bool> UploadPhoto(MediaFile file)
        {
            return await restService.UploadPhoto(file);
        }

    }
}
