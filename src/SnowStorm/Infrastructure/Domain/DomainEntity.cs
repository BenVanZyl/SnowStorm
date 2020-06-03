using System;

namespace SnowStorm.Infrastructure.Domain
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
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
    }
}
