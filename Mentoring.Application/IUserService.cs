using System;
using System.Collections.Generic;
using Mentoring.Core;
using Mentoring.Core.Enums;

namespace Mentoring.Application
{
    public interface IUserService
    {
        void AddRole(int id, UserRoleCode roleId);

        User CreateUser(string userName, string defaultAvatarUrl);
        
        IEnumerable<User> GetAllUsers();
        
        User GetUserById(int id);
        
        User GetUserByName(string userName);
        
        string NameWithoutDomain(string userName);
        
        void RemoveRole(int id, UserRoleCode roleId);
        
        User UpdateUser(int id, string nickName, string avatarUrl, string email, string location, string seniority);
    }
}