using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins
{
    public class UsersAddress
    {
        [Key]
        [ForeignKey("WifiUser")]
        public int UserId { get; set; }

        [MaxLength(100)]
        public string Address1 { get; set; }

        [MaxLength(100)]
        public string Address2 { get; set; }

        [MaxLength(100)]
        public string PostTown { get; set; }

        [MaxLength(100)]
        public string County { get; set; }

        [MaxLength(100)]
        public string State { get; set; }

        [MaxLength(100)]
        public string Country { get; set; }

        [MaxLength(100)]
        public string PostCode { get; set; }

        [MaxLength(100)]
        public string Notes { get; set; }

        [JsonIgnore]
        public virtual WifiUser WifiUser { get; set; }
    }
}
