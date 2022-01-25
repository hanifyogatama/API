using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table("TB_M_Education")]
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public string Degree { get; set; }
        public string GPA { get; set; }

        [JsonIgnore]
        public virtual University University { get; set; }

        [JsonIgnore]
        public virtual ICollection<Profiling> Profilings { get; set; }


        [ForeignKey("University_Id")]
        public int University_Id { get; set; }
    }
}
