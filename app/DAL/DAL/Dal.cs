// <copyright file="Dal.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable
using System;
using System.Linq;
using System.Collections.Generic;

namespace DAL
{
    public class DBManager
    {
        private static fifteens_databaseContext db = new fifteens_databaseContext();

        public static int AddUser(string login, string password)
        {
            User u = new User { Login = login, Password = password };
            db.Users.Add(u);
            db.SaveChanges();
            return u.UserId;
        }

        public static void AddMatch(Match match)
        {
            db.Matches.Add(match);
            db.SaveChanges();
        }

        public static bool LogIn(string login, string password)
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

        public static List<Match> GetUserMatches(int user_id)
        {
            return (from usermatches in db.Matches
                    where usermatches.UserId == user_id
                    select usermatches).ToList();
        }

        public static Match GetMatch(int match_id)
        {
            return (from usermatches in db.Matches
                    where usermatches.Result == false &&
                    usermatches.MatchId == match_id
                    select usermatches).FirstOrDefault();
        }

        public static void Main(string[] args)
        {

        }
    }
}
