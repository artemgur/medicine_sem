using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Npgsql;

namespace Sem
{
    public class DataBase
    {
        public static string[] pars = new[] { "login", "password", "repeat password" };

        public static byte[] salt = new byte[128/8];
        public static byte[] RandomSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static List<WebSocket> socketsUsers = new List<WebSocket>();
        private static string connectionString { get; } = @"Server=127.0.0.1;Port=5432;Database=m;User Id=postgres;Password=UUigbubv842387YVY;";

        public static HashSet<T> SelectCheck<T>(string sqlExpression)
        {

            HashSet<T> result = new HashSet<T>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
                NpgsqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные (можно сразу типизировать)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            result.Add((T)reader.GetValue(i));
                        }
                    }
                }

                reader.Close();
                connection.Close();
            }
            return result;
        }

        public static List<List<object>> Select(string sqlExpression)
        {

            List<List<object>> result = new List<List<object>>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
                NpgsqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные (можно сразу типизировать)
                    {
                        result.Add(new List<object>());
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            result[result.Count - 1].Add(reader.GetValue(i));
                        }
                    }
                }

                reader.Close();
                connection.Close();
            }
            return result;
        }

        public static bool Add(string sqlExpression)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
                NpgsqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }

                connection.Close();
            }

            return false;
        }

        public static string GenerateHash(string password, byte[] saltBytes)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        }
    }
}
