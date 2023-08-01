using Microsoft.EntityFrameworkCore;    
using MGAware.Database.Context;

namespace MGAware.Database;

public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
{
    private DbSet<T> _entities;
    private string _errorMessage = string.Empty;
    private bool _isDisposed;

    public GenericRepository(IUnitOfWork<MGADBContext> unitOfWork)
        : this(unitOfWork.Context)
    {
        _isDisposed = false;
        //Context = unitOfWork.Context;
    }

    public GenericRepository(MGADBContext context)
    {
        _isDisposed = false;
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public MGADBContext Context { get; set; }
    
    protected virtual DbSet<T> Entities
    {
        get { return _entities ?? (_entities = Context.Set<T>()); }
    }

    public void Dispose()
    {
        if (Context != null)
            Context.Dispose();
        _isDisposed = true;
    }

    public virtual IEnumerable<T> GetAll()
    {
        return Entities.ToList();
    }

    public virtual T GetById(object id)
    {
        return Entities.Find(id);
    }

    public virtual void Insert(T entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }  
            
            if (Context == null || _isDisposed)
            {
                Context = new MGADBContext();
            }

            Entities.Add(entity);

        }
        catch (Exception dbEx)
        {
            //HandleUnitOfWorkException(dbEx);
            throw new Exception(_errorMessage, dbEx);
        }
    }
    
    public virtual void Update(T entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
                
            if (Context == null || _isDisposed)
            {
                Context = new MGADBContext();
            }
            Context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //commented out call to SaveChanges as Context save changes will be called with Unit of work
            //Context.SaveChanges(); 
        }
        catch (Exception dbEx)
        {
            //HandleUnitOfWorkException(dbEx);
            throw new Exception(_errorMessage, dbEx);
        }
    }

    public virtual void Delete(T entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }

            if (Context == null || _isDisposed)
            {
                Context = new MGADBContext();
            }
            
            Entities.Remove(entity);
        }
        catch (Exception dbEx)
        {
            //HandleUnitOfWorkException(dbEx);
            throw new Exception(_errorMessage, dbEx);
        }
    }

    // private void HandleUnitOfWorkException(DbEntityValidationException dbEx)
    // {
    //     foreach (var validationErrors in dbEx.EntityValidationErrors)
    //     {
    //         foreach (var validationError in validationErrors.ValidationErrors)
    //         {
    //             _errorMessage = _errorMessage + $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage} {Environment.NewLine}";
    //         }
    //     }
    // }
}
