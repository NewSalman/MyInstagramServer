using Microsoft.AspNetCore.Http;
using MyInstagramApi.InstagramModel;
using MyInstagramApi.Model;
using MyInstagramApi.Models;
using MyInstagramApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyInstagramApi.Services.PostServices
{
    public interface IPostService
    {
        Task<PostModel> SavePost(PostModel model, IFormFile file);

        Task<ObservableCollection<HomeFeedModel>> GetAllPost();

        Task<IEnumerable<HomeFeedModel>> ProfilePage(string username);
    }
}
