using System;
using Mentoring.Core;

namespace Mentoring.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MentoringEntities _context = new MentoringEntities();
        private GenericRepository<Mentor> _mentorRepository;
        private GenericRepository<Mentee> _menteeRepository;
        private GenericRepository<User> _userRepository;
        private GenericRepository<TimeSlot> _timeSlotRepository;
        private GenericRepository<Topic> _mentorTopicRepository;
        private GenericRepository<ProgramStatus> _programStatusRepository;
        private GenericRepository<EmailMessage> _emailMessageRepository;
        private GenericRepository<EmailTemplate> _emailTemplateRepository;
        private GenericRepository<UserRole> _userRoleRepository;
        private GenericRepository<UserLog> _userLogRepository;
        private GenericRepository<MenteeSeniority> _menteeSeniorityRepository;
        private bool _disposed;

        public GenericRepository<Mentor> MentorRepository
        {
            get
            {
                if (_mentorRepository == null)
                {
                    _mentorRepository = new GenericRepository<Mentor>(_context);
                }

                return _mentorRepository;
            }
        }

        public GenericRepository<Mentee> MenteeRepository
        {
            get
            {
                if (_menteeRepository == null)
                {
                    _menteeRepository = new GenericRepository<Mentee>(_context);
                }

                return _menteeRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new GenericRepository<User>(_context);
                }

                return _userRepository;
            }
        }

        public GenericRepository<TimeSlot> TimeSlotRepository
        {
            get
            {
                if (_timeSlotRepository == null)
                {
                    _timeSlotRepository = new GenericRepository<TimeSlot>(_context);
                }

                return _timeSlotRepository;
            }
        }

        public GenericRepository<Topic> TopicRepository
        {
            get
            {
                if (_mentorTopicRepository == null)
                {
                    _mentorTopicRepository = new GenericRepository<Topic>(_context);
                }

                return _mentorTopicRepository;
            }
        }

        public GenericRepository<ProgramStatus> ProgramStatusRepository
        {
            get
            {
                if (_programStatusRepository == null)
                {
                    _programStatusRepository = new GenericRepository<ProgramStatus>(_context);
                }

                return _programStatusRepository;
            }
        }

        public GenericRepository<EmailMessage> EmailMessageRepository
        {
            get
            {
                if (_emailMessageRepository == null)
                {
                    _emailMessageRepository = new GenericRepository<EmailMessage>(_context);
                }

                return _emailMessageRepository;
            }
        }

        public GenericRepository<EmailTemplate> EmailTemplateRepository
        {
            get
            {
                if (_emailTemplateRepository == null)
                {
                    _emailTemplateRepository = new GenericRepository<EmailTemplate>(_context);
                }

                return _emailTemplateRepository;
            }
        }

        public GenericRepository<UserRole> UserRoleRepository
        {
            get
            {
                if (_userRoleRepository == null)
                {
                    _userRoleRepository = new GenericRepository<UserRole>(_context);
                }

                return _userRoleRepository;
            }
        }

        public GenericRepository<UserLog> UserLogRepository
        {
            get
            {
                if (_userLogRepository == null)
                {
                    _userLogRepository = new GenericRepository<UserLog>(_context);
                }

                return _userLogRepository;
            }
        }

        public GenericRepository<MenteeSeniority> MenteeSeniorityRepository
        {
            get
            {
                if (_menteeSeniorityRepository == null)
                {
                    _menteeSeniorityRepository = new GenericRepository<MenteeSeniority>(_context);
                }

                return _menteeSeniorityRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }
    }
}