namespace PaginationSearchSort.Models
{
    public class EmployeeViewModel
    {
        public IQueryable<Employee> Employees { get; set; }
        public string? OrderByName { get; set; }
        public string? OrderByEmail { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        //we need to add when we will click on page need to pass these things via href for proper results
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; }
    }
}
