using Npgsql;

using Hopital_npgsql.Models;

namespace Hopital_npgsql.Services
{
	public class UtilisateursService
	{
		public static List<Utilisateur> GetAll()
		{
			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			List<Utilisateur> utilisateursList = new List<Utilisateur>();

			// Requête et traitement sans factorisation
			//using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			//{
			//	connexion.Open();

			//	using (var cmd = new NpgsqlCommand("SELECT utilisateurs.id, utilisateurs.nom, roles.role FROM utilisateurs INNER JOIN roles ON utilisateurs.id_role = roles.id; ", connexion))
			//	{
			//		using (var reader = cmd.ExecuteReader())
			//		{
			//			while (reader.Read())
			//			{
			//				Utilisateur u = new Utilisateur()
			//				{
			//					p_id = reader.GetInt32(0),
			//					p_name = reader.GetString(1),
			//					p_role = reader.GetString(2),
			//				};

			//				utilisateursList.Add(u);
			//			}
			//		}
			//	}
			//}

			// V.2 avec fonction factorisée de la Helper Class : synchrone, étiquettes par ordre (aucune utilisée)
			ConnectService.RequestSync("SELECT utilisateurs.id, utilisateurs.nom, roles.role FROM utilisateurs INNER JOIN roles ON utilisateurs.id_role = roles.id;", (reader) =>
			{
				Utilisateur u = new Utilisateur()
				{
					p_id = reader.GetInt32(0),
					p_name = reader.GetString(1),
					p_role = reader.GetString(2),
				};
				utilisateursList.Add(u);
			}); // aucune valeur à passer, donc pas de tableau en paramètre

			return utilisateursList;
		}

		public static List<Utilisateur> GetAllByRole(string roleName)
		{
			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			List<Utilisateur> list = new List<Utilisateur>();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand("SELECT utilisateurs.id, utilisateurs.nom, roles.role FROM utilisateurs INNER JOIN roles ON utilisateurs.id_role = roles.id WHERE roles.role = @p1; ", connexion))
				{
					cmd.Parameters.Add(new("p1", roleName));
					cmd.Prepare();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							Utilisateur u = new Utilisateur()
							{
								p_id = reader.GetInt32(0),
								p_name = reader.GetString(1),
								p_role = reader.GetString(2),
							};

							list.Add(u);
						}
					}
				}
			}
			return list;
		}

		public static Utilisateur? GetById(int id)
		{
			Utilisateur? item = null;

			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand("SELECT utilisateurs.nom, roles.role FROM utilisateurs INNER JOIN roles ON utilisateurs.id_role = roles.id WHERE utilisateurs.id=@p1; ",connexion))
				{
					cmd.Parameters.Add(new("p1",id));
					cmd.Prepare();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							//Console.WriteLine(reader.GetValue(0));
							//Console.WriteLine(reader.GetValue(1));
							//Console.WriteLine("---------------");

							item = new Utilisateur();
							item.p_id = id;
							item.p_name = reader.GetString(0);
							item.p_role = reader.GetString(1);
						}
					}
				}
			}
			return item;
		}

		public static Utilisateur? GetByName(string name)
		{
			Utilisateur? item = null;

			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand("SELECT utilisateurs.id, roles.role FROM utilisateurs INNER JOIN roles ON utilisateurs.id_role = roles.id WHERE utilisateurs.nom=@p1; ", connexion))
				{
					cmd.Parameters.Add(new("p1", name));
					cmd.Prepare();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							item = new Utilisateur();
							item.p_id = reader.GetInt32(0);
							item.p_name = name;
							item.p_role = reader.GetString(1);
						}
					}
				}
			}
			return item;
		}

		public static void Post(string name, int roleId)  // async si non factorisée
		{
			if (name == null || name == string.Empty) return; // champ de la table non null et non vide

			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			//using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			//{
			//	connexion.Open();

			//	await using (var cmd = new NpgsqlCommand("INSERT INTO utilisateurs (id_role, nom) VALUES(@p1, @p2);", connexion))
			//	{
			//		cmd.Parameters.Add(new("p1", roleId));
			//		cmd.Parameters.Add(new("p2", name));
			//		cmd.Prepare();
			//		await cmd.ExecuteNonQueryAsync();
			//	}
			//}

			// V.2 avec fonction factorisée de la Helper Class : Asynchrone, étiquettes de position
			ConnectService.RequestAsync("INSERT INTO utilisateurs (id_role, nom) VALUES($1, $2);", new Object[]{ roleId, name});
		}

		public static async void Update(int id, string name, int roleId)
		{
			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				await using (var cmd = new NpgsqlCommand("UPDATE utilisateurs SET nom = @p1, id_role = @p2 WHERE id = @p3;", connexion))
				{
					cmd.Parameters.Add(new("p1", name));
					cmd.Parameters.Add(new("p2", roleId));
					cmd.Parameters.Add(new("p3", id));
					cmd.Prepare();
					await cmd.ExecuteNonQueryAsync();
				}
			}
		}

		public static async void Delete(int id)
		{
			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				await using (var cmd = new NpgsqlCommand("DELETE FROM utilisateurs WHERE id = @p1", connexion))
				{
					cmd.Parameters.Add(new("p1", id));
					cmd.Prepare();
					await cmd.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
