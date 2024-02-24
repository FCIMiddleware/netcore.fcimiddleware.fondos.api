namespace netcore.fcimiddleware.fondos.domain.Common
{
    public abstract class BaseDomainModel
    {
        public int Id { get; set; }        
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool IsSincronized { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
