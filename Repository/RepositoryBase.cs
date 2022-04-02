using System;
using System.Linq;
using System.Linq.Expressions;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
	public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class

	{
		protected readonly RepositoryContext Context;

		public RepositoryBase(RepositoryContext context)
		{
			Context = context;
		}

		public IQueryable<T> FindAll(bool trackChanges)
		{
			return trackChanges ? Context.Set<T>() : Context.Set<T>().AsNoTracking();
		}

		public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
		{
			var sets = Context.Set<T>().Where(expression);
			return trackChanges ? sets : sets.AsNoTracking();
		}

		public void Create(T entity)
		{
			Context.Set<T>().Add(entity);
		}

		public void Update(T entity)
		{
			Context.Set<T>().Update(entity);
		}

		public void Delete(T entity)
		{
			Context.Set<T>().Remove(entity);
		}
	}
}