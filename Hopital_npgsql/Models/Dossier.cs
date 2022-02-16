namespace Hopital_npgsql.Models
{
	public class Dossier
	{
		public int p_id { get; set; }
		public DateTime? p_dateEntree { get; set; }
		public string p_dateEntreeCulture { get; set; } = string.Empty;
		public DateTime? p_dateSortie { get; set; }
		public string p_dateSortieCulture { get; set; } = string.Empty;
		public string? p_motif { get; set; } = string.Empty;
		public string? p_utilisateurNom{ get; set; } 
		public string? p_utilisateurRole {get; set; }
	}
}
