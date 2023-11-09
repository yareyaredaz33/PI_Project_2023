// MyApp.UI/Program.cs
using System;
using TrackMe.DataLogic;// Use the correct namespace for UserService
using TrackMe.BLL; // Use the correct namespace for MyDBLogic

namespace MyApp.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new MyDBLogic())
            {
                var userService = new UserService(dbContext);

                // Use UserService methods to interact with users
                var users = userService.GetAllUsers();

                foreach (var user in users)
                {
                    Console.WriteLine($"User ID: {user.Id}, Name: {user.Name}");
                }
            }
        }
    }
}