using Core.Data.Model;
using Core.Infrastruture.Specification.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repository
{
    public interface IRepository<T,W> : IDisposable
        where T : class
        where W : System.Data.Entity.DbContext
    {

        Result Add(T item);

        Result AddRange(List<T> items);

        Result DeleteBy(T item);

        Result DeleteByID(params object[] ids);

        Result DeleteRangeBy(params T[] items);

        Result RemoveBy(T item);

        Result Update(T item);

        /// <summary>
        /// Sets modified entity into the repository. 
        /// When calling Commit() method in UnitOfWork 
        /// these changes will be saved into the storage
        /// </summary>
        /// <param name="persisted">The persisted item</param>
        /// <param name="current">The current item</param>
        Result Merge(T persisted, T current);

        T GetByID(params object[] ids);

        IQueryable<T> GetAll();

        /// <summary>
        /// Get all elements of type T that matching a
        /// Specification <paramref name="specification"/>
        /// </summary>
        /// <param name="specification">Specification that result meet</param>
        /// <returns></returns>
        IQueryable<T> AllMatching(ISpecification<T> specification);

        /// <summary>
        /// Get all elements of type T in repository
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        IQueryable<T> GetPaged<Property>(int pageIndex, int pageCount, Expression<Func<T, Property>> orderByExpression, bool ascending);

        /// <summary>
        /// Get  elements of type T in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        IQueryable<T> GetFiltered(Expression<Func<T, bool>> filter);
    }
}
