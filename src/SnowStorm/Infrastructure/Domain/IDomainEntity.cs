using System;

namespace SnowStorm.Infrastructure.Domain
{
    public interface IDomainEntity
    {
    }


    public interface IDomainEntityWithId : IDomainEntity
    {
        int Id { get; }

        string CreateUserId { get; set; }
        DateTime CreateDateTime { get; set; }
        string ModifyUserId { get; set; }
        DateTime ModifyDateTime { get; set; }
    }
}
