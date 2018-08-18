using System.Data.Entity;
using Mentoring.Core;

namespace Mentoring.Data
{
    public interface IMentoringEntities
    {
        DbSet<Mentor> Mentors { get; set; }

        DbSet<Mentee> Mentees { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<TimeSlot> TimeSlots { get; set; }
        
        DbSet<Topic> Topics { get; set; }

        DbSet<ProgramStatus> ProgramStatus { get; set; }

        DbSet<EmailMessage> EmailMessages { get; set; }

        DbSet<EmailTemplate> EmailTemplates { get; set; }

        DbSet<UserRole> UserRoles { get; set; }

        DbSet<UserLog> UserLogs { get; set; }

        DbSet<MenteeSeniority> MenteeSeniorities { get; set; }
        
        EntityState GetState(object entity);

        void SetModified(object entity);

        DbSet<T> GetSet<T>() where T : class;
    }
}