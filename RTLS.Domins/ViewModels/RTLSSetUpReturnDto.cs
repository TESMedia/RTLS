using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.ViewModels
{
   public class RTLSSetUpReturnDto
    {
        public int Id { get; set; }

        public string Mac { get; set; }

        public string Status { get; set; }

        public string omniUniqueId { get; set; }

        public bool IsTrackByAdmin { get; set; }

        public bool IsEntryNotify { get; set; }

        public bool IsDisplay { get; set; }

        public bool IsCreatedByAdmin { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
