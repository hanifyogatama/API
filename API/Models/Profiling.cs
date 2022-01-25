using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table("TB_M_Profiling")]
    public class Profiling
    {
        [Key]
        public string NIK { get; set; }

        [ForeignKey("Education")]
        public int Id { get; set; }

        [JsonIgnore]
        public virtual Account Account { get; set; }

        [JsonIgnore]
        public virtual Education Education { get; set; }

    }
}
