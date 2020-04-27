using Moq;
using NodaTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Core.Features.GlobalX.Transactions;
using Xunit;

namespace WCA.UnitTests.GlobalX
{
    public class CreateDisbursementsCommandTests
    {
        [Fact]
        public async Task SplitTransactionIsMappedCorrectly()
        {
            // Arrange
            var mockActionstepService = new Mock<IActionstepService>();
            const int taxCodeGSTFree = 20;
            const int taxCodeGSTTaxable = 30;

            mockActionstepService
                .Setup(a => a.Handle<ListDisbursementsResponse>(It.IsAny<CreateDisbursementsRequest>()))
                .Returns(Task.FromResult(new ListDisbursementsResponse()
                {
                    Disbursements =
                    {
                        new Disbursement()
                        {
                            Id = 1,
                            Date = new LocalDate(2020, 3, 10),
                            Description = "Description1",
                            Links = { TaxCode = taxCodeGSTFree }
                        },
                        new Disbursement()
                        {
                            Id = 3,
                            Date = new LocalDate(2020, 3, 10),
                            Description = "Description2",
                            Links = { TaxCode = taxCodeGSTTaxable }
                        }
                    }
                }));

            var handler = new CreateDisbursementsCommand.CreateDisbursementsCommandHandler(mockActionstepService.Object, new CreateDisbursementsCommand.Validator());
            CreateDisbursementsCommand request = CreateCommand(taxCodeGSTFree, taxCodeGSTTaxable);

            // Act
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.Equal(40, result.GlobalXTransactionId);
            Assert.Equal(1, result.GSTFreeDisbursementId);
            Assert.Equal(3, result.GSTTaxableDisbursementId);
        }

        [Fact]
        public async Task GSTTaxableOnlyTransactionIsMappedCorrectly()
        {
            // Arrange
            var mockActionstepService = new Mock<IActionstepService>();
            const int taxCodeGSTFree = 20;
            const int taxCodeGSTTaxable = 30;

            mockActionstepService
                .Setup(a => a.Handle<ListDisbursementsResponse>(It.IsAny<CreateDisbursementsRequest>()))
                .Returns(Task.FromResult(new ListDisbursementsResponse()
                {
                    Disbursements =
                    {
                        new Disbursement()
                        {
                            Id = 1,
                            Date = new LocalDate(2020, 3, 10),
                            Description = "Description2",
                            Links = { TaxCode = taxCodeGSTTaxable }
                        }
                    }
                }));

            var handler = new CreateDisbursementsCommand.CreateDisbursementsCommandHandler(mockActionstepService.Object, new CreateDisbursementsCommand.Validator());
            CreateDisbursementsCommand request = CreateCommand(taxCodeGSTFree, taxCodeGSTTaxable);

            // Act
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.Equal(40, result.GlobalXTransactionId);
            Assert.Null(result.GSTFreeDisbursementId);
            Assert.Equal(1, result.GSTTaxableDisbursementId);
        }

        [Fact]
        public async Task GSTFreeOnlyTransactionIsMappedCorrectly()
        {
            // Arrange
            var mockActionstepService = new Mock<IActionstepService>();
            const int taxCodeGSTFree = 20;
            const int taxCodeGSTTaxable = 30;

            mockActionstepService
                .Setup(a => a.Handle<ListDisbursementsResponse>(It.IsAny<CreateDisbursementsRequest>()))
                .Returns(Task.FromResult(new ListDisbursementsResponse()
                {
                    Disbursements =
                    {
                        new Disbursement()
                        {
                            Id = 1,
                            Date = new LocalDate(2020, 3, 10),
                            Description = "Description2",
                            Links = { TaxCode = taxCodeGSTFree }
                        }
                    }
                }));

            var handler = new CreateDisbursementsCommand.CreateDisbursementsCommandHandler(mockActionstepService.Object, new CreateDisbursementsCommand.Validator());
            CreateDisbursementsCommand request = CreateCommand(taxCodeGSTFree, taxCodeGSTTaxable);

            // Act
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.Equal(40, result.GlobalXTransactionId);
            Assert.Equal(1, result.GSTFreeDisbursementId);
            Assert.Null(result.GSTTaxableDisbursementId);
        }

        private static CreateDisbursementsCommand CreateCommand(int taxCodeGSTFree, int taxCodeGSTTaxable)
        {
            return new CreateDisbursementsCommand()
            {
                ActionstepUserId = "UserId",
                ActionstepOrgKey = "OrgKey",
                ActionstepMatterId = 10,
                TaxCodeIdNoGST = taxCodeGSTFree,
                TaxCodeIdWithGST = taxCodeGSTTaxable,
                Transaction = new WCA.GlobalX.Client.Transactions.Transaction()
                {
                    TransactionId = 40,
                    Matter = "50",
                    RetailGst = 1,
                    RetailPrice = 10,
                    SearchReference = "SearchReference",
                    OrderId = "OrderId",
                    TransactionDateTime = new DateTimeOffset(2020, 3, 10, 0, 0, 0, TimeSpan.Zero),
                    User = { UserId = "GlobalXUserId", CustomerRef = "" },
                    Product = { ProductDescription = "ProductDescription", ProductCode = "ProductCode" }
                },
            };
        }
    }
}