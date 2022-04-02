namespace Entities.ConfigurationModels
{
	public class JwtOptions
	{
		public static string Section = "JwtOptions";
		public string ValidIssuer { get; set; }
		public string ValidAudience { get; set; }
		public int Expires { get; set; }
		public string Key { get; set; }
	}
}