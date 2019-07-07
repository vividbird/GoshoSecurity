namespace GoshoSecurityAPI.Controllers
{
    using GoshoSecurity.Infrastructure;
    using GoshoSecurity.Services.Interfaces;
    using GoshoSecurityAPI.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService photoService;

        public PhotosController(IPhotoService photoService)
        {
            this.photoService = photoService;
        }

        [HttpPost("upload/{id}")]
        [AuthorizeToken]
        [Consumes("application/json", "application/json-patch+json", "multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile file, string id)
        {
            try
            {
                if (file == null)
                {
                    return BadRequest(NotificationCodes.ImageNullError);
                }

                var result = await photoService.Upload(file, id);   

                if (result == null)
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(NotificationCodes.ImageSuccess);
        }

        [HttpGet("get/{userId}")]
        [AuthorizeToken]
        public async Task<IActionResult> Get(string userId)
        {
            return Ok(photoService.GetAllPhotosForUser(userId).ToArray());
        }

        [HttpDelete("delete/{userId}/{photoId}")]
        [AuthorizeToken]
        [Consumes("application/json", "application/json-patch+json", "multipart/form-data")]
        public async Task<IActionResult> Delete(string userId, string photoId)
        {
            try
            {
                await photoService.Delete(userId, photoId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(NotificationCodes.ImageSuccess);
        }

    }
}