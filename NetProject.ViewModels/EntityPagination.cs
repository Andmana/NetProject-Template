using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProject.ViewModels
{
    public class EntityPagination<T> 
    {
        public int? PageNumber { get; set; }
        public int? TotalPages { get; set; }
        public int? TotalData { get; set; }

        public List<T> Items { get; set; }

        public EntityPagination(List<T> items, int count, int pageIndex, int showData)
        {
            this.PageNumber = pageIndex;
            this.TotalPages = (int)Math.Ceiling(count / (double)showData);
            this.TotalData = count;
            this.Items = items;

        }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
