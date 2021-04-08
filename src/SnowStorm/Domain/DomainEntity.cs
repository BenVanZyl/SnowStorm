using System;

namespace SnowStorm.Domain
{
    public class DomainEntity : IDomainEntity
    {
    }

    public class DomainEntityWithId : IDomainEntity
    {
        public long Id { get; private set; }
    }

    public class DomainEntityWithIdWithAudit : DomainEntityWithId
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
