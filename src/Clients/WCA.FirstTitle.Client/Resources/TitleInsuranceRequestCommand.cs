using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WCA.FirstTitle.Client.Resources
{
    public class TitleInsuranceRequestCommand : FirstTitleRequestBase
    {
        public TitleInsuranceRequestCommand(TitleInsuranceRequest titleInsuranceRequest, FirstTitleCredential firstTitleCredential)
        {
            TitleInsuranceRequest = titleInsuranceRequest;
            FirstTitleCredential = firstTitleCredential;
        }

        public TitleInsuranceRequest TitleInsuranceRequest { get; set; }

        public override FirstTitleCredential FirstTitleCredential { get; set; }

        public override HttpMethod HttpMethod => HttpMethod.Post;

        public override string SOAPAction => "http://ws.etitle.com.au/schemas/submitAlbum";

        public override string Content
        {
            get
            {
                if (TitleInsuranceRequest == null)
                {
                    return null;
                }

                // TODO Put this in a reusable static class or extension method
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "http://ws.etitle.com.au/schemas");

                var serializer = new XmlSerializer(typeof(TitleInsuranceRequest));
                string content = null;

                using (var stringWriter = new Utf8StringWriter())
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.OmitXmlDeclaration = true;

                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, TitleInsuranceRequest, ns);
                        content = stringWriter.ToString();

                        content = $@"<TitleInsuranceRequest xmlns=""http://ws.etitle.com.au/schemas"">
                                {content}
                            </TitleInsuranceRequest>";
                    }
                };

                return content;
            }
        }
    }
}
