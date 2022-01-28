using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("TB_M_RoleAccount")]
    public class RoleAccount
    {
        [Key]
        public string NIK { get; set; }

        [ForeignKey("Role")]
        public int Id { get; set; }

        [JsonIgnore]
        public virtual Account Accounts { get; set; }

        [JsonIgnore]
        public virtual Role Roles { get; set; }
    }
}
