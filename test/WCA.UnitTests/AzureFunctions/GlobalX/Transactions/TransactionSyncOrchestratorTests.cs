using MediatR;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using WCA.AzureFunctions.GlobalX;
using WCA.AzureFunctions.GlobalX.Transactions;
using WCA.Core.Features.GlobalX;
using WCA.Core.Features.GlobalX.Transactions;
using WCA.GlobalX.Client.Transactions;
using Xunit;
using static WCA.Core.Features.GlobalX.ValidateActionstepMatterCommand;

namespace WCA.UnitTests.AzureFunctions.GlobalX.Transactions
{
    /// <summary>
    /// Uses Moq because FakeItEasy doesn't support faking generic types with in parameters:
    /// </summary>
    public class TransactionSyncOrchestratorTests
    {
        private Transaction CreateTestTransaction() => new Transaction
        {
            TransactionId = 1,
            Matter = "2",
            OrderId = "OrderABC123",
            TransactionDateTime = new DateTimeOffset(2020, 1, 1, 1, 1, 0, TimeSpan.Zero),
            RetailPrice = 10,
            RetailGst = 1,
            WholesalePrice = 10,
            WholesaleGst = 1,
            SearchReference = "SearchReference",
            User = new User()
            {
                UserId = "TransactionUserId",
                CustomerRef = "TransactionUserCustomerRef"
            },
            Product = new Product()
            {
                ProductCode = "ProductCode",
                ProductDescription = "Product Description",
                ProductSubGroup = "Product Sub Group"
            }
        };

        [Fact]
        public async Task CanCreateDisbursements()
        {
            // Arrange
            var disbursementCreationDetails = new CreateDisbursementsCommand()
            {
                ActionstepMatterId = 1,
                ActionstepOrgKey = "OrgKey",
                TaxCodeIdNoGST = 2,
                TaxCodeIdWithGST = 3,
                Transaction = CreateTestTransaction(),
                ActionstepUserId = "4",
                MinimumMatterIdToSync = -1,
            };

            var durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();

            durableOrchestrationContextMock.Setup(d => d.GetInput<CreateDisbursementsCommand>()).Returns(disbursementCreationDetails);
            durableOrchestrationContextMock.Setup(d => d.InstanceId).Returns(GlobalXTransactionSyncOrchestrator.InstancePrefix + disbursementCreationDetails.Transaction.TransactionId.ToString(CultureInfo.InvariantCulture));
            durableOrchestrationContextMock.Setup(d => d.CallActivityAsync<TransactionDisbursementRelationship>(
                nameof(GlobalXTransactionSyncOrchestrator.CreateDisbursementsForTransaction),
                It.IsAny<CreateDisbursementsCommand>()))
            .Returns(Task.FromResult(new TransactionDisbursementRelationship(disbursementCreationDetails.Transaction.TransactionId, "OrgKey", 1, 3, 2)));

            durableOrchestrationContextMock.Setup(d => d.CallActivityAsync<ActionstepMatterValidationResult>(
                nameof(SharedActivities.ValidateActionstepMatter),
                It.IsAny<ValidateActionstepMatterCommand>()))
            .ReturnsAsync(new ActionstepMatterValidationResult(MatterIdStatus.Valid, 1));

            var mediatorMock = new Mock<IMediator>();
            var transactionSyncOrchestrator = new GlobalXTransactionSyncOrchestrator(mediatorMock.Object, NullLogger<GlobalXTransactionSyncOrchestrator>.Instance);

            // Act
            var result = await transactionSyncOrchestrator.Run(durableOrchestrationContextMock.Object);

            // Assert
            durableOrchestrationContextMock.Verify(d => d.SetCustomStatus(null), Times.Once);
            Assert.Equal(1, result.GlobalXTransactionId);
            Assert.Equal(2, result.GSTFreeDisbursementId);
            Assert.Equal(3, result.GSTTaxableDisbursementId);
        }

        /// <summary>
        /// Uses Moq because FakeItEasy doesn't support faking generic types with in parameters:
        /// https://github.com/FakeItEasy/FakeItEasy/issues/1382
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CanManuallyResumeFromCreateDisbursementsFailure()
        {
            // Arrange
            var disbursementCreationDetails = new CreateDisbursementsCommand()
            {
                ActionstepMatterId = 1,
                ActionstepOrgKey = "OrgKey",
                TaxCodeIdNoGST = 2,
                TaxCodeIdWithGST = 3,
                Transaction = CreateTestTransaction(),
                ActionstepUserId = "4"
            };

            var durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();

            durableOrchestrationContextMock.Setup(d => d.GetInput<CreateDisbursementsCommand>()).Returns(disbursementCreationDetails);
            durableOrchestrationContextMock.Setup(d => d.InstanceId).Returns(GlobalXTransactionSyncOrchestrator.InstancePrefix + disbursementCreationDetails.Transaction.TransactionId.ToString(CultureInfo.InvariantCulture));

            durableOrchestrationContextMock.Setup(d => d.CallActivityAsync<ActionstepMatterValidationResult>(
                nameof(SharedActivities.ValidateActionstepMatter),
                It.IsAny<ValidateActionstepMatterCommand>()))
            .ReturnsAsync(new ActionstepMatterValidationResult(MatterIdStatus.Valid, 1));

            // Fail on the first attempt. This will be fixed later and retried.
            durableOrchestrationContextMock.Setup(d => d.CallActivityAsync<TransactionDisbursementRelationship>(
                nameof(GlobalXTransactionSyncOrchestrator.CreateDisbursementsForTransaction),
                disbursementCreationDetails))
                .Throws<Exception>();

            var timerTask = new Task<object>(() => new object());
            var retryEventTask = new Task<object>(() => new object());

            // Uses Moq because FakeItEasy doesn't support faking generic types with in parameters:
            // https://github.com/FakeItEasy/FakeItEasy/issues/1382
            durableOrchestrationContextMock.Setup(d => d.CreateTimer<Object>(It.IsAny<DateTime>(), It.IsAny<It.IsAnyType>(), It.IsAny<CancellationToken>())).Returns(timerTask);
            durableOrchestrationContextMock.Setup(d => d.WaitForExternalEvent<Object>(Events.RetryFailedActivityEvent)).Returns(retryEventTask);

            var mediatorMock = new Mock<IMediator>();
            var transactionSyncOrchestrator = new GlobalXTransactionSyncOrchestrator(mediatorMock.Object, NullLogger<GlobalXTransactionSyncOrchestrator>.Instance);

            // Act
            var orchestrationTask = transactionSyncOrchestrator.Run(durableOrchestrationContextMock.Object);

            // - Update mock so it works now
            durableOrchestrationContextMock.Setup(d => d.CallActivityAsync<TransactionDisbursementRelationship>(
                nameof(GlobalXTransactionSyncOrchestrator.CreateDisbursementsForTransaction),
                It.IsAny<CreateDisbursementsCommand>()))
                .Returns(Task.FromResult(new TransactionDisbursementRelationship(disbursementCreationDetails.Transaction.TransactionId, "OrgKey", 1, 3, 2)));


            // - Simulate raising of retry event by completing retryEventTask
            retryEventTask.Start();

            // - Wait for orchestration to finish
            var result = await orchestrationTask;

            // Assert
            durableOrchestrationContextMock.Verify(d => d.SetCustomStatus("Exception of type 'System.Exception' was thrown."), Moq.Times.Once);
            durableOrchestrationContextMock.Verify(d => d.SetCustomStatus(null), Moq.Times.Once);
            Assert.Equal(1, result.GlobalXTransactionId);
            Assert.Equal(2, result.GSTFreeDisbursementId);
            Assert.Equal(3, result.GSTTaxableDisbursementId);
        }

        /// <summary>
        /// Uses Moq because FakeItEasy doesn't support faking generic types with in parameters:
        /// https://github.com/FakeItEasy/FakeItEasy/issues/1382
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AutoRetryRunsCorrectNumberOfTimes()
        {
            // === Arrange ===
            var disbursementCreationDetails = new CreateDisbursementsCommand()
            {
                ActionstepMatterId = 1,
                ActionstepOrgKey = "OrgKey",
                TaxCodeIdNoGST = 2,
                TaxCodeIdWithGST = 3,
                Transaction = CreateTestTransaction(),
                ActionstepUserId = "4",
                MinimumMatterIdToSync = -1,
            };

            var durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();

            durableOrchestrationContextMock.Setup(d => d.GetInput<CreateDisbursementsCommand>()).Returns(disbursementCreationDetails);
            durableOrchestrationContextMock.Setup(d => d.InstanceId).Returns(GlobalXTransactionSyncOrchestrator.InstancePrefix + disbursementCreationDetails.Transaction.TransactionId.ToString(CultureInfo.InvariantCulture));

            durableOrchestrationContextMock.Setup(d => d.CallActivityAsync<ActionstepMatterValidationResult>(
                nameof(SharedActivities.ValidateActionstepMatter),
                It.IsAny<ValidateActionstepMatterCommand>()))
            .ReturnsAsync(new ActionstepMatterValidationResult(MatterIdStatus.Valid, 1));

            // Fail until corrected. This will be fixed later and retried.
            durableOrchestrationContextMock.Setup(d => d.CallActivityAsync<TransactionDisbursementRelationship>(
                nameof(GlobalXTransactionSyncOrchestrator.CreateDisbursementsForTransaction),
                disbursementCreationDetails))
            .Throws<Exception>();

            var retryEventTask = new Task<object>(() => new object());

            // Uses Moq because FakeItEasy doesn't support faking generic types with in parameters:
            // https://github.com/FakeItEasy/FakeItEasy/issues/1382
            durableOrchestrationContextMock.Setup(d => d.CreateTimer<Object>(It.IsAny<DateTime>(), It.IsAny<It.IsAnyType>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new object()));
            durableOrchestrationContextMock.Setup(d => d.WaitForExternalEvent<Object>(Events.RetryFailedActivityEvent)).Returns(retryEventTask);

            var mediatorMock = new Mock<IMediator>();
            var transactionSyncOrchestrator = new GlobalXTransactionSyncOrchestrator(mediatorMock.Object, NullLogger<GlobalXTransactionSyncOrchestrator>.Instance);

            // === Act ===
            var orchestrationTask = transactionSyncOrchestrator.Run(durableOrchestrationContextMock.Object);

            // Update mock so it works now
            durableOrchestrationContextMock.Setup(d => d.CallActivityAsync<TransactionDisbursementRelationship>(
                nameof(GlobalXTransactionSyncOrchestrator.CreateDisbursementsForTransaction),
                It.IsAny<CreateDisbursementsCommand>()))
            .Returns(Task.FromResult(new TransactionDisbursementRelationship(disbursementCreationDetails.Transaction.TransactionId, "OrgKey", 1, 3, 2)));

            // Simulate raising of retry event by completing retryEventTask. This will complete the workflow.
            retryEventTask.Start();

            // Wait for orchestration to finish
            var result = await orchestrationTask;

            // === Assert ===
            // We expect the error to be thrown five times. Once from the initial failure, then four retries.
            const int expectedErrorCallCount = 5;

            durableOrchestrationContextMock.Verify(d => d.SetCustomStatus("Exception of type 'System.Exception' was thrown."), Times.Exactly(expectedErrorCallCount));
            durableOrchestrationContextMock.Verify(d => d.SetCustomStatus(null), Times.Once);
            Assert.Equal(1, result.GlobalXTransactionId);
            Assert.Equal(2, result.GSTFreeDisbursementId);
            Assert.Equal(3, result.GSTTaxableDisbursementId);
        }
    }
}
