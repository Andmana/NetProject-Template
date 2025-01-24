using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProject.ViewModels
{
    public class ViewUserAccount
    {
        public long Id { get; set; }

        public long? RoleId { get; set; }

        public string? RoleName { get; set; }


        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public long? CreatedBy { get; set; }


        public long? ModifiedBy { get; set; }


        public long? DeletedBy { get; set; }


    }
}
