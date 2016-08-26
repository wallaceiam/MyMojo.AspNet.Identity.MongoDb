using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMojo.AspNet.Identity.MongoDb.Internal
{
    public interface IDbStoreOptions
    {
        string UserCollection { get; set; }
        string RoleCollection { get; set; }
    }

    public class NoSqlDbStoreOptions : IDbStoreOptions
    {
        public string UserCollection { get; set; }
        public string RoleCollection { get; set; }
    }
}
