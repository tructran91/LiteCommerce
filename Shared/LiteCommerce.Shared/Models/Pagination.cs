using LiteCommerce.Shared.Constants;

namespace LiteCommerce.Shared.Models
{
    public class Pagination
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecords { get; set; }

        public Pagination(int totalRecords, int currentPage = PaginationSetting.DefaultCurrentPage, int pageSize = PaginationSetting.DefaultPageSize)
        {
            var totalPages = (int)Math.Ceiling(totalRecords / (decimal)pageSize);

            if (currentPage < 1)
            {
                currentPage = 1;
            }
            else if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            TotalRecords = totalRecords;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
        }
    }
}
