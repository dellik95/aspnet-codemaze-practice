using System.Collections.Generic;
using System.Dynamic;
using Entities.Models;

namespace Contracts
{
	public interface IDataShaper<T>
	{
		IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fields);
		ShapedEntity ShapeData(T entity, string fields);
	}
}