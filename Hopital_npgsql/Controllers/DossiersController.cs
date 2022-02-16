using Microsoft.AspNetCore.Mvc;

using Hopital_npgsql.Models;
using Hopital_npgsql.Services;


namespace Hopital_npgsql.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DossiersController : ControllerBase
	{
		[HttpGet("Get all")]
		public ActionResult<List<Dossier>> GetAll()
		{
			return DossiersService.GetAll();
		}

		[HttpGet("Get all by {name}")]
		public ActionResult<List<Dossier>> GetAllByName(string name)
		{
			return DossiersService.GetAllByName(name);
		}

		[HttpGet("Get by id {id}")]
		public ActionResult<Dossier> GetById(int id)
		{
			Dossier? d = DossiersService.GetById(id);
			if (d == null) return Content("Rien"); // dossier introuvable
			return d;
		}

		[HttpPost("Post by {name} and get its id")]
		public ActionResult<int> Post(string name)
		{
			Utilisateur? u = UtilisateursService.GetByName(name);
			if (u == null) return Content("Rien"); // utilisateur introuvable

			int? result = DossiersService.Post(u.p_id);
			if (result == null) return Content("Oula"); // insertion ratée

			return result;
		}

		[HttpPut("Update by {id}, one or more medical values")]
		public ActionResult Update(int id, string? dateEntree, string? dateSortie, string? motif)
		{
			if (dateEntree == null && dateSortie == null && motif == null) return Content("Oula"); // passer au moins 1 valeur

			DateTime? dateEntreeDT = null;  
			if (dateEntree != null)
			{
				bool isDateCorrect = DateTime.TryParse(dateEntree, out DateTime dt);
				if (!isDateCorrect) return Content("Ouladate"); // date d'entrée pourrie
				dateEntreeDT = dt;
			}

			DateTime? dateSortieDT = null;
			if (dateSortie != null)
			{
				bool isDateCorrect = DateTime.TryParse(dateSortie, out DateTime dt);
				if (!isDateCorrect) return Content("Ouladate"); // date de sortie pourrie
				dateSortieDT = dt;
			}

			Dossier? d = DossiersService.GetById(id);
			if (d == null) return Content("Rien"); // dossier introuvable

			bool value = DossiersService.UpdateHelperClassSync(id, dateEntreeDT, dateSortieDT, motif);

			return Content(value.ToString());
		}

		[HttpDelete("Delete by {id}")]
		public ActionResult<bool> Delete(int id)
		{
			Dossier? d = DossiersService.GetById(id);
			if (d == null) return Content("Rien"); // dossier introuvable

			bool value = DossiersService.Delete(id);
			return Content("Bravo");
		}
	}
}
