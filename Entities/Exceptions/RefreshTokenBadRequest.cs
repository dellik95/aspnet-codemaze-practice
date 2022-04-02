namespace Entities.Exceptions
{
	public class RefreshTokenBadRequest : BadRequestException
	{
		public RefreshTokenBadRequest() : base("Some data in token is invalid")
		{
		}
	}
}