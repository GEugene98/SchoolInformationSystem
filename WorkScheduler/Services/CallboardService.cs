using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Shared;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Services
{
    public class CallboardService
    {
        protected Context Db;

        public CallboardService(Context context)
        {
            Db = context;
        }

        public IEnumerable<PostViewModel> GetPosts(int schoolId)
        {
            try
            {
                var posts =
                 Db.Posts
                .Include(p => p.User)
                .Where(p => p.User.SchoolId == schoolId)
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToList()
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Text = p.Text,
                    Author = new UserViewModel
                    {
                        Id = p.User.Id,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        SurName = p.User.SurName
                    },
                    CreatedAt = p.CreatedAt,
                    Color = p.Color == Color.Blue ? "c-blue" : (p.Color == Color.Red ? "c-red" : (p.Color == Color.Green ? "c-green" : ""))
                }
                );

                return posts;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public void DeletePost(long id)
        {
            var post = Db.Posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                throw new Exception("Объявление уже было удалено ранее. Обновите страницу");
            }

            Db.Posts.Remove(post);

            Db.SaveChanges();
        }

        public void PublishPost(PostViewModel post, string userId)
        {
            var newPost = new Post
            {
                Text = post.Text,
                UserId = userId,
                Color = post.Color == "c-blue" ? Color.Blue : (post.Color == "c-red" ? Color.Red : (post.Color == "c-green" ? Color.Green : Color.White)),
                CreatedAt = DateTime.Now
            };

            Db.Posts.Add(newPost);

            Db.SaveChanges();
        }
    }
}
