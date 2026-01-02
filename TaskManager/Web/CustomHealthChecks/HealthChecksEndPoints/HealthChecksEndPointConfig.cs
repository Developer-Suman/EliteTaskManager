namespace Web.CustomHealthChecks.HealthChecksEndPoints
{
    public class HealthChecksEndPointConfig
    {
        public List<string> Endpoints { get; set; } = new List<string>
        {

              "https://localhost:7202/api/Account/GetAllUser",
            

        };



    }
}
