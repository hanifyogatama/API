using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("TB_M_Profiling")]
    public class Profiling
    {
        [Key]
        public string NIK { get; set; }
        [ForeignKey("Education")]
        public int Id { get; set; }
        public virtual Account Account { get; set; }
        public virtual Education Education { get; set; }

    }
}
