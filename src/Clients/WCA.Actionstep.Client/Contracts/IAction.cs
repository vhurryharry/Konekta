namespace WCA.Actionstep.Client.Contracts
{
    public interface IAction
    {
        /// <summary>
        /// Actionstep Organisation Key.
        /// </summary>
        string OrgKey { get; }

        /// <summary>
        /// Also known as Matter Id.
        /// </summary>
        int ActionId { get; }

        /// <summary>
        /// Data Collections for the specified Action Id and Organisation Key.
        /// </summary>
        IDataCollections DataCollections { get; }
    }
}