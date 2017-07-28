using System.Web;

namespace StudentNav.Models
{
    public class UpdateUserProfileViewModel
    {
        public HttpPostedFileWrapper ProfileImagePath { get; set; }
        public string Institution { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public Race Race { get; set; }

        public InstitutionType InstitutionType { get; set; }

        public string HighSchool { get; set; }

        public string FieldOfStudy { get; set; }

        public string Province { get; set; }

        public int Grade { get; set; }

        public string LevelOfStudy { get; set; }
    }
}