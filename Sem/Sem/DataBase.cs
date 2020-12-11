using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Npgsql;
using Sem.ModelsTables;

namespace Sem
{
    public class DataBase
    {
        public static List<Article> SearchFromTags(string name, string array)
        {
            var title = new StringBuilder();
            if (name != null)
            {
                title.Append("title LIKE \'%");
                title.Append(name);
                title.Append("%\'");
                title.Append(" AND ");
            }
            var tags = new StringBuilder();
            var countTags = 0;
            if (array != "Теги" && array != null)
            {
                var arr = array.Split(", ");
                countTags = arr.Length;
                tags.Append(" AND (");
                for (int i = 0; i < arr.Length; i++)
                {
                    tags.Append("tag = \'");
                    tags.Append(arr[i].ToString());
                    tags.Append("\'");
                    tags.Append(" OR ");
                }
                tags.Remove(tags.Length - 4, 4);
                tags.Append(")");
            }
            return Sem.DB_Operations.ArticleOps.GetSearchArticles(title.ToString(), tags.ToString(), countTags);

        }
        public static string ReplacingChars(string answer)
        {
            return answer.Replace("\'", "");
        }

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

        private static string connectionString { get; } = @"Server=ec2-18-203-7-163.eu-west-1.compute.amazonaws.com;Port=5432;Database=ddi0ro15so2vti;Username=dxvwtsxookmswf;Password=283aad42c6811a3de82b10865d1c8ed67f2273b17873e46bc96222c1977f6d55;SslMode=Require;Trust Server Certificate=true;";

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
