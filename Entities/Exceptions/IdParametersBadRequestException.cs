namespace Entities.Exceptions
{
	public sealed class IdParametersBadRequestException: BadRequestException
	{
		public IdParametersBadRequestException() : base("Parameter Ids is null")
		{
		}
	}
}