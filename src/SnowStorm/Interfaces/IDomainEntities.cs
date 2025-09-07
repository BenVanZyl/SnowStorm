using System;

namespace SnowStorm.Interfaces
{
    public interface IDomainEntity
    {
    }

    public interface IAuditingCreate
    {
        DateTime CreatedOn { get; set; }
        void SetCreatedOn();
    }

    public interface IAuditingUpdate : IAuditingCreate
    {
        DateTime ModifiedOn { get; set; }
        void SetModifiedOn();
    }
}
