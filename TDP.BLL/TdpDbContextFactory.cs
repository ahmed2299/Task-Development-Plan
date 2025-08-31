using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TDP.BLL
{
    public class TdpDbContextFactory : IDesignTimeDbContextFactory<TdpDbContext>
    {
        public TdpDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TdpDbContext>();

            optionsBuilder.UseSqlServer("server=.; Initial Catalog=TDP;Integrated Security=SSPI;MultipleActiveResultSets=True;TrustServerCertificate=True;");

            return new TdpDbContext(optionsBuilder.Options);
        }
    }
}
