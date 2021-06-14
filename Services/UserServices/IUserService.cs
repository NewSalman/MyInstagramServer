using MyInstagramApi.Model;
using MyInstagramApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyInstagramApi.Services.UserServices
{
    public interface IUserService
    {
        Task<List<string>> RegisterAsync(RegisterModel model);

        Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);
    }
}
