using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace WCA.FirstTitle.Client
{
    // from https://stackoverflow.com/a/54238550/8503440
    public class LoggingEndpointBehaviour : IEndpointBehavior
    {
        public LoggingMessageInspector MessageInspector { get; }

        public LoggingEndpointBehaviour(LoggingMessageInspector messageInspector)
        {
            MessageInspector = messageInspector ?? throw new ArgumentNullException(nameof(messageInspector));
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (clientRuntime is null)
            {
                throw new ArgumentNullException(nameof(clientRuntime));
            }

            clientRuntime.ClientMessageInspectors.Add(MessageInspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    public class LoggingMessageInspector : IClientMessageInspector
    {
        public LoggingMessageInspector(ILogger<FirstTitleClient> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ILogger<FirstTitleClient> Logger { get; }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (reply is null)
            {
                throw new ArgumentNullException(nameof(reply));
            }

            using (var buffer = reply.CreateBufferedCopy(int.MaxValue))
            {
                var document = GetDocument(buffer.CreateMessage());
                Logger.LogTrace(document.OuterXml);

                reply = buffer.CreateMessage();
            }
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var buffer = request.CreateBufferedCopy(int.MaxValue))
            {
                var document = GetDocument(buffer.CreateMessage());
                Logger.LogTrace(document.OuterXml);

                request = buffer.CreateMessage();
                return null;
            }
        }

        private XmlDocument GetDocument(System.ServiceModel.Channels.Message request)
        {
            XmlDocument document = new XmlDocument();
            using (MemoryStream memoryStream = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(memoryStream))
            {
                // write request to memory stream
                request.WriteMessage(writer);
                writer.Flush();
                memoryStream.Position = 0;

                // load memory stream into a document
                document.Load(memoryStream);
            }

            return document;
        }
    }
}
