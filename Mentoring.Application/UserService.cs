using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Core.Logging;
using Mentoring.Core;
using Mentoring.Core.Enums;
using Mentoring.Data;

namespace Mentoring.Application
{
    public class UserService : Mentoring.Application.IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;
        private readonly IUserLogService _userLogService;

        public UserService(IUnitOfWork unitOfWork, ILogManager logManager, IUserLogService userLogService)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
            _userLogService = userLogService;
        }

        public User GetUserByName(string userName)
        {
            try
            {
                return _unitOfWork.UserRepository.Search(u => u.Name == userName, null, "Roles").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.UserService.GetUserByName", ex);
                return null;
            }
        }

        public User GetUserById(int id)
        {
            try
            {
                return _unitOfWork.UserRepository.Search(u => u.Id == id, null, "Roles").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.UserService.GetUserById", ex);
                return null;
            }
        }

        public User CreateUser(string userName, string defaultAvatarUrl)
        {
            try
            {
                var user = GetUserByName(userName);

                if (user == null)
                {
                    var nameWithoutDomain = NameWithoutDomain(userName);
                    user = new User
                    {
                        Name = userName,
                        NickName = nameWithoutDomain,
                        Email = nameWithoutDomain + "@company.com",
                        JoinDate = DateTime.Now,
                        AvatarUrl = defaultAvatarUrl,
                        Location = "unknown",
                        Seniority = "unknown"
                    };

                    _unitOfWork.UserRepository.Insert(user);
                    _unitOfWork.Save();

                    _userLogService.Add(user.Id, UserLogAction.UserCreated, string.Format(Resources.Messages.UserCreated, user.Name));
                }

                return user;
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.UserService.CreateUser", ex);
                return null;
            }
        }

        public string NameWithoutDomain(string userName)
        {
            int indexOf = userName.IndexOf("\\", StringComparison.Ordinal);
            if (indexOf != -1)
            {
                userName = userName.Substring(indexOf + 1, userName.Length - indexOf - 1);
            }

            return userName;
        }

        public User UpdateUser(int id, string nickName, string avatarUrl, string email, string location, string seniority)
        {
            if (string.IsNullOrEmpty(nickName))
            {
                throw new ArgumentNullException("nickName");
            }

            if (string.IsNullOrEmpty(nickName))
            {
                throw new ArgumentNullException("avatarUrl");
            }

            try
            {
                var user = _unitOfWork.UserRepository.Get(id);
                user.NickName = nickName;
                user.AvatarUrl = avatarUrl;
                user.Email = email;
                user.Location = location;
                user.Seniority = seniority;

                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();

                return user;
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.UserService.UpdateUser", ex);
                return null;
            }
        }

        public void AddRole(int id, UserRoleCode roleId)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Get(id);
                var role = _unitOfWork.UserRoleRepository.Search(r => r.Id == (int)roleId).FirstOrDefault();
                if (role != null)
                {
                    user.Roles.Add(role);
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();

                    _userLogService.Add(user.Id, UserLogAction.RoleChange, string.Format(Resources.Messages.AddedCareer, user.Name));
                }
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.UserService.AddRole", ex);
            }
        }

        public void RemoveRole(int id, UserRoleCode roleId)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Search(u => u.Id == id, null, "Roles").FirstOrDefault();
                var role = _unitOfWork.UserRoleRepository.Search(r => r.Id == (int)roleId).FirstOrDefault();
                if (role != null)
                {
                    user.Roles.Remove(role);
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();

                    _userLogService.Add(user.Id, UserLogAction.RoleChange, string.Format(Resources.Messages.RemovedCareer, user.Name));
                }
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.UserService.RemoveRole", ex);
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                var users = _unitOfWork.UserRepository.Search(u => true, q => q.OrderByDescending(m => m.Name), "Roles");
                return users.ToList();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.UserService.GetAllUsers", ex);
                return null;
            }
        }
    }
}