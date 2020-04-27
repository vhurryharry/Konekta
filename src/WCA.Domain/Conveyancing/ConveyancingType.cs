namespace WCA.Domain.Conveyancing
{
    public enum ConveyancingType
    {
        None = 0,
        Sale = 1,
        Purchase = 2,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "API Dependent")]
        Purchase_OTP = 3,
        Transfer = 4
    }
}
