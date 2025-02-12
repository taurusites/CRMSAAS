using Application.Common.Repositories;
using Application.Common.Services.SaaSManager;
using Domain.Entities;

namespace Infrastructure.SaaSManager;


public class SaaSService : ISaaSService
{
    private readonly ICommandRepository<Company> _company;
    private readonly ICommandRepository<Warehouse> _warehouse;
    private readonly IUnitOfWork _unitOfWork;

    public SaaSService(
        ICommandRepository<Company> company,
        ICommandRepository<Warehouse> warehouse,
        IUnitOfWork unitOfWork
        )
    {
        _company = company;
        _warehouse = warehouse;
        _unitOfWork = unitOfWork;
    }

    public async Task InitTenantAsync(CancellationToken cancellationToken = default)
    {
        //company

        var company = new Company
        {
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false,
            Name = "Acme Corp",
            Currency = "USD",
            Street = "123 Main St",
            City = "Metropolis",
            State = "New York",
            ZipCode = "10001",
            Country = "USA",
            PhoneNumber = "+1-212-555-1234",
            FaxNumber = "+1-212-555-5678",
            EmailAddress = "info@acmecorp.com",
            Website = "https://www.acmecorp.com"
        };

        await _company.CreateAsync(company);

        //warehouse system
        var warehouses = new List<Warehouse>
            {
                new Warehouse { Name = "Customer", SystemWarehouse = true },
                new Warehouse { Name = "Vendor", SystemWarehouse = true },
                new Warehouse { Name = "Transfer", SystemWarehouse = true },
                new Warehouse { Name = "Adjustment", SystemWarehouse = true },
                new Warehouse { Name = "StockCount", SystemWarehouse = true },
                new Warehouse { Name = "Scrapping", SystemWarehouse = true }
            };

        foreach (var warehouse in warehouses)
        {
            await _warehouse.CreateAsync(warehouse);
        }



        //tax

        //bookingGroup

        //bookingResource

        //customerCategory

        //customerGroup

        //vendorCategory

        //vendorGroup

        //unitMeasure

        //productGroup

        //warehouse

        //programManagerResource

        //salesTeam

        //salesRepresentative

        //paymentMethod

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
