using Cleverbit.CodingTask.Data.Models;
using Cleverbit.CodingTask.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Data
{
    public static class CodingTaskContextExtensions
    {
        public static async Task Initialize(this CodingTaskContext context, IHashService hashService)
        {
            await context.Database.EnsureCreatedAsync();

            await SeedUsers(context, hashService);
            await SeedMatches(context);
        }

        private static async Task<bool> SeedUsers(this CodingTaskContext context, IHashService hashService)
        {
            var currentUsers = await context.Users.ToListAsync();

            bool anyNewUser = false;

            if (!currentUsers.Any(u => u.UserName == "User1"))
            {
                context.Users.Add(new Models.User
                {
                    UserName = "User1",
                    Password = await hashService.HashText("Password1")
                });

                anyNewUser = true;
            }

            if (!currentUsers.Any(u => u.UserName == "User2"))
            {
                context.Users.Add(new Models.User
                {
                    UserName = "User2",
                    Password = await hashService.HashText("Password2")
                });

                anyNewUser = true;
            }

            if (!currentUsers.Any(u => u.UserName == "User3"))
            {
                context.Users.Add(new Models.User
                {
                    UserName = "User3",
                    Password = await hashService.HashText("Password3")
                });

                anyNewUser = true;
            }

            if (!currentUsers.Any(u => u.UserName == "User4"))
            {
                context.Users.Add(new Models.User
                {
                    UserName = "User4",
                    Password = await hashService.HashText("Password4")
                });

                anyNewUser = true;
            }

            if (anyNewUser)
            {
                await context.SaveChangesAsync();
            }

            return true;
        }

        private static async Task<bool> SeedMatches(this CodingTaskContext context)
        {
            var currentMatches = await context.Matches.ToListAsync();

            //expired matches
            for(int count = 1; count <= 3; count++)
            {
                if (!currentMatches.Any(u => u.MatchId == count))
                {
                    context.Matches.Add(new Models.Match
                    {
                        ExpiryDate = DateTime.Now.AddMinutes(0 - (count * 5)),
                        MatchWinnerUserId = null
                    });
                }
            }

            //active matches
            for (int count = 1; count <= 10; count++)
            {
                if (!currentMatches.Any(u => u.MatchId == count))
                {
                    context.Matches.Add(new Models.Match
                    {
                        ExpiryDate = DateTime.Now.AddMinutes(count * 5),
                        MatchWinnerUserId = null
                    });
                }
            }

            await context.SaveChangesAsync();

            return true;
        }

    }
}
