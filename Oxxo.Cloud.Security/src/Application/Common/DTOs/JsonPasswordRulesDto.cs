namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    /// <summary>
    /// DTO class
    /// </summary>
    public class JsonPasswordRulesDto
    {
        /// <summary>
        /// Constructor class
        /// </summary>
        public JsonPasswordRulesDto()
        {
            Expressions = new List<string>();
        }

        public int LimitExpressionOK { get; set; }
        public List<string> Expressions { get; set; }
    }
}
