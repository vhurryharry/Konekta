using NodaTime;
using System;
using System.Collections.Generic;
using WCA.Actionstep.Client.Resources;
using WCA.GlobalX.Client.Transactions;

namespace WCA.Core.Features.GlobalX
{
    public static partial class DisbursementFactory
    {
        public static IEnumerable<Disbursement> FromTransaction(int matterId, Transaction source, GstTaxCode gstTaxCode, NonGstTaxCode nonGstTaxCode)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            if (source.RetailGst == 0)
            {
                // If no GST, then create a simple disbursement
                yield return CreateDisbursement(source, matterId, source.RetailPrice, nonGstTaxCode);
                yield break;
            }

            var gstCheck = Math.Round(source.RetailPrice / 10, 2);

            if (gstCheck == source.RetailGst)
            {
                // There is GST, but it is 10% of the price. So again a simple disbursement for the whole amount with GST.
                yield return CreateDisbursement(source, matterId, source.RetailPrice, gstTaxCode);
                yield break;
            }

            var gstTaxableComponent = source.RetailGst * 10;
            var gstNonTaxableComponent = source.RetailPrice - gstTaxableComponent;

            if (gstTaxableComponent > source.RetailPrice)
            {
                throw new UnknownGSTException(
                    $"Couldn't determine GST split. The GST seems to be for more than the total price." +
                    $" Not sure how to proceed. The GST amount is {source.RetailGst}, and the Retail Price is {source.RetailPrice}");
            }

            yield return CreateDisbursement(source, matterId, gstNonTaxableComponent, nonGstTaxCode);
            yield return CreateDisbursement(source, matterId, gstTaxableComponent, gstTaxCode);
        }

        private static Disbursement CreateDisbursement(Transaction source, int matter, decimal unitPrice, ITaxCode taxCode)
        {
            var disbursement = new Disbursement()
            {
                Date = LocalDate.FromDateTime(source.TransactionDateTime.Date),
                Description = GenerateDescription(source, taxCode),
                Quantity = 1,
                UnitPrice = unitPrice,
                UnitPriceIncludesTax = false,
                ImportExternalReference = ImportExternalReference(source.TransactionId, taxCode)
            };

            disbursement.Links.Action = matter;
            disbursement.Links.TaxCode = taxCode.TaxCode;

            return disbursement;
        }

        private static string ImportExternalReference(int transactionId, ITaxCode taxCode)
        {
            var suffix = taxCode switch
            {
                GstTaxCode _ => "-GST",
                NonGstTaxCode _ => "-GSTFree",
                _ => ""
            };

            return $"GX-{transactionId}{suffix}";
        }

        private static string GenerateDescription(Transaction source, ITaxCode taxCode)
        {
            var gstTerm = taxCode switch
            {
                GstTaxCode _ => ", GST Taxable",
                NonGstTaxCode _ => ", GST Free",
                _ => ", GST Unknown"
            };

            return $"GlobalX order {source.OrderId}" +
                $", product '{source.Product.ProductDescription}'" +
                gstTerm;
        }
    }
}