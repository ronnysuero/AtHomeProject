using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AtHomeProject.Data.Entities;
using AtHomeProject.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AtHomeProject.Web
{
    public static class DataGenerator
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            if ((await unitOfWork.Users.CountAsync) > 0)
                return; // Data was already seeded


            await unitOfWork.Users.InsertRangeAsync(
                new Users
                {
                    Id = 1,
                    Name = "User API 1",
                    Username = "user1",
                    Password = "admin1",
                    Claims = new List<UsersClaims>
                    {
                        new()
                        {
                            Id = 1,
                            UserId = 1,
                            ClaimType = Constants.ROLE,
                            ClaimValue = Constants.API_JSON1
                        }
                    }
                },
                new Users
                {
                    Id = 2,
                    Name = "User API 2",
                    Username = "user2",
                    Password = "admin2",
                    Claims = new List<UsersClaims>
                    {
                        new()
                        {
                            Id = 2,
                            UserId = 2,
                            ClaimType = Constants.ROLE,
                            ClaimValue = Constants.API_JSON2
                        }
                    }
                },
                new Users
                {
                    Id = 3,
                    Name = "User API 3",
                    Username = "user3",
                    Password = "admin3",
                    Claims = new List<UsersClaims>
                    {
                        new()
                        {
                            Id = 3,
                            UserId = 3,
                            ClaimType = Constants.ROLE,
                            ClaimValue = Constants.API_XML
                        }
                    }
                }
            );

            await unitOfWork.SaveAsync();
        }
    }
}
