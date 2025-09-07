using System;

namespace SnowStorm.Domain
{
    public class DomainEntityWithAudit : DomainEntity
    {
        public DateTime CreatedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }

        public virtual void SetCreatedOn() => CreatedOn = DateTime.Now;

        public virtual void SetModifiedOn() => ModifiedOn = DateTime.Now;
    }

    public class DomainEntityWithIdWithAudit : DomainEntityWithId
    {
        public DateTime CreatedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }

        public virtual void SetCreatedOn() => CreatedOn = DateTime.Now;

        public virtual void SetModifiedOn() => ModifiedOn = DateTime.Now;
    }

    public class DomainEntityWithIdWithAuditUserId : DomainEntityWithIdWithAudit
    {
        public long CreatedBy { get; private set; }
        public long ModifiedBy { get; private set; }

        public override void SetCreatedOn()
        {
            base.SetCreatedOn();
            CreatedBy = Container.GetCurrentUserId();
        }

        public override void SetModifiedOn()
        {
            base.SetModifiedOn();
            ModifiedBy = Container.GetCurrentUserId();
        }
    }

    public class DomainEntityWithIdWithAuditUserName : DomainEntityWithIdWithAudit
    {
        public string CreatedBy { get; private set; }
        public string ModifiedBy { get; private set; }

        public override void SetCreatedOn()
        {
            base.SetCreatedOn();
            CreatedBy = Container.GetCurrentUserName();
        }

        public override void SetModifiedOn()
        {
            base.SetModifiedOn();
            ModifiedBy = Container.GetCurrentUserName();
        }
    }

    public class DomainEntityWithIdWithAuditAspNetUserGuid : DomainEntityWithIdWithAudit
    {
        public string CreatedBy { get; private set; }
        public string ModifiedBy { get; private set; }

        public override void SetCreatedOn()
        {
            base.SetCreatedOn();
            CreatedBy = Container.GetCurrentAspNetUserGuid();
        }

        public override void SetModifiedOn()
        {
            base.SetModifiedOn();
            ModifiedBy = Container.GetCurrentAspNetUserGuid();
        }
    }
}