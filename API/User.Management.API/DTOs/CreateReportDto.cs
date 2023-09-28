using Shopx.API.Data;

namespace Shopx.API.DTOs
{
    public class CreateReportDto
    {
        private string _reportReason;

        public int ProductId { get; set; }
        public string ReportReason
        {
            get { return _reportReason; }
            set 
            {
                if (!_options.ReportReasons().Contains(value))
                    throw new Exception("Invalid report reason");
                
                _reportReason = value;
            }
        }

        public string ReportDetails { get; set; }
    }
}
