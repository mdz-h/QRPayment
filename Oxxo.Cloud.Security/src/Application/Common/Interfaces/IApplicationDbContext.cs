#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Interface application data base context.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Oxxo.Cloud.Security.Domain.Common.Models;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Device> DEVICE { get; }
    DbSet<DeviceNumber> DEVICE_NUMBER { get; }
    DbSet<DeviceStatus> DEVICE_STATUS { get; }
    DbSet<SessionToken> SESSION_TOKEN { get; }
    DbSet<StorePlace> STORE_PLACE { get; }
    DbSet<Operator> OPERATOR { get; }
    DbSet<SessionStatus> SESSION_STATUS { get; }
    DbSet<SystemParam> SYSTEM_PARAM { get; }
    DbSet<ExternalApplication> EXTERNAL_APPLICATION { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DatabaseFacade database { get; }
    DbSet<TokenPermission> SpResult { get; set; }

    DbSet<Workgroup> WORKGROUP { get; set; }
    DbSet<UserWorkgroupLink> USER_WORKGROUP_LINK { get; set; }

    DbSet<Permission> PERMISSION { get; }
    DbSet<PermissionType> PERMISSION_TYPE { get; }
    DbSet<Module> MODULE { get; }
    DbSet<DeviceType> DEVICE_TYPE { get; }

    DbSet<ValidIPRange> VALID_IP_RANGE { get; }
    DbSet<WorkgroupPermissionLink> WORKGROUP_PERMISSION_LINK { get; set; }
    DbSet<OperatorStatus> OPERATOR_STATUS { get; }
    DbSet<Person> PERSON { get; }
    DbSet<WorkgroupPermissionStoreLink> WORKGROUP_PERMISSION_STORE_LINK { get; }

    DbSet<OperatorPassword> OPERATOR_PASSWORD { get; }
    DbSet<ApiKey> API_KEY { get; }
    DbSet<PermissionFrontEnd> PERMISSION_FRONTEND { get; }
    DbSet<OperatorStoreLink> OPERATOR_STORE_LINK { get; }
}
