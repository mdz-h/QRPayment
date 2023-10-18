namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    public class RolesDto
    {
        public int WorkgroupId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int Lcount { get; set; }
        public Guid CreatedByOperatorId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Guid? ModifiedByOperatorId { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
