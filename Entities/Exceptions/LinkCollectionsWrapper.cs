using System.Collections.Generic;
using Entities.LinkModels;

namespace Entities.Exceptions
{
	public class LinkCollectionsWrapper<T> : LinkResourceBase
	{
		public List<T> Value { get; set; } = new List<T>();

		public LinkCollectionsWrapper()
		{
		}

		public LinkCollectionsWrapper(List<T> items) => Value = items;
	}
}