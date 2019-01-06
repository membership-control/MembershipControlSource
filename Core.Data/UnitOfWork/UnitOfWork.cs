using Core.Data.EF;
using Core.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.UnitOfWork
{
    public class UnitOfWork<W> :IUnitOfWork<W> where W:DbContext
    {
        public W DbContext { get; set; }

        public UnitOfWork(W dbcotext)
        {
            DbContext = dbcotext;
        }

        public int Commit()
        {
            _sql = "";
            this.DbContext.Database.Log = s => _sql = _sql + s;
            return this.DbContext.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    this.DbContext.SaveChanges();
                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });
                }
            } while (saveFailed);
        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            this.DbContext.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public DbSet<T> CreateSet<T>() where T : class
        {
            return this.DbContext.Set<T>();
        }

        public void Attach<T>(T item) where T : class
        {
            //attach and set as unchanged
            this.DbContext.Entry<T>(item).State = EntityState.Unchanged;
        }

        public void SetModified<T>(T item) where T : class
        {
            //this operation also attach item in object state manager
            this.DbContext.Entry<T>(item).State = EntityState.Modified;
        }

        public void ApplyCurrentValues<T>(T original, T current) where T : class
        {
            //if it is not attached, attach original and set current values
            this.DbContext.Entry<T>(original).CurrentValues.SetValues(current);
        }

        public void Dispose()
        {
            this.DbContext.Dispose();
        }

        public IEnumerable<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters)
        {
            return this.DbContext.Database.SqlQuery<T>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return this.DbContext.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        public DbContextTransaction BeginTransaction()
        {
           return  this.DbContext.Database.BeginTransaction();
        }

        public void CommitTransaction(DbContextTransaction transaction)
        {
            transaction.Commit();
        }

        public void RollbackTransaction(DbContextTransaction transaction)
        {
            transaction.Rollback();
        }
        
        private string _sql;
        public string Sql
        {
            get 
            {
                return _sql;
            }
        }

        

        #region Logging method

        public virtual void Log(LogModel logmodel)
        {
            try
            {
                using (Core.Data.EF.TGF_IntegrationEntities db = new Core.Data.EF.TGF_IntegrationEntities())
                {
                    var entity = new Core.Data.EF.TGF_GI_ControlTower_Logging();
                    entity.PK = logmodel.PK;
                    entity.Mode_ID = logmodel.Mode_ID;
                    entity.Details = !String.IsNullOrEmpty(logmodel.Details) && logmodel.Details.Length > 4000? 
                        logmodel.Details.Substring(0,4000) : logmodel.Details;
                    entity.Insert_Date = logmodel.Insert_Date;
                    entity.Insert_User = logmodel.Insert_User;
                    entity.Client = logmodel.Client;
                    entity.Action = logmodel.Action;
                    entity.SessionID = logmodel.SessionID;
                    entity.Status = logmodel.Status;
                    entity.Remark = logmodel.Remark;

                    db.Set<Core.Data.EF.TGF_GI_ControlTower_Logging>().Add(entity);
                    db.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }

        #endregion

    }


    public class IntegrationUnitOfWork : UnitOfWork<TGF_IntegrationEntities>
    {
        public IntegrationUnitOfWork(TGF_IntegrationEntities dbcontext)
            : base(dbcontext)
        {

        }
    }

    public class WKTempUnitOfWork : UnitOfWork<DI_WK_TEMPEntities>
    {
        public WKTempUnitOfWork(DI_WK_TEMPEntities dbcontext)
            : base(dbcontext)
        {

        }
    }

    public class DCTUnitOfWork : UnitOfWork<DCT_MangementEntities>
    {
        public DCTUnitOfWork(DCT_MangementEntities dbcontext)
            : base(dbcontext)
        {

        }
    }
}
