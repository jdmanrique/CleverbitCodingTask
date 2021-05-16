using Entities = Cleverbit.CodingTask.Data.Models;
using Cleverbit.CodingTask.Data.Repository;
using Cleverbit.CodingTask.Services;
using DTO = Cleverbit.CodingTask.Services.DTO;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Cleverbit.CodingTask.Data.Models;

namespace Cleverbit.CodingTask.Test
{
    public class NumberGameServiceTests
    {
        private Mock<IUserRepository> userRepo;
        private Mock<IUserMatchRepository> userMatchRepo;
        private Mock<IMatchRepository> matchRepo;
        private INumberGameService serviceUnderTest;

        private List<Entities.Match> matches;
        private List<Entities.User> users;
        private List<Entities.UserMatch> userMatches;


        [SetUp]
        public void Setup()
        {
            this.userRepo = new Mock<IUserRepository>();
            this.userMatchRepo = new Mock<IUserMatchRepository>();
            this.matchRepo = new Mock<IMatchRepository>();

            this.serviceUnderTest = new NumberGameService(matchRepo.Object, userMatchRepo.Object, userRepo.Object);

            this.matches = new List<Entities.Match>()
            {
                new Entities.Match
                {
                    MatchId = 1,
                    ExpiryDate = DateTime.Now.AddMinutes(10)
                },
                new Entities.Match
                {
                    MatchId = 2,
                    ExpiryDate = DateTime.Now.AddMinutes(-10)
                }
            };


            this.users = new List<Entities.User>()
            {
                new Entities.User
                {
                    Id = 1,
                    Password = String.Empty,
                    UserName = "Test1"
                },
                new Entities.User
                {
                    Id = 2,
                    Password = String.Empty,
                    UserName = "Test2"
                },
                new Entities.User
                {
                    Id = 3,
                    Password = String.Empty,
                    UserName = "Test3"
                },
                new Entities.User
                {
                    Id = 4,
                    Password = String.Empty,
                    UserName = "Test4"
                }
            };

        }

        [Test]
        public void GetActiveMatch_ReturnEmpty()
        {
            this.matchRepo.Setup(x => x.GetAllMatches()).ReturnsAsync(() => { return Enumerable.Empty<Entities.Match>().ToList(); });

            var ex = Assert.ThrowsAsync<NullReferenceException>(() => this.serviceUnderTest.GetActiveMatch(1));
            Assert.That(ex.Message, Is.EqualTo("No active match found."));
        }

        [Test]
        public async Task GetActiveMatch_ReturnOneMatch()
        {
            this.matchRepo.Setup(x => x.GetAllMatches()).ReturnsAsync(() => { return this.matches; });
            this.userMatchRepo.Setup(x => x.GetUserMatchByUserIdMatchId(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(() => { return null; });

            var result = await this.serviceUnderTest.GetActiveMatch(1);

            Assert.IsTrue(result.MatchId == 1);
            Assert.IsTrue(result.NumberInput == 0);
        }

        [Test]
        public async Task GetExpiredMatches_ReturnList()
        {
            var userMatches = new List<Entities.UserMatch>()
            {
                new Entities.UserMatch
                {
                    UserId = 1,
                    MatchId = 2,
                    NumberInput = 5,
                    CreatedDate = DateTime.Now
                },
                new Entities.UserMatch
                {
                    UserId = 2,
                    MatchId = 2,
                    NumberInput = 6,
                    CreatedDate = DateTime.Now
                }
                ,
                new Entities.UserMatch
                {
                    UserId = 3,
                    MatchId = 2,
                    NumberInput = 10,
                    CreatedDate = DateTime.Now
                },
                new Entities.UserMatch
                {
                    UserId = 4,
                    MatchId = 1,
                    NumberInput = 99,
                    CreatedDate = DateTime.Now
                }
            };
            this.userMatchRepo.Setup(x => x.GetUserMatchesByMatchId(It.IsAny<int>())).ReturnsAsync(() => { return userMatches; });
            this.matchRepo.Setup(x => x.GetAllMatches()).ReturnsAsync(() => { return this.matches; });
            this.userRepo.Setup(x => x.GetAllUsers()).ReturnsAsync(() => { return this.users; });

            var result = await this.serviceUnderTest.GetExpiredMatches();
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Select(x => x.Winner).Contains("Test4"));
        }

        [Test]
        public async Task GetExpiredMatches_ReturnsMatchWithMoreThanOneWinner()
        {
            var userMatches = new List<Entities.UserMatch>()
            {
                new Entities.UserMatch
                {
                    UserId = 1,
                    MatchId = 2,
                    NumberInput = 5,
                    CreatedDate = DateTime.Now
                },
                new Entities.UserMatch
                {
                    UserId = 2,
                    MatchId = 2,
                    NumberInput = 6,
                    CreatedDate = DateTime.Now
                }
                ,
                new Entities.UserMatch
                {
                    UserId = 3,
                    MatchId = 2,
                    NumberInput = 99,
                    CreatedDate = DateTime.Now
                },
                new Entities.UserMatch
                {
                    UserId = 4,
                    MatchId = 2,
                    NumberInput = 99,
                    CreatedDate = DateTime.Now
                }
            };

            this.userMatchRepo.Setup(x => x.GetUserMatchesByMatchId(It.IsAny<int>())).ReturnsAsync(() => { return userMatches; });
            this.matchRepo.Setup(x => x.GetAllMatches()).ReturnsAsync(() => { return this.matches; });
            this.userRepo.Setup(x => x.GetAllUsers()).ReturnsAsync(() => { return this.users; });

            var result = await this.serviceUnderTest.GetExpiredMatches();
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Select(x => x.Winner).Contains("Test3,Test4"));
        }

        [Test]
        public async Task GetExpiredMatches_ReturnsNoWinner()
        {
            var userMatches = new List<Entities.UserMatch>()
            {
                new Entities.UserMatch
                {
                    UserId = 1,
                    MatchId = 2,
                    NumberInput = null,
                    CreatedDate = DateTime.Now
                },
                new Entities.UserMatch
                {
                    UserId = 2,
                    MatchId = 2,
                    NumberInput = null,
                    CreatedDate = DateTime.Now
                }
                ,
                new Entities.UserMatch
                {
                    UserId = 3,
                    MatchId = 2,
                    NumberInput = null,
                    CreatedDate = DateTime.Now
                },
                new Entities.UserMatch
                {
                    UserId = 4,
                    MatchId = 1,
                    NumberInput = null,
                    CreatedDate = DateTime.Now
                }
            };

            this.userMatchRepo.Setup(x => x.GetUserMatchesByMatchId(It.IsAny<int>())).ReturnsAsync(() => { return userMatches; });
            this.matchRepo.Setup(x => x.GetAllMatches()).ReturnsAsync(() => { return this.matches; });
            this.userRepo.Setup(x => x.GetAllUsers()).ReturnsAsync(() => { return this.users; });

            var result = await this.serviceUnderTest.GetExpiredMatches();
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Select(x => x.Winner).Contains("No Winner"));
        }

        [Test]
        public void PlayNow_NoActiveMatch_ThrowErrror()
        {
            this.matchRepo.Setup(x => x.GetAllMatches()).ReturnsAsync(() => { return Enumerable.Empty<Entities.Match>().ToList(); });
            var ex = Assert.ThrowsAsync<NullReferenceException>(() => this.serviceUnderTest.PlayNow(1));
            Assert.That(ex.Message, Is.EqualTo("No active match found."));
        }

        [Test]
        public void PlayNow_Success()
        {
            //this.userMatchRepo.Setup(x => x.AddUserMatch(It.IsAny<UserMatch>())).Fa
            this.matchRepo.Setup(x => x.GetAllMatches()).ReturnsAsync(() => { return this.matches; });

            Assert.DoesNotThrowAsync(() => this.serviceUnderTest.PlayNow(1));
        }
    }
}