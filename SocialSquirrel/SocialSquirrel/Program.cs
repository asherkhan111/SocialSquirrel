using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocialSquirrel.Models;

namespace SocialSquirrel
{
    class Program
    {
        static void Main(string[] args)
        {
            var users = new List<User>();
            var userWantsToExit = false;

            while (!userWantsToExit)
            {
                Console.WriteLine(Responses.PleaseEnterCommand);
                var userResponse = Console.ReadLine();

                var userNameAndResponse = userResponse.Split(null, 2);

                userWantsToExit = userNameAndResponse[0].ToLower() == CommandIdentifiers.ExitCommand;

                if (!userWantsToExit)
                {
                    var userName = userNameAndResponse[0];
                    var response = userNameAndResponse.Length > 1 ? userNameAndResponse[1] : string.Empty;

                    var splitResponse = response.Split(null, 2);

                    User user;
                    getUser(userName, users, out user);

                    //Reading
                    if (string.IsNullOrEmpty(response))
                    {
                        displayPosts(user.Posts);
                    }

                    //Wall
                    else if (response.ToLower() == CommandIdentifiers.WallCommand)
                    {
                        var posts = getUserPostsAndSubscribedPosts(user, users);
                        displayPosts(posts);
                    }

                    //Following
                    else if (splitResponse[0].ToLower() == CommandIdentifiers.FollowCommand)
                    {
                        var userToFollow = splitResponse[1];
                        followUser(user, userToFollow);
                    }

                    //Posting
                    else
                    {
                        submitPost(response, user);
                    }
                }
            }
        }


        private static void getUser(string userName, List<User> users, out User addedUser)
        {
            if (!users.Select(u => u.UserName).Contains(userName))
            {
                addedUser = User.CreateNew(userName);
                users.Add(addedUser);
            }
            else
            {
                addedUser = users.First(u => u.UserName == userName);
            }
        }

        private static void displayPosts(IEnumerable<Post> posts)
        {
            if (posts.ToList().Count != 0)
            {
                posts
                    .OrderByDescending(p => p.CreationDate).ToList()
                    .ForEach(p => Console.WriteLine(Responses.DisplayPostPlaceholder, p.UserName, p.Message, p.FormattedCreationDate));
            }
        }

        private static void followUser(User user, string userNameToFollow)
        {
            if (!user.FollowingUserNames.Contains(userNameToFollow))
            {
                user.FollowingUserNames.Add(userNameToFollow);
                Console.WriteLine(Responses.FollowingPlaceholder, userNameToFollow);
            }
            else
            {
                Console.WriteLine(Responses.UserAlreadyFollowed);
            }
        }

        private static List<Post> getUserPostsAndSubscribedPosts(User user, List<User> allUsers)
        {
            var posts = user.Posts.ToList();
            foreach (var followingUserName in user.FollowingUserNames)
            {
                var followedUser = allUsers.First(u => u.UserName == followingUserName);
                foreach (var followedUserPost in followedUser.Posts)
                {
                    posts.Add(followedUserPost);
                }
            }
            return posts;
        }

        private static void submitPost(string message, User user)
        {
            var messageWithoutCommand = removePostCommandIfExists(message);
            user.Posts.Add(Post.CreateNew(messageWithoutCommand, user.UserName));
            Console.WriteLine(Responses.PostAdded);
        }

        private static string removePostCommandIfExists(string message)
        {
            var splitMessage = message.Split(null, 2);
            if (splitMessage[0] == CommandIdentifiers.PostCommand)
            {
                return splitMessage[1];
            }
            else
            {
                return message;
            }
        }
    }
}
