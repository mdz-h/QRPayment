#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Class application data base context.
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Common.Models;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;
using System.Reflection;
using Module = Oxxo.Cloud.Security.Domain.Entities.Module;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;

    }

    public DbSet<Device> DEVICE => Set<Device>();
    public DbSet<DeviceNumber> DEVICE_NUMBER => Set<DeviceNumber>();
    public DbSet<DeviceStatus> DEVICE_STATUS => Set<DeviceStatus>();
    public DbSet<SessionToken> SESSION_TOKEN => Set<SessionToken>();
    public DbSet<SystemParam> SYSTEM_PARAM => Set<SystemParam>();
    public DbSet<StorePlace> STORE_PLACE => Set<StorePlace>();
    public DbSet<Operator> OPERATOR => Set<Operator>();
    public DbSet<SessionStatus> SESSION_STATUS => Set<SessionStatus>();
    public DbSet<ExternalApplication> EXTERNAL_APPLICATION => Set<ExternalApplication>();
    public DbSet<Permission> PERMISSION => Set<Permission>();
    public DbSet<PermissionType> PERMISSION_TYPE => Set<PermissionType>();
    public DbSet<Module> MODULE => Set<Module>();
    public DbSet<TokenPermission> SpResult { get; set; }
    public DbSet<DeviceType> DEVICE_TYPE => Set<DeviceType>();
    DatabaseFacade IApplicationDbContext.database => base.Database;
    public DbSet<Workgroup> WORKGROUP { get; set; }
    public DbSet<UserWorkgroupLink> USER_WORKGROUP_LINK { get; set; }
    public DbSet<WorkgroupPermissionLink> WORKGROUP_PERMISSION_LINK { get; set; }
    public DbSet<OperatorStatus> OPERATOR_STATUS => Set<OperatorStatus>();
    public DbSet<Person> PERSON => Set<Person>();
    public DbSet<WorkgroupPermissionStoreLink> WORKGROUP_PERMISSION_STORE_LINK => Set<WorkgroupPermissionStoreLink>();
    public DbSet<ValidIPRange> VALID_IP_RANGE => Set<ValidIPRange>();
    public DbSet<OperatorPassword> OPERATOR_PASSWORD => Set<OperatorPassword>();
    public DbSet<ApiKey> API_KEY => Set<ApiKey>();
    public DbSet<PermissionFrontEnd> PERMISSION_FRONTEND => Set<PermissionFrontEnd>();
    public DbSet<OperatorStoreLink> OPERATOR_STORE_LINK => Set<OperatorStoreLink>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
