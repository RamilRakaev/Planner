using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Planner.Model
{
    class PlannerContext : DbContext
    {
        public PlannerContext() : base("PlannerDBConnection")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PlannerContext>());
        }
        public DbSet<Diary> DiaryDB { get; set; }
        public DbSet<PlannerTask> PlannerTaskDB { get; set; }
        public DbSet<Plan> PlanDB { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diary>().Map(m => { m.MapInheritedProperties(); m.ToTable("Diary"); });
            modelBuilder.Entity<PlannerTask>().Map(m => { m.MapInheritedProperties(); m.ToTable("PlannerTask"); });
            modelBuilder.Entity<Plan>().Map(m => { m.MapInheritedProperties(); m.ToTable("Plan"); });
        }
    }
}
