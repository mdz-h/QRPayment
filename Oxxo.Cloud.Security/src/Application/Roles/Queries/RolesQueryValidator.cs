#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-28.
// Comment: Class Roles query validator.
//===============================================================================
#endregion
using FluentValidation;
using Oxxo.Cloud.Security.Domain.Consts;
using System.Net;

namespace Oxxo.Cloud.Security.Application.Roles.Queries
{
    public class RolesQueryValidator : AbstractValidator<RolesQuery>
    {
        public RolesQueryValidator()
        {
            RuleFor(x => x.Skip - 1).GreaterThanOrEqualTo(0).WithMessage(GlobalConstantMessages.WORKGROUPEMPTYGETSKIP).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
            RuleFor(x => x.Take).GreaterThan(0).WithMessage(GlobalConstantMessages.WORKGROUPEMPTYGETTAKE).WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());
        }
    }
}
