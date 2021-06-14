using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyInstagramApi.InstagramModel;
using MyInstagramApi.Model;
using MyInstagramApi.Models;
using MyInstagramApi.Services.PostServices;
using MyInstagramApi.Services.UserServices;
using MyInstagramApi.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyInstagramApi.Controllers
{
    [Route("api/mobile")]
    [ApiController]
    [Authorize]
    public class MobileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public MobileController(IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;
        }

        // GET: api/<MobileController>
        [HttpPost("user/login")]
        [AllowAnonymous]
        public async Task<AuthenticationModel> Login(TokenRequestModel model)
        {
            var ReqToken = await _userService.GetTokenAsync(model);

            return ReqToken;
        }

        // POST api/<MobileController>
        [HttpPost("user/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var listError = await _userService.RegisterAsync(model);
           if (listError == null)
            {
                return Ok();
            }
            return BadRequest(listError);
        }

        [HttpPost("user/post")]
        public async Task<IActionResult> Post()
        {
            var file = Request.Form.Files;
            var user = Request.Form.Files.GetFile("DataUser");

            StreamReader reader = new StreamReader(user.OpenReadStream());

            var data = JsonConvert.DeserializeObject<PostModel>(await reader.ReadToEndAsync());

            var result = await _postService.SavePost(data, file.GetFile("FileUser"));

            if(result.Succeded == false)
            {
                return BadRequest(result);
            }
            reader.Dispose();
            return Ok();
        }

        [HttpGet("user/home")]
        public async Task<ObservableCollection<HomeFeedModel>> Index()
        {
            return await _postService.GetAllPost();
        }

        [HttpGet("user/profile/{username}")]
        public async Task<IActionResult> ProfilePage(string username)
        {
            if (string.IsNullOrEmpty(username)) return BadRequest();

            return Ok(await _postService.ProfilePage(username));
        }
    }
}
