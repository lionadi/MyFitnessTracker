namespace MyFitnessTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MyFitnessTracker.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MyFitnessTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyFitnessTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //List<int> targets = new List<int>();
            //targets.Add(12);
            //targets.Add(23);
            //targets.Add(30);
            //List<Exercise> exercises1 = new List<Exercise>();
            //exercises1.Add(new Exercise{  Name = "Exercise 1", Target = 26});
            //exercises1.Add(new Exercise { Name = "Exercise 2", Target = 25 });

            //List<Exercise> exercises2 = new List<Exercise>();
            //exercises1.Add(new Exercise { Name = "Exercise 3", Target = 21 });
            //exercises1.Add(new Exercise { Name = "Exercise 4", Target = 22 });
            //exercises1.Add(new Exercise { Name = "Exercise 5", Target = 23 });

            //context.Sets.AddOrUpdate(p => p.Name,
            //    new Set
            //    {
            //        Name = "Set 1",
            //    },
            //    new Set
            //    {
            //        Name = "Set 2"
            //    }
            //    );

            //context.Exercises.AddOrUpdate(p => p.Name,
            //    new Exercise { Name = "Exercise 1", Target = 23 },
            //    new Exercise { Name = "Exercise 2", Target = 25 });
        }
    }
}
