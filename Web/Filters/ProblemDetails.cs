namespace GenericFunctions.Web.Filters
{
    public class ProblemDetails
    {
        public string Detail { get; set; }

        public string Instance { get; set; }

        public int? Status { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string StackTrace { get; set; }

        public bool IsDevelopment { get; set; }
    }
}
