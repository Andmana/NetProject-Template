using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProject.ViewModels
{
    public class ViewRole
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public long? CreateBy { get; set; }
        public long? ModifyBy { get; set; }
    }
}
