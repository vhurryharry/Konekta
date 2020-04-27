using FluentValidation;
using MediatR;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WCA.Core.Features.Pexa.Authentication
{
    public class StorePexaApiTokenCommandHandler : AsyncRequestHandler<StorePexaApiTokenCommand>
    {
        private const string _credentialNamePrefix = "PEXA-API-token-for-user-";

        private readonly IKeyVaultClient _keyVaultClient;
        private readonly StorePexaApiTokenCommand.Validator _validator;
        private readonly WCACoreSettings _appSettings;

        public StorePexaApiTokenCommandHandler(
            IKeyVaultClient keyVaultClient,
            StorePexaApiTokenCommand.Validator validator,
            IOptions<WCACoreSettings> appSettingsAccessor)
        {
            _appSettings = appSettingsAccessor.Value;
            _keyVaultClient = keyVaultClient;
            _validator = validator;
        }

        protected override async Task Handle(StorePexaApiTokenCommand command, CancellationToken token)
        {
            await _validator.ValidateAndThrowAsync(command);
                
            await _keyVaultClient.SetSecretAsync(
                _appSettings.CredentialAzureKeyVaultUrl,
                $"{_credentialNamePrefix}{command.AuthenticatedUser.Id}",
                JsonConvert.SerializeObject(command.PexaApiToken));
        }
    }
}
