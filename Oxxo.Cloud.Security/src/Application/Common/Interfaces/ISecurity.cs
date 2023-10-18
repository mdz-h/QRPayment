using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Application.Common.Interfaces
{
    public interface ISecurity
    {
        /// <summary>
        /// Validate if the password is correct
        /// </summary>
        /// <param name="systemsparam">System parameters</param>
        /// <param name="password">Password</param>
        /// <returns>Return boolean value</returns>
        bool ValidatePassword(List<SystemParam> systemsparam, string password, bool applyExceptions);

        /// <summary>
        /// Build dynamic password
        /// </summary>
        /// <param name="systemsparam">System parameters</param>
        /// <returns>dynamic password</returns>
        string BuildPassword(List<SystemParam> systemsparam);

        /// <summary>
        /// Get a list of system parameters
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of System parameters</returns>
        Task<List<SystemParam>> GetSystemParamsPasswordRules(CancellationToken cancellationToken);

        /// <summary>
        /// This method validate if the password rules are correct
        /// </summary>
        /// <param name="lstSystemParam">System parameters</param>
        void ValidatePasswordRules(List<SystemParam> lstSystemParam);

        /// <summary>
        /// Get a list of system parameters to API key generate
        /// </summary>
        /// <returns>List of System parameters</returns>
        Task<List<SystemParam>> GetSystemParamsApiKeyRules(CancellationToken cancellationToken);

        /// <summary>
        /// This method validate if the API keys rules are correct
        /// </summary>
        /// <param name="lstSystemParam">System parameters</param>
        void ValidateApiKeyRules(List<SystemParam> lstSystemParam);

        /// <summary>
        /// This method generated the API Key
        /// </summary>
        /// <param name="lstSystemParam">System parameters</param>
        /// <returns>API Key</returns>
        string GenerateApiKey(List<SystemParam> lstSystemParam);
    }
}
