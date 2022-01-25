using System;
using System.Text.Json.Serialization;

namespace API.ViewModel
{
    public class RegisterVM
    {
        public string NIK { get; set; }    
        [JsonIgnore]
        public string Password { get; set; }    
        public string FirstName { get; set; }    
        public string LastName { get; set; }    
        public string Phone { get; set; }    
        public DateTime BirthDate { get; set; }    
        public int Salary { get; set; }    
        public string Email { get; set; }
        public string Degree { get; set; }
        public string GPA { get; set; }
        public Gender Gender { get; set; }
        public int University_Id { get; set; }

    }

    public enum Gender
    {
        Male,
        Female
    }
}
