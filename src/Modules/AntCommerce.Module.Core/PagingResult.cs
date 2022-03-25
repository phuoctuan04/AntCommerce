namespace AntCommerce.Module.Core
{
    public class PagingResult<T> where T : class
    {
        public List<T>? Data { get; set; }

        public MetaData? MetaData { get; set; }
    }

    public class MetaData
    {
        public int Total { get; set; }

        public int Limit { get; set; }

        public int Page { get; set; }
    }
}
