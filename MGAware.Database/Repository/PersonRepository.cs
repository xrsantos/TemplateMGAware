using MGAware.Database.Context;
using MGAware.Database.DAL;
using Microsoft.EntityFrameworkCore;

namespace MGAware.Database.Repository;

public class PersonRepository : GenericRepository<Person>
{
    public PersonRepository(IUnitOfWork<MGADBContext> unitOfWork) : base(unitOfWork)
    {
    }

    public IQueryable<Person> GetAllAndContracts()
    {
        return this.Context.Person.Include(p => p.Contacts);
    }

}