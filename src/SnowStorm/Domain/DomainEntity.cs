using System;

namespace SnowStorm.Domain
{
    public class DomainEntity : IDomainEntity
    {
    }

    public class DomainEntityWithId : IDomainEntityWithId
    {
        public long Id { get; set; }
    }

    public class DomainEntityWithIdWithAudit : DomainEntityWithId
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
