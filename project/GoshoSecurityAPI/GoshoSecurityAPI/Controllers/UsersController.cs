namespace GoshoSecurityAPI.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using GoshoSecurity.Data.Interfaces;
    using GoshoSecurity.Infrastructure;
    using GoshoSecurity.Infrastructure.CognitiveServicesFaceConfig;
    using GoshoSecurity.Models;
    using GoshoSecurity.Services.Interfaces;
    using GoshoSecurityAPI.Authentication;
    using GoshoSecurityAPI.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.CognitiveServices.Vision.Face;
    using Microsoft.Extensions.Options;

    [Route("api/[controller]")]
    
    [ApiController]

    public class UsersController : ControllerBase
    {
        private FaceClient faceClient;
        private readonly CognitiveServicesFaceConfig cognitiveServicesConfig;
        private readonly IAccountService accountService;
        private readonly IPhotoService photoService;
        private readonly UserManager<GoshoSecurityUser> userManager;

        public UsersController(UserManager<GoshoSecurityUser> userManager, IRepository<GoshoSecurityUser> repository,
            IOptions<CognitiveServicesFaceConfig> cognitiveServicesConfig, IAccountService accountService,
            IPhotoService photoService)
        {
            this.userManager = userManager;
            this.cognitiveServicesConfig = cognitiveServicesConfig.Value;
            this.accountService = accountService;
            this.photoService = photoService;

            faceClient = new FaceClient(new ApiKeyServiceClientCredentials(this.cognitiveServicesConfig.ApiKey))
            {
                Endpoint = this.cognitiveServicesConfig.ApiEndpoint
            };
        }

        [HttpGet]

        [AuthorizeToken(Roles = GlobalConstants.Administrator)]
        public IActionResult Get()
        {
            return Ok(userManager.GetUsersInRoleAsync(string.Empty));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserLoginModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(NotificationCodes.UserIncorrectData);
            }

            var jwtToken = await accountService.GetTokenForUser(model.Username, model.Password);

            if (jwtToken == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "aa");
            }

            var user = await userManager.FindByNameAsync(model.Username);

            var userRole = (await userManager.GetRolesAsync(user)).SingleOrDefault();

            return Ok(new LoggedUser()
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                Token = jwtToken,
                Role = userRole
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(NotificationCodes.RequiredInput);
                }

                var userExists = await userManager.FindByNameAsync(model.Username);

                if (userExists != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict, NotificationCodes.UserExists);
                }

                var person = await faceClient.PersonGroupPerson.CreateAsync(GlobalConstants.PersonGroupdId, model.Name, model.Email);
                var user = new GoshoSecurityUser()
                {
                    Id = person.PersonId.ToString(),
                    Name = model.Name,
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded || person == null)
                {
                    return BadRequest(NotificationCodes.Error);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();          
        }

        [HttpPost("update")]
        [AuthorizeToken]
        public async Task<IActionResult> UpdateProfile([FromBody] UserEditModel userEditModel)
        {
            if (!ModelState.IsValid || userEditModel == null)
            {
                return BadRequest(NotificationCodes.RequiredInput);
            }

            var user = await this.userManager.FindByIdAsync(userEditModel.Id);

            if (user == null)
            {
                return BadRequest(NotificationCodes.NoUserExists);
            }

            await faceClient.PersonGroupPerson
                .UpdateAsync(GlobalConstants.PersonGroupdId, new Guid(user.Id), userEditModel.Name);

            user.Name = userEditModel.Name;
            user.Email = userEditModel.Email;

            await this.userManager.UpdateAsync(user);

            return Ok(NotificationCodes.UserDataSuccess);
        }

        [HttpPost("passwordchange")]
        [AuthorizeToken]
        public async Task<IActionResult> PasswordChange([FromBody] PasswordChangeModel model)
        {
            try
            {
                if (!ModelState.IsValid || model == null)
                {
                    return BadRequest(NotificationCodes.RequiredInput);
                }

                var user = await this.userManager.FindByIdAsync(model.UserId);

                if (user == null)
                {
                    return BadRequest(NotificationCodes.NoUserExists);
                }

                await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                var result = await this.userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(NotificationCodes.PasswordSuccessfullyChanged);
                }
                return BadRequest();

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete/{id}")]
        [AuthorizeToken]
        public async Task<IActionResult> Delete(string id)
        {
            var userDeleted = await accountService.Delete(id);
            photoService.Delete(id);

            await faceClient.PersonGroupPerson.DeleteAsync(GlobalConstants.PersonGroupdId, new Guid(id));

            if (userDeleted)
            {
                return Ok(NotificationCodes.UserDeleted);
            }

            return BadRequest(NotificationCodes.Error);
        }
    }
}
