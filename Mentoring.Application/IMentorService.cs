using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mentoring.Core;

namespace Mentoring.Application
{
    public interface IMentorService
    {
        int NumberOfDaysToAutoassignMentor { get; }
        
        void AcceptMentee(int userId, int menteeId);
        
        void AcceptMentor(int mentorId);
        
        void AssignPendingMentees();
        
        int Count(Expression<Func<Mentor, bool>> filter);

        IEnumerable<Mentor> GetAll();
        
        IEnumerable<Mentor> GetAllActive();
        
        IEnumerable<Mentor> GetAllAvailable();
        
        Mentor GetById(int id);
        
        Mentor GetByUserId(int id);
        
        Mentor NewMentor(Mentor mentor, int userId, List<int> selectedTopicIds, List<int> selectedTimeSlotIds, List<int> selectedMenteeSeniorityIds, string location, string seniority);
                
        void RejectMentee(int userId, int menteeId);
        
        void RejectMentor(int mentorId);
        
        IEnumerable<Mentor> Search(Expression<Func<Mentor, bool>> filter);
        
        void SetInactive(int mentorId);
        
        void SetPendingRenewal();
        
        Mentor UpdateMentor(Mentor mentor);
    }
}