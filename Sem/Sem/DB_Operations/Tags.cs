using System.Threading.Tasks;
using Npgsql;

namespace Sem.DB_Operations
{
	public static class Tags
	{
		public static async Task<string[]> GetEnum(string name)
		{
			//Utilities.CheckIfTableNameValid(name);
			var query = $"SELECT enum_range(NULL::{name})";
			await using var connection = new NpgsqlConnection(DataBase.ConnectionString);
			await connection.OpenAsync();
			await using var command = new NpgsqlCommand(query, connection);
			await using var reader = await command.ExecuteReaderAsync();
			if (reader.HasRows)
			{
				//var columns = GetColumnNames(reader);
				await reader.ReadAsync();
				return (string[]) reader[0];
			}
			return null;
		}

		public static async Task<string[]> GetTags() => await GetEnum("disease_b");

	}
}