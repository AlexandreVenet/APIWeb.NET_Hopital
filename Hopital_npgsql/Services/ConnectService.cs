using Npgsql;
using System.Text;

namespace Hopital_npgsql.Services
{
	public static class ConnectService
	{
		static private Dictionary<string, string> m_data = new Dictionary<string, string>(){
			{ "Host",       "localhost" },
			{ "Port",       "5432" },
			{ "Username",   "postgres" },
			{ "Password",   "simplon59" },
			{ "Database",   "Hopital" }
		};

		// chaîne de connexion renseignée à partir de Program.cs et appsettings.json
		public static string m_connectString = string.Empty;

		public static string DataForConnecting()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < m_data.Count; i++)
			{
				sb.Append(m_data.ElementAt(i).Key);
				sb.Append("=");
				sb.Append(m_data.ElementAt(i).Value);
				if (i < m_data.Count - 1) sb.Append(";");
			}

			return sb.ToString();
		}

		// Ce qui suit est la factorisation des fonctions de requêtes bdd
		
		// Fonction synchrone avec retour de données de la base de données
		//	- Passer la requête avec étiquette de remplacement par ordre de position ("$x")
		//	- Si valeurs à passer pour préparer la requête (tableau facultatif) :
		//		- utiliser "Object[]? args = null" et convertir 
		//		- OU BIEN "NpgsqlParameter[]? args = null" pour disposer des bons types immédiatement (mais plus de code en entrée à prévoir)
		public static void RequestSync(string request, Action<NpgsqlDataReader> Do, Object[]? args = null)
		{
			using (var connexion = new NpgsqlConnection(m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand(request, connexion))
				{
					if(args != null)
					{
						for (int i = 0; i < args.Length; i++)
						{
							// Convertir le type à l'index actuel
							Object t = Convert.ChangeType(args[i], args[i].GetType());
							// Ajouter un paramètre avec la valeur convertie
							cmd.Parameters.Add(new() { Value = t }); 
						}
					}
					cmd.Prepare();

					using (var rd = cmd.ExecuteReader())
					{
						while (rd.Read())
						{
							//Console.WriteLine(rd.GetHashCode()); // c'est rd
							// Appelons l'Action qui a un NpgsqlDataReader en paramètre.
							// Ceci n'a lieu que s'il y a une donnée à lire et que la base de données à envoyée.
							Do(rd); 
							// Ici, Do() utilise "rd" en paramètre et NON PAS son propre paramètre.
							// C'est-à-dire que C# ne prend pas rd pour un placeholder à remplacer.
							// ... C'est ce qu'on veut.
							// Ensuite, l'appelant va recevoir les valeurs de rd à la place de son reader (placeholder).
							// ... C'est ce qu'on veut.
							//Console.WriteLine(rd.GetHashCode()); // c'est toujours rd
						}
					}
				}
			}
		}

		// Fonction asynchrone sans récupération de données de la base de données
		//	- Idem fonction précédente
		public static async void RequestAsync(string request, Object[]? args = null)
		{
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				await using (var cmd = new NpgsqlCommand(request, connexion))
				{
					if (args != null)
					{
						for (int i = 0; i < args.Length; i++)
						{
							Object t = Convert.ChangeType(args[i], args[i].GetType());
							cmd.Parameters.Add(new() { Value = t });
						}
					}
					cmd.Prepare();

					await cmd.ExecuteNonQueryAsync();
				}
			}
		}

		// Version de la fonction synchrone avec une collection pour permettre la construction dynamique de requête.
		//	- Passer la requête avec étiquette de remplacement nommée ("@nom")
		//	- Utiliser en paramètre :
		//		- "Dictionary<string, Object>? args = null"
		//		- OU BIEN "List<NpgsqlParameters>? ags = null" pour disposer des objets Npgsql immédiatement (moins de traitement de données et peu de code en plus en entrée)
		public static void RequestSyncPlaceholders(string request, Action < NpgsqlDataReader> Do, List<NpgsqlParameter>? args = null)
		{
			using (var connexion = new NpgsqlConnection(m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand(request, connexion))
				{
					if (args != null)
					{
						//for (int i = 0; i < args.Count; i++)
						//{
						//	// avec dictionnaire
						//	//Object t = Convert.ChangeType(args.ElementAt(i).Value, args.ElementAt(i).Value.GetType());
						//	//cmd.Parameters.Add(new(args.ElementAt(i).Key, t));

						//	// avec liste de NpgsqlParameters
						//	cmd.Parameters.Add(args.ElementAt(i)); 
						//}

						// La liste de NpgsqlParameters peut être convertie automatiquement sans boucle
						cmd.Parameters.AddRange(args.ToArray());
					}
					cmd.Prepare();

					using (var rd = cmd.ExecuteReader())
					{
						while (rd.Read())
						{
							Do(rd);
						}
					}
				}
			}
		}

		// version asynchrone de la fonction précédente
		public static async void RequestAsyncPlaceholders(string request, List<NpgsqlParameter>? args = null)
		{
			using (var connexion = new NpgsqlConnection(m_connectString))
			{
				connexion.Open();

				await using (var cmd = new NpgsqlCommand(request, connexion))
				{
					if (args != null)
					{
						cmd.Parameters.AddRange(args.ToArray());
					}
					cmd.Prepare();

					await cmd.ExecuteNonQueryAsync();
				}
			}
		}

		
	}
}
