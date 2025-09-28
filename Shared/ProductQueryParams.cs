using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductQueryParams
    {
        private const int defaultPageSize = 5;
        private const int maxPageSize = 10;
        public int? Brandid { get; set; }
        public int? Typeid { get; set; }
        public ProductSortingOptions sortingOption { get; set; }
        public string? SearchValue { get; set; }

        //Pagination
        public int PageIndex { get; set; } = 1;

        private int pageSize = defaultPageSize ;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > maxPageSize ? maxPageSize : value; }
            
        }


    }
}
