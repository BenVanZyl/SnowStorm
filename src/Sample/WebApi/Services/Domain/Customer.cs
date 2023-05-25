using SnowStorm.Domain;

namespace WebApi.Services.Domain
{
    public class Customer : IDomainEntity
    {
        public int CustomerID { get; set; }
    }
}
