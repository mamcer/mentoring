namespace Mentoring.Data
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using Core;

    public class MentoringEntities : DbContext, IMentoringEntities
    {
        public MentoringEntities()
            : base("name=MentoringEntities")
        {
        }

        public virtual DbSet<Mentor> Mentors { get; set; }

        public virtual DbSet<Mentee> Mentees { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<TimeSlot> TimeSlots { get; set; }

        public virtual DbSet<Topic> Topics { get; set; }

        public virtual DbSet<ProgramStatus> ProgramStatus { get; set; }

        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

        public virtual DbSet<UserLog> UserLogs { get; set; }

        public virtual DbSet<MenteeSeniority> MenteeSeniorities { get; set; }

        public EntityState GetState(object entity)
        {
            return Entry(entity).State;
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public DbSet<T> GetSet<T>() where T : class
        {
            return Set<T>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Mentor>()
                .HasMany(c => c.Availability)
                .WithMany(m => m.Mentors)
                      .Map(
                           m =>
                           {
                               m.MapLeftKey("MentorId");
                               m.MapRightKey("TimeSlotId");
                               m.ToTable("MentorAvailability");
                           });

            modelBuilder.Entity<Mentor>()
                .HasMany(c => c.MenteeSeniorities)
                .WithMany(m => m.Mentors)
                      .Map(
                           m =>
                           {
                               m.MapLeftKey("MentorId");
                               m.MapRightKey("MenteeSeniorityId");
                               m.ToTable("MentorMenteeSeniority");
                           });

            modelBuilder.Entity<Mentor>()
            .HasMany(c => c.Topic)
            .WithMany(m => m.Mentors)
                  .Map(
                       m =>
                       {
                           m.MapLeftKey("MentorId");
                           m.MapRightKey("TopicId");
                           m.ToTable("MentorTopic");
                       });

            modelBuilder.Entity<User>()
            .HasMany(c => c.Roles)
            .WithMany(m => m.Users)
                .Map(
                       m =>
                       {
                           m.MapLeftKey("UserId");
                           m.MapRightKey("UserRoleId");
                           m.ToTable("UserUserRole");
                       });
        }
    }
}