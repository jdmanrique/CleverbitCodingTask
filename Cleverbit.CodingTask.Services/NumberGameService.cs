using Cleverbit.CodingTask.Data;
using Cleverbit.CodingTask.Data.Models;
using Cleverbit.CodingTask.Data.Repository;
using Cleverbit.CodingTask.Services.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Services
{
    public class NumberGameService: INumberGameService
    {

        private readonly IMatchRepository matchRepo;
        private readonly IUserMatchRepository userMatchRepo;
        private readonly IUserRepository userRepo;

        public NumberGameService(IMatchRepository _matchRepo, IUserMatchRepository _userMatchRepo, IUserRepository _userRepo)
        {
            this.matchRepo = _matchRepo;
            this.userMatchRepo = _userMatchRepo;
            this.userRepo = _userRepo;

        }

        public async Task<List<DTO.Match>> GetExpiredMatches()
        {
            var result = new List<DTO.Match>();

            var matches = await this.matchRepo.GetAllMatches();
            var expiredMatches = matches.Where(x => x.ExpiryDate <= DateTime.Now).OrderByDescending(x => x.ExpiryDate);
            foreach (var match in expiredMatches)
            {
                var matchwinners = await this.GetMatchWinner(match.MatchId);
                var item = new DTO.Match()
                {
                    MatchId = match.MatchId,
                    ExpiryDate = match.ExpiryDate,
                    Winner = matchwinners.Count == 0 ? "No Winner" : String.Join(",", matchwinners.Select(x => x.UserName))
                };
                result.Add(item);
            }

            return result;
        }

        public async Task<DTO.Match> GetActiveMatch(int userId)
        {
            var result = new List<DTO.Match>();

            var matches = await this.matchRepo.GetAllMatches();
            var activeMatch = matches.Where(x => x.ExpiryDate > DateTime.Now).OrderBy(x => x.ExpiryDate).FirstOrDefault();
            if (activeMatch == null)
            {
                throw new NullReferenceException("No active match found.");
            }

            var userEntry = await this.userMatchRepo.GetUserMatchByUserIdMatchId(userId, activeMatch.MatchId);

            return new DTO.Match()
            {
                MatchId = activeMatch.MatchId,
                ExpiryDate = activeMatch.ExpiryDate,
                Winner = String.Empty,
                NumberInput = userEntry != null ? userEntry.NumberInput.Value : 0
            };
        }

        public async Task<List<MatchParticipant>> GetMatchWinner(int matchId)
        {
            var result = new MatchParticipant();

            var userMatches = await this.userMatchRepo.GetUserMatchesByMatchId(matchId);

            if (!userMatches.Any(x => x.NumberInput.HasValue))
            {
                return Enumerable.Empty<MatchParticipant>().ToList();
            }

            var winner = userMatches.Where(x => x.NumberInput == userMatches.Max(x => x.NumberInput));
               
            var winnerList = from um in winner
                               join u in await userRepo.GetAllUsers()
                               on um.UserId equals u.Id
                               select new MatchParticipant { UserId = u.Id, MatchId = um.MatchId, NumberInput = um.NumberInput, UserName = u.UserName };

            return winnerList.ToList();
        }

        public async Task<int> PlayNow(int userId)
        {
            var activeMatch = await this.GetActiveMatch(userId);
            var randomNumber = new Random().Next(1, 100);
            var newMatch = new UserMatch()
            {
                UserId = userId,
                MatchId = activeMatch.MatchId,
                NumberInput = randomNumber,
                CreatedDate = DateTime.Now
            };

            await this.userMatchRepo.AddUserMatch(newMatch);
            return randomNumber;
        }
    }
}
