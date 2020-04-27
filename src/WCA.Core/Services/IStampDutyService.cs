using WCA.Domain.Models;

namespace WCA.Core.Services
{
    public interface IStampDutyService
    {
        FinancialResults Calculate(PropertySaleInformation propertySaleInformation);
    }
}