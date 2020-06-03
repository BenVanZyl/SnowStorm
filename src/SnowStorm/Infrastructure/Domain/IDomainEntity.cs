using System;

namespace SnowStorm.Infrastructure.Domain
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
        DateTime CreateDateTime { get; set; }
        DateTime ModifyDateTime { get; set; }
    }
}
