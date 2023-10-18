#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristain (NEORIS).
// Date:    2022-12-22.
// Comment: Class for queries to Change Password.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using Oxxo.Cloud.Security.Infrastructure.Extensions;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.ChangePassword
{
    public class ChangePassword : IChangePassword
    {
        private readonly IApplicationDbContext context;
        private readonly ISecurity security;
        private readonly ICryptographyService cryptographyService;

        /// <summary>
        /// Constructor that injects the database context, security and cryptographyService.  
        /// </summary>
        /// <param name="context">Context database</param>  
        /// <param name="security">Interface security</param>  
        /// <param name="cryptographyService">Interface cryptographyService</param>  
        public ChangePassword(IApplicationDbContext context, ISecurity security, ICryptographyService cryptographyService)
        {
            this.context = context;
            this.security = security;
            this.cryptographyService = cryptographyService;
        }

        /// <summary>
        /// This function is responsible that execute change password for Administrators
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        public async Task<bool> ChangePasswordAdministratorAsync(ChangePasswordCommand request, CancellationToken cancellationToken)
        {

            OperatorPassword? operator_password = await context.OPERATOR_PASSWORD.Include(o => o.OPERATOR)
                      .FirstOrDefaultWithNoLockAsync(o => o.OPERATOR_ID == Guid.Parse(request.UserId) && o.PASSWORD == cryptographyService.EncryptHash(request.OldPassword)
                      && o.OPERATOR.IS_ACTIVE && o.IS_ACTIVE, cancellationToken);

            if (operator_password is null)
            {
                return false;
            }

            operator_password.IS_ACTIVE = false;
            operator_password.LCOUNT += 1;
            operator_password = new OperatorPassword()
            {
                OPERATOR_ID = Guid.Parse(request.UserId),
                PASSWORD = cryptographyService.EncryptHash(request.NewPassword),
                EXPIRATION_TIME = await GetExpirationTimeAsync(cancellationToken),
                IS_ACTIVE = true,
                LCOUNT = GlobalConstantHelpers.ZERO,
                CREATED_BY_OPERATOR_ID = Guid.Parse(request.Identification),
                CREATED_DATETIME = DateTime.UtcNow
            };

            await context.OPERATOR_PASSWORD.AddAsync(operator_password, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// This function is responsible that validate if exists a Administrator
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        public async Task<bool> ValidateExistsAdministratorAsync(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return await context.OPERATOR.AsNoTracking().AnyWithNoLockAsync(o => o.OPERATOR_ID == Guid.Parse(request.UserId) && o.IS_ACTIVE, cancellationToken);
        }

        /// <summary>
        /// This function is responsible that validate if password is equal to the current password
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        public async Task<bool> ValidateIfPasswordIsEqualToTheCurrentPassword(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return await context.OPERATOR_PASSWORD.AsNoTracking().Include(o => o.OPERATOR)
                       .AnyWithNoLockAsync(o => o.OPERATOR_ID == Guid.Parse(request.UserId)
                        && o.PASSWORD == cryptographyService.EncryptHash(request.OldPassword)
                        && o.OPERATOR.IS_ACTIVE && o.IS_ACTIVE, cancellationToken);
        }

        /// <summary>
        /// This function is responsible for validating that the new password complies with all the rules.
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        public async Task<bool> ValidateNewPassword(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            List<string> codes_system_param = GlobalConstantHelpers.SYSTEMPARAMSPASSWORDRULES.Split(',').ToList();
            var config = await context.SYSTEM_PARAM.ToListWithNoLockAsync(w => codes_system_param.Contains((w.PARAM_CODE ?? string.Empty).ToUpper()), cancellationToken);
            return security.ValidatePassword(config, request.NewPassword, true);
        }

        /// <summary>
        /// This function is responsible that validate that the new password not is equal in the last passwords
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        public async Task<bool> ValidateNewPasswordWithTheLastPasswords(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var newPassword = cryptographyService.EncryptHash(request.NewPassword);
            var lastPasswords = await context.OPERATOR_PASSWORD.AsNoTracking()
                        .Include(o => o.OPERATOR)
                        .Where(o => o.OPERATOR_ID == Guid.Parse(request.UserId)
                         && o.OPERATOR.IS_ACTIVE)
                        .OrderByDescending(o => o.CREATED_DATETIME)
                        .Take(GlobalConstantHelpers.NUMBEROFRECORDSFORVALIDATELASTPASSWORDS)
                        .ToListAsync(cancellationToken);
            return !lastPasswords.Any(p => p.PASSWORD == newPassword);
        }

        /// <summary>
        /// This function is responsible that add Days to a Date and time current
        /// </summary>
        /// <param name="request"></param>        
        /// <param name="cancellationToken"></param>        
        /// <returns>Bolean</returns>
        private async Task<DateTime> GetExpirationTimeAsync(CancellationToken cancellationToken)
        {
            SystemParam systemParams = await context.SYSTEM_PARAM.AsNoTracking()
                        .FirstOrDefaultWithNoLockAsync(system_param => system_param.PARAM_CODE == GlobalConstantHelpers.PARAMEXPIRATIONTIME, cancellationToken);

            int iDays = 0;
            if (systemParams != null)
            {
                iDays = Convert.ToInt32(systemParams.PARAM_VALUE);
            }

            return DateTime.UtcNow.AddDays(iDays);
        }
    }
}
