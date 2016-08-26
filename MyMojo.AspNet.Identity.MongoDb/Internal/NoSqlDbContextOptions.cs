using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMojo.AspNet.Identity.MongoDb.Internal
{
    public interface IDbContextOptions
    {
        string ConnectionString { get; set; }
    }
    public class DbContextOptions<TContext> : IDbContextOptions
    {
        public string ConnectionString { get; set; }
    }

    public class NoSqlDbContextOptions : IDbContextOptions
    {
        public string ConnectionString { get; set; }
    }
}
