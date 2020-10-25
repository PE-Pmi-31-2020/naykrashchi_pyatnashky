﻿using System;
using Npgsql;

namespace ManageDatabase
{
    class Program
    {
        static void CreateRandomStrings(int count, out string[] userStrings, out string[] matchStrings)
        {
            userStrings = new string[count];
            matchStrings = new string[count];
            Random rnd = new Random();
            for (int i = 1; i <= count; i++)
            {
                userStrings[i - 1] = "(\'user\' || (SELECT count(user_id) + " + i + " FROM \"user\"), \'" + rnd.Next(10000) + "\')";
                matchStrings[i - 1] = "((SELECT user_id FROM \"user\" WHERE login = (\'user\' || (SELECT count(user_id) - " + (i - 1) + " FROM \"user\"))), "
                    + rnd.Next(1000) + ", \'" + DateTime.Now + "\', " + rnd.Next(1000) + ", true)";
            }
        }

        static void FillUserWithTestData(string connectionString, int linesCount)
        {
            string queryString = "INSERT INTO \"user\"(login, password) VALUES";

            string[] userStrings;
            string[] matchStrings;

            CreateRandomStrings(linesCount, out userStrings, out matchStrings);
            queryString += string.Join(',', userStrings) + ";";

            queryString += "\nINSERT INTO match(user_id, duration, date_time, score, result) VALUES";
            queryString += string.Join(',', matchStrings) + ";";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    command.Prepare();
                    command.ExecuteNonQuery();
                    Console.WriteLine(linesCount + " test lines inserted into both tables");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void Main(string[] args)
        {
            string connectionString = "Server=127.0.0.1;Port=5432;Database=fifteens_database;User Id=postgres;Password=Andriy1986;";
            //FillUserWithTestData(connectionString, 30);
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var sql = "SELECT * FROM \"user\"";

                using (var cmd = new NpgsqlCommand(sql, con))
                {

                    using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    {
                        Console.WriteLine($"{rdr.GetName(0),-10} {rdr.GetName(1),-10} {rdr.GetName(2),10}");

                        while (rdr.Read())
                        {
                            Console.WriteLine($"{rdr.GetInt32(0),-10} {rdr.GetString(1),-10} {rdr.GetString(2),10}");
                        }
                    }
                }
                sql = "SELECT * from \"match\"";
                using (var cmd = new NpgsqlCommand(sql, con))
                {

                    using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    {
                        Console.WriteLine($"{rdr.GetName(0),-10} {rdr.GetName(1),-10} {rdr.GetName(2),-10} {rdr.GetName(3), -30} {rdr.GetName(4),10}");

                        while (rdr.Read())
                        {
                            Console.WriteLine($"{rdr.GetInt32(0),-10} {rdr.GetInt32(1),-10} {rdr.GetInt32(2),-10} {rdr.GetDateTime(3),-30}  {rdr.GetInt32(4),10}");
                        }
                    }
                }
            }
        }
    }
}
