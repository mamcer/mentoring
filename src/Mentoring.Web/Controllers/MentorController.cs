using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mentoring.Application;
using Mentoring.Core;
using Mentoring.Core.Enums;
using Mentoring.Web.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Collections.Specialized;

namespace Mentoring.Web.Controllers
{
    public class MentorController : Controller
    {
        private IMenteeSeniorityService _menteeSeniorityService;
        private IMenteeService _menteeService;
        private IMentorService _mentorService;
        private readonly IEmailMessageService _emailMessageService;
        private readonly ITimeSlotService _timeSlotService;
        private readonly ITopicService _topicService;

        public MentorController(IMenteeSeniorityService menteeSeniorityService, IEmailMessageService emailMessageService, ITimeSlotService timeSlotService, ITopicService topicService, IMenteeService menteeService, IMentorService mentorService)
        {
            _menteeSeniorityService = menteeSeniorityService;
            _emailMessageService = emailMessageService;
            _timeSlotService = timeSlotService;
            _topicService = topicService;
            _menteeService = menteeService;
            _mentorService = mentorService;
        }

        public ActionResult NewMentor()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (!userInfo.IsInRole(UserRoleCode.Mentor))
            {
                var allTopics = _topicService.GetAll();
                var topics = allTopics.Select(t => new TopicModel { Checked = false, Description = t.Description, Id = t.Id });

                var allTimeSlots = _timeSlotService.GetAll();
                var availability = allTimeSlots.Select(t => new TimeSlotModel { Checked = false, Description = t.Description, Id = t.Id });

                var allMenteeSeniorityLevels = _menteeSeniorityService.GetAll();
                var menteeSeniority = allMenteeSeniorityLevels.Select(s => new MenteeSeniorityModel { Id = s.Id, Checked = false, Description = s.Description });

                var meetingMode = from MeetingMode e in Enum.GetValues(typeof(MeetingMode))
                                  select new { Value = e.ToString(), Text = EnumExtension.GetEnumDescription(e) };
                ViewBag.MeetingModes = new SelectList(meetingMode, "Value", "Text");

                return View(new NewMentorModel
                {
                    Topics = topics.ToList(),
                    Availability = availability.ToList(),
                    SeniorityLevel = menteeSeniority.ToList(),
                    Seniority = !userInfo.Seniority.Equals("unknown") ? userInfo.Seniority : string.Empty,
                    Location = !userInfo.Location.Equals("unknown") ? userInfo.Location : string.Empty,
                });
            }

            return View("Unauthorized");
        }

        [HttpPost]
        public ActionResult NewMentor(NewMentorModel newMentor)
        {
            if (ModelState.IsValid)
            {
                var currentUser = UserHelper.GetCurrentUserInfo();

                var isExistingMentor = _mentorService.Search(m => m.User.Id == currentUser.Id && (m.Status == MentorStatus.Active || m.Status == MentorStatus.PendingApproval)).Any();

                if (!isExistingMentor)
                {
                    var mentor = new Mentor
                        {
                            CurrentActivity = newMentor.CurrentActivity,
                            Expectations = newMentor.Expectations,
                            MaxMentees = newMentor.MaxMentees,
                            MeetingsMode = newMentor.MeetingsMode,
                            MentoringTarget = newMentor.MentoringTarget,
                            OtherAvailability = newMentor.OtherAvailability,
                            OtherTopic = newMentor.OtherTopic
                        };

                    var selectedTopicIds = newMentor.Topics.Where(t => t.Checked).Select(t => t.Id).ToList();
                    var selectedTimeSlotIds = newMentor.Availability.Where(t => t.Checked).Select(t => t.Id).ToList();
                    var selectedMenteeSeniorityIds = newMentor.SeniorityLevel.Where(t => t.Checked).Select(t => t.Id).ToList();
                    _mentorService.NewMentor(mentor, currentUser.Id, selectedTopicIds, selectedTimeSlotIds, selectedMenteeSeniorityIds, newMentor.Location, newMentor.Seniority);

                    UserHelper.SetUserInfo(mentor.User);

                    _emailMessageService.SaveMessage(mentor.User.Email, "PreApprovedMentor", null);
                }

                return View("NewMentorPreApproved");
            }

            return View(newMentor);
        }

        private MentorModel ToMentorModel(Mentor mentor)
        {
            if (mentor != null)
            {
                return new MentorModel
                {
                    Id = mentor.Id,
                    CurrentActivity = mentor.CurrentActivity,
                    Email = mentor.User.Email,
                    Expectations = mentor.Expectations,
                    Location = mentor.User.Location,
                    MentoringTarget = mentor.MentoringTarget,
                    Name = mentor.User.Name,
                    NickName = mentor.User.NickName,
                    OtherTopic = mentor.OtherTopic,
                    Seniority = mentor.User.Seniority,
                    Topics = mentor.Topic.Select(t => new TopicModel { Checked = false, Description = t.Description, Id = t.Id }).ToList(),
                    Status = mentor.Status,
                    MaxMentees = mentor.MaxMentees,
                    MeetingsMode = EnumExtension.GetEnumDescription(mentor.MeetingsMode)
                };
            }

            return null;
        }

        private IEnumerable<MentorModel> ToMentorModelCollection(IEnumerable<Mentor> mentors)
        {
            return mentors.Select(ToMentorModel);
        }

        public ActionResult Summary()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var mentorsWithSlots = _mentorService.Count(m => m.Status == MentorStatus.Active && m.Mentees.Count > 0 && m.Mentees.Count < m.MaxMentees);
                var mentorsWithoutSlots = _mentorService.Count(m => m.Status == MentorStatus.Active && m.Mentees.Count > 0 && m.Mentees.Count == m.MaxMentees);
                var pendingApproval = _mentorService.Count(m => m.Status == MentorStatus.PendingApproval);
                var pendingRenewal = _mentorService.Count(m => m.Status == MentorStatus.PendingRenewal);
                var rejected = _mentorService.Count(m => m.Status == MentorStatus.Rejected);
                var inactive = _mentorService.Count(m => m.Status == MentorStatus.Inactive);

                var mentorModel = new AllMentorsCountModel
                {
                    MentorsWithSlots = mentorsWithSlots,
                    MentorsWithoutSlots = mentorsWithoutSlots,
                    PendingApproval = pendingApproval,
                    PendingRenewal = pendingRenewal,
                    Rejected = rejected,
                    Inactive = inactive
                };
                return View(mentorModel);
            }

            return View("Unauthorized");
        }

        public ActionResult MentorDetails(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var mentor = _mentorService.Search(m => m.Id == id).FirstOrDefault();
                if (mentor != null)
                {
                    var mentorModel = new MentorModel
                        {
                            Id = mentor.Id,
                            CurrentActivity = mentor.CurrentActivity,
                            Email = mentor.User.Email,
                            Expectations = mentor.Expectations,
                            Location = mentor.User.Location,
                            MentoringTarget = mentor.MentoringTarget,
                            Name = mentor.User.Name,
                            NickName = mentor.User.NickName,
                            OtherTopic = mentor.OtherTopic,
                            Seniority = mentor.User.Seniority,
                            Topics = mentor.Topic.Select(t => new TopicModel { Checked = false, Description = t.Description, Id = t.Id }).ToList(),
                            Status = mentor.Status,
                            MaxMentees = mentor.MaxMentees,
                            MeetingsMode = EnumExtension.GetEnumDescription(mentor.MeetingsMode)
                        };
                    return View(mentorModel);
                }
            }
            else
            {
                return View("Unauthorized");
            }

            return null;
        }

        public ActionResult AcceptMentor(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var mentor = _mentorService.Search(m => m.Id == id).FirstOrDefault();
                if (mentor != null)
                {
                    _mentorService.AcceptMentor(mentor.Id);

                    //send email
                    var parameters = new StringDictionary();
                    var link = string.Format("{0}://{1}{2}", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Authority, "/Mentor/MyMentorInfo");
                    parameters.Add("@@mentoringinfo", link);
                    _emailMessageService.SaveMessage(mentor.User.Email, "ApprovedMentor", parameters);
                }

                return RedirectToAction("PendingApproval");
            }

            return View("Unauthorized");
        }

        public ActionResult RejectMentor(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var mentor = _mentorService.Search(m => m.Id == id).FirstOrDefault();
                if (mentor != null)
                {
                    ViewBag.UserName = mentor.User.Name;
                    ViewBag.UserEmail = mentor.User.Email;
                }

                return View(new RejectedMentorModel { Id = id });
            }

            return View("Unauthorized");
        }

        [HttpPost]
        public ActionResult RejectMentor(RejectedMentorModel rejectedMentor)
        {
            var mentor = _mentorService.Search(m => m.Id == rejectedMentor.Id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var userInfo = UserHelper.GetCurrentUserInfo();
                if (userInfo.IsInRole(UserRoleCode.Career))
                {
                    if (mentor != null)
                    {
                        _mentorService.RejectMentor(mentor.Id);

                        var parameters = new StringDictionary
                            {
                                {"@@content", rejectedMentor.Message}
                            };
                        _emailMessageService.SaveMessage(mentor.User.Email, "RejectMentor", parameters);
                    }

                    return RedirectToAction("PendingApproval");
                }

                return View("Unauthorized");
            }

            if (mentor != null)
            {
                ViewBag.UserName = mentor.User.Name;
                ViewBag.UserEmail = mentor.User.Email;
            }

            return View(new RejectedMentorModel { Id = rejectedMentor.Id });
        }

        public ActionResult MyMentor()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Mentee))
            {
                var mentee = _menteeService.Search(m => m.User.Id == userInfo.Id).FirstOrDefault();
                if (mentee != null && mentee.Mentor != null)
                {
                    return View(ToMentorModel(mentee.Mentor));
                }

                return View("PendingMentor");
            }

            return View("Unauthorized");
        }

        public ActionResult MyMentorInfo()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Mentor))
            {
                var mentor = _mentorService.Search(m => m.User.Id == userInfo.Id).FirstOrDefault();
                ViewBag.EditEnabled = true;
                return View(ToMentorModel(mentor));
            }

            return View("Unauthorized");
        }

        public ActionResult ExportAllMentors()
        {
            var gv = new GridView();
            gv.DataSource = _mentorService.GetAll().Select(m => new
            {
                Name = m.User.Name,
                NickName = m.User.NickName,
                Email = m.User.Email,
                Topics = string.Join(" - ", m.Topic.Select(t => t.Description).ToArray()),
                OtherTopic = m.OtherTopic,
                MaxMentees = m.MaxMentees,
                MeetingsMode = m.MeetingsMode,
                SeniorityLevel = string.Join(", ", m.MenteeSeniorities.Select(s => s.Description).ToArray()),
                MentorSeniority = m.User.Seniority,
                MentorLocation = m.User.Location,
                Status = m.Status
            }).ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}_All_Mentors.xls", DateTime.Today.ToShortDateString()));
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("MentorExports");
        }

        public ActionResult EditMyMentorInfo(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Mentor))
            {
                var mentor = _mentorService.Search(m => m.Id == id).FirstOrDefault();

                if (mentor != null && mentor.User.Id == userInfo.Id)
                {
                    return View(ToNewMentorModel(mentor));
                }
            }

            return View("Unauthorized");
        }

        private NewMentorModel ToNewMentorModel(Mentor mentor)
        {
            var allTopics = _topicService.GetAll();
            var topics = allTopics.Select(t => new TopicModel { Checked = false, Description = t.Description, Id = t.Id });

            var allTimeSlots = _timeSlotService.GetAll();
            var availability = allTimeSlots.Select(t => new TimeSlotModel { Checked = false, Description = t.Description, Id = t.Id });

            var allMenteeSeniorityLevels = _menteeSeniorityService.GetAll();
            var allMenteeSeniorities = allMenteeSeniorityLevels.Select(s => new MenteeSeniorityModel { Id = s.Id, Checked = false, Description = s.Description });

            var meetingMode = from MeetingMode e in Enum.GetValues(typeof(MeetingMode))
                              select new { Value = e.ToString(), Text = EnumExtension.GetEnumDescription(e) };
            ViewBag.MeetingModes = new SelectList(meetingMode, "Value", "Text");

            var newMentorModel = new NewMentorModel
            {
                Topics = topics.ToList(),
                Availability = availability.ToList(),
                SeniorityLevel = allMenteeSeniorities.ToList(),
                CurrentActivity = mentor.CurrentActivity
            };

            foreach (var topic in mentor.Topic)
            {
                var specificTopic = newMentorModel.Topics.FirstOrDefault(t => t.Id == topic.Id);
                if (specificTopic != null)
                {
                    specificTopic.Checked = true;
                }
            }

            foreach (var menteeSeniorities in mentor.MenteeSeniorities)
            {
                var menteeSeniority = newMentorModel.SeniorityLevel.FirstOrDefault(t => t.Id == menteeSeniorities.Id);
                if (menteeSeniority != null)
                {
                    menteeSeniority.Checked = true;
                }
            }

            newMentorModel.OtherTopic = mentor.OtherTopic;
            newMentorModel.MentoringTarget = mentor.MentoringTarget;
            newMentorModel.MaxMentees = mentor.MaxMentees;
            foreach (var avail in mentor.Availability)
            {
                var specificAvailability = newMentorModel.Availability.FirstOrDefault(a => a.Id == avail.Id);
                if (specificAvailability != null)
                {
                    specificAvailability.Checked = true;
                }
            }
            newMentorModel.OtherAvailability = mentor.OtherAvailability;
            newMentorModel.MeetingsMode = mentor.MeetingsMode;
            newMentorModel.Expectations = mentor.Expectations;
            newMentorModel.Seniority = mentor.User.Seniority;
            newMentorModel.Location = mentor.User.Location;


            return newMentorModel;
        }

        [HttpPost]
        public ActionResult EditMyMentorInfo(NewMentorModel updatedMentor)
        {
            if (ModelState.IsValid)
            {
                var currentUser = UserHelper.GetCurrentUserInfo();

                var isExistingMentor = _mentorService.Search(m => m.User.Id == currentUser.Id && (m.Status == MentorStatus.Active || m.Status == MentorStatus.PendingApproval)).Any();

                if (isExistingMentor)
                {
                    var mentor = new Mentor
                    {
                        Id = updatedMentor.Id,
                        CurrentActivity = updatedMentor.CurrentActivity,
                        Expectations = updatedMentor.Expectations,
                        MaxMentees = updatedMentor.MaxMentees,
                        MeetingsMode = updatedMentor.MeetingsMode,
                        MentoringTarget = updatedMentor.MentoringTarget,
                        OtherAvailability = updatedMentor.OtherAvailability,
                        OtherTopic = updatedMentor.OtherTopic
                    };

                    _mentorService.UpdateMentor(mentor);
                }

                return RedirectToAction("MyMentorInfo");
            }

            return View(updatedMentor);
        }

        public ActionResult Remove(int id)
        {
            var mentor = _mentorService.GetById(id);

            var removeMentor = new RemoveMentor
            {
                Id = id,
                Name = mentor.User.Name
            };

            foreach (var m in mentor.Mentees)
            {
                var mentee = _menteeService.GetById(m.Id);
                removeMentor.MenteeNames.Add(mentee.User.Name);
            }

            return View(removeMentor);
        }

        [HttpPost]
        public ActionResult Remove(RemoveMentor removeMentor)
        {
            _mentorService.SetInactive(removeMentor.Id);

            return RedirectToAction("Summary");
        }

        public ActionResult PendingApproval()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var pendingApprovalMentors = _mentorService.Search(m => m.Status == MentorStatus.PendingApproval).ToList();

                var mentorModel = new AllMentorsModel()
                {
                    PendingApproval = ToMentorModelCollection(pendingApprovalMentors),
                };

                return View(mentorModel);
            }

            return View("Unauthorized");
        }

        public ActionResult ByStatus()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var mentorsWithSlots = _mentorService.Search(m => m.Status == MentorStatus.Active && m.Mentees.Count > 0 && m.Mentees.Count < m.MaxMentees);
                var mentorsWithoutSlots = _mentorService.Search(m => m.Status == MentorStatus.Active && m.Mentees.Count > 0 && m.Mentees.Count == m.MaxMentees);

                var mentorModel = new AllMentorsModel
                {
                    MentorsWithSlots = ToMentorModelCollection(mentorsWithSlots),
                    MentorsWithoutSlots = ToMentorModelCollection(mentorsWithoutSlots),
                };
                return View(mentorModel);
            }

            return View("Unauthorized");
        }

        public ActionResult Inactive()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var inactiveMentors = _mentorService.Search(m => m.Status == MentorStatus.Inactive);

                var mentorModel = new AllMentorsModel
                {
                    Inactive = ToMentorModelCollection(inactiveMentors)
                };
                return View(mentorModel);
            }

            return View("Unauthorized");
        }

        public ActionResult PendingRenewal()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var pendingRenewalMentors = _mentorService.Search(m => m.Status == MentorStatus.PendingRenewal);

                var mentorModel = new AllMentorsModel
                {
                    PendingRenewal = ToMentorModelCollection(pendingRenewalMentors),
                };

                return View(mentorModel);
            }

            return View("Unauthorized");
        }

        public ActionResult Rejected()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var rejectedMentors = _mentorService.Search(m => m.Status == MentorStatus.Rejected);

                var mentorModel = new AllMentorsModel
                {
                    Rejected = ToMentorModelCollection(rejectedMentors),
                };

                return View(mentorModel);
            }

            return View("Unauthorized");
        }
    }
}