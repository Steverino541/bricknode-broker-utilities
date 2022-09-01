using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bricknode_Broker_Utilities.Contracts.Settings
{
    public class UserSecrets
    {
        public List<BfsCredential> BfsCredentials { get; set; }
    }

    public class BfsCredential
    {
        public string BfsInstanceKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Identifier { get; set; }
        public string ApiEndpoint { get; set; }
    }
}
