using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;

namespace WCA.PEXA.Client.Resources
{
    public class WorkspaceInvitationRequestCommand : PEXARequestBase
    {
        public WorkspaceInvitationRequestCommand(CreateWorkspaceInvitationRequestType workspaceInvitationRequest, string bearerToken)
        {
            WorkspaceInvitationRequest = workspaceInvitationRequest;
            BearerToken = bearerToken;
        }

        public CreateWorkspaceInvitationRequestType WorkspaceInvitationRequest { get; set; }

        public override string Path => "/v1/workspace/invitation";

        public override HttpMethod HttpMethod => HttpMethod.Post;

        public override HttpContent Content
        {
            get
            {
                if (WorkspaceInvitationRequest == null)
                {
                    return null;
                }

                // TODO Put this in a reusable static class or extension method
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
                ns.Add("", "http://api.pexa.net.au/schema/1/");

                var serializer = new XmlSerializer(typeof(CreateWorkspaceInvitationRequestType));
                StringContent content = null;

                using (var stringWriter = new Utf8StringWriter())
                {
                    serializer.Serialize(stringWriter, WorkspaceInvitationRequest, ns);
                    content = new StringContent(stringWriter.ToString(), Encoding.UTF8);
                    content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/xml");
                };

                return content;
            }
        }

        public override int Version => 1;
    }
}
