using static CedulasController;

namespace HealthyPawsV2.Models
{
    public class ApiResponse
    {
        public string nombre { get; set; }
        public List<Result> Results { get; set; }
        public string cedula { get; set; }
    }
}
