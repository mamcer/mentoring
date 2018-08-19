using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CrossCutting.Core.Logging;
using Mentoring.Core;
using Mentoring.Core.Enums;
using Mentoring.Data;

namespace Mentoring.Application
{
    public class MenteeService : IMenteeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;
        private readonly IUserLogService _userLogService;
        private readonly IUserService _userService;

        public MenteeService(IUnitOfWork unitOfWork, ILogManager logManager, IUserLogService userLogService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
            _userLogService = userLogService;
            _userService = userService;
        }

        public Mentee NewMentee(Mentee mentee, int userId, int firstMentorId, int secondMentorId, string location, string seniority)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Get(userId);

                var firstMentor = _unitOfWork.MentorRepository.Get(firstMentorId);
                var secondMentor = _unitOfWork.MentorRepository.Get(secondMentorId);

                user.Roles.Add(_unitOfWork.UserRoleRepository.Get((int)UserRoleCode.Mentee));
                user.Location = location;
                user.Seniority = seniority;
                mentee.User = user;
                mentee.FirstOption = firstMentor;
                mentee.MentorOptionDate = DateTime.Now;
                mentee.SecondOption = secondMentor;
                mentee.Status = MenteeStatus.PendingApproval;

                _userLogService.Add(user.Id, UserLogAction.PendingApprovalMentee, string.Format(Resources.Messages.MenteePendingApproval, user.Name));

                _unitOfWork.MenteeRepository.Insert(mentee);
                _unitOfWork.Save();

                return mentee;
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MenteeService.NewMentee", ex);
                return null;
            }
        }

        public IEnumerable<Mentee> Search(Expression<Func<Mentee, bool>> filter)
        {
            try
            {
                return _unitOfWork.MenteeRepository.Search(filter, null, "FirstOption,SecondOption,Mentor,User,FirstOption.User,FirstOption.Topic,SecondOption.User,SecondOption.Topic,Mentor.User,Mentor.Topic");
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("MentorService.Application.MenteeService.Search", ex);
                return null;
            }
        }

        public int Count(Expression<Func<Mentee, bool>> filter)
        {
            try
            {
                return _unitOfWork.MenteeRepository.Search(filter).Count();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("MentorService.Application.MenteeService.Count", ex);
                return 0;
            }
        }

        public void AcceptMentee(int menteeId)
        {
            try
            {
                var mentee = _unitOfWork.MenteeRepository.Get(menteeId);
                mentee.Status = MenteeStatus.Active;

                _unitOfWork.MenteeRepository.Update(mentee);

                _unitOfWork.Save();

                _userLogService.Add(mentee.User.Id, UserLogAction.MenteeAccepted, string.Format(Resources.Messages.MenteeActive, mentee.User.Name));
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MenteeService.AcceptMentee", ex);
            }
        }

        public void RejectMentee(int menteeId)
        {
            try
            {
                var mentee = _unitOfWork.MenteeRepository.Get(menteeId);

                _userService.RemoveRole(mentee.User.Id, UserRoleCode.Mentee);
                mentee.Status = MenteeStatus.Rejected;

                _unitOfWork.MenteeRepository.Update(mentee);

                _unitOfWork.Save();

                _userLogService.Add(mentee.User.Id, UserLogAction.MenteeRejected, string.Format(Resources.Messages.MenteeRejected, mentee.User.Name));
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.RejectMentee", ex);
            }
        }

        public Mentee GetByUserId(int id)
        {
            try
            {
                return _unitOfWork.MenteeRepository.Search(m => (m.User.Id == id && m.Status != MenteeStatus.Inactive), null, "Mentor,User,Mentor.User,Mentor.Topic").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MenteeService.GetByUserId", ex);
                return null;
            }
        }

        public IEnumerable<Mentee> GetAllActive()
        {
            try
            {
                return _unitOfWork.MenteeRepository.Search(m => m.Status == MenteeStatus.Active, null, "Mentor,User,Mentor.User,Mentor.Topic, FirstOption.User, SecondOption.User");
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MenteeService.GetAllActive", ex);
                return null;
            }
        }

        public Mentee UpdateMentee(Mentee mentee)
        {
            try
            {
                _unitOfWork.MenteeRepository.Update(mentee);
                _unitOfWork.Save();

                return mentee;
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MenteeService.UpdateMentee", ex);
                return null;
            }
        }

        public void SetPendingRenewal()
        {
            try
            {
                var allActive = _unitOfWork.MenteeRepository.Search(m => m.Status == MenteeStatus.Active);
                foreach (var mentee in allActive)
                {
                    mentee.Status = MenteeStatus.PendingRenewal;
                    _unitOfWork.MenteeRepository.Update(mentee);
                }

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MenteeService.SetPendingRenewal", ex);
            }
        }

        public Mentee GetById(int id)
        {
            try
            {
                var mentee = _unitOfWork.MenteeRepository.Search(m => m.Id == id, null, "FirstOption, SecondOption, User, Mentor").FirstOrDefault();

                return mentee;
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MenteeService.GetById", ex);
                return null;
            }
        }

        public void SetInactive(int menteeId)
        {
            try
            {
                var mentee = _unitOfWork.MenteeRepository.Get(menteeId);
                mentee.Status = MenteeStatus.Inactive;

                if (mentee.Mentor != null)
                {
                    var mentor = _unitOfWork.MentorRepository.Get(mentee.Mentor.Id);
                    mentor.Mentees.Remove(mentee);
                    _unitOfWork.MentorRepository.Update(mentor);
                }

                _unitOfWork.MenteeRepository.Update(mentee);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.SetInactive", ex);
            }
        }
    }
}