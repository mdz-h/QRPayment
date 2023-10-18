#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    14/12/2022.
// Comment: Query Administrators.
//===============================================================================
#endregion

using FluentValidation;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Administrators.Queries
{
    /// <summary>
    /// Validate class
    /// </summary>
    public class AdministratorsQueryByIdValidator : AbstractValidator<AdministratorsQueryById>
    {
        /// <summary>
        /// Constructor Class
        /// </summary> 
        /// <param name="authenticateQuery">Authenticate</param>
        /// <param name="logService">log service</param>
        /// <param name="tokenGenerator">Token</param>
        public AdministratorsQueryByIdValidator(IAuthenticateQuery authenticateQuery, ILogService logService, ITokenGenerator tokenGenerator)
        {
            RuleFor(x => x.UserId)
               .NotEmpty().WithMessage(string.Format(GlobalConstantMessages.FIELDEMPTY, "{PropertyName}")).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }
    }
}
