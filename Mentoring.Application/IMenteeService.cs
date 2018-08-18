using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mentoring.Core;

namespace Mentoring.Application
{
    public interface IMenteeService
    {
        void AcceptMentee(int menteeId);

        int Count(Expression<Func<Mentee, bool>> filter);

        IEnumerable<Mentee> GetAllActive();

        Mentee GetById(int id);

        Mentee GetByUserId(int id);

        Mentee NewMentee(Mentee mentee, int userId, int firstMentorId, int secondMentorId, string location, string seniority);

        void RejectMentee(int menteeId);

        IEnumerable<Mentee> Search(Expression<Func<Mentee, bool>> filter);

        void SetInactive(int menteeId);

        void SetPendingRenewal();

        Mentee UpdateMentee(Mentee mentee);
    }
}