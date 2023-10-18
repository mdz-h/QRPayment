namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    public class ExternalAppApiKeyDto
    {
        public ExternalAppApiKeyDto() 
        {
            Api_Key = string.Empty;
        }

        public int Api_Key_Id { get; set; }
        public Guid External_application_Id { get; set; }
        public string Api_Key { get; set; }
        public DateTime Expiration_Time { get; set; }
        public bool Is_Active { get; set; }
        public int Lcount { get; set; }
        public Guid Created_By_Operator_Id { get; set; }
        public DateTime Created_DateTime { get; set; }
        public Guid? Modified_By_Operator_Id { get; set; }
        public DateTime? Modified_DateTime { get; set; }
    }
}
