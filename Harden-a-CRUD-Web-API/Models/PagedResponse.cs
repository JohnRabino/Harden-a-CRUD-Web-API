namespace Harden_a_CRUD_Web_API.Models
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
            = Enumerable.Empty<T>();

        public MetaData Meta { get; set; }
            = new();
    }

    public class MetaData
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }
    }
}