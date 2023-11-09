// TrackMe.BLL/UserService.cs
using System.Collections.Generic;
using System.Linq;
using TrackMe.DataLogic;// Make sure to use the correct namespace

namespace TrackMe.BLL
{
    public class UserService
    {
        private readonly MyDBLogic _dbContext;

        public UserService(MyDBLogic dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        // Add other methods to work with users if needed
    }
}