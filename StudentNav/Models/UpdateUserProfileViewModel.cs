using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace StudentNav.Models
{
    public class UpdateUserProfileViewModel
    {
        public HttpPostedFileWrapper ProfileImagePath { get; set; }
        public string Institution { get; set; }
        [Display(Name = "First Name(s)")]
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public Race Race { get; set; }
        [Display(Name = "Date Of Birth")]
        public string DateOfBirth { get; set; }
        public InstitutionType InstitutionType { get; set; }

        public string HighSchool { get; set; }
        [Display(Name = "Field Of Study")]
        public string FieldOfStudy { get; set; }

        public string Province { get; set; }

        public int Grade { get; set; }
        [Display(Name = "Level Of Study")]
        public string LevelOfStudy { get; set; }

        public IEnumerable<SelectListItem> LevelsOfStudys { get; set; } = new SelectList(GlobVars.LevelOfStudy);
    }
}