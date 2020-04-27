
using Newtonsoft.Json;

namespace WCA.Domain.Models.Settlement
{
    /// <summary>
    /// TODO: Is MatterDetails reqired? Gang of four: favour composition over inheritance.
    ///       Perhaps have MatterDetails as a property of ActionstepMatter. But even better,
    ///       we just remove MatterDetails and rethink this in the context of a ConveyancingMatter.
    /// </summary>
    public class ActionstepMatter : MatterDetails
    {
        public float Price { get; }
        public float Deposit { get; }

        public ActionstepMatter() : base()
        {
            Price = 0;
            Deposit = 0;
        }

        public ActionstepMatter(MatterDetails matterDetails, float price, float deposit) :
            base(matterDetails)
        {
            Price = price;
            Deposit = deposit;
        }

        public static ActionstepMatter FromString(string dataString)
        {
            ActionstepMatter actionstepData = JsonConvert.DeserializeObject<ActionstepMatter>(dataString);
            return actionstepData;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
