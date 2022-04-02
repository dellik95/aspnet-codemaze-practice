using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.RequestFeatures
{
	public class PagedList<T> : List<T>
	{
		public MetaData MetaData { get; }

		public PagedList(List<T> items, int count, int pageNumber, int pageSize)
		{
			MetaData = new MetaData()
			{
				TotalCount = count,
				PageSize = pageSize,
				CurrentPage = pageNumber,
				TotalPages = (int) Math.Ceiling(count / (double) pageSize)
			};
			AddRange(items);
		}


		public static PagedList<T> ToPagedList(IEnumerable<T> source, int count, int pageNubmer, int pageSize)
		{
			var items = source.Skip((pageNubmer - 1) * pageSize)
				.Take(pageSize).ToList();
			return new PagedList<T>(items, count, pageNubmer, pageSize);
		}
	}
}