using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WCA.Core.Features.Conveyancing.PolicyRequest;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.Conveyancing.Services
{
    public class FirstTitleToWCAMapper : IFirstTitleToWCAMapper
    {
        public async Task<SendFirstTitlePolicyRequestResponse> MapFromFirstTitleResponse(TitleInsuranceResponse titleInsuranceResponse)
        {
            if (titleInsuranceResponse is null)
            {
                throw new System.ArgumentNullException(nameof(titleInsuranceResponse));
            }

            if (titleInsuranceResponse.Message[0].MessageBody[0].Status[0].Name == StatusName.Succeeded)
            {
                var prices = titleInsuranceResponse.Message[0].TitleInsuranceResponseSegment.Price;
                var response = new SendFirstTitlePolicyRequestResponse
                {
                    PolicyNumber = titleInsuranceResponse.Message[0].TitleInsuranceResponseSegment.Policy.PolicyCode,
                    Price = new FirstTitlePrice
                    {
                        Premium = prices.First(p => p.PriceType == PricePriceType.Premium).Value,
                        GSTOnPremium = prices.First(p => p.PriceType == PricePriceType.GSTOnPremium).Value,
                        StampDuty = prices.First(p => p.PriceType == PricePriceType.StampDuty).Value,
                    }
                };

                if (titleInsuranceResponse.AttachmentSegment != null && titleInsuranceResponse.AttachmentSegment.Length > 0)
                {
                    response.AttachmentPaths = new FTAttachment[titleInsuranceResponse.AttachmentSegment.Length];

                    for(var i = 0; i < titleInsuranceResponse.AttachmentSegment.Length; i ++)
                    {
                        var attachment = titleInsuranceResponse.AttachmentSegment[i];
                        var path = Path.Join(Path.GetTempPath(), attachment.Filename);

                        await File.WriteAllBytesAsync(path, attachment.InlineAttachment);

                        response.AttachmentPaths[i] = new FTAttachment()
                        {
                            FileName = attachment.Filename,
                            FileUrl = path
                        };
                    }
                }

                return response;
            }
            else if (titleInsuranceResponse.Message[0].MessageBody[0].Status[0].Name == StatusName.Pending)
            {
                var prices = titleInsuranceResponse.Message[0].TitleInsuranceResponseSegment.Price;
                return new SendFirstTitlePolicyRequestResponse
                {
                    PolicyNumber = titleInsuranceResponse.Message[0].TitleInsuranceResponseSegment.Policy.PolicyCode,
                    Price = new FirstTitlePrice
                    {
                        Premium = prices.First(p => p.PriceType == PricePriceType.Premium).Value,
                        GSTOnPremium = prices.First(p => p.PriceType == PricePriceType.GSTOnPremium).Value,
                        StampDuty = prices.First(p => p.PriceType == PricePriceType.StampDuty).Value,
                    }
                };
            }
            else
            {
                var errorMessage = titleInsuranceResponse.Message[0].MessageBody[0].MessageAnnotation[0].Value;
                throw new FirstTitlePolicyRequestException(errorMessage);
            }
        }
    }
}
