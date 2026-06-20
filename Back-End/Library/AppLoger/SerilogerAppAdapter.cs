using library.AppLoger;
using Microsoft.Extensions.Logging;

namespace Library.AppLoger
{
    public class SerilogerAppAdapter<T>(ILogger<T> logger) : IAppLoger<T> where T : class
    {
        public void LogError (Exception ex , string messege)
        {
            logger.LogError(ex, messege);
        }

        public void LogInfo(string messege)
        {
            logger.LogWarning(messege);
        }

        public void LoginInformation(string messege)
        {
            logger.LogInformation(messege);
        }
    }


}