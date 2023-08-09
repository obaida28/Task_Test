using Core.Entites;
using Core.Interfaces;
namespace Infrastructure.Data;
public class CustomerRepository : GenericRepository<Customer> , ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context) {}
}