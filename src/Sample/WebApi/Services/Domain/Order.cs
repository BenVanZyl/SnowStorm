using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnowStorm;
using SnowStorm.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Shared.Dto;

namespace WebApi.Services.Domain
{
    public class Order : IDomainEntity
    {
        protected Order() { }

        public int Id { get; private set; }

        public string? CustomerID { get; private set; }
        public int? EmployeeID { get; private set; }
        public DateTime? OrderDate { get; private set; }
        public DateTime? RequiredDate { get; private set; }
        public DateTime? ShippedDate { get; private set; }
        public int? ShipVia { get; private set; }
        public decimal? Freight { get; private set; }
        public string? ShipName { get; private set; }
        public string? ShipAddress { get; private set; }
        public string? ShipCity { get; private set; }
        public string? ShipRegion { get; private set; }
        public string? ShipPostalCode { get; private set; }
        public string? ShipCountry { get; private set; }

        //[ForeignKey("CustomerID")]
        //public Customer Customer { get; private set; }
        //[ForeignKey("EmployeeID")]
        //public Employee Employee { get; private set; }
        //[ForeignKey("ShipVia")]
        //public ShipVia ShipVia { get; private set; }


        #region Methods

        public static async Task<Order> Create(OrderDto data, bool autoSave = true)
        {
            if (data == null)
                throw new ArgumentNullException("Create Failed due to missing data!: Orders");

            var dataContext = Container.GetAppDbContext();

            var v = new Order(data);
            await dataContext.Add<Order>(v);

            if (autoSave)
                await dataContext.Save();

            return v;
        }

        private Order(OrderDto data)
        {
            Save(data);
        }

        public void Save(OrderDto data)
        {
            SetCustomerID(data.CustomerID);
            SetEmployeeID(data.EmployeeID);
            SetOrderDate(data.OrderDate);
            SetRequiredDate(data.RequiredDate);
            SetShippedDate(data.ShippedDate);
            SetShipVia(data.ShipVia);
            SetFreight(data.Freight);
            SetShipName(data.ShipName);
            SetShipAddress(data.ShipAddress);
            SetShipCity(data.ShipCity);
            SetShipRegion(data.ShipRegion);
            SetShipPostalCode(data.ShipPostalCode);
            SetShipCountry(data.ShipCountry);

        }

        public void SetCustomerID(string? v)
        {
            if (CustomerID != v)
                CustomerID = v;
        }

        public void SetEmployeeID(int? v)
        {
            if (EmployeeID != v)
                EmployeeID = v;
        }

        public void SetOrderDate(DateTime? v)
        {
            if (OrderDate != v)
                OrderDate = v;
        }

        public void SetRequiredDate(DateTime? v)
        {
            if (RequiredDate != v)
                RequiredDate = v;
        }

        public void SetShippedDate(DateTime? v)
        {
            if (ShippedDate != v)
                ShippedDate = v;
        }

        public void SetShipVia(int? v)
        {
            if (ShipVia != v)
                ShipVia = v;
        }

        public void SetFreight(decimal? v)
        {
            if (Freight != v)
                Freight = v;
        }

        public void SetShipName(string? v)
        {
            if (ShipName != v)
                ShipName = v;
        }

        public void SetShipAddress(string? v)
        {
            if (ShipAddress != v)
                ShipAddress = v;
        }

        public void SetShipCity(string? v)
        {
            if (ShipCity != v)
                ShipCity = v;
        }

        public void SetShipRegion(string? v)
        {
            if (ShipRegion != v)
                ShipRegion = v;
        }

        public void SetShipPostalCode(string? v)
        {
            if (ShipPostalCode != v)
                ShipPostalCode = v;
        }

        public void SetShipCountry(string? v)
        {
            if (ShipCountry != v)
                ShipCountry = v;
        }


        #endregion Methods


        #region Configuration
        internal class Mapping : IEntityTypeConfiguration<Order>
        {
            public void Configure(EntityTypeBuilder<Order> builder)
            {
                builder.ToTable("Orders", "dbo");
                builder.HasKey(u => u.Id);  // PK.
                builder.Property(p => p.Id).HasColumnName("OrderID");

                builder.Property(p => p.CustomerID).HasMaxLength(5);
                builder.Property(p => p.OrderDate);
                builder.Property(p => p.RequiredDate);
                builder.Property(p => p.ShippedDate);
                builder.Property(p => p.ShipName).HasMaxLength(40);
                builder.Property(p => p.ShipAddress).HasMaxLength(60);
                builder.Property(p => p.ShipCity).HasMaxLength(15);
                builder.Property(p => p.ShipRegion).HasMaxLength(15);
                builder.Property(p => p.ShipPostalCode).HasMaxLength(10);
                builder.Property(p => p.ShipCountry).HasMaxLength(15);
                builder.Property(p => p.Freight).HasColumnType("Money").HasPrecision(19);//.HasConversion();
            }
        }
        #endregion //config
    }
}
