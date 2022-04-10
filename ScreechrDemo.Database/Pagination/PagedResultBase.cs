namespace ScreechrDemo.Databases.Pagination
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {

            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }

    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }

    }

    public class UserProfileViewModel
    {
        public ulong Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string ProfileImageUri { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ScreechViewModel
    {
        public ulong Id { get; set; }
        public string content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
