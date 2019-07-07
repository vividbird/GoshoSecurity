namespace GoshoSecurity.Services.Interfaces
{
    using CloudinaryDotNet.Actions;
    using GoshoSecurity.Models;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPhotoService
    {
        void Delete(string userId);

        Task Delete(string userId, string photoId);

        List<Photo> GetAllPhotosForUser(string userId);

        Task<T> GetPhotoById<T>(string photoId);

        Task<ImageUploadResult> Upload(IFormFile photo, string userId);
    }
}
