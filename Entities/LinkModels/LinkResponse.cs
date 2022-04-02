using System.Collections.Generic;
using Entities.Exceptions;
using Entities.Models;

namespace Entities.LinkModels
{
	public class LinkResponse
	{
		public bool HasLinks { get; set; }
		public List<Entity> ShapedEntities { get; set; }
		public LinkCollectionsWrapper<Entity> LinkedEntities { get; set; }

		public LinkResponse()
		{
			LinkedEntities = new LinkCollectionsWrapper<Entity>();
			ShapedEntities = new List<Entity>();
		}
	}
}