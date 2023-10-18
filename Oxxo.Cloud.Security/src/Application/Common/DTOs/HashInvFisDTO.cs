using Oxxo.Cloud.Security.Domain.Enum;

namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    public class HashInvFisDTO
    {
        /// <summary>
        /// Current CR from store
        /// </summary>
        public string CrStore { get; set; } = string.Empty;
        /// <summary>
        /// identification number of auditor to login
        /// </summary>
        public int Auditor { get; set; }
        /// <summary>
        /// Represent the previus admin date from a previus inventory
        /// </summary>
        public DateTime PreviusAdminDate { get; set; }
        /// <summary>
        /// Determine the kind of inventory type of a previus Inventory
        /// </summary>
        public InvType PreviusInvType { get; set; }
        /// <summary>
        /// Previus inventory status
        /// </summary>
        public bool IsPreviusInvSuccess { get; set; }
        /// <summary>
        /// Current admin date or last admin date register
        /// </summary>
        public DateTime AdminDate { get; set; }
        /// <summary>
        /// represents a local getDate from store
        /// </summary>
        public DateTime CurrentStoreDate { get; set; }
        /// <summary>
        /// Optional parameter, comes from login combo selection
        /// </summary>
        public string InvType { get; set; } = string.Empty;
        /// <summary>
        /// Auditor password
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// Determine id the store has a previus Inventory
        /// </summary>
        public bool HasPrevius { get; set; }
        /// <summary>
        /// Flag = EncendidoRMS16 from XposStore
        /// </summary>
        public bool ActivatedCore { get; set; }

    }
}
