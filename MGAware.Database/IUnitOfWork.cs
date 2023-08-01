using MGAware.Database.Context;
using MGAware.Database.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MGAware.Database;

public interface IUnitOfWork<out TContext> where TContext : IdentityDbContext<ApplicationUser>, new()
{
    TContext Context { get; }
    void CreateTransaction();
    void Commit();
    void Rollback();
    void Save();
}
