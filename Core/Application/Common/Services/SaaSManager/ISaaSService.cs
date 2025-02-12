namespace Application.Common.Services.SaaSManager;

public interface ISaaSService
{

    public Task InitTenantAsync(CancellationToken cancellationToken = default);
}
