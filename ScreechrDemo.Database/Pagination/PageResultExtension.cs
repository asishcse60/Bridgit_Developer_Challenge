namespace ScreechrDemo.Databases.Pagination
{
    public static class PageResultExtension
    {
        public static PagedResult<TR> GetPaged<T, TR>(IQueryable<T> query,
            int page, int pageSize, Func<T, TR> output) where T : class where TR : class
        {
            var result = new PagedResult<TR>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).AsEnumerable().Select(output).ToList();

            return result;
        }
    }
}
