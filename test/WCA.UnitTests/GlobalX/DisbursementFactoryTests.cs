using NodaTime;
using System;
using System.Globalization;
using System.Linq;
using WCA.Core.Features.GlobalX;
using WCA.GlobalX.Client.Transactions;
using Xunit;

namespace WCA.UnitTests.GlobalX
{
    public class DisbursementFactoryTests
    {
        [Fact]
        public void SimpleTransactionToDisbursement()
        {
            var matterId = 2;
            Transaction source = new Transaction()
            {
                TransactionId = 1,
                Matter = matterId.ToString(CultureInfo.InvariantCulture),
                OrderId = "OrderABC123",
                TransactionDateTime = new DateTimeOffset(2020, 1, 1, 1, 1, 0, TimeSpan.Zero),
                RetailPrice = 10,
                RetailGst = 1,
                WholesalePrice = 10,
                WholesaleGst = 1,
                Product = new Product()
                {
                    ProductCode = "ProductCode",
                    ProductDescription = "Product Description",
                    ProductSubGroup = "Product Sub Group"
                }
            };

            int gstTaxCodeId = 3;
            int nonGstTaxCodeId = 4;

            var result = DisbursementFactory.FromTransaction(matterId, source, gstTaxCodeId, nonGstTaxCodeId);

            var singleDisbursement = result.First();

            Assert.Equal(new LocalDate(2020, 1, 1), singleDisbursement.Date);
            Assert.Equal(2, singleDisbursement.Links.Action);
            Assert.Equal(@"GlobalX order OrderABC123, product 'Product Description', GST Taxable", singleDisbursement.Description);
            Assert.Equal(1, singleDisbursement.Quantity);
            Assert.Equal(10, singleDisbursement.UnitPrice);
            Assert.False(singleDisbursement.UnitPriceIncludesTax);
            Assert.Equal(3, singleDisbursement.Links.TaxCode);
            Assert.Equal("GX-1-GST", singleDisbursement.ImportExternalReference);
        }

        [Fact]
        public void WithGstCreatedCorrectly()
        {
            // Arrange
            var matterId = 2;
            Transaction source = new Transaction()
            {
                TransactionId = 1,
                Matter = matterId.ToString(CultureInfo.InvariantCulture),
                RetailPrice = 10,
                RetailGst = 1
            };

            int gstTaxCodeId = 3;
            int nonGstTaxCodeId = 4;

            // Act
            var result = DisbursementFactory.FromTransaction(matterId, source, gstTaxCodeId, nonGstTaxCodeId);

            // Assert
            var singleDisbursement = result.First();
            Assert.Equal(gstTaxCodeId, singleDisbursement.Links.TaxCode);
            Assert.Equal(10, singleDisbursement.UnitPrice);
            Assert.False(singleDisbursement.UnitPriceIncludesTax);
            Assert.Equal("GX-1-GST", singleDisbursement.ImportExternalReference);
        }

        [Fact]
        public void WithoutGstCreatedCorrectly()
        {
            // Arrange
            var matterId = 2;
            Transaction source = new Transaction()
            {
                TransactionId = 1,
                Matter = matterId.ToString(CultureInfo.InvariantCulture),
                RetailPrice = 10,
                RetailGst = 0
            };

            int gstTaxCodeId = 3;
            int nonGstTaxCodeId = 4;

            // Act
            var result = DisbursementFactory.FromTransaction(matterId, source, gstTaxCodeId, nonGstTaxCodeId);

            // Assert
            var singleDisbursement = result.First();
            Assert.Equal(nonGstTaxCodeId, singleDisbursement.Links.TaxCode);
            Assert.Equal(10, singleDisbursement.UnitPrice);
            Assert.False(singleDisbursement.UnitPriceIncludesTax);
            Assert.Equal("GX-1-GSTFree", singleDisbursement.ImportExternalReference);
        }

        [Fact]
        public void RefundWithGstCreatedCorrectly()
        {
            // Arrange
            var matterId = 2;
            Transaction source = new Transaction()
            {
                TransactionId = 1,
                Matter = matterId.ToString(CultureInfo.InvariantCulture),
                RetailPrice = -10,
                RetailGst = -1
            };

            int gstTaxCodeId = 3;
            int nonGstTaxCodeId = 4;

            // Act
            var result = DisbursementFactory.FromTransaction(matterId, source, gstTaxCodeId, nonGstTaxCodeId);

            // Assert
            var singleDisbursement = result.First();
            Assert.Equal(gstTaxCodeId, singleDisbursement.Links.TaxCode);
            Assert.Equal(-10, singleDisbursement.UnitPrice);
            Assert.False(singleDisbursement.UnitPriceIncludesTax);
            Assert.Equal("GX-1-GST", singleDisbursement.ImportExternalReference);
        }

        [Fact]
        public void RefundWithoutGstCreatedCorrectly()
        {
            // Arrange
            var matterId = 2;
            Transaction source = new Transaction()
            {
                TransactionId = 1,
                Matter = matterId.ToString(CultureInfo.InvariantCulture),
                RetailPrice = -10,
                RetailGst = 0
            };

            int gstTaxCodeId = 3;
            int nonGstTaxCodeId = 4;

            // Act
            var result = DisbursementFactory.FromTransaction(matterId, source, gstTaxCodeId, nonGstTaxCodeId);

            // Assert
            var singleDisbursement = result.First();
            Assert.Equal(nonGstTaxCodeId, singleDisbursement.Links.TaxCode);
            Assert.Equal(-10, singleDisbursement.UnitPrice);
            Assert.False(singleDisbursement.UnitPriceIncludesTax);
            Assert.Equal("GX-1-GSTFree", singleDisbursement.ImportExternalReference);
        }

        [Fact]
        public void SplitGstCreatedCorrectly()
        {
            // Arrange
            var matterId = 2;
            Transaction source = new Transaction()
            {
                TransactionId = 1,
                Matter = matterId.ToString(CultureInfo.InvariantCulture),
                RetailPrice = 10,
                RetailGst = 0.1M
            };

            int gstTaxCodeId = 3;
            int nonGstTaxCodeId = 4;

            // Act
            var result = DisbursementFactory.FromTransaction(matterId, source, gstTaxCodeId, nonGstTaxCodeId);

            // Assert
            var disbursements = result.ToArray();

            var disbursementWithoutGst = disbursements.Single(d => d.Links.TaxCode == nonGstTaxCodeId);
            Assert.Equal(9, disbursementWithoutGst.UnitPrice);
            Assert.False(disbursementWithoutGst.UnitPriceIncludesTax);
            Assert.Equal("GX-1-GSTFree", disbursementWithoutGst.ImportExternalReference);

            var disbursementWithGst = disbursements.Single(d => d.Links.TaxCode == gstTaxCodeId);
            Assert.Equal(1, disbursementWithGst.UnitPrice);
            Assert.False(disbursementWithGst.UnitPriceIncludesTax);
            Assert.Equal("GX-1-GST", disbursementWithGst.ImportExternalReference);
        }

        [Fact]
        public void SimpleGstRoundedCorrectly()
        {
            // Arrange
            var matterId = 2;
            Transaction source = new Transaction()
            {
                TransactionId = 1,
                Matter = matterId.ToString(CultureInfo.InvariantCulture),
                RetailPrice = 65.35M,
                RetailGst = 6.54M
            };

            int gstTaxCodeId = 3;
            int nonGstTaxCodeId = 4;

            // Act
            var result = DisbursementFactory.FromTransaction(matterId, source, gstTaxCodeId, nonGstTaxCodeId);

            // Assert
            var disbursements = result.ToArray();

            var disbursementWithGst = disbursements.Single(d => d.Links.TaxCode == gstTaxCodeId);
            Assert.Equal(65.35M, disbursementWithGst.UnitPrice);
            Assert.False(disbursementWithGst.UnitPriceIncludesTax);
            Assert.Equal("GX-1-GST", disbursementWithGst.ImportExternalReference);
        }
    }
}