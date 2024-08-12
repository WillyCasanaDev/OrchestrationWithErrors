using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orchestrator.Core.Interfaces;
using Orchestrator.Infrastructure.Options;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

namespace Orchestrator.Infrastructure.Providers;

public class VaultSecretProvider : ISecretProvider
{
    private readonly VaultClient _vaultClient;
    private readonly VaultOptions _vaultOptions;
    private readonly ILogger _logger;

    public VaultSecretProvider(IOptions<VaultOptions> vaultOptions, ILogger<VaultSecretProvider> logger)
    {
        _logger = logger;
        _vaultOptions = vaultOptions.Value;
        IAuthMethodInfo authMethod = new TokenAuthMethodInfo(_vaultOptions.Token);
        var vaultClientSettings = new VaultClientSettings(_vaultOptions.Uri, authMethod);
        _vaultClient = new VaultClient(vaultClientSettings);
    }

    public async Task<IDictionary<string, object>> GetSecret()
    {
        try
        {
            var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
                path: $"{_vaultOptions.ApplicationName}/{_vaultOptions.Profile}", mountPoint: _vaultOptions.MountPoint);

            return secret.Data.Data;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading secret from vault");
            throw;
        }
    }
}