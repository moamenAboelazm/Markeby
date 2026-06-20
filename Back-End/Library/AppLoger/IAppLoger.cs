using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.AppLoger
{
    public interface IAppLoger<T>
    {
        void LoginInformation (string messege);
        void LogInfo (string messege);
        void LogError (Exception ex , string messege);
    }
}
