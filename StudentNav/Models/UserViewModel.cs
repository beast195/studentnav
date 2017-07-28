using System.Collections.Generic;

namespace StudentNav.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProfilePic { get; set; }
    }

    public static class GlobVars
    {
        public static List<string> LevelOfStudy = new List<string>
        {
        "1stYear","2ndYear","3rdYear","BTech","Degree","Honours","Masters","Doctrate"
        };
    }
}