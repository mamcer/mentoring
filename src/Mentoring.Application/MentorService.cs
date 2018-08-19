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
    public class MentorService : IMentorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;
        private readonly IUserLogService _userLogService;
        private readonly IUserService _userService;

        public MentorService(IUnitOfWork unitOfWork, ILogManager logManager, IEmailTemplateService emailTemplateService, IUserLogService userLogService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
            _userLogService = userLogService;
            _userService = userService;
        }

        public int NumberOfDaysToAutoassignMentor
        {
            get
            {
                return 7;
            }
        }

        public Mentor NewMentor(Mentor mentor, int userId, List<int> selectedTopicIds, List<int> selectedTimeSlotIds, List<int> selectedMenteeSeniorityIds, string location, string seniority)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Get(userId);

                var topics = _unitOfWork.TopicRepository.GetAll();
                var timeSlots = _unitOfWork.TimeSlotRepository.GetAll();
                var menteeSeniorities = _unitOfWork.MenteeSeniorityRepository.GetAll();

                var userTopics = topics.Where(t => selectedTopicIds.Exists(st => st == t.Id));
                foreach (var topic in userTopics)
                {
                    mentor.Topic.Add(topic);
                }

                var userTimeSlots = timeSlots.Where(t => selectedTimeSlotIds.Exists(ts => ts == t.Id));
                foreach (var timeSlot in userTimeSlots)
                {
                    mentor.Availability.Add(timeSlot);
                }

                var userMenteeSeniorities = menteeSeniorities.Where(t => selectedMenteeSeniorityIds.Exists(ts => ts == t.Id));
                foreach (var menteeSeniority in userMenteeSeniorities)
                {
                    mentor.MenteeSeniorities.Add(menteeSeniority);
                }

                user.Roles.Add(_unitOfWork.UserRoleRepository.Get((int)UserRoleCode.Mentor));
                user.Location = location;
                user.Seniority = seniority;
                mentor.User = user;
                mentor.Status = MentorStatus.PendingApproval;

                _userLogService.Add(user.Id, UserLogAction.PendingApprovalMentor, string.Format(Resources.Messages.MentorPendingApproval, user.Name));

                _unitOfWork.MentorRepository.Insert(mentor);
                _unitOfWork.Save();

                return mentor;
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.NewMentor", ex);
                return null;
            }
        }

        public Mentor UpdateMentor(Mentor mentor)
        {
            try
            {
                _unitOfWork.MentorRepository.Update(mentor);
                _unitOfWork.Save();

                return mentor;
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.UpdateMentor", ex);
                return null;
            }
        }

        public IEnumerable<Mentor> Search(Expression<Func<Mentor, bool>> filter)
        {
            try
            {
                return _unitOfWork.MentorRepository.Search(filter, null, "User,Topic,Mentees,Availability,MenteeSeniorities");
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.Search", ex);
                return null;
            }
        }

        public int Count(Expression<Func<Mentor, bool>> filter)
        {
            try
            {
                return _unitOfWork.MentorRepository.Search(filter).Count();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.Count", ex);
                return 0;
            }
        }

        public Mentor GetById(int id)
        {
            try
            {
                return _unitOfWork.MentorRepository.Search(m => m.Id == id, null, "User,Topic,Mentees,Availability,MenteeSeniorities").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.GetById", ex);
                return null;
            }
        }

        public Mentor GetByUserId(int id)
        {
            try
            {
                return _unitOfWork.MentorRepository.Search(m => (m.User.Id == id && m.Status != MentorStatus.Inactive)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.GetByUserId", ex);
                return null;
            }
        }

        public IEnumerable<Mentor> GetAllAvailable()
        {
            try
            {
                return _unitOfWork.MentorRepository.Search(m => m.MaxMentees > m.Mentees.Count() && m.Status == MentorStatus.Active, null, "Topic,User");
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.GetAllAvailable", ex);
                return null;
            }
        }

        public IEnumerable<Mentor> GetAll()
        {
            try
            {
                return _unitOfWork.MentorRepository.Search(m => true, null, "Topic,User");
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.GetAll", ex);
                return null;
            }
        }

        public IEnumerable<Mentor> GetAllActive()
        {
            try
            {
                return _unitOfWork.MentorRepository.Search(m => m.Status == MentorStatus.Active, null, "Topic,User");
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.GetAllActive", ex);
                return null;
            }
        }

        public void AcceptMentee(int userId, int menteeId)
        {
            try
            {
                var mentor = _unitOfWork.MentorRepository.Search(m => m.User.Id == userId && m.Status == MentorStatus.Active, null, "Mentees").FirstOrDefault();
                if (mentor != null)
                {
                    var mentee = _unitOfWork.MenteeRepository.Search(m => m.Id == menteeId, null, "User,FirstOption,SecondOption").FirstOrDefault();
                    if (mentee != null && mentee.FirstOption.Id == mentor.Id)
                    {
                        mentee.FirstOptionAccepted = true;

                        //first option accepted
                        if (mentor.MaxMentees > mentor.Mentees.Count())
                        {
                            mentee.Mentor = mentor;

                            mentor.Mentees.Add(mentee);
                            _unitOfWork.MentorRepository.Update(mentor);
                        }
                    }

                    if (mentee != null && mentee.SecondOption.Id == mentor.Id)
                    {
                        mentee.SecondOptionAccepted = true;

                        //second option accepted but no the first
                        if (mentee.FirstOptionAccepted.HasValue && !mentee.FirstOptionAccepted.Value && mentor.MaxMentees > mentor.Mentees.Count())
                        {
                            mentee.Mentor = mentor;

                            mentor.Mentees.Add(mentee);
                            _unitOfWork.MentorRepository.Update(mentor);
                        }
                    }

                    _unitOfWork.MenteeRepository.Update(mentee);
                    _unitOfWork.Save();
                }
                else
                {
                    var ex = new InvalidOperationException(string.Format("Didn't found an active mentor with user id {0}", userId));
                    _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.AcceptMentee", ex);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.AcceptMentee", ex);
            }
        }

        public void RejectMentee(int userId, int menteeId)
        {
            try
            {
                var mentor = _unitOfWork.MentorRepository.Search(m => m.User.Id == userId && m.Status == MentorStatus.Active, null, "Mentees").FirstOrDefault();
                if (mentor != null)
                {
                    var mentee = _unitOfWork.MenteeRepository.Search(m => m.Id == menteeId, null, "User,FirstOption,SecondOption").FirstOrDefault();
                    if (mentee != null && mentee.FirstOption.Id == mentor.Id)
                    {
                        mentee.FirstOptionAccepted = false;

                        //first option reject but second option already accepted him
                        var secondMentor = mentee.SecondOption;
                        if (mentee.SecondOptionAccepted.HasValue && mentee.SecondOptionAccepted.Value && secondMentor.MaxMentees > secondMentor.Mentees.Count())
                        {
                            mentee.Mentor = secondMentor;

                            secondMentor.Mentees.Add(mentee);
                            _unitOfWork.MentorRepository.Update(secondMentor);
                        }
                    }

                    if (mentee != null && mentee.SecondOption.Id == mentor.Id)
                    {
                        mentee.SecondOptionAccepted = false;
                    }

                    _unitOfWork.MenteeRepository.Update(mentee);
                    _unitOfWork.Save();
                }
                else
                {
                    var ex = new InvalidOperationException(string.Format("Didn't found an active mentor with user id {0}", userId));
                    _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.RejectMentee", ex);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.RejectMentee", ex);
            }
        }

        public void AssignPendingMentees()
        {
            try
            {
                var mentees = _unitOfWork.MenteeRepository.Search(m => m.Mentor == null, null, "FirstOption,SecondOption,User").ToList();

                foreach (var mentee in mentees)
                {
                    if (mentee.Mentor == null && mentee.SecondOptionAccepted.HasValue && mentee.SecondOptionAccepted.Value && (DateTime.Now - mentee.MentorOptionDate).Days > NumberOfDaysToAutoassignMentor)
                    {
                        if (mentee.SecondOption.MaxMentees > mentee.SecondOption.Mentees.Count())
                        {
                            mentee.Mentor = mentee.SecondOption;
                            _unitOfWork.MenteeRepository.Update(mentee);

                            mentee.SecondOption.Mentees.Add(mentee);
                            _unitOfWork.MentorRepository.Update(mentee.SecondOption);
                        }
                    }
                }

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.AcceptMentee", ex);
            }
        }

        public void AcceptMentor(int mentorId)
        {
            try
            {
                var mentor = _unitOfWork.MentorRepository.Get(mentorId);
                mentor.Status = MentorStatus.Active;

                _unitOfWork.MentorRepository.Update(mentor);

                _unitOfWork.Save();

                _userLogService.Add(mentor.User.Id, UserLogAction.MentorAccepted, string.Format(Resources.Messages.MentorActive, mentor.User.Name));
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.AcceptMentor", ex);
            }
        }

        public void RejectMentor(int mentorId)
        {
            try
            {
                var mentor = _unitOfWork.MentorRepository.Get(mentorId);

                _userService.RemoveRole(mentor.User.Id, UserRoleCode.Mentor);
                mentor.Status = MentorStatus.Rejected;

                _unitOfWork.MentorRepository.Update(mentor);

                _unitOfWork.Save();

                _userLogService.Add(mentor.User.Id, UserLogAction.MentorRejected, string.Format(Resources.Messages.MentorRejected, mentor.User.Name));
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.SetActive", ex);
            }
        }

        public void SetPendingRenewal()
        {
            try
            {
                var allActive = _unitOfWork.MentorRepository.Search(m => m.Status == MentorStatus.Active);
                foreach (var mentor in allActive)
                {
                    mentor.Status = MentorStatus.PendingRenewal;
                    _unitOfWork.MentorRepository.Update(mentor);
                }

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.SetPendingRenewal", ex);
            }
        }

        public void SetInactive(int mentorId)
        {
            try
            {
                var mentor = _unitOfWork.MentorRepository.Get(mentorId);
                mentor.Status = MentorStatus.Inactive;
                foreach (var mentee in mentor.Mentees)
                {
                    mentee.Mentor = null;
                    _unitOfWork.MenteeRepository.Update(mentee);
                }

                _unitOfWork.MentorRepository.Update(mentor);

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MentorService.SetInactive", ex);
            }
        }
    }
}