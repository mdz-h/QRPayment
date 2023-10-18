using Microsoft.EntityFrameworkCore;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Entities;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Oxxo.Cloud.Security.Application.Common.Security
{
    /// <summary>
    /// This class contains generic methods to process an API Key and dynamic password
    /// </summary>
    public class Security : ISecurity
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Constructor class (Inject objects)
        /// </summary>
        /// <param name="context">Database Context</param>
        public Security(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Validate if the password is correct
        /// </summary>
        /// <param name="systemsparam">System parameters</param>
        /// <param name="applyExceptions">Apply Exceptions is the value is true the exception exec</param>
        public bool ValidatePassword(List<SystemParam> systemsparam, string password, bool applyExceptions)
        {
            List<string> Errors = new();
            bool isValid = true;
            JsonPasswordRulesDto objRules = BuildRegex(systemsparam);
            int iCont = 0;
            objRules.Expressions.ForEach(expres =>
            {
                var reg = new Regex(expres);
                reg.GetGroupNumbers();

                if (reg.IsMatch(password))
                {
                    iCont++;
                }
                else
                {
                    Errors.Add(string.Format(GlobalConstantMessages.PASSWORDMESSAGEERORS, expres));
                }
            });


            if (iCont < objRules.LimitExpressionOK)
            {
                isValid = false;
                Errors.Add(string.Format(GlobalConstantMessages.PASSWORRULES, iCont, objRules.LimitExpressionOK));
            }

            if (applyExceptions && Errors.Any())
            {
                throw new CustomException(string.Join(", ", Errors), HttpStatusCode.UnprocessableEntity);
            }

            return isValid;
        }

        /// <summary>
        /// Build dynamic password
        /// </summary>
        /// <param name="systemsparam">System parameters</param>
        /// <returns>dynamic password</returns>
        public string BuildPassword(List<SystemParam> systemsparam)
        {
            string chars = systemsparam.Where(W => W.PARAM_CODE == GlobalConstantHelpers.PASSWORDRANDOMLETTERS).Select(s => s.PARAM_VALUE).FirstOrDefault() ?? string.Empty;
            string iMin = systemsparam.Where(w => w.PARAM_CODE == GlobalConstantHelpers.PASSWORDMINLENGTHRULES).Select(s => s.PARAM_VALUE).FirstOrDefault() ?? string.Empty;
            string iMax = systemsparam.Where(w => w.PARAM_CODE == GlobalConstantHelpers.PASSWORDMAXLENGTHRULES).Select(s => s.PARAM_VALUE).FirstOrDefault() ?? string.Empty;

            string password = string.Empty;

            Random randomNnmber = new();
            StringBuilder randomStr = new();
            int len = randomNnmber.Next(Convert.ToInt32(iMin), Convert.ToInt32(iMax));


            for (int i = 0; i < len; i++)
            {
                int index = randomNnmber.Next(chars.Length);
                randomStr.Append(chars[index]);
            }

            password = randomStr.ToString();

            return password;
        }

        /// <summary>
        /// This method build the password rules
        /// </summary>
        /// <param name="systems">Systems parameters</param>
        /// <returns>Regex</returns>
        public JsonPasswordRulesDto BuildRegex(List<SystemParam> systems)
        {
            string passwordRules = systems.Where(W => W.PARAM_CODE == GlobalConstantHelpers.PASSWORDRULES).Select(s => s.PARAM_VALUE).FirstOrDefault() ?? string.Empty;

            var objRules = JsonSerializer.Deserialize<JsonPasswordRulesDto>(passwordRules) ?? new JsonPasswordRulesDto();
            return objRules;
        }

        /// <summary>
        /// Get a list of system parameters
        /// </summary>
        /// <returns>List of System parameters</returns>
        public async Task<List<SystemParam>> GetSystemParamsPasswordRules(CancellationToken cancellationToken)
        {
            List<string> lstSystemParams = GlobalConstantHelpers.SYSTEMPARAMSPASSWORDRULES.Split(',').ToList();
            var config = await context.SYSTEM_PARAM.Where(w => lstSystemParams.Contains((w.PARAM_CODE ?? "").ToUpper())).ToListAsync(cancellationToken);

            ValidatePasswordRules(config);

            return config;
        }

        /// <summary>
        /// This method validate if the password rules are correct
        /// </summary>
        /// <param name="lstSystemParam">System parameters</param>
        public void ValidatePasswordRules(List<SystemParam> lstSystemParam)
        {
            List<string> rules = new();

            if (!lstSystemParam.Any())
            {
                throw new CustomException(GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDRULES, HttpStatusCode.UnprocessableEntity);
            }

            if (!lstSystemParam.Any(w => (w.PARAM_CODE ?? string.Empty).ToUpper() == GlobalConstantHelpers.PASSWORDRANDOMLETTERS && !string.IsNullOrWhiteSpace(w.PARAM_VALUE)))
            {
                rules.Add(GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDRANDMOLETTER);
            }

            if (!lstSystemParam.Any(w => (w.PARAM_CODE ?? string.Empty).ToUpper() == GlobalConstantHelpers.PASSWORDRULES && !string.IsNullOrWhiteSpace(w.PARAM_VALUE)))
            {
                rules.Add(GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORD);
            }

            if (!lstSystemParam.Any(w => (w.PARAM_CODE ?? string.Empty).ToUpper() == GlobalConstantHelpers.PASSWORDMINLENGTHRULES && !string.IsNullOrWhiteSpace(w.PARAM_VALUE)))
            {
                rules.Add(GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDMIN);
            }

            if (!lstSystemParam.Any(w => (w.PARAM_CODE ?? string.Empty).ToUpper() == GlobalConstantHelpers.PASSWORDMAXLENGTHRULES && !string.IsNullOrWhiteSpace(w.PARAM_VALUE)))
            {
                rules.Add(GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDMAX);
            }

            if (!rules.Any())
            {
                int iMin = Convert.ToInt32(lstSystemParam.Where(w => w.PARAM_CODE == GlobalConstantHelpers.PASSWORDMINLENGTHRULES).Select(s => s.PARAM_VALUE).FirstOrDefault());
                int iMax = Convert.ToInt32(lstSystemParam.Where(w => w.PARAM_CODE == GlobalConstantHelpers.PASSWORDMAXLENGTHRULES).Select(s => s.PARAM_VALUE).FirstOrDefault());

                if (iMax < iMin)
                    rules.Add(GlobalConstantMessages.CONFIGSYSTEMPARAMPASSWORDMINMAX);
            }

            if (rules.Any())
            {
                throw new CustomException(string.Join(", ", rules), HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Get a list of system parameters to API key generate
        /// </summary>
        /// <returns>List of System parameters</returns>
        public async Task<List<SystemParam>> GetSystemParamsApiKeyRules(CancellationToken cancellationToken)
        {
            List<string> lstSystemParams = GlobalConstantHelpers.SYSTEMPARAMSAPIKEYRULES.Split(',').ToList();
            var config = await context.SYSTEM_PARAM.Where(w => lstSystemParams.Contains((w.PARAM_CODE ?? string.Empty).ToUpper())).ToListAsync(cancellationToken);

            ValidateApiKeyRules(config);

            return config;
        }

        /// <summary>
        /// This method validate if the API keys rules are correct
        /// </summary>
        /// <param name="lstSystemParam">System parameters</param>
        public void ValidateApiKeyRules(List<SystemParam> lstSystemParam)
        {
            List<string> rules = new();

            if (!lstSystemParam.Any())
            {
                throw new CustomException(GlobalConstantMessages.CONFIGSYSTEMPARAMAPIKEYSRULES, HttpStatusCode.UnprocessableEntity);
            }

            if (!lstSystemParam.Any(w => (w.PARAM_CODE ?? string.Empty).ToUpper() == GlobalConstantHelpers.APIKEYPREFIX && !string.IsNullOrWhiteSpace(w.PARAM_VALUE)))
            {
                rules.Add(string.Format(GlobalConstantMessages.CONFIGSYSTEMPARAMNOTFOUND, GlobalConstantHelpers.APIKEYPREFIX));
            }

            if (!lstSystemParam.Any(w => (w.PARAM_CODE ?? string.Empty).ToUpper() == GlobalConstantHelpers.APIKEYSECUREBYTES && !string.IsNullOrWhiteSpace(w.PARAM_VALUE) && int.TryParse(w.PARAM_VALUE, out int iResult) && iResult > 0))
            {
                rules.Add(string.Format(GlobalConstantMessages.CONFIGSYSTEMPARAMNOTFOUND, GlobalConstantHelpers.APIKEYSECUREBYTES));
            }

            if (!lstSystemParam.Any(w => (w.PARAM_CODE ?? string.Empty).ToUpper() == GlobalConstantHelpers.APIKEYEXPIRATIONTIME && !string.IsNullOrWhiteSpace(w.PARAM_VALUE)))
            {
                rules.Add(string.Format(GlobalConstantMessages.CONFIGSYSTEMPARAMNOTFOUND, GlobalConstantHelpers.APIKEYEXPIRATIONTIME));
            }

            if (rules.Any())
            {
                throw new CustomException(string.Join(", ", rules), HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// This method generated the API Key
        /// </summary>
        /// <param name="lstSystemParam">System parameters</param>
        /// <returns>API Key</returns>
        public string GenerateApiKey(List<SystemParam> lstSystemParam)
        {
            ValidateApiKeyRules(lstSystemParam);


            int legthbytes = lstSystemParam.Where(W => W.PARAM_CODE == GlobalConstantHelpers.APIKEYSECUREBYTES).Select(s => Convert.ToInt32(s.PARAM_VALUE)).FirstOrDefault();
            string prefix = lstSystemParam.Where(w => w.PARAM_CODE == GlobalConstantHelpers.APIKEYPREFIX).Select(s => s.PARAM_VALUE).FirstOrDefault() ?? string.Empty;
            string excludeSymbols = lstSystemParam.Where(w => w.PARAM_CODE == GlobalConstantHelpers.APIKEYEXCLUDESYMBOL).Select(s => s.PARAM_VALUE).FirstOrDefault() ?? string.Empty;
            List<string> symbols = excludeSymbols.Split(',').Where(w => !string.IsNullOrWhiteSpace(w)).ToList();

            var bytes = RandomNumberGenerator.GetBytes(legthbytes);

            string base64String = Convert.ToBase64String(bytes);

            symbols
                .ForEach(s =>
                {
                    base64String = base64String.Replace(s, string.Empty);
                });

            var keyLength = legthbytes - prefix!.Length;

            return prefix + base64String[..keyLength];
        }
    }
}
