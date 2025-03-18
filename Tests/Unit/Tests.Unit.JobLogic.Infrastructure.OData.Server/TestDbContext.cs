using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tests.Unit.JobLogic.Infrastructure.OData.Server
{
    public class Model
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Url { get; set; }
        public int Age { get; set; }
        public int ExpYears { get; set; }
        public DateTime Birthday { get; set; }

        public int? Navigation_Model_Id { get; set; }

        [ForeignKey("Navigation_Model_Id")]
        public virtual Navigation_Model Navigation_Model { get; set; }
    }

    public class Navigation_Model
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    internal class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Model> Models { get; set; }
        public DbSet<Navigation_Model> Navigation_Models { get; set; }
    }
}
