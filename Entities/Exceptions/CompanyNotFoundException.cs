namespace Entities.Exceptions
{
	public sealed class CompanyNotFoundException : NotFoundException
	{
		public CompanyNotFoundException(int id) : base($"The company with Id: {id} does not exist in database")
		{
		}
	}
}