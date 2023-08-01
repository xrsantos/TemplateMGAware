using MGAware.Database.Context;
using MGAware.Database.DAL;

namespace MGAware.Database.Repository;

public class ContactRepository : GenericRepository<Contact>
{
    public ContactRepository(IUnitOfWork<MGADBContext> unitOfWork) : base(unitOfWork)
    {
    }
}