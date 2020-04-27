using WCA.Domain.Extensions;
using WCA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WCA.Core.Services
{
    public class StampDutyService : IStampDutyService
    {
        private TieredDiscountService firstHomeDiscountService;
        private TieredDiscountService firstHomeVacantLandDiscountService;
        private decimal foreignerDutyPercentage;
        private TieredRateService homeConsessionService;
        private TieredRateService transferDutyService;
        private TieredRateService transferFeeService;

        public StampDutyService()
        {
            // 'Transfer duty rates' from https://www.qld.gov.au/housing/buying-owning-home/transfer-duty-rates/
            transferDutyService = new TieredRateService(new[]
            {
                new TieredRateTier(0, 5000, 100, 0),
                new TieredRateTier(5000, 75000, 100, 1.5M),
                new TieredRateTier(75000, 540000, 100, 3.5M),
                new TieredRateTier(540000, 1000000, 100, 4.5M),
                new TieredRateTier(1000000, decimal.MaxValue, 100, 5.75M),
            });

            // 'Home concession' rates from https://www.qld.gov.au/housing/buying-owning-home/home-transfer-duty-concession-rates/
            homeConsessionService = new TieredRateService(new[]
            {
                new TieredRateTier(0, 350000, 100, 1),
                new TieredRateTier(350000, 540000, 100, 3.5M),
                new TieredRateTier(540000, 1000000, 100, 4.5M),
                new TieredRateTier(1000000, decimal.MaxValue, 100, 5.75M),
            });

            // Section 2 in 'Titles Registry lodgment fees 2016-17PDF' at https://publications.qld.gov.au/dataset/titles-registry-lodgement-fees
            transferFeeService = new TieredRateService(new[] {
                new TieredRateTier(0, 180000, 0, 0),
                new TieredRateTier(180000, decimal.MaxValue, 10000, 36),
            });

            // 'First home concession' rates from https://www.qld.gov.au/housing/buying-owning-home/home-transfer-duty-concession-rates/
            firstHomeDiscountService = new TieredDiscountService(new[] {
                new TieredDiscountTier(0, 504999.99M, 8750M),
                new TieredDiscountTier(505000, 509999.99M, 7875),
                new TieredDiscountTier(510000, 514999.99M, 7000),
                new TieredDiscountTier(515000, 519999.99M, 6125),
                new TieredDiscountTier(520000, 524999.99M, 5250),
                new TieredDiscountTier(525000, 529999.99M, 4375),
                new TieredDiscountTier(530000, 534999.99M, 3500),
                new TieredDiscountTier(535000, 539999.99M, 2625),
                new TieredDiscountTier(540000, 544999.99M, 1750),
                new TieredDiscountTier(545000, 549999.99M, 875),
                new TieredDiscountTier(550000, decimal.MaxValue, 0)
            });

            // 'First home vacant land concession' rates from https://www.qld.gov.au/housing/buying-owning-home/home-transfer-duty-concession-rates/
            firstHomeVacantLandDiscountService = new TieredDiscountService(new[] {
                new TieredDiscountTier(0, 250000, decimal.MaxValue),
                new TieredDiscountTier(250000.01M, 259999.99M, 7175),
                new TieredDiscountTier(260000, 269999.99M, 6700),
                new TieredDiscountTier(270000, 279999.99M, 6225),
                new TieredDiscountTier(280000, 289999.99M, 5750),
                new TieredDiscountTier(290000, 299999.99M, 5275),
                new TieredDiscountTier(300000, 309999.99M, 4800),
                new TieredDiscountTier(310000, 319999.99M, 4325),
                new TieredDiscountTier(320000, 329999.99M, 3850),
                new TieredDiscountTier(330000, 339999.99M, 3375),
                new TieredDiscountTier(340000, 349999.99M, 2900),
                new TieredDiscountTier(350000, 359999.99M, 2425),
                new TieredDiscountTier(360000, 369999.99M, 1950),
                new TieredDiscountTier(370000, 379999.99M, 1475),
                new TieredDiscountTier(380000, 389999.99M, 1000),
                new TieredDiscountTier(390000, 399999.99M, 525),
                new TieredDiscountTier(400000, decimal.MaxValue, 0)
            });

            foreignerDutyPercentage = 3;
        }

        private enum StampDutyType
        {
            FirstHomeVacantLand,
            FirstHome,
            Home,
            NoConcession
        }

        /// <summary>
        /// Calculates the specified property sale information.
        /// Additional references for calculations:
        ///  - https://www.business.qld.gov.au/industries/building-property-development/titles-property-surveying/titles-property/fee-calculator
        ///  - https://www.dnrm.qld.gov.au/__data/assets/pdf_file/0007/369592/titles-registry-lodgement-fees-2016-2017.pdf
        /// </summary>
        /// <param name="propertySaleInformation">The property sale information.</param>
        /// <returns></returns>
        /// <exception cref="InvalidPropertySaleInformationException">
        /// Transfer duty can currently only be calculated for Queensland.
        /// or
        /// </exception>
        public FinancialResults Calculate(PropertySaleInformation propertySaleInformation)
        {
            if (propertySaleInformation.State != State.QLD)
            {
                throw new InvalidPropertySaleInformationException("Transfer duty can currently only be calculated for Queensland.");
            }

            Fraction allTransfereesInterests = propertySaleInformation.Buyers.Sum(b => b.Shares);
            if (allTransfereesInterests > 1)
            {
                throw new InvalidPropertySaleInformationException($"The combined fraction of ownership for all buyers is {allTransfereesInterests} which is more than 1. All buyers fractions must be less than or equal to 1.");
            }

            var feeLineItems = new List<FinancialResultLineItem>();
            var concessionLineItems = new List<FinancialResultLineItem>();
            var stampDutySummaryLineItems = new List<FinancialResultLineItem>();
            decimal propertyDutiableAmount = propertySaleInformation.PurchasePrice * allTransfereesInterests;

            decimal fullMorgtgageFee = 192M;
            decimal fullTransferFee = 192 + transferFeeService.CalculateTieredRate(propertySaleInformation.PurchasePrice);
            decimal fullTransferDuty = transferDutyService.CalculateTieredRate(propertyDutiableAmount);
            decimal dutyPayable = 0M;

            Fraction buyerFractionWhereHomeDutyApplies = propertySaleInformation.Buyers.Where(b => BuyerHomeDutyApplies(b)).Sum(b => b.Shares);
            Fraction buyerFractionWhereHomeDutyAppliesInverse = buyerFractionWhereHomeDutyApplies.InverseOrZero();

            Fraction buyerFractionHomeDutyDeductionApplies = propertySaleInformation.Buyers.Where(b => BuyerHomeDutyDeductionApplies(b)).Sum(b => b.Shares);
            Fraction buyerFractionHomeDutyDeductionAppliesInverse = buyerFractionHomeDutyDeductionApplies.InverseOrZero();

            Fraction buyerFractionWhereFirstHomeDutyApplies = propertySaleInformation.Buyers.Where(b => BuyerFirstHomeDutyApplies(b)).Sum(b => b.Shares);
            Fraction buyerFractionWhereFirstHomeDutyAppliesInverse = buyerFractionWhereFirstHomeDutyApplies.InverseOrZero();

            decimal totalFirstHomeVacantLandDeduction = 0M;
            decimal totalDeductionAmount = 0M;
            decimal totalHomeDuty = 0M;
            decimal totalFirstHomeDuty = 0M;

            if (propertySaleInformation.PropertyType == PropertyType.VacantLand)
            {
                totalFirstHomeVacantLandDeduction = GetTotalVacantLandDeductionAmount(propertySaleInformation, propertySaleInformation.PurchasePrice, fullTransferDuty, buyerFractionWhereFirstHomeDutyApplies);
                dutyPayable = fullTransferDuty - totalFirstHomeVacantLandDeduction;
            }
            else
            {
                totalDeductionAmount = GetTotalDeductionAmount(propertySaleInformation, propertySaleInformation.PurchasePrice, buyerFractionHomeDutyDeductionApplies);
                totalHomeDuty = GetTotalHomeDutyAmount(propertySaleInformation, propertySaleInformation.PurchasePrice, buyerFractionWhereHomeDutyApplies);
                totalFirstHomeDuty = GetTotalFirstHomeDutyAmount(propertySaleInformation, propertySaleInformation.PurchasePrice, buyerFractionWhereFirstHomeDutyApplies);
                dutyPayable = fullTransferDuty - totalDeductionAmount + totalHomeDuty + totalFirstHomeDuty;
            }

            feeLineItems.Add(new FinancialResultLineItem($"Mortgage Fee", fullMorgtgageFee));
            feeLineItems.Add(new FinancialResultLineItem($"Transfer Fee", fullTransferFee));
            feeLineItems.Add(new FinancialResultLineItem($"Transfer Duty Payable", dutyPayable));

            Fraction allTransfereesInterestsInverse = allTransfereesInterests.InverseOrZero();
            int buyerCount = 0;
            foreach (PropertyBuyer buyer in propertySaleInformation.Buyers)
            {
                buyerCount++;

                decimal buyerTransferDuty = fullTransferDuty * buyer.Shares * allTransfereesInterestsInverse;
                if (propertySaleInformation.PropertyType == PropertyType.VacantLand)
                {
                    if (BuyerFirstHomeDutyApplies(buyer))
                    {
                        buyerTransferDuty = buyerTransferDuty - totalFirstHomeVacantLandDeduction * buyer.Shares * buyerFractionWhereFirstHomeDutyAppliesInverse;
                    }
                }
                else
                {
                    if (BuyerHomeDutyDeductionApplies(buyer))
                    {
                        buyerTransferDuty = buyerTransferDuty - totalDeductionAmount * buyer.Shares * buyerFractionHomeDutyDeductionAppliesInverse;
                    }

                    if (BuyerHomeDutyApplies(buyer))
                    {
                        buyerTransferDuty = buyerTransferDuty + totalHomeDuty * buyer.Shares * buyerFractionWhereHomeDutyAppliesInverse;
                    }

                    if (BuyerFirstHomeDutyApplies(buyer))
                    {
                        buyerTransferDuty = buyerTransferDuty + totalFirstHomeDuty * buyer.Shares * buyerFractionWhereFirstHomeDutyAppliesInverse;
                    }
                }

                stampDutySummaryLineItems.Add(new FinancialResultLineItem($"Buyer {buyer.BuyerNumber}", buyerTransferDuty));

                decimal foreignBuyerDuty = GetForeignBuyerDuty(propertySaleInformation.PurchasePrice, buyer);
                if (foreignBuyerDuty > 0)
                {
                    feeLineItems.Add(new FinancialResultLineItem($"Buyer {buyer.BuyerNumber} - Foreign Buyers Duty", foreignBuyerDuty));
                }

                decimal firstHomeOwnerGrant = GetFirstHomeOwnerGrant(propertySaleInformation, buyer);
                if (firstHomeOwnerGrant > 0)
                {
                    concessionLineItems.Add(new FinancialResultLineItem($"Buyer {buyer.BuyerNumber} - First Home Owners Grant", firstHomeOwnerGrant));
                }
            }

            // Wrap up categories to be returned
            FinancialResultCategory feeCategory = new FinancialResultCategory("Fees", feeLineItems.ToArray());
            FinancialResultCategory concessionCategory = new FinancialResultCategory("Concessions", concessionLineItems.ToArray());
            FinancialResultCategory stampDutySummaryCategory = new FinancialResultCategory("Stamp Duty Summary", stampDutySummaryLineItems.ToArray());

            if (buyerCount > 1 && stampDutySummaryCategory.Total > 0)
            {
                return new FinancialResults(new[] { feeCategory, stampDutySummaryCategory, concessionCategory });
            }
            else
            {
                return new FinancialResults(new[] { feeCategory, concessionCategory });
            }
        }

        private bool BuyerFirstHomeDutyApplies(PropertyBuyer buyer)
        {
            return buyer.IntendedUse == IntendedPropertyUse.PrimaryResidence && buyer.FirstHomeBuyer;
        }

        private bool BuyerHomeDutyApplies(PropertyBuyer buyer)
        {
            return buyer.IntendedUse == IntendedPropertyUse.PrimaryResidence && !buyer.FirstHomeBuyer;
        }

        private bool BuyerHomeDutyDeductionApplies(PropertyBuyer buyer)
        {
            return buyer.IntendedUse == IntendedPropertyUse.PrimaryResidence;
        }

        private bool BuyerVacantLandApplies(PropertyBuyer buyer, PropertyType propertyType)
        {
            return buyer.IntendedUse == IntendedPropertyUse.PrimaryResidence && buyer.FirstHomeBuyer && propertyType == PropertyType.VacantLand;
        }

        private decimal GetFirstHomeOwnerGrant(PropertySaleInformation propertySaleInformation, PropertyBuyer buyer)
        {
            // https://firsthomeowners.initiatives.qld.gov.au/eligibility.php
            if (propertySaleInformation.PurchasePrice < 750000 &&
                propertySaleInformation.PropertyType == PropertyType.NewHome)
            {
                if (BuyerFirstHomeDutyApplies(buyer))
                {
                    return 15000M;
                }
            }

            return 0M;
        }

        private decimal GetForeignBuyerDuty(decimal propertyDutiableAmount, PropertyBuyer buyer)
        {
            decimal buyersInterestValue = buyer.Shares * propertyDutiableAmount;
            return (buyer.IsForeignBuyer) ? buyersInterestValue * foreignerDutyPercentage / 100 : 0;
        }

        private decimal GetTotalDeductionAmount(PropertySaleInformation propertySaleInformation, decimal fullPurchasePrice, Fraction buyerFractionHomeDutyDeductionApplies)
        {
            if (propertySaleInformation.Buyers.Count() > 0)
            {
                decimal deductionBase = (decimal)(buyerFractionHomeDutyDeductionApplies * Math.Min(350000M, fullPurchasePrice));
                foreach (PropertyBuyer buyer in propertySaleInformation.Buyers)
                {
                    if (buyer.FirstHomeBuyer)
                    {
                        deductionBase = (decimal)(buyerFractionHomeDutyDeductionApplies * fullPurchasePrice);
                        break;
                    }
                }

                decimal totalDeductionAmount = transferDutyService.CalculateTieredRate(deductionBase);
                return totalDeductionAmount;
            }
            return 0M;
        }

        private decimal GetTotalFirstHomeDutyAmount(PropertySaleInformation propertySaleInformation, decimal fullPurchasePrice, Fraction buyerFractionWhereHomeDutyApplies)
        {
            decimal firstHomeDuty = homeConsessionService.CalculateTieredRate(fullPurchasePrice);
            decimal firstHomeDiscount = firstHomeDiscountService.CalculateDiscountRate(propertySaleInformation.PurchasePrice);
            firstHomeDuty = Math.Max(firstHomeDuty - firstHomeDiscount, 0M);
            firstHomeDuty *= buyerFractionWhereHomeDutyApplies;
            return firstHomeDuty;
        }

        private decimal GetTotalHomeDutyAmount(PropertySaleInformation propertySaleInformation, decimal fullPurchasePrice, Fraction buyerFractionWithHomeDiscount)
        {
            decimal homeDutiableAmount = Math.Min(fullPurchasePrice, 350000);
            decimal homeDuty = homeConsessionService.CalculateTieredRate(homeDutiableAmount);
            homeDuty *= buyerFractionWithHomeDiscount;
            return homeDuty;
        }

        private decimal GetTotalVacantLandDeductionAmount(PropertySaleInformation propertySaleInformation, decimal fullPurchasePrice, decimal fullTransferDuty, Fraction buyerFractionWhereFirstHomeDutyApplies)
        {
            decimal firstHomeVacantLandTransferDutyPortion = fullTransferDuty * buyerFractionWhereFirstHomeDutyApplies;
            decimal firstHomeVacantLandRebate = firstHomeVacantLandDiscountService.CalculateDiscountRate(fullPurchasePrice * buyerFractionWhereFirstHomeDutyApplies);
            return Math.Min(firstHomeVacantLandTransferDutyPortion, firstHomeVacantLandRebate);
        }
    }
}