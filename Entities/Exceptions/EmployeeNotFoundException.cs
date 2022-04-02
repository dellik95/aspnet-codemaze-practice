namespace Entities.Exceptions
{
	public class EmployeeNotFoundException : NotFoundException
	{
		public EmployeeNotFoundException(int id) : base($"The employee with Id: {id} does not exist in database")
		{
		}
	}
}