using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.DTO
{
    public class PagedResponse<T> where T: class
    {
        public int TotalPages { get; set; }
        public IEnumerable<T> Items { get; set; }

    }
}
