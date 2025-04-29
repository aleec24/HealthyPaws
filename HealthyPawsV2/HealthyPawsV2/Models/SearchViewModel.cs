using HealthyPawsV2.DAL;

namespace HealthyPawsV2.Models
{
	public class SearchViewModel
	{
		public List<PetType> PetTypes { get; set; }
		public List<ApplicationUser> Users { get; set; }
	}
}
