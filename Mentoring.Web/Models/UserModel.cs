using Mentoring.Core;
using Mentoring.Core.Enums;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mentoring.Web.Models
{
    public class UserModel
    {
        public UserModel()
        {
        }

        public UserModel(User user)
        {
            Id = user.Id;
            AvatarUrl = user.AvatarUrl;
            JoinDate = user.JoinDate;
            Location = user.Location;
            NickName = user.NickName;
            Seniority = user.Seniority;
            Name = user.Name;
            Email = user.Email;

            Roles = new List<UserRole>();
            foreach (var role in user.Roles)
            {
                Roles.Add(role);
            }

            CurrentRole = Roles.OrderByDescending(r => r.Id).FirstOrDefault();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100), MinLength(3)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100), MinLength(3)]
        [Display(Name = "Nickname")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(2083)]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Avatar Url")]
        public string AvatarUrl { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; }

        [Display(Name = "Current Role")]
        public UserRole CurrentRole { get; set; }

        [Display(Name = "Roles")]
        public List<UserRole> Roles { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Seniority")]
        public string Seniority { get; set; }

        public bool IsInRole(UserRoleCode role)
        {
            return Roles.Any(r => r.Id == (int)role);
        }

        public bool IsLoggedAs(UserRoleCode role)
        {
            if (CurrentRole != null)
            {
                return CurrentRole.Id == (int) role;
            }

            return false;
        }
    }
}