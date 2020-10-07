/**
 * DbInitializer class initialize database by creating admin users, users' roles,
 * and also initialize some data in the Team table.
 */
using Lab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Data
{
    public static class DbInitializer
    {
        public static AppSecrets appSecrets {get; set;}
        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            // create the database if it doesn't exist
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if roles already exist and exit if there are
            if (roleManager.Roles.Count() > 0)
                return 1;  // should log an error message here

            // Seed roles
            int result = await SeedRoles(roleManager);
            if (result != 0)
                return 2;  // should log an error message here

            // Check if users already exist and exit if there are
            if (userManager.Users.Count() > 0)
                return 3;  // should log an error message here

            // Seed users
            result = await SeedUsers(userManager);
            if (result != 0)
                return 4;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Create Manager Role
            var result = await roleManager.CreateAsync(new IdentityRole("Manager"));
            if (!result.Succeeded)
                return 1;  // should log an error message here

            // Create Player Role
            result = await roleManager.CreateAsync(new IdentityRole("Player"));
            if (!result.Succeeded)
                return 2;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Create Manager User
            var adminUser = new ApplicationUser
            {
                UserName = appSecrets.ManagerEmail,
                Email = appSecrets.ManagerEmail,
                FirstName = "Mr.",
                LastName = "Manager",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, appSecrets.ManagerPwd);
            if (!result.Succeeded)
                return 1;  // should log an error message here

            // Assign user to Manager role
            result = await userManager.AddToRoleAsync(adminUser, "Manager");
            if (!result.Succeeded)
                return 2;  // should log an error message here

            // Create Player User
            var memberUser = new ApplicationUser
            {
                UserName = appSecrets.PlayerEmail,
                Email = appSecrets.PlayerEmail,
                FirstName = "Mr.",
                LastName = "Player",
                EmailConfirmed = true
            };

            result = await userManager.CreateAsync(memberUser, appSecrets.PlayerPwd);
            if (!result.Succeeded)
                return 3;  // should log an error message here

            // Assign user to player role
            result = await userManager.AddToRoleAsync(memberUser, "Player");
            if (!result.Succeeded)
                return 4;  // should log an error message here

            return 0;
        }


        public static void TeamInitializer(ApplicationDbContext context)
        {
            // Look for any teams.
            if (context.Teams.Any())
            {
                return;   // DB has been seeded
            }

            var teams = new Team[]
            {
                 new Team
                 {
                    TeamName = "Hamilton Soccer Club",
                    Email = "hamiltonsoccer@mail.com",
                    EstablishedDate = new DateTime(2011, 4, 29)
                 },
                new Team
                {
                    TeamName = "Burlington Soccer Club",
                    Email = "burlintonsoccer@mail.com",
                    EstablishedDate = new DateTime(2010, 4, 21)
                },
                new Team
                {
                    TeamName = "Toronto Basketball",
                    Email = "tbasketball@mail.com",
                    EstablishedDate = new DateTime(2010, 4, 30)
                },
                new Team
                {
                    TeamName = "Toronto Cricket Inc.",
                    Email = "crictoronto@mail.com",
                    EstablishedDate = new DateTime(2008, 5, 5)
                }
            };

            foreach (Team team in teams)
            {
                context.Teams.Add(team);
            }
            context.SaveChanges();
        }
    }
}
