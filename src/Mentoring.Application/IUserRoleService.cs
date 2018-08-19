using Mentoring.Core;
using Mentoring.Core.Enums;

namespace Mentoring.Application
{
    public interface IUserRoleService
    {
        UserRole GetUserRoleByCode(UserRoleCode roleId);
    }
}