using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mentoring.Application;
using Mentoring.Core;
using Mentoring.Core.Enums;
using Mentoring.Web.Models;

namespace Mentoring.Web.Controllers
{
    public class MenteeController : Controller
    {
        private readonly IEmailMessageService _emailMessageService;
        private readonly IMenteeService _menteeService;
        private readonly IMentorService _mentorService;
        private readonly IProgramStatusService _programStatusService;

        public MenteeController(IEmailMessageService emailMessageService, IMenteeService menteeService, IMentorService mentorService, IProgramStatusService programStatusService)
        {
            _emailMessageService = emailMessageService;
            _menteeService = menteeService;
            _mentorService = mentorService;
            _programStatusService = programStatusService;
        }

        public ActionResult NewMentee()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (!userInfo.IsInRole(UserRoleCode.Mentee))
            {
                var mentorsSelectList = GetAvailableMentors();

                var newMentorModel = new NewMenteeModel
                {
                    FirstMentorOption = mentorsSelectList,
                    SecondMentorOption = mentorsSelectList,
                    Location = !userInfo.Location.Equals("unknown") ? userInfo.Location : string.Empty,
                    Seniority = !userInfo.Seniority.Equals("unknown") ? userInfo.Seniority : string.Empty,
                };

                return View(newMentorModel);
            }

            return View("Unauthorized");
        }

        [HttpPost]
        public ActionResult NewMentee(NewMenteeModel newMentee)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (!userInfo.IsInRole(UserRoleCode.Mentee))
            {
                //TODO: <code smells>
                var firstMentor = newMentee.FirstMentorOption.Where(m => m.IsSelected).ToList();
                var isInvalidModel = false;
                if (firstMentor.Count == 0)
                {
                    ModelState.AddModelError("FirstMentorOption", "This field is required");
                    isInvalidModel = true;
                }

                if (firstMentor.Count > 1)
                {
                    ModelState.AddModelError("FirstMentorOption", "Please select only one option");
                    isInvalidModel = true;
                }

                var secondMentor = newMentee.SecondMentorOption.Where(m => m.IsSelected).ToList();
                if (secondMentor.Count == 0)
                {
                    ModelState.AddModelError("SecondMentorOption", "This field is required");
                    isInvalidModel = true;
                }

                if (secondMentor.Count > 1)
                {
                    ModelState.AddModelError("SecondMentorOption", "Please select only one option");
                    isInvalidModel = true;
                }

                if (secondMentor.Count == 1 && firstMentor.Count == 1)
                {
                    if (secondMentor.First().Id == firstMentor.First().Id)
                    {
                        ModelState.AddModelError("SecondMentorOption", "First and Second mentor options are equals");
                        isInvalidModel = true;
                    }
                }
                //TODO: </code smells>

                if (isInvalidModel)
                {
                    var mentorsSelectList = GetAvailableMentors();

                    newMentee.FirstMentorOption = mentorsSelectList;
                    newMentee.SecondMentorOption = mentorsSelectList;

                    return View(newMentee);
                }

                var currentUser = UserHelper.GetCurrentUserInfo();
                var mentee = new Mentee
                {
                    CurrentActivity = newMentee.CurrentActivity,
                    Focus = newMentee.SkillFocus
                };
               
                _menteeService.NewMentee(mentee, currentUser.Id, firstMentor.First().Id, secondMentor.First().Id, newMentee.Location, newMentee.Seniority);

                UserHelper.SetUserInfo(mentee.User);

                _emailMessageService.SaveMessage(mentee.User.Email, "PreApprovedMentee", null);

                return View("NewMenteePendingApproval");
            }

            return View("Unauthorized");
        }

        private List<SelectMentorModel> GetAvailableMentors()
        {
            var mentorModels = ToMentorModelCollection(_mentorService.GetAllAvailable());

            var mentorsSelectList = (from MentorModel e in mentorModels
                                     select new SelectMentorModel
                                     {
                                         Id = e.Id,
                                         IsSelected = false,
                                         Name = e.Name,
                                         Email = e.Email,
                                         CurrentActivity = e.CurrentActivity,
                                         Location = e.Location,
                                         MeetingsMode = e.MeetingsMode,
                                         MentoringTarget = e.MentoringTarget,
                                         Seniority = e.Seniority,
                                         SeniorityLevel = e.SeniorityLevel,
                                         Topics = e.Topics,
                                         OtherTopic = e.OtherTopic
                                     }).ToList();
            return mentorsSelectList;
        }

        private MenteeModel ToMenteeModel(Mentee mentee)
        {
            return new MenteeModel
            {
                Id = mentee.Id,
                CurrentActivity = mentee.CurrentActivity,
                Email = mentee.User.Email,
                FirstMentorOption = ToMentorModel(mentee.FirstOption),
                FirstMentorOptionAccepted = mentee.FirstOptionAccepted.HasValue && mentee.FirstOptionAccepted.Value,
                SecondMentorOptionAccepted = mentee.SecondOptionAccepted.HasValue && mentee.SecondOptionAccepted.Value,
                SecondMentorOption = ToMentorModel(mentee.SecondOption),
                Name = mentee.User.Name,
                SkillFocus = mentee.Focus
            };
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
            return mentors.Select(m => new MentorModel
            {
                Id = m.Id,
                CurrentActivity = m.CurrentActivity,
                Email = m.User.Email,
                Expectations = m.Expectations,
                Location = m.User.Location,
                MentoringTarget = m.MentoringTarget,
                NickName = m.User.NickName,
                Name = m.User.Name,
                OtherTopic = m.OtherTopic,
                Seniority = m.User.Seniority,
                Topics = m.Topic.Select(t => new TopicModel { Checked = false, Description = t.Description, Id = t.Id }).ToList(),
                Status = m.Status,
                MaxMentees = m.MaxMentees,
                MeetingsMode = EnumExtension.GetEnumDescription(m.MeetingsMode)
            });
        }

        private IEnumerable<MenteeModel> ToMenteeModelCollection(IEnumerable<Mentee> mentees)
        {
            if (mentees != null)
            {
                return mentees.Select(m => new MenteeModel
                        {
                            Id = m.Id,
                            CurrentActivity = m.CurrentActivity,
                            FirstMentorOption = ToMentorModel(m.FirstOption),
                            SecondMentorOption = ToMentorModel(m.SecondOption),
                            Mentor = ToMentorModel(m.Mentor),
                            SkillFocus = m.Focus,
                            Name = m.User.Name,
                            Email = m.User.Email,
                            FirstMentorOptionAccepted = m.FirstOptionAccepted.HasValue && m.FirstOptionAccepted.Value,
                            SecondMentorOptionAccepted = m.SecondOptionAccepted.HasValue && m.SecondOptionAccepted.Value
                        });
            }

            return new List<MenteeModel>();
        }

        public ActionResult Summary()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Career))
            {
                var menteeWithoutMentor = _menteeService.Count(m => m.Mentor == null && m.Status == MenteeStatus.Active);
                var menteesWithMentor = _menteeService.Count(m => m.Mentor != null && m.Status == MenteeStatus.Active);
                var pendingApproval = _menteeService.Count(m => m.Status == MenteeStatus.PendingApproval);
                var pendingRenewal = _menteeService.Count(m => m.Status == MenteeStatus.PendingRenewal);
                var rejected = _menteeService.Count(m => m.Status == MenteeStatus.Rejected);
                var inactive = _menteeService.Count(m => m.Status == MenteeStatus.Inactive);

                return View(new AllMenteesCountModel
                {
                    MenteesWithoutMentor = menteeWithoutMentor,
                    PendingApproval = pendingApproval,
                    MenteesWithMentor = menteesWithMentor,
                    PendingRenewal = pendingRenewal,
                    Rejected = rejected,
                    Inactive = inactive
                });
            }

            return View("Unauthorized");
        }

        public ActionResult PendingApproval()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Career))
            {
                var pendingApproval = _menteeService.Search(m => m.Status == MenteeStatus.PendingApproval);

                return View(new AllMenteesModel
                {
                    PendingApproval = ToMenteeModelCollection(pendingApproval),
                });
            }

            return View("Unauthorized");
        }

        public ActionResult ByStatus()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Career))
            {
                var menteeWithoutMentor = _menteeService.Search(m => m.Mentor == null && m.Status == MenteeStatus.Active);
                var menteesWithMentor = _menteeService.Search(m => m.Mentor != null && m.Status == MenteeStatus.Active);

                return View(new AllMenteesModel
                {
                    MenteesWithoutMentor = ToMenteeModelCollection(menteeWithoutMentor),
                    MenteesWithMentor = ToMenteeModelCollection(menteesWithMentor),
                });
            }

            return View("Unauthorized");
        }

        public ActionResult Inactive()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Career))
            {
                var inactive = _menteeService.Search(m => m.Status == MenteeStatus.Inactive);

                return View(new AllMenteesModel
                {
                    Inactive = ToMenteeModelCollection(inactive)
                });
            }

            return View("Unauthorized");
        }

        public ActionResult Rejected()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Career))
            {
                var rejected = _menteeService.Search(m => m.Status == MenteeStatus.Rejected);

                return View(new AllMenteesModel
                {
                    Rejected = ToMenteeModelCollection(rejected),
                });
            }

            return View("Unauthorized");
        }

        public ActionResult PendingRenewal()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Career))
            {
                var pendingRenewal = _menteeService.Search(m => m.Status == MenteeStatus.PendingRenewal);

                return View(new AllMenteesModel
                {
                    PendingRenewal = ToMenteeModelCollection(pendingRenewal),
                });
            }

            return View("Unauthorized");
        }

        public ActionResult PendingMenteeDetails(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Career))
            {
                var mentee = _menteeService.Search(m => m.Id == id).FirstOrDefault();
                return View(ToMenteeModel(mentee));
            }

            return View("Unauthorized");
        }

        public ActionResult MyMentees()
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Mentor))
            {
                var mentor = _mentorService.Search(m => m.User.Id == userInfo.Id).FirstOrDefault();

                var mentees = _menteeService.Search(m => m.FirstOption.Id == mentor.Id || m.SecondOption.Id == mentor.Id);

                var programStatus = _programStatusService.GetCurrentProgramStatus();

                IEnumerable<Mentee> enumerable = mentees as IList<Mentee> ?? mentees.ToList();

                var pendingMentees = enumerable.Where(m => mentor != null && ((m.FirstOption.Id == mentor.Id && !m.FirstOptionAccepted.HasValue) || (m.SecondOption.Id == mentor.Id && !m.SecondOptionAccepted.HasValue)));

                var acceptedMentees = enumerable.Where(m => mentor != null && ((m.FirstOption.Id == mentor.Id && m.FirstOptionAccepted.HasValue && m.FirstOptionAccepted.Value) || (m.SecondOption.Id == mentor.Id && m.SecondOptionAccepted.HasValue && m.SecondOptionAccepted.Value)));

                var rejectedMentees = enumerable.Where(m => mentor != null && ((m.FirstOption.Id == mentor.Id && m.FirstOptionAccepted.HasValue && m.FirstOptionAccepted.Value == false) || (m.SecondOption.Id == mentor.Id && m.SecondOptionAccepted.HasValue && m.SecondOptionAccepted.Value == false)));

                var activeMentees = enumerable.Where(m => mentor != null && (m.Mentor != null && m.Mentor.Id == mentor.Id));

                ViewBag.ProgramInProgress = programStatus.StatusCode == (int)ProgramStatusCode.ProgramInProgress;

                return View(new MyMenteesModel
                {
                    ActiveMentees = ToMenteeModelCollection(activeMentees).ToList(),
                    PendingMentees = ToMenteeModelCollection(pendingMentees).ToList(),
                    AcceptedMentees = ToMenteeModelCollection(acceptedMentees).ToList(),
                    RejectedMentees = ToMenteeModelCollection(rejectedMentees).ToList(),
                });
            }

            return View("Unauthorized");
        }

        public ActionResult MenteeDetails(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Mentor))
            {
                var mentee = _menteeService.Search(m => m.Id == id).FirstOrDefault();

                return View(ToMenteeModel(mentee));
            }

            return View("Unauthorized");
        }

        public ActionResult ActiveMenteeDetails(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Career))
            {
                var mentee = _menteeService.Search(m => m.Id == id).FirstOrDefault();

                return View(ToMenteeModel(mentee));
            }

            return View("Unauthorized");
        }

        public ActionResult MentorAcceptMentee(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Mentor))
            {
                _mentorService.AcceptMentee(userInfo.Id, id);

                return RedirectToAction("MyMentees");
            }

            return View("Unauthorized");
        }

        public ActionResult MentorRejectMentee(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsLoggedAs(UserRoleCode.Mentor))
            {
                _mentorService.RejectMentee(userInfo.Id, id);

                return RedirectToAction("MyMentees");
            }

            return View("Unauthorized");
        }

        public ActionResult ExportAllMentees()
        {
            GridView gv = new GridView();
            gv.DataSource = _menteeService.GetAllActive().Select(m => new
            {
                Name = m.User.Name,
                NickName = m.User.NickName,
                Email = m.User.Email,
                CurrentActivity = m.CurrentActivity,
                FirstMentorOption = m.FirstOption.User.Name,
                SecondMentorOption = m.SecondOption.User.Name,
                Focus = m.Focus,
                Status = m.Status,
                MentorName = m.Mentor != null ? m.Mentor.User.Name : string.Empty,
                MentorEmail = m.Mentor != null ? m.Mentor.User.Email : string.Empty
            }).ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}_All_Mentees.xls", DateTime.Today.ToShortDateString()));
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("MenteeExports");
        }

        public ActionResult AcceptMentee(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var mentee = _menteeService.Search(m => m.Id == id).FirstOrDefault();
                if (mentee != null)
                {
                    _menteeService.AcceptMentee(mentee.Id);
                }

                return RedirectToAction("Summary");
            }

            return View("Unauthorized");
        }

        public ActionResult RejectMentee(int id)
        {
            var userInfo = UserHelper.GetCurrentUserInfo();
            if (userInfo.IsInRole(UserRoleCode.Career))
            {
                var mentee = _menteeService.Search(m => m.Id == id).FirstOrDefault();
                if (mentee != null)
                {
                    ViewBag.UserName = mentee.User.Name;
                    ViewBag.UserEmail = mentee.User.Email;
                }

                return View(new RejectedMenteeModel { Id = id });
            }

            return View("Unauthorized");
        }

        [HttpPost]
        public ActionResult RejectMentee(RejectedMenteeModel rejectedMentee)
        {
            var mentee = _menteeService.Search(m => m.Id == rejectedMentee.Id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var userInfo = UserHelper.GetCurrentUserInfo();
                if (userInfo.IsInRole(UserRoleCode.Career))
                {
                    if (mentee != null)
                    {
                        _menteeService.RejectMentee(mentee.Id);

                        var parameters = new StringDictionary
                            {
                                {"@@content", rejectedMentee.Message}
                            };
                        _emailMessageService.SaveMessage(mentee.User.Email, "RejectMentee", parameters);
                    }

                    return RedirectToAction("Summary");
                }

                return View("Unauthorized");
            }

            if (mentee != null)
            {
                ViewBag.UserName = mentee.User.Name;
                ViewBag.UserEmail = mentee.User.Email;
            }

            return View(new RejectedMenteeModel { Id = rejectedMentee.Id });
        }

        public ActionResult Remove(int id)
        {
            var mentee = _menteeService.GetById(id);

            var removeMentee = new RemoveMentee
            {
                Id = id,
                Name = mentee.User.Name
            };

            if (mentee.Mentor != null)
            {
                var mentor = _mentorService.GetById(mentee.Mentor.Id);
                removeMentee.MentorName = mentor.User.Name;
            }

            return View(removeMentee);
        }

        [HttpPost]
        public ActionResult Remove(RemoveMentee removeMentee)
        {
            _menteeService.SetInactive(removeMentee.Id);

            return RedirectToAction("Summary");
        }
    }
}