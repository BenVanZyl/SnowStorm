using System;

namespace SnowStorm.Domain
{
    public class DomainEntity : IDomainEntity
    {
    }

    public class DomainEntityWithAudit : DomainEntity
    {
        public DateTime CreatedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }

        public virtual void SetCreatedOn() => CreatedOn = DateTime.Now;

        public virtual void SetModifiedOn() => ModifiedOn = DateTime.Now;
    }

    public class DomainEntityWithIdString : DomainEntity
    {
        public string Id { get; set; }
    }

    public class DomainEntityWithIdInt : DomainEntity
    {
        public int Id { get; set; }
    }

    public class DomainEntityWithId : DomainEntity
    {
        public long Id { get; set; }
    }

    public class DomainEntityWithIdWithAudit : DomainEntityWithId
    {
        public DateTime CreatedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }

        public virtual void SetCreatedOn() => CreatedOn = DateTime.Now;

        public virtual void SetModifiedOn() => ModifiedOn = DateTime.Now;
    }

}