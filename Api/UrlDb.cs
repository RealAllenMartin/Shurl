using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class UrlDb : DbContext
    {
        public UrlDb(DbContextOptions<UrlDb> options)
        : base(options) { }

    public DbSet<Url> Urls => Set<Url>();
    }
}