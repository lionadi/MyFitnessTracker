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
            List<int> targets = new List<int>();
            targets.Add(12);
            targets.Add(23);
            targets.Add(30);
            List<Exercise> exercises1 = new List<Exercise>();
            exercises1.Add(new Exercise{  Name = "Exercise 1", Targets = targets});
            exercises1.Add(new Exercise{  Name = "Exercise 2", Targets = targets});

            List<Exercise> exercises2 = new List<Exercise>();
            exercises1.Add(new Exercise{  Name = "Exercise 3", Targets = targets});
            exercises1.Add(new Exercise{  Name = "Exercise 4", Targets = targets});
            exercises1.Add(new Exercise{  Name = "Exercise 5", Targets = targets});

            context.Sets.AddOrUpdate(p => p.Name,
                new Set
                {
                    Name = "Set 1",
                    Excercises = exercises1
                },
                new Set
                {
                    Name = "Set 2",
                    Excercises = exercises2
                }
                );
        }
    }
}
