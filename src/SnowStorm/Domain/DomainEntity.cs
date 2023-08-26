using System;

namespace SnowStorm.Domain
{
    public class DomainEntity : IDomainEntity
    {
    }

    public class DomainEntityWithAudit : DomainEntity    {
        public DateTime CreatedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }

        public virtual void SetCreatedOn()
        {
            CreatedOn = DateTime.Now;
        }
        public virtual void SetModifiedOn()
        {
            ModifiedOn = DateTime.Now;
        }
    }

    public class DomainEntityWithId : DomainEntity
    {
        public long Id { get; set; }
    }

    public class DomainEntityWithIdWithAudit : DomainEntityWithId
    {
        public DateTime CreatedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }

        public virtual void SetCreatedOn()
        {
            CreatedOn = DateTime.Now;
        }
        public virtual void SetModifiedOn()
        {
            ModifiedOn = DateTime.Now;
        }
    }

    public class DomainEntityWithIdWithAuditUserId : DomainEntityWithIdWithAudit
    {
        public long CreatedBy { get; private set; }
        public long ModifiedBy { get; private set; }

        public override void SetCreatedOn()
        {
            base.SetCreatedOn();
            CreatedBy = -1; //todo: get from current user in di container
        }
        public override void SetModifiedOn()
        {
            base.SetModifiedOn();
            ModifiedBy = -1; //todo: get from current user in di container
        }
    }

    public class DomainEntityWithIdWithAuditUserName : DomainEntityWithIdWithAudit
    {
        public string CreatedBy { get; private set; }
        public string ModifiedBy { get; private set; }

        public override void SetCreatedOn()
        {
            base.SetCreatedOn();
            CreatedBy = "SYSTEM"; //todo: get from current user in di container
        }
        public override void SetModifiedOn()
        {
            base.SetModifiedOn();
            ModifiedBy = "SYSTEM"; //todo: get from current user in di container
        }
    }
}
