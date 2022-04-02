namespace Entities.Models
{
	public class ShapedEntity
	{
		public ShapedEntity()
		{
			Entity = new Entity();
		}

		public int Id { get; set; }
		public Entity Entity { get; set; }
	}
}