namespace Shopx.API.Helper
{
    public class PaginationParams
    {
        private const int MaxSize = 1000000;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = MaxSize;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxSize ? MaxSize : value;
        }
    }
}
