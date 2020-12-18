using System;
using System.Linq;
using System.Collections.Generic;
using Foosball.Persistance.ProjectionStore.Entites;
using LiteDB;

namespace Foosball.Persistance.ProjectionStore
{
    public class LiteDbProjectionStore : IProjectionStore
    {
        private const string _dbName = "FoosballProjections";
        private const string _listCollection = "games";
        private const string _detailsCollection = "gameDetails";
        public void AddFoosballGameDetails(FoosballGameDetails game)
        {
            using (var db = new LiteDatabase(_dbName))
            {
                var collection = db.GetCollection<FoosballGameDetails>(_detailsCollection);
                collection.EnsureIndex(x => x.Id);
                collection.Insert(game);
            }
        }

        public void AddFoosballGameListItem(FoosballGameListItem game)
        {
            using (var db = new LiteDatabase(_dbName))
            {
                var collection = db.GetCollection<FoosballGameListItem>(_listCollection);
                collection.EnsureIndex(x => x.Id);
                collection.Insert(game);
            }
        }

        public void AddGoal(Guid gameId, int setNumber, string scorer, DateTimeOffset scoredAt)
        {          
            using (var db = new LiteDatabase(_dbName))
            {
                var collection = db.GetCollection<FoosballGameDetails>(_detailsCollection);
                var game = collection.FindOne(x => x.Id == gameId);
                game?.AddGoal(setNumber, scorer, scoredAt);
                collection.Update(game);
            }
        }

        public void AddSetWin(Guid gameId, string winningTeam)
        {
            using (var db = new LiteDatabase(_dbName))
            {
                var collection = db.GetCollection<FoosballGameListItem>(_listCollection);
                var game = collection.FindOne(x => x.Id == gameId);
                game?.AddSetWin(winningTeam);
                collection.Update(game);
            }            
        }

        public IReadOnlyCollection<FoosballGameListItem> GetFoosballGames()
        {
            using (var db = new LiteDatabase(_dbName))
            {
                var collection = db.GetCollection<FoosballGameListItem>(_listCollection);
                var allItems = collection.FindAll();

                return allItems.OrderByDescending(x=> x.StartDate).ToList();
            }  
        }

        public FoosballGameDetails GetGameDetails(Guid id)
        {
            using (var db = new LiteDatabase(_dbName))
            {
                var collection = db.GetCollection<FoosballGameDetails>(_detailsCollection);
                return collection.FindOne(x=> x.Id == id);             
            }  
        }
    }
}