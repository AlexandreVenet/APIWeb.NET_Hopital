using Microsoft.AspNetCore.Mvc;

using Hopital_npgsql.Models;
using Hopital_npgsql.Services;

namespace Hopital_npgsql.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UtilisateursController : ControllerBase
	{
		[HttpGet("Get all")]
		public ActionResult<List<Utilisateur>> GetAll()
		{
			return UtilisateursService.GetAll();
		}

		[HttpGet("Get all by {roleName}")]
		public ActionResult<List<Utilisateur>> GetAllByRole(string roleName)
		{
			return UtilisateursService.GetAllByRole(roleName);
		}

		[HttpGet("Get by id {id}")]
		public ActionResult<Utilisateur> GetById(int id)
		{
			Utilisateur? u = UtilisateursService.GetById(id);
			if (u == null) return Content("Rien");
			return u;
		}

		[HttpGet("Get by name {name}")]
		public ActionResult<Utilisateur> GetByName(string name)
		{
			Utilisateur? u = UtilisateursService.GetByName(name);
			if (u == null) return Content("Rien");
			return u;
		}

		[HttpPost("Post by {name} {role}")]
		public ActionResult Post(string name, string role)
		{
			Utilisateur? u = UtilisateursService.GetByName(name);
			if (u != null) Content("Déjà"); // l'utilisateur existe déjà

			int? roleId = RolesService.GetByName(role);
			if (roleId == null) Content("Rien"); // le rôle n'existe pas

			UtilisateursService.Post(name, (int) roleId);
			return CreatedAtAction("Post", new { name = name, role = roleId });
		}

		[HttpPut("Update by {id} {name} {role}")]
		public ActionResult Update(int id, string name, string role)
		{
			Utilisateur? u = UtilisateursService.GetById(id);
			if (u == null) return Content("Rien"); // utilisateur inexistant

			int? roleId = RolesService.GetByName(role);
			if (roleId == null) return Content("Rien"); // rôle inexistant

			UtilisateursService.Update(id, name, (int) roleId);
			return NoContent();
		}

		[HttpDelete("Delete by {id}")]
		public IActionResult Delete(int id)
		{
			Utilisateur? u = UtilisateursService.GetById(id);
			if (u == null) return Content("Rien"); // utilisateur inexistant

			UtilisateursService.Delete(id);
			return NoContent(); 
		}
	}
}
