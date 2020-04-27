using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using WCA.Actionstep.Client;
using WCA.Core;
using WCA.Core.Features.InfoTrack;
using WCA.Core.Services;
using WCA.Data;
using WCA.Domain.InfoTrack;
using WCA.UnitTests.TestInfrastructure;
using Xunit;

namespace WCA.UnitTests.InfoTrack
{
    [Collection(WebContainerCollection.WebContainerCollectionName)]
    public class UpdateOrderTests
    {
        private readonly WebContainerFixture _webContainerFixture;

        public UpdateOrderTests(WebContainerFixture webContainerFixture)
        {
            _webContainerFixture = webContainerFixture;
        }

        [Fact]
        public void UpdateOrderFeeValidationTests()
        {
             _webContainerFixture.ExecuteScope(serviceProvider =>
            {
                var actionStepService = new TestActionstepService();
                var mediator = serviceProvider.GetService<IMediator>();
                var dbContext = serviceProvider.GetService<WCADbContext>();
                var telemetryLogger = serviceProvider.GetService<ITelemetryLogger>();
                var mapper = serviceProvider.GetService<IMapper>();
                var infoTrackCredentialRepository = new TestInfoTrackCredentialRepository();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                var appSettings = serviceProvider.GetService<IOptions<WCACoreSettings>>();
                var tokenSetRepository = serviceProvider.GetService<ITokenSetRepository>();

                var handler = new UpdateOrder.Handler(
                    actionStepService,
                    mediator,
                    dbContext,
                    mapper,
                    telemetryLogger,
                    infoTrackCredentialRepository,
                    loggerFactory,
                    appSettings,
                    tokenSetRepository);

                var updateMessage = new InfoTrackOrderUpdateMessage
                {
                    InfoTrackRetailerFee = 1.0m,
                    InfoTrackRetailerFeeGST = 1.0m,
                    InfoTrackRetailerFeeTotal = 2.0m,
                    InfoTrackSupplierFee = 1.5m,
                    InfoTrackSupplierFeeGST = 1.0m,
                    InfoTrackSupplierFeeTotal = 2.5m,
                    InfoTrackTotalFee = 2.5m,
                    InfoTrackTotalFeeGST = 2.0m,
                    InfoTrackTotalFeeTotal = 4.5m
                };

                var handleMethod = handler.GetType().GetMethod("ValidateFees", BindingFlags.NonPublic | BindingFlags.Instance);

                // Initial data valid
                Assert.True(InvokeFeeValidation(handleMethod, handler, updateMessage));

                updateMessage.InfoTrackRetailerFeeTotal = 2.1m;
                Assert.False(InvokeFeeValidation(handleMethod, handler, updateMessage));

                updateMessage.InfoTrackRetailerFeeTotal = 2.0m;
                updateMessage.InfoTrackSupplierFeeTotal = 2.4m;
                Assert.False(InvokeFeeValidation(handleMethod, handler, updateMessage));

                updateMessage.InfoTrackSupplierFeeTotal = 2.5m;
                updateMessage.InfoTrackTotalFeeTotal = 4.6m;
                Assert.False(InvokeFeeValidation(handleMethod, handler, updateMessage));

                // Reverted all values, should be valid again
                updateMessage.InfoTrackTotalFeeTotal = 4.5m;
                Assert.True(InvokeFeeValidation(handleMethod, handler, updateMessage));

            });

        }

        private Boolean InvokeFeeValidation(MethodInfo methodInfo, UpdateOrder.Handler handler, InfoTrackOrderUpdateMessage message)
        {
            try
            {
                methodInfo.Invoke(handler, new object[] { message });
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return false;
            }

            return true;
        }
    }
}
