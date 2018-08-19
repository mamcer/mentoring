using System.Collections.Generic;
using Mentoring.Core;

namespace Mentoring.Application
{
    public interface IUserLogService
    {
        void Add(int userId, UserLogAction action, string description);

        IEnumerable<UserLog> GetAll();
    }
}