using System.Globalization;

namespace WCA.Actionstep.Client.Resources
{
    public class ActionFolder
    {
        public ActionFolder()
        {
        }

        public ActionFolder(int actionId)
        {
            Links = new Link()
            {
                Action = actionId.ToString(CultureInfo.InvariantCulture)
            };
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public Link Links { get; set; } = new Link();

        public class Link
        {
            public string Action { get; set; }
            public string ParentFolder { get; set; }
        }
    }
}
