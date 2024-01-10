using GroupSpace23.Areas.Identity.Data;

namespace GroupSpace23.Services
{
    public class UserStats
    {
        readonly RequestDelegate _next;  // Verwijzing naar de volgende middleware method in de afhandeling van de request

        struct Statistics  // Als test gebruiken we struct ipv class
        {
            public GroupSpace23User User { get; set; }
            public DateTime Connected { get; set; }
            public int NumberOfRequest { get; set; }
            public DateTime LastConnected { get; set; }
        }

        static Dictionary<string, Statistics> DictStatistics = null;


        public UserStats(RequestDelegate next) 
        {
            DictStatistics = new Dictionary<string, Statistics>();

            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IMyUser myUser)
        {

            try
            {
                Statistics stat = DictStatistics[myUser.User().UserName];
                stat.LastConnected = DateTime.Now;
                stat.NumberOfRequest = stat.NumberOfRequest + 1;

                // omdat "stat" op de stack staat ("struct") moet deze terug naar de dictionary
                // op de heap gecopieerd worden.
                DictStatistics[myUser.User().UserName] = stat;
            }
            catch 
            {
                Statistics stats = new Statistics();
                stats.User = myUser.User();
                stats.Connected = DateTime.Now;
                stats.NumberOfRequest = 1;
                stats.LastConnected = DateTime.Now;
                DictStatistics[myUser.User().UserName] = stats;
            }

            await _next(httpContext);
        }

        public static int GetCountofRequest(string name)
        {
            return DictStatistics[name].NumberOfRequest;
        }
    }
}
