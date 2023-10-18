#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    07/12/2022.
// Comment: Administrator.
//===============================================================================
#endregion
using MediatR;
using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Administrators.Commands.UpdateAdministrators
{
    /// <summary>
    /// Principal Class
    /// </summary>
    public class UpdateAdministratorsCommand : BasePropertiesDto, IRequest<bool>
    {
        public UpdateAdministratorsCommand()
        {
            UserId = string.Empty;
            NickName = string.Empty;
            UserName = string.Empty;
            LastNamePat = string.Empty;
            LastNameMat = string.Empty;
            MiddleName = string.Empty;
            Email = string.Empty;
        }

        public string UserId { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string LastNamePat { get; set; }
        public string LastNameMat { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// Update Process
    /// </summary>
    public class UpdateAdministratorsHandler : IRequestHandler<UpdateAdministratorsCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ILogService logService;

        /// <summary>
        /// Constructor that injects the DeleteAdministratorsHandler.
        /// </summary>
        /// <param name="logService">"Inject" the log instance</param>
        /// <param name="context">Inject context</param>
        public UpdateAdministratorsHandler(ILogService logService, IApplicationDbContext context)
        {
            this.logService = logService;
            this.context = context;
        }

        /// <summary>
        /// Principal method
        /// </summary>
        /// <param name="request">Request App</param>
        /// <param name="cancellationToken">CancelationToken</param>
        /// <returns>logic value to indicated the result to operation</returns>
        public async Task<bool> Handle(UpdateAdministratorsCommand request, CancellationToken cancellationToken)
        {
            bool result = false;

            try
            {
               var Operator = await context.OPERATOR
                                        .Include(i => i.PERSON)
                                        .Where(w => w.OPERATOR_ID.ToString() == request.UserId)
                                        .FirstAsync(cancellationToken);

                if (request.IsActive ?? false)
                {
                    #region [OPERATOR UPDATE]
                    Operator.USER_NAME = request.NickName;
                    #endregion

                    #region [PERSON UPDATE]
                    Operator.PERSON.NAME = request.UserName;
                    Operator.PERSON.MIDDLE_NAME = request.MiddleName;
                    Operator.PERSON.LASTNAME_PAT = request.LastNamePat;
                    Operator.PERSON.LASTNAME_MAT = request.LastNameMat;
                    Operator.PERSON.EMAIL = request.Email;
                    Operator.PERSON.LCOUNT++;
                    #endregion
                }

                Operator.LCOUNT++;
                Operator.IS_ACTIVE = request.IsActive ?? false;
                Operator.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                Operator.MODIFIED_DATETIME = DateTime.UtcNow;
                Operator.PERSON.MODIFIED_BY_OPERATOR_ID = Guid.Parse(request.Identification);
                Operator.PERSON.MODIFIED_DATETIME = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);               
                result = true;
            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.EVENTMETHODADMINISTRATORS
                    , GlobalConstantHelpers.METHOADMINISTRATORHANDLERUPDATE
                    , LogTypeEnum.Error
                    , request.UserIdentification
                    , string.Concat(GlobalConstantMessages.LOGERRORADMINISTRATORSAPIUPDATE, ex.GetException())
                    , GlobalConstantHelpers.NAMECLASSADMINISTRATORCOMMANDUPDATE);
                throw;
            }

            return result;
        }
    }

}
