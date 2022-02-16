using Npgsql;

using Hopital_npgsql.Models;
using System.Diagnostics;

namespace Hopital_npgsql.Services
{
	public class RolesService
	{
		public static List<Role> GetAll()
		{
			// test de performance
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start(); 
			// ------------------------

			// Connexion à bdd
			//string connString = ConnectService.DataForConnecting(); // connexion v.1 début

			List<Role> rolesList = new List<Role>();

			// Requête et traitement sans factorisation
			//using (var connexion = new NpgsqlConnection(connString)) // connexion v.1 fin
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString)) // connexion v.2
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand("SELECT id, role FROM roles;", connexion))
				{
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							//Console.WriteLine(reader.GetInt32(0));
							//Console.WriteLine(reader.GetString(1));
							//Console.WriteLine("---------------");

							Role r = new Role()
							{
								p_id = reader.GetInt32(0),
								p_role = reader.GetString(1),
							};

							rolesList.Add(r);
						}
					}
				}
			}

			// ------------------------

			// Arrêter le test
			stopwatch.Stop();
			Console.WriteLine($"Temps en ms de la requête : {stopwatch.ElapsedMilliseconds}");

			return rolesList;
		}

		public static Role GetById(int id)
		{
			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			Role r = null;//new Role();

			// Requête et traitement sans factorisation
			//using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			//{
			//	connexion.Open();

			//	using (var cmd = new NpgsqlCommand("SELECT id, role FROM roles WHERE id=@p1", connexion))
			//	{
			//		cmd.Parameters.Add(new("p1",id)); // passer un objet avec champ et valeur
			//		cmd.Prepare();

			//		using (var reader = cmd.ExecuteReader())
			//		{
			//			while (reader.Read())
			//			{
			//				r.p_id = reader.GetInt32(0);
			//				r.p_role = reader.GetString(1);
			//			}
			//		}
			//	}
			//}

			// V.2 avec fonction factorisée de la Helper Class : synchrone, étiquette par ordre
			ConnectService.RequestSync("SELECT id, role FROM roles WHERE id=$1", (reader) =>
			 {
				 //Console.WriteLine(reader.GetHashCode()); // reader prend les valeurs du rd de la fonction appelée
				 if (reader.GetValue(0) != null)
				 {
					 r = new Role();
				 }
				 r.p_id = reader.GetInt32(0);
				 r.p_role = reader.GetString(1);
			 }, new Object[]{ id });

			return r;
		}

		public static int? GetByName(string name)
		{
			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			int? id = null;

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand("SELECT id FROM roles WHERE role=$1", connexion))
				{
					// autre façon de faire une requête préparée. Différence : pas d'étiquette nommée, seulement l'ordre
					cmd.Parameters.Add(new() { Value = name }); // $1
					cmd.Prepare();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							id = reader.GetInt32(0);
						}
					}
				}
			}

			return id;
		}

		public static void Post(string name) // n'est pas async avec la factorisation du traitement
		{
			if (name == null || name == string.Empty) return; // le champ de la table ne peut pas être null

			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			//using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			//{
			//	connexion.Open();

			//	await using (var cmd = new NpgsqlCommand("INSERT INTO roles (role) VALUES (@p1)", connexion))
			//	{
			//		cmd.Parameters.Add(new("p1", name));
			//		cmd.Prepare();
			//		await cmd.ExecuteNonQueryAsync();
			//	}
			//}

			// V.2 avec fonction factorisée de la Helper Class : Asynchrone, étiquette par ordre
			ConnectService.RequestAsync("INSERT INTO roles (role) VALUES ($1)", new Object[] {name});
		}

		public static async void Update(int id, string role) // async si non factorisée
		{
			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				await using (var cmd = new NpgsqlCommand("UPDATE roles SET role = @p1 WHERE id = @p2", connexion))
				{
					cmd.Parameters.Add(new("p1", role));
					cmd.Parameters.Add(new("p2", id));
					cmd.Prepare();
					await cmd.ExecuteNonQueryAsync();
				}
			}

			
		}

		public static void Delete(int id) // async si non factorisée
		{
			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			//using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			//{
			//	connexion.Open();

			//	await using (var cmd = new NpgsqlCommand("DELETE FROM roles WHERE id = @p1", connexion))
			//	{
			//		cmd.Parameters.Add(new("p1", id));
			//		cmd.Prepare();
			//		await cmd.ExecuteNonQueryAsync();
			//	}
			//}

			// V.2 avec fonction factorisée de la Helper Class : Asyhnchrone, étiquettes nommées
			ConnectService.RequestAsyncPlaceholders("DELETE FROM roles WHERE id = @p1", new (){ new("p1", id) });
		}
	}
}
