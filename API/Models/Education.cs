using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("TB_M_Education")]
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public string GPA { get; set; }
        public int University_Id { get; set; }

        public virtual University University { get; set; }  
        public ICollection<Profiling> Profilings { get; set; }
    }
}
