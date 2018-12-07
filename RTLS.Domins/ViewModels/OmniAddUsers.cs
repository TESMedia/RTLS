using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.ViewModels
{
    public class OmniAddUsers
    {
        public OmniAddUsers()
        {
            social_media = new SocialMedia();
        }
        public SocialMedia social_media { get; set; }
        public class SocialMedia
        {
            public string type { get; set; } = "cp_user";
        }
        public string user_name { get; set; }
    }
}
