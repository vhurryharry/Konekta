namespace WCA.Actionstep.Client.Resources
{
    public class DataCollectionRecordValue
    {
        public string Id { get; set; }
        public string StringValue { get; set; }
        public DataCollectionRecordValueLink Links { get; set; } = new DataCollectionRecordValueLink();
    }

    public class DataCollectionRecordValueLink
    {
        public string Action { get; set; }
        public string DataCollectionField { get; set; }
        public string DataCollectionRecord { get; set; }
        public string DataCollection { get; set; }
    }


}
