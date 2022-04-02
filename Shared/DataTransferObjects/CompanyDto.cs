namespace Shared.DataTransferObjects
{
	public record CompanyDto
	{
		public int Id { get; init; }
		public string Name { get; init; }
		public string FullAddress { get; init; }
	}
}