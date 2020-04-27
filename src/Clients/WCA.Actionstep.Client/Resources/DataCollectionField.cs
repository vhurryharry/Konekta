namespace WCA.Actionstep.Client.Resources
{
    public class DataCollectionField
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CustomHtmlAbove { get; set; }
        public string CustomHtmlBelow { get; set; }
        public string DataType { get; set; }
        public string Label { get; set; }
        public int FormOrder { get; set; }
        public int ListOrder { get; set; }
        public string Required { get; set; }
        public DataCollectionFieldLink Links { get; set; }
    }

    public class DataCollectionFieldLink
    {
        public string Action { get; set; }
        public string DataCollectionField { get; set; }
        public string DataCollectionRecord { get; set; }
        public string DataCollection { get; set; }
    }
}
