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

        static int LogIn(string login, string password)
        {
            User user = (from users in db.Users
                         where users.Login == login &&
                         users.Password == password
                         select users).FirstOrDefault();
            if (user != null)
            {
                return user.UserId;
            }
            return -1;
        }

        static List<Match> GetUserCompletedMatches(int user_id)
        {
            return (from usermatches in db.Matches
                    where usermatches.UserId == user_id &&
                    usermatches.Result == true
                    select usermatches).ToList();
        }

        static List<Match> GetUserUnfinishedMatches(int user_id)
        {
            return (from usermatches in db.Matches
                    where usermatches.UserId == user_id &&
                    usermatches.Result == false
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
