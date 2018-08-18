using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Mentoring.Application;
using Mentoring.Core.Enums;
using Mentoring.Web.Models;
using Mentoring.Core;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace Mentoring.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMenteeService _menteeService;
        private readonly IProgramStatusService _programStatusService;
        private readonly IMentorService _mentorService;
        private readonly IUserLogService _userLogService;
        private readonly IUserService _userService;

        public HomeController(IUserLogService userLogService, IMentorService mentorService, IMenteeService menteeService, IProgramStatusService programStatusService, IUserService userService)
        {
            _menteeService = menteeService;
            _programStatusService = programStatusService;
            _mentorService = mentorService;
            _userLogService = userLogService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            var currentStatus = _programStatusService.GetCurrentProgramStatus();

            var programStatusInfo = new ProgramStatusInfo
            {
                ProgramInProgress = (ProgramStatusCode)currentStatus.StatusCode == ProgramStatusCode.ProgramInProgress,
                ProgramRenewal = (ProgramStatusCode)currentStatus.StatusCode == ProgramStatusCode.ProgramRenewal
            };

            if (programStatusInfo.ProgramRenewal)
            {
                var currentUser = UserHelper.GetCurrentUserInfo();

                programStatusInfo.MentorPendingRenewal = IsMentorPendingRenewal(currentUser.Id);

                programStatusInfo.MenteePendingRenewal = IsMenteePendingRenewal(currentUser.Id);
            }

            return View(programStatusInfo);
        }

        private bool IsMentorPendingRenewal(int userId)
        {
            var mentor = _mentorService.GetByUserId(userId);
            if (mentor != null)
            {
                return mentor.Status == MentorStatus.PendingRenewal;
            }

            return false;
        }

        private bool IsMenteePendingRenewal(int userId)
        {
            var mentee = _menteeService.GetByUserId(userId);
            if (mentee != null)
            {
                return mentee.Status == MenteeStatus.PendingRenewal;
            }

            return false;
        }

        public ActionResult UserProfile()
        {
            var userProfileModel = new UserProfileModel();

            var userModel = UserHelper.GetCurrentUserInfo();
            userProfileModel.UserModel = userModel;

            userProfileModel.IsLeaveProgramEnabled = userModel.IsInRole(UserRoleCode.Mentor) || userModel.IsInRole(UserRoleCode.Mentee);

            userProfileModel.IsProgramRenewalEnabled = IsMenteePendingRenewal(userModel.Id) || IsMentorPendingRenewal(userModel.Id);

            return View(userProfileModel);
        }

        public ActionResult EditUserProfile()
        {
            var userProfile = UserHelper.GetCurrentUserInfo();

            return View(userProfile);
        }

        [HttpPost]
        public ActionResult EditUserProfile(UserModel modifiedUser)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(modifiedUser.AvatarUrl))
                {
                    modifiedUser.AvatarUrl = UserHelper.GetDefaultAvatarUrl(System.Web.HttpContext.Current.Request);
                }

                var user = _userService.UpdateUser(modifiedUser.Id, modifiedUser.NickName, modifiedUser.AvatarUrl, modifiedUser.Email, modifiedUser.Location, modifiedUser.Seniority);
                UserHelper.SetUserInfo(user);
            }

            return RedirectToAction("UserProfile");
        }

        public ActionResult Admin()
        {
            var currentUser = UserHelper.GetCurrentUserInfo();
            if (currentUser.IsLoggedAs(UserRoleCode.Career))
            {
                var currentStatus = _programStatusService.GetCurrentProgramStatus();
                ViewBag.CurrentProgramStatus = currentStatus.StatusDescription;
                ViewBag.CreationDate = currentStatus.CreationDate.ToLongDateString();

                return View();
            }

            return View("Unauthorized");
        }

        public ActionResult ProgramRenewal()
        {
            var currentUser = UserHelper.GetCurrentUserInfo();
            if (currentUser.IsLoggedAs(UserRoleCode.Career))
            {
                var currentProgramStatus = _programStatusService.GetCurrentProgramStatus();
                if (currentProgramStatus.StatusCode != (int)ProgramStatusCode.ProgramRenewal)
                {
                    _mentorService.SetPendingRenewal();

                    _menteeService.SetPendingRenewal();

                    _programStatusService.SaveProgramStatus(ProgramStatusCode.ProgramRenewal, EnumExtension.GetEnumDescription(ProgramStatusCode.ProgramRenewal));
                }

                return RedirectToAction("Admin");
            }

            return View("Unauthorized");
        }

        public ActionResult StartProgram()
        {
            var currentUser = UserHelper.GetCurrentUserInfo();
            if (currentUser.IsLoggedAs(UserRoleCode.Career))
            {
                var currentProgramStatus = _programStatusService.GetCurrentProgramStatus();
                if (currentProgramStatus.StatusCode != (int)ProgramStatusCode.ProgramInProgress)
                {
                    _programStatusService.SaveProgramStatus(ProgramStatusCode.ProgramInProgress, EnumExtension.GetEnumDescription(ProgramStatusCode.ProgramInProgress));
                }

                return RedirectToAction("Admin");
            }

            return View("Unauthorized");
        }

        public ActionResult ConfigureUserRoles()
        {
            var currentUser = UserHelper.GetCurrentUserInfo();
            if (currentUser.IsLoggedAs(UserRoleCode.Career))
            {
                var allUsers = _userService.GetAllUsers();

                var allUserModels = allUsers.Select(user => new UserModel(user)).ToList();

                return View(allUserModels);
            }

            return View("Unauthorized");
        }

        public ActionResult ChangeRole(string name)
        {
            var currentUser = UserHelper.GetCurrentUserInfo();
            if (currentUser.IsLoggedAs(UserRoleCode.Career))
            {
                var user = _userService.GetUserByName(name);
                if (user.Roles.Any(r => r.Id == (int)UserRoleCode.Career))
                {
                    _userService.RemoveRole(user.Id, UserRoleCode.Career);
                }
                else
                {
                    _userService.AddRole(user.Id, UserRoleCode.Career);
                }

                return RedirectToAction("ConfigureUserRoles");
            }

            return View("Unauthorized");
        }

        public ActionResult ChangeLoggedAs(int roleId)
        {
            var currentUser = UserHelper.GetCurrentUserInfo();
            currentUser.CurrentRole = currentUser.Roles.FirstOrDefault(r => r.Id == roleId);
            UserHelper.SetUserInfo(currentUser);

            return RedirectToAction("Index");
        }

        public ActionResult SetAsInactive(int userId, UserRoleCode userRole)
        {
            var currentUser = UserHelper.GetCurrentUserInfo();
            if (currentUser.IsInRole(UserRoleCode.Career))
            {
                if (userRole == UserRoleCode.Mentor)
                {
                    var mentor = _mentorService.GetByUserId(userId);
                    mentor.Status = MentorStatus.Inactive;
                    _mentorService.UpdateMentor(mentor);
                }

                if (userRole == UserRoleCode.Mentee)
                {
                    var mentee = _menteeService.GetByUserId(userId);
                    mentee.Status = MenteeStatus.Inactive;
                    _menteeService.UpdateMentee(mentee);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult LeaveProgram()
        {
            var currentUser = UserHelper.GetCurrentUserInfo();
            var userRoles = (from role in currentUser.Roles
                             where role.Id != (int)UserRoleCode.Career
                             select new { Value = role.Id, Text = role.Description }).ToList();

            if (userRoles.Count > 1)
            {
                userRoles.Add(new { Value = 8, Text = "All" });
            }

            ViewBag.UserRoles = new SelectList(userRoles, "Value", "Text");

            var leaveProgram = new LeaveProgramModel();

            return View(leaveProgram);
        }

        private void SetMenteeInactive(int userId)
        {
            var mentee = _menteeService.GetByUserId(userId);
            if (mentee != null)
            {
                mentee.Status = MenteeStatus.Inactive;
                _menteeService.UpdateMentee(mentee);
            }
        }

        private void SetMentorInactive(int userId)
        {
            var mentor = _mentorService.GetByUserId(userId);
            if (mentor != null)
            {
                mentor.Status = MentorStatus.Inactive;
                _mentorService.UpdateMentor(mentor);
            }
        }

        [HttpPost]
        public ActionResult LeaveProgram(LeaveProgramModel leaveProgramInfo)
        {
            var currentUser = UserHelper.GetCurrentUserInfo();

            switch (leaveProgramInfo.Value)
            {
                case (int)UserRoleCode.Mentee:
                    {
                        SetMenteeInactive(currentUser.Id);
                    }
                    break;
                case (int)UserRoleCode.Mentor:
                    {
                        SetMentorInactive(currentUser.Id);
                    }
                    break;
                default:
                    {
                        SetMenteeInactive(currentUser.Id);
                        SetMentorInactive(currentUser.Id);
                    }
                    break;
            }

            return RedirectToAction("Index");
        }

        private List<UserLogModel> ToUserLogModel(IEnumerable<UserLog> userLogs)
        {
            var userLogModels = new List<UserLogModel>();

            foreach (var userLog in userLogs)
            {
                userLogModels.Add(
                    new UserLogModel
                    {
                        User = userLog.User,
                        Action = userLog.Action,
                        Date = userLog.Date,
                        Description = userLog.Description
                    }
                );
            }

            return userLogModels;
        }

        public ActionResult HistoryLog()
        {
            var userLogs = _userLogService.GetAll();

            return View(ToUserLogModel(userLogs));
        }

        public ActionResult AssignPendingMentees()
        {
            _mentorService.AssignPendingMentees();

            return RedirectToAction("Admin");
        }
        
        public ActionResult ExportAllHistoryLogs()
        {
            GridView gv = new GridView();
            gv.DataSource = _userLogService.GetAll().Select(m => new
            {
                Name = m.User.Name,
                NickName = m.User.NickName,
                Email = m.User.Email,
                Date = m.Date,
                Action = m.Action,
                Description = m.Description
            }).ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.{1:00}.{2:00}_All_User_History_Logs.xls", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day));
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("HistoryLog");
        }
    }
}