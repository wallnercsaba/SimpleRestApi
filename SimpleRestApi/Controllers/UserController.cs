using Microsoft.AspNetCore.Mvc;
using SimpleRestApi.Models;
using SimpleRestApi.Repositories;
using SimpleRestApi.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public const string DefaultUserId = "aaa";

        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return new OkObjectResult((await userRepository.GetAll()).Select(user => new { user.Id, user.LastName, user.FirstName, user.ProfilePicture }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await userRepository.Get(id);
            return new OkObjectResult(new
            {
                user.Id,
                user.LastName,
                user.FirstName,
                user.ProfilePicture,
                Email = user.EmailVisibility ? user.Email : null,
                PhoneNumber = user.PhoneNumberVisibility ? user.PhoneNumber : null
            });
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            return new OkObjectResult(await userRepository.Get(DefaultUserId));
        }

        [HttpPut("me")]
        public async Task<IActionResult> Update([FromBody] UserRequest request)
        {
            var user = await userRepository.Get(DefaultUserId);

            if (user == null)
            {
                return new BadRequestObjectResult("user not exists");
            }

            user.Email = request.Email;
            user.EmailVisibility = request.EmailVisibility;
            user.PhoneNumber = request.PhoneNumber;
            user.PhoneNumberVisibility = request.PhoneNumberVisibility;
            user.LastName = request.LastName;
            user.FirstName = request.FirstName;

            await userRepository.Update(user);
            await userRepository.SaveChanges();

            return new OkObjectResult(user);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Add(string id, [FromBody] UserRequest request)
        {
            if ((await userRepository.Get(id)) != null)
            {
                return new BadRequestObjectResult("user id exists");
            }

            var user = new User()
            {
                Id = id,
                Email = request.Email,
                EmailVisibility = request.EmailVisibility,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                PhoneNumberVisibility = request.PhoneNumberVisibility
            };

            await userRepository.Add(user);
            await userRepository.SaveChanges();

            return new OkObjectResult(user);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (userRepository.Get(id) == null)
                return new BadRequestResult();

            await userRepository.Delete(id);
            await userRepository.SaveChanges();
            return new OkResult();
        }

        [HttpPost("me/profilepicture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm]ProfilePictureRequest request)
        {
            var image = request.Image;

            List<string> AcceptableImageExtentions = new List<string> { ".jpg", ".jpeg", ".png" };

            string fileExtention = System.IO.Path.GetExtension(image.FileName);


            if (!AcceptableImageExtentions.Contains(fileExtention))
            {
                return new BadRequestObjectResult("Image must be png, jpg or jpeg");
            }
            if (image.Length > 10485760)
            {
                return new BadRequestObjectResult("Max 10 MB");
            }
            
            var user = await userRepository.Get(DefaultUserId);
            if (user.ProfilePicture != null)
            {
                System.IO.File.Delete(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Images"), Path.GetFileName(user.ProfilePicture)));
            }

            var fileName = Guid.NewGuid().ToString() + fileExtention;

            if (image.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Images"), fileName), FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }

            user.ProfilePicture = "/images/" + fileName;
            await userRepository.Update(user);
            await userRepository.SaveChanges();

            return new OkResult();
        }
    }
}
