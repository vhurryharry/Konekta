using NodaTime;
using System.Collections.Generic;

namespace WCA.Domain.Conveyancing
{
    public class ConveyancingMatter
    {
        public int Id { get; set; }

        public string ActionType { get; set; }
        public ConveyancingType ConveyancingType { get; set; }
        public string ConveyancingSubType { get; set; }

        public string FileReference { get; set; }
        public string Name { get; set; }
        public Instant Created { get; set; }
        public Instant Updated { get; set; }
        public PropertyDetails PropertyDetails { get; set; }

        public List<Party> PropertyAddresses { get; } = new List<Party>();
        public List<Party> Buyers { get; } = new List<Party>();
        public List<Party> Sellers { get; } = new List<Party>();
        public List<Party> Conveyancers { get; } = new List<Party>();
        public List<Party> Clients { get; } = new List<Party>();
        public List<Party> ClientPrimaryContacts { get; } = new List<Party>();
        public List<Party> AgentsOffice { get; } = new List<Party>();
        public List<Party> PrincipalSolicitor { get; } = new List<Party>();
        public List<Party> IncomingBanks { get; } = new List<Party>();
        public List<Party> OthersideSolicitor { get; } = new List<Party>();
        public List<Party> OthersideSolicitorPrimaryContact { get; } = new List<Party>();

        public decimal? PurchasePrice { get; set; }

        public decimal? FullDeposit { get; set; }
        public decimal? InitialDeposit { get; set; }
        public decimal? BalanceDeposit { get; set; }
        public string DepositHolder { get; set; }
        public bool DepositBondExists { get; set; }
        public bool OriginalBondGuaranteedHeldByAgent { get; set; }
        public decimal? Reduction { get; set; }
        public List<string> SpecialConditions { get; } = new List<string>();
        public string SettlementBookingTime { get; set; }
        public LocalDate SettlementDate { get; set; }
        public Interval ContractLength { get; set; }
        public string SettlementLocation { get; set; }
        public string SettlementVenue { get; set; }
        public string BookingReference { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? PayoutFigure { get; set; }
        public decimal? AgentCommissionPayable { get; set; }
    }
}
