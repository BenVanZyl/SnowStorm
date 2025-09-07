using SnowStorm.Interfaces;
using System;

namespace SnowStorm
{
    public class DomainEntity : IDomainEntity
    { }

    public class DomainEntityWithId : DomainEntity
    {
        public long Id { get; set; }
    }

    public class DomainEntityWithIdString : DomainEntity
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DomainEntityWithIdWithAudit : DomainEntityWithId, IAuditingUpdate
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual void SetCreatedOn() => CreatedOn = DateTime.Now;

        public virtual void SetModifiedOn() => ModifiedOn = DateTime.Now;
    }

    public class DomainEntityWithIdStringWithAudit : DomainEntityWithIdString, IAuditingUpdate
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual void SetCreatedOn() => CreatedOn = DateTime.Now;

        public virtual void SetModifiedOn() => ModifiedOn = DateTime.Now;
    }
}