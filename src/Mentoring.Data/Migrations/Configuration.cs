namespace Mentoring.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Core;
    using Core.Enums;

    internal sealed class Configuration : DbMigrationsConfiguration<MentoringEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MentoringEntities context)
        {
            var eightToNine = new TimeSlot { Id = 1, Description = "8 to 9" };
            var seventeenToEighteen = new TimeSlot { Id = 10, Description = "17 to 18" };
            var thirteenToFourteen = new TimeSlot { Id = 6, Description = "13 to 14" };
            context.TimeSlots.AddOrUpdate(
                                    eightToNine,
                                    new TimeSlot { Id = 2, Description = "9 to 10" },
                                    new TimeSlot { Id = 3, Description = "10 to 11" },
                                    new TimeSlot { Id = 4, Description = "11 to 12" },
                                    new TimeSlot { Id = 5, Description = "12 to 13" },
                                    thirteenToFourteen,
                                    new TimeSlot { Id = 7, Description = "14 to 15" },
                                    new TimeSlot { Id = 8, Description = "15 to 16" },
                                    new TimeSlot { Id = 9, Description = "16 to 17" },
                                    seventeenToEighteen,
                                    new TimeSlot { Id = 11, Description = "18 to 19" });

            var softSkills = new Topic { Id = 1, Description = "Soft skills development (E.g. Leadership, Team Work, Time Management, Effective Communication, Management, etc.)" };
            var preparationCertification = new Topic { Id = 4, Description = "Preparation for a certification (Scrum, PMI, ISO, Agile, English)" };
            var areaChange = new Topic { Id = 3, Description = "Changes of area/ profile/ position/ site/ country" };
            context.Topics.AddOrUpdate(
                softSkills,
                new Topic { Id = 2, Description = "Technical skills development (E.g. Java, Web UI, Big data, NET, Frameworks)" },
                areaChange,
                preparationCertification,
                new Topic { Id = 5, Description = "Other" });

            var mentorRole = new UserRole { Id = (int)UserRoleCode.Mentor, Description = Enum.GetName(typeof(UserRoleCode), UserRoleCode.Mentor) };
            var menteeRole = new UserRole { Id = (int)UserRoleCode.Mentee, Description = Enum.GetName(typeof(UserRoleCode), UserRoleCode.Mentee) };
            var careerRole = new UserRole { Id = (int)UserRoleCode.Career, Description = Enum.GetName(typeof(UserRoleCode), UserRoleCode.Career) };
            context.UserRoles.AddOrUpdate(mentorRole);
            context.UserRoles.AddOrUpdate(menteeRole);
            context.UserRoles.AddOrUpdate(careerRole);

            var defaultUser = new User
            {
                Name = "mario",
                AvatarUrl = "https://blzmedia-a.akamaihd.net/d3/icons/portraits/42/barbarian_male.png",
                Email = "mario@company.com",
                JoinDate = DateTime.Today,
                Location = "",
                Seniority = "Senior",
                NickName = "Mario"
            };
            defaultUser.Roles.Add(careerRole);
            context.Users.AddOrUpdate(u => u.Name, defaultUser);

            if (!context.ProgramStatus.Any())
            {
                var newProgramStatus = new ProgramStatus
                    {
                        Id = 1,
                        CreationDate = DateTime.Now,
                        StatusCode = (int)ProgramStatusCode.ProgramInProgress,
                        StatusDescription = "Program In Progress"
                    };

                context.ProgramStatus.Add(newProgramStatus);
            }

            context.SaveChanges();
        }
    }
}