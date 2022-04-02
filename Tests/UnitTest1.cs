using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Moq;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void GetCompanies_ReturnCompanies()
		{
			var moq = new Mock<ICompanyRepository>();
			moq.Setup(o => o.GetAllCompaniesAsync(false)).Returns(Task.FromResult(GetCompanies()));
			var result = moq.Object.GetAllCompaniesAsync(false).GetAwaiter().GetResult().ToList();

			Assert.IsInstanceOf<List<Company>>(result);
			Assert.That(result, Has.Exactly(1).Items);
		}

		private IEnumerable<Company> GetCompanies()
		{
			return new List<Company>()
			{
				new Company()
				{
					Id = 1,
					Address = "Test Addres",
					Country = "Test country",
					Name = "Test Company"
				}
			};
		}
	}
}