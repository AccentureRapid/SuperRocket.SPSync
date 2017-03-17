using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SuperRocket.Orchard.MultiTenancy.Dto;

namespace SuperRocket.Orchard.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultDto<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
