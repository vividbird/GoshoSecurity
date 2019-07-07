namespace GoshoSecurity.Services
{
    using AutoMapper.QueryableExtensions;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using GoshoSecurity.Data.Interfaces;
    using GoshoSecurity.Infrastructure;
    using GoshoSecurity.Infrastructure.CloudinaryConfig;
    using GoshoSecurity.Infrastructure.CognitiveServicesFaceConfig;
    using GoshoSecurity.Models;
    using GoshoSecurity.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.CognitiveServices.Vision.Face;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PhotoService : BaseService, IPhotoService
    {
        private readonly Cloudinary cloudinary;
        private readonly CloudinarySettings cloudinarySettings;
        private readonly CognitiveServicesFaceConfig cognitiveServicesConfig;
        private readonly FaceClient faceClient;
        private readonly IRepository<Photo> photosRepository;
        private readonly IRepository<GoshoSecurityUser> usersRepository;

        public PhotoService(IOptions<CognitiveServicesFaceConfig> cognitiveServicesConfig,
            IOptions<CloudinarySettings> settings,
            IRepository<GoshoSecurityUser> usersRepository,
            IRepository<Photo> photosRepository)
        {
            this.cognitiveServicesConfig = cognitiveServicesConfig.Value;
            this.cloudinarySettings = settings.Value;
            this.usersRepository = usersRepository;
            this.photosRepository = photosRepository;

            var account = new Account(
                this.cloudinarySettings.CloudName,
                this.cloudinarySettings.ApiKey,
                this.cloudinarySettings.ApiSecret);

            this.cloudinary = new Cloudinary(account);

            faceClient = new FaceClient(new ApiKeyServiceClientCredentials(this.cognitiveServicesConfig.ApiKey))
            {
                Endpoint = this.cognitiveServicesConfig.ApiEndpoint
            };
        }

        public void Delete(string userId)
            => this.cloudinary.DeleteResourcesByPrefix($"{userId}/");

        public async Task Delete(string userId, string photoId)
        {
            this.cloudinary.DeleteResourcesByPrefix($"{userId}/{photoId}");

            var photoToRemove = await this.photosRepository
                .All()
                .SingleOrDefaultAsync(p => p.Id == photoId);

            if (photoToRemove == null)
            {
                return;
            }

            await faceClient.PersonGroupPerson.DeleteFaceAsync(GlobalConstants.PersonGroupdId, new Guid(userId), new Guid(photoToRemove.Id));

            this.photosRepository.Remove(photoToRemove);
            await this.photosRepository.SaveChangesAsync();
        }

        public async Task<T> GetPhotoById<T>(string photoId)
           => await this.photosRepository
               .All()
               .Where(p => p.Id == photoId)
               .ProjectTo<T>()
               .SingleOrDefaultAsync();

        public List<Photo> GetAllPhotosForUser(string userId)
        {
            return this.photosRepository
                          .All()
                          .Where(p => p.UserId == userId)
                          .ToList();
        }

        public async Task<ImageUploadResult> Upload(IFormFile photo, string userId)
        {
            var uploadResult = new ImageUploadResult();

            var user = await usersRepository.All().SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            var result = await faceClient.PersonGroupPerson
                .AddFaceFromStreamAsync(GlobalConstants.PersonGroupdId,new Guid(userId), photo.OpenReadStream());

            if (result == null)
            {
                return null;
            }

            var uploadParams = new ImageUploadParams
            {
                PublicId = result.PersistedFaceId.ToString(),
                File = new FileDescription(result.PersistedFaceId.ToString(), photo.OpenReadStream()),
                Folder = $"{userId}",
            };

            uploadResult = this.cloudinary.Upload(uploadParams);

            var photoToAdd = new Photo()
            {
                Id = uploadResult.PublicId.Substring(uploadResult.PublicId.LastIndexOf('/') + 1),
                UserId = userId,
                Url = uploadResult.SecureUri.AbsoluteUri
            };

            await faceClient.PersonGroupPerson.AddFaceFromUrlAsync(GlobalConstants.PersonGroupdId, new Guid(userId), photoToAdd.Url);

            await this.photosRepository.AddAsync(photoToAdd);
            await this.photosRepository.SaveChangesAsync();

            return uploadResult;
        }

        
    }
}
