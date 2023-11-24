//TrackMe.BLL/UserService.cs
using System.Collections.Generic;
using System.Linq;
using DataLogic;
// Make sure to use the correct namespace

namespace TrackMe.BLL
{
    public class UserService
    {
        private readonly DataLogic.MyDBLogic _dbContext;

        public UserService(DataLogic.MyDBLogic dbContext)
        {
            _dbContext = dbContext;
        }

        public List<DataLogic.User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public void AddUser(DataLogic.User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        // Add other methods to work with users if needed
    }
}