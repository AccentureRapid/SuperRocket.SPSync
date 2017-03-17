using System.Threading.Tasks;
using Abp.Application.Services;
using SuperRocket.Orchard.Roles.Dto;

namespace SuperRocket.Orchard.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
