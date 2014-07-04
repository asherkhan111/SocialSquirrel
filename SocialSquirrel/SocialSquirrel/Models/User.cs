using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialSquirrel.Models
{
    public class User
    {
        public string UserName { get; set; }

        public List<Post> Posts { get; set; }

        public List<string> FollowingUserNames { get; set; }

        public static User CreateNew(string userName)
        {
            return new User() 
            { 
                UserName = userName,
                Posts = new List<Post>(),
                FollowingUserNames = new List<string>()
            };
        }
    }
}
