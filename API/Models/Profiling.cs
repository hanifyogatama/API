using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("TB_M_Profiling")]
    public class Profiling
    {
        [Key, ForeignKey("Account")]
        public string NIK { get; set; }
        public int Education_Id { get; set; }


        public virtual Account Account { get; set; }
        public virtual Education Education { get; set; }

    }
}
