using System;
using CrossCutting.Core.Logging;
using Mentoring.Core;
using Mentoring.Core.Enums;
using Mentoring.Data;

namespace Mentoring.Application
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;

        public UserRoleService(IUnitOfWork unitOfWork, ILogManager logManager)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
        }

        public UserRole GetUserRoleByCode(UserRoleCode roleId)
        {
            try
            {
                return _unitOfWork.UserRoleRepository.Get((int)roleId);
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.UserRoleService.GetUserRoleByCode", ex);
                return null;
            }
        }
    }
}