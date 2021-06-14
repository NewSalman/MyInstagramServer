using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyInstagramApi.Data;
using MyInstagramApi.InstagramModel;
using MyInstagramApi.Model;
using MyInstagramApi.Models;
using MyInstagramApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MyInstagramApi.Services.PostServices
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        string[] mimeType = new string[] { ".jpg", ".png" };

        string path = @"{0}\wwwroot\{1}";

        public PostService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<PostModel> SavePost(PostModel model, IFormFile file)
        {
            string filename = $"{Guid.NewGuid() + model.Sender + model.MimeType}";
            string UploadPath = string.Format(path, Program.ProjectPath, (mimeType.Contains(model.MimeType)) ? $"images\\{filename}" : $"videos\\{filename}");
            
            PostModel postModel = new PostModel();

            var user = await _userManager.FindByNameAsync(model.Sender);

            if (user == null)
            {
                postModel.Succeded = false;
                postModel.Message = "user not found";
                return postModel;
            }

            _context.Add(new Post()
            {
                Filename = filename,
                Caption = model.Caption,
                UserId = user.Id,
                Likes = 0,
                CommentsCount = 0,
                share = 0,
                PostedAt = model.PostedAt
            });

            try
            {
                using(Stream s = file.OpenReadStream())
                using(MemoryStream ms = new MemoryStream())
                {
                    await s.CopyToAsync(ms);
                    await File.WriteAllBytesAsync(UploadPath, ms.ToArray());
                    await _context.SaveChangesAsync();
                }
                postModel.Succeded = true;
                return postModel;
            } catch(FileNotFoundException)
            {
                postModel.Message = "File not found or missing";
                postModel.Succeded = false;
                return postModel;
            }
        }

        public async Task<ObservableCollection<HomeFeedModel>> GetAllPost()
        {
            ObservableCollection<HomeFeedModel> posts = new ObservableCollection<HomeFeedModel>();

            try
            {
                var post = await Task.Run(() =>
                {
                    return _context.Post.OrderByDescending(e => e.TestID).ToList();
                });

                if (post == null) return posts;

                foreach(var item in post)
                {
                    FileInfo fi = new FileInfo(item.Filename);

                    FileInfo stringPath = new FileInfo(string.Format(path, Program.ProjectPath, (mimeType.Contains(fi.Extension)) ? $"images\\{fi.Name}" : $"videos\\{fi.Name}"));

                    HomeFeedModel x = new HomeFeedModel();

                    var user = await _userManager.FindByIdAsync(item.UserId);

                    x.ID = item.ID.ToString();
                    x.Filename = item.Filename;
                    x.UserID = item.UserId;
                    x.PostedAt = item.PostedAt;
                    x.Caption = item.Caption;
                    x.Username = user.UserName;
                    posts.Add(x);

                }

                return posts;

            }catch(ArgumentNullException)
            {
                Console.WriteLine("post not found");
                return posts;
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return posts;
            }
        }

        public async Task<IEnumerable<HomeFeedModel>> ProfilePage(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            var posts = await Task.Run(() =>
            {
                return _context.Post.Where(e => e.UserId == user.Id).OrderByDescending(e => e.TestID).ToList();
            });

            var result = from i in posts
                         select new HomeFeedModel()
                         {
                             Filename = i.Filename,
                             Caption = i.Caption,
                             Likes = i.Likes,
                             CommentsCount = i.CommentsCount,
                             ID = i.ID,
                             PostedAt = i.PostedAt,
                             Share = i.share,
                             Username = user.UserName
                         };

            return result;
        }
    }
}
