// <copyright file="DBManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// class DBManager.
    /// </summary>
    public class DBManager
    {
        private static fifteens_databaseContext db = new fifteens_databaseContext();

        /// <summary>
        /// Function to add user to the database.
        /// </summary>
        /// <param name="login"> Users login. </param>
        /// <param name="password"> Users password. </param>
        /// <returns> Users id. </returns>
        public static int AddUser(string login, string password)
        {
            User u = new User { Login = login, Password = password };
            db.Users.Add(u);
            db.SaveChanges();
            return u.UserId;
        }

        /// <summary>
        /// Function to delete user from the database.
        /// </summary>
        /// <param name="id">Users id.</param>
        public static void DeleteUser(int id)
        {
            db.Users.Remove(db.Users.Find(id));
            db.SaveChanges();
        }

        /// <summary>
        /// Function to add a match to the database.
        /// </summary>
        /// <param name="match"> Object of the class match.</param>
        public static void AddMatch(Match match)
        {
            db.Matches.Add(match);
            db.SaveChanges();
        }

        /// <summary>
        /// Function to check if the user is in the database.
        /// </summary>
        /// <param name="login"> Users login. </param>
        /// <param name="password"> Users password. </param>
        /// <param name="user_id"> Users id. </param>
        /// <returns>A bulean value if the user is in a datadase.</returns>
        public static bool LogIn(string login, string password, ref int user_id)
        {
            User user = (from users in db.Users
                         where users.Login == login &&
                         users.Password == password
                         select users).FirstOrDefault();
            if (user != null)
            {
                user_id = user.UserId;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Function to get user matches.
        /// </summary>
        /// <param name="user_id"> Users id. </param>
        /// <param name="result"> Matches result. </param>
        /// <returns>All matches of a user.</returns>
        public static List<Match> GetUserMatches(int user_id, bool result)
        {
            return (from usermatches in db.Matches
                    where usermatches.UserId == user_id &&
                    usermatches.Result == result
                    select usermatches).ToList();
        }

        /// <summary>
        /// Function to get match.
        /// </summary>
        /// <param name="match_id"> Matches id. </param>
        /// <returns>Object of a class match.</returns>
        public static Match GetMatch(int match_id)
        {
            return (from usermatches in db.Matches
                    where usermatches.Result == false &&
                    usermatches.MatchId == match_id
                    select usermatches).FirstOrDefault();
        }

        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args"> Any string. </param>
        public static void Main(string[] args)
        {
            var salt = CreateSalt("nyanlove17".Length);
            for (int i = 0; i < salt.Length; i++)
            {
                salt
            }
            GenerateSaltedHash(Encoding.ASCII.GetBytes("nyanlove17"), salt);
        }

        private static string CreateSalt(int size)
        {
            // Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        private static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }

            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
