using Npgsql;

using Hopital_npgsql.Models;
using System.Globalization;
using System.Text;

namespace Hopital_npgsql.Services
{
	public class DossiersService
	{
		public static List<Dossier> GetAll()
		{
			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			List<Dossier> list = new List<Dossier>();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand("SELECT dossiers.id, dossiers.date_entree, dossiers.date_sortie, dossiers.motif, utilisateurs.nom, roles.role FROM dossiers INNER JOIN utilisateurs ON dossiers.id_utilisateur = utilisateurs.id INNER JOIN roles ON utilisateurs.id_role = roles.id;", connexion))
				{
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							DateTime? dateEntree = null;
							string dateEntreeCulture = string.Empty ;
							if(!reader.IsDBNull(1))
							{
								dateEntree = reader.GetDateTime(1);
								DateTime test = (DateTime) dateEntree;
								dateEntreeCulture = test.ToString("f", CultureInfo.GetCultureInfo("fr-FR"));
							}

							DateTime? dateSortie = null;
							string dateSortieCulture = string.Empty;
							if (!reader.IsDBNull(2))
							{
								dateSortie = reader.GetDateTime(2);
								DateTime test = (DateTime) dateSortie;
								dateSortieCulture = test.ToString("f", CultureInfo.GetCultureInfo("fr-FR"));
							}

							string motif = string.Empty;
							if (!reader.IsDBNull(3))
							{
								motif = reader.GetString(3);
							}

							Dossier x = new Dossier()
							{
								p_id = reader.GetInt32(0),
								p_dateEntree = dateEntree,
								p_dateEntreeCulture = dateEntreeCulture,
								p_dateSortie = dateSortie,
								p_dateSortieCulture = dateSortieCulture,
								p_motif = motif,
								p_utilisateurNom = reader.GetString(4),
								p_utilisateurRole = reader.GetString(5),
							};

							list.Add(x);
						}
					}
				}
			}
			return list;
		}

		public static List<Dossier> GetAllByName(string name)
		{
			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			List<Dossier> list = new List<Dossier>();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand("SELECT dossiers.id, dossiers.date_entree, dossiers.date_sortie, dossiers.motif, roles.role FROM dossiers INNER JOIN utilisateurs ON dossiers.id_utilisateur = utilisateurs.id INNER JOIN roles ON utilisateurs.id_role = roles.id WHERE utilisateurs.nom = @p1;", connexion))
				{
					cmd.Parameters.Add(new("p1", name));
					cmd.Prepare();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							DateTime? dateEntree = null;
							string dateEntreeCulture = string.Empty;
							if (!reader.IsDBNull(1))
							{
								dateEntree = reader.GetDateTime(1);
								DateTime test = (DateTime)dateEntree;
								dateEntreeCulture = test.ToString("f", CultureInfo.GetCultureInfo("fr-FR"));
							}

							DateTime? dateSortie = null;
							string dateSortieCulture = string.Empty;
							if (!reader.IsDBNull(2))
							{
								dateSortie = reader.GetDateTime(2);
								DateTime test = (DateTime)dateSortie;
								dateSortieCulture = test.ToString("f", CultureInfo.GetCultureInfo("fr-FR"));
							}

							string motif = string.Empty;
							if (!reader.IsDBNull(3))
							{
								motif = reader.GetString(3);
							}

							Dossier x = new Dossier()
							{
								p_id = reader.GetInt32(0),
								p_dateEntree = dateEntree,
								p_dateEntreeCulture = dateEntreeCulture,
								p_dateSortie = dateSortie,
								p_dateSortieCulture = dateSortieCulture,
								p_motif = motif,
								p_utilisateurNom = name,
								p_utilisateurRole = reader.GetString(4),
							};

							list.Add(x);
						}
					}
				}
			}

			return list;
		}

		public static Dossier? GetById(int id)
		{
			Dossier? x = null;

			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand("SELECT dossiers.date_entree, dossiers.date_sortie, dossiers.motif, utilisateurs.nom, roles.role FROM dossiers INNER JOIN utilisateurs ON dossiers.id_utilisateur = utilisateurs.id INNER JOIN roles ON utilisateurs.id_role = roles.id WHERE dossiers.id = @p1; ", connexion))
				{
					cmd.Parameters.Add(new("p1", id));
					cmd.Prepare();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							DateTime? dateEntree = null;
							string dateEntreeCulture = string.Empty;
							if (!reader.IsDBNull(0))
							{
								dateEntree = reader.GetDateTime(0);
								DateTime test = (DateTime) dateEntree;
								dateEntreeCulture = test.ToString("f", CultureInfo.GetCultureInfo("fr-FR"));
							}

							DateTime? dateSortie = null;
							string dateSortieCulture = string.Empty;
							if (!reader.IsDBNull(1))
							{
								dateSortie = reader.GetDateTime(1);
								DateTime test = (DateTime) dateSortie;
								dateSortieCulture = test.ToString("f", CultureInfo.GetCultureInfo("fr-FR"));
							}

							string motif = string.Empty;
							if (!reader.IsDBNull(2))
							{
								motif = reader.GetString(2);
							}

							x = new Dossier()
							{
								p_id = id,
								p_dateEntree = dateEntree,
								p_dateEntreeCulture = dateEntreeCulture,
								p_dateSortie = dateSortie,
								p_dateSortieCulture = dateSortieCulture,
								p_motif = motif,
								p_utilisateurNom = reader.GetString(3),
								p_utilisateurRole = reader.GetString(4),
							};
						}
					}
				}
			}
			return x;
		}

		public static int? Post(int userId) // synchrone car réception de données (RETURNING)
		{
			int? value = null;

			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand("INSERT INTO dossiers (id_utilisateur) VALUES(@p1) RETURNING id;", connexion))
				{
					cmd.Parameters.Add(new("p1", userId));
					cmd.Prepare();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							value = reader.GetInt32(0);
						}
					}
				}
			}
			return value;
		}

		public static bool Update(int id, DateTime? dateEntree, DateTime? dateSortie, string? motif) // synchrone car RETURNING
		{
			if (dateEntree == null && dateSortie == null && motif == null) return false;

			bool result = default;

			StringBuilder sb = new StringBuilder();
			sb.Append("UPDATE dossiers SET "); // espace de fin
			if (dateEntree != null)
			{
				sb.Append("date_entree = @p1");
				if (dateSortie != null || motif != null) sb.Append(", ");
			}
			if (dateSortie != null)
			{
				sb.Append("date_sortie = @p2");
				if (motif != null) sb.Append(", ");
			}
			if (motif != null) sb.Append("motif = @p3");
			sb.Append(" WHERE id = @p4 RETURNING TRUE;"); // espace de début
			//Console.WriteLine(sb);

			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand(sb.ToString(), connexion))
				{
					if (dateEntree != null) cmd.Parameters.Add(new("p1", dateEntree));
					if (dateSortie != null) cmd.Parameters.Add(new("p2", dateSortie));
					if (motif != null) cmd.Parameters.Add(new("p3", motif));
					cmd.Parameters.Add(new("p4", id));
					cmd.Prepare();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							result = reader.GetBoolean(0);
						}
					}
				}
			}
			return result;
		}

		// Même fonction que précédente mais avec la Helper Class : synchrone car RETURNING, étiquettes nommées
		public static bool UpdateHelperClassSyncPlaceholder(int id, DateTime? dateEntree, DateTime? dateSortie, string? motif)
		{
			if (dateEntree == null && dateSortie == null && motif == null) return false;

			bool result = default;

			// Créer la requête
			StringBuilder sb = new StringBuilder();
			sb.Append("UPDATE dossiers SET "); // espace de fin
			if (dateEntree != null)
			{
				sb.Append("date_entree = @p1");
				if (dateSortie != null || motif != null) sb.Append(", ");
			}
			if (dateSortie != null)
			{
				sb.Append("date_sortie = @p2");
				if (motif != null) sb.Append(", ");
			}
			if (motif != null) sb.Append("motif = @p3");
			sb.Append(" WHERE id = @p4 RETURNING TRUE;"); // espace de début

			//Console.WriteLine(sb);

			// Collection répertoriant les valeurs utiles pour la préparation de la requête
			List<NpgsqlParameter> preparationParams = new List<NpgsqlParameter>();
			if (dateEntree != null) preparationParams.Add(new("p1", dateEntree));
			if (dateSortie != null) preparationParams.Add(new("p2", dateSortie));
			if (motif != null) preparationParams.Add(new("p3", motif));
			preparationParams.Add(new("p4", id));

			// Lancer la requête utilisant des étiquettes de remplacement
			ConnectService.RequestSyncPlaceholders(sb.ToString(), (reader) =>
			{
				result = reader.GetBoolean(0);
			}, preparationParams);

			return result;
		}

		// Même fonction que précédente (Helper Class) : synchrone car RETURNING, étiquettes de position
		public static bool UpdateHelperClassSync(int id, DateTime? dateEntree, DateTime? dateSortie, string? motif)
		{
			if (dateEntree == null && dateSortie == null && motif == null) return false;

			bool result = default;

			// Créer la requête
			int labelNumber = 1;
			StringBuilder sb = new StringBuilder();
			sb.Append("UPDATE dossiers SET "); // espace de fin
			if (dateEntree != null)
			{
				sb.Append($"date_entree = ${labelNumber}");
				if (dateSortie != null || motif != null) sb.Append(", ");
				labelNumber++;
			}
			if (dateSortie != null)
			{
				sb.Append($"date_sortie = ${labelNumber}");
				if (motif != null) sb.Append(", ");
				labelNumber++;
			}
			if (motif != null)
			{ 
				sb.Append($"motif = ${labelNumber}");
				labelNumber++;
			}
			sb.Append($" WHERE id = ${labelNumber} RETURNING TRUE;"); // espace de début

			Console.WriteLine(sb);

			// Collection répertoriant les valeurs utiles pour la préparation de la requête
			List<Object> preparationParams = new List<Object>();
			if (dateEntree != null) preparationParams.Add(dateEntree);
			if (dateSortie != null) preparationParams.Add(dateSortie);
			if (motif != null) preparationParams.Add(motif);
			preparationParams.Add(id);

			// Lancer la requête utilisant des étiquettes de position
			ConnectService.RequestSync(sb.ToString(), (reader) =>
			{
				result = reader.GetBoolean(0);
			}, preparationParams.ToArray());

			return result;
		}


		public static bool Delete(int id) // Synchrone car RETURNING
		{
			bool result = default;

			// Connexion à bdd
			//var connString = ConnectService.DataForConnecting();

			// Requête et traitement sans factorisation
			using (var connexion = new NpgsqlConnection(ConnectService.m_connectString))
			{
				connexion.Open();

				using (var cmd = new NpgsqlCommand("DELETE FROM dossiers WHERE id = @p1 RETURNING TRUE", connexion))
				{
					cmd.Parameters.Add(new("p1", id));
					cmd.Prepare();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							result = reader.GetBoolean(0);
						}
					}
				}
			}
			return result;
		}
	}
}
