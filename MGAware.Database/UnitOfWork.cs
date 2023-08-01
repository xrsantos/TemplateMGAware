using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using MGAware.Database.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MGAware.Database
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable 
        where TContext : IdentityDbContext<ApplicationUser>, new()
    {
        private bool _disposed;
        private string _errorMessage = string.Empty;

        private DbContextTransaction _objTran;

        public UnitOfWork() => Context = new TContext();

        public UnitOfWork(TContext context)
        {
            Context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TContext Context { get; }

        public void CreateTransaction()
        {
        }

        public void Commit()
        {
            _objTran.Commit();
        }

        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }

        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage = _errorMessage + $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage} {Environment.NewLine}";
                    }   
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    Context.Dispose();
            _disposed = true;
        }
    }
}