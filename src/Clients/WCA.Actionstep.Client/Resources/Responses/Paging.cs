namespace WCA.Actionstep.Client.Resources.Responses
{
    public class Paging
    {
        public int RecordCount { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string PrevPage { get; set; }
        public string NextPage { get; set; }
    }
}
