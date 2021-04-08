using System;

namespace SnowStorm.Domain
{
    public interface IDomainEntity
    {
    }


    public interface IDomainEntityWithId : IDomainEntity
    {
        long Id { get; set; }
    }

    public interface IDomainEntityWithIdWithAudit : IDomainEntityWithId
    {
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; }
    }
}
