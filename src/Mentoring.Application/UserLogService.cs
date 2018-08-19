using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using CrossCutting.Core.Logging;
using Mentoring.Core;
using Mentoring.Data;

namespace Mentoring.Application
{
    public enum UserLogAction
    {
        [Description("Role Change")]
        RoleChange,
        [Description("User Created")]
        UserCreated,
        [Description("Mentee Rejected")]
        MenteeRejected,
        [Description("Pending Approval Mentee")]
        PendingApprovalMentee,
        [Description("Mentee Accepted")]
        MenteeAccepted,
        [Description("Mentor Accepted")]
        MentorAccepted,
        [Description("Mentor Rejected")]
        MentorRejected,
        [Description("Pending Approval Mentor")]
        PendingApprovalMentor,
    }

    public class UserLogService : IUserLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;

        public UserLogService(IUnitOfWork unitOfWork, ILogManager logManager)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return value.ToString();
        }

        public void Add(int userId, UserLogAction action, string description)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Get(userId);

                var userLog = new UserLog
                {
                    User = user,
                    Description = description,
                    Action = GetEnumDescription(action),
                    Date = DateTime.Now
                };

                _unitOfWork.UserLogRepository.Insert(userLog);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.UserLogService.Add", ex);
            }
        }

        public IEnumerable<UserLog> GetAll()
        {
            try
            {
                return _unitOfWork.UserLogRepository.Search(m => true, null, "User");
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.UserLogService.GetAll", ex);
                return null;
            }
        }
    }
}