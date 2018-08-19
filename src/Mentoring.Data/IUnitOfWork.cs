using System;
using Mentoring.Core;

namespace Mentoring.Data
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<Mentor> MentorRepository { get; }

        GenericRepository<Mentee> MenteeRepository { get; }

        GenericRepository<User> UserRepository { get; }

        GenericRepository<TimeSlot> TimeSlotRepository { get; }

        GenericRepository<Topic> TopicRepository { get; }

        GenericRepository<ProgramStatus> ProgramStatusRepository { get; }

        GenericRepository<EmailMessage> EmailMessageRepository { get; }

        GenericRepository<EmailTemplate> EmailTemplateRepository { get; }

        GenericRepository<UserRole> UserRoleRepository { get; }

        GenericRepository<UserLog> UserLogRepository { get; }

        GenericRepository<MenteeSeniority> MenteeSeniorityRepository { get; }
        
        void Save();
    }
}