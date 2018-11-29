using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreMentoring.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            //PreviousUserPasswords = new List<PreviousPassword>();
        }
        //public virtual IList<PreviousPassword> PreviousUserPasswords { get; set; }

        //public bool ChangePasswordRequired { get; set; }

        //public DateTime LastPasswordChangedDate { get; set; }
    }
}
