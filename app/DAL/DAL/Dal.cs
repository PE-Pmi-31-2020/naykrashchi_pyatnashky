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

        static List<Match> GetUserMatches(int id)
        {
            return (from usermatches in db.Matches
                    where usermatches.UserId == id
                    select usermatches).ToList();
        }

        static Match GetMatch()
        {
            return (from usermatches in db.Matches
                    where usermatches.Result == false
                    select usermatches).FirstOrDefault();
        }

        static void Main(string[] args)
        {

            
        }
    }
}
