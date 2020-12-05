// <copyright file="Dal.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable
using System;
using System.Linq;
using System.Collections.Generic;

namespace DAL
{
    class DBManager
    {
        private static fifteens_databaseContext db = new fifteens_databaseContext();

        static void AddUser(string login, string password)
        {
            db.Users.Add(new User { Login = login, Password = password });
            db.SaveChanges();
        }

        static void AddMatch(Match match)
        {
            db.Matches.Add(match);
            db.SaveChanges();
        }

        static bool LogIn(string login, string password)
        {
            User user = (from users in db.Users
                         where users.Login == login &&
                         users.Password == password
                         select users).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }

        static List<Match> GetUserMatches(int user_id)
        {
            return (from usermatches in db.Matches
                    where usermatches.UserId == user_id
                    select usermatches).ToList();
        }

        static Match GetMatch(int match_id)
        {
            return (from usermatches in db.Matches
                    where usermatches.Result == false &&
                    usermatches.MatchId == match_id
                    select usermatches).FirstOrDefault();
        }

        static void Main(string[] args)
        {

            
        }
    }
}
