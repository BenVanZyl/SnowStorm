using System;

namespace SnowStorm.Infrastructure.Domain
{
    public class DomainEntity : IDomainEntity
    {
    }

    public class DomainEntityWithId : IDomainEntity
    {
        public int Id { get; private set; }
    }

    public class DomainEntityWithIdAudit : DomainEntityWithId
    {
        public string CreateUserId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string ModifyUserId { get; set; }
        public DateTime ModifyDateTime { get; set; }
    }
}
