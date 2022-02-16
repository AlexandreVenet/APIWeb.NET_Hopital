using Microsoft.AspNetCore.Mvc;

using Hopital_npgsql.Models;
using Hopital_npgsql.Services;

namespace Hopital_npgsql.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RolesController : ControllerBase
	{
		[HttpGet("Get all")]
		public ActionResult<List<Role>> GetAll()
		{
			return RolesService.GetAll();
		}

		[HttpGet("Get role by {id}")]
		public ActionResult<Role> GetById(int id)
		{
			Role r = RolesService.GetById(id);
			//if (r == null) return StatusCode(StatusCodes.Status200OK, "Rien"); // status code avec message 
			if (r == null) return Content("Rien"); // seulement un message (200 et OK implicites)
			return r;
		}

		[HttpGet("Get id by {name}")]
		public ActionResult<int> GetId(string name)
		{
			int? id = RolesService.GetByName(name);
			if(id == null) return Content("Rien"); 
			return id;
		}

		[HttpPost("Post by {name}")]
		public ActionResult Post(string name)
		{
			int? id = RolesService.GetByName(name);
			if (id != null) return Content("Déjà");

			RolesService.Post(name);
			return CreatedAtAction("Post", new { name = name });
		}

		[HttpPut("Update by {id} {role}")]
		public IActionResult Update(int? id, string role)
		{
			if (id == null || role == null || role == string.Empty) return Content("Oula");

			Role r = RolesService.GetById((int)id);
			//if (r == null) return NotFound(new { messageRetour = "Rôle introuvable." });
			if(r == null) return Content("Rien");
			
			RolesService.Update((int)id, role);
			return NoContent();
		}

		[HttpDelete("Delete by {id}")]
		public IActionResult Delete(int? id)
		{
			if (id == null) return Content("Oula");

			Role r = RolesService.GetById((int)id);
			if (r == null) return Content("Rien");

			// Tester ensuite si l'entrée n'est pas utilisée dans le reste de la bdd...

			RolesService.Delete((int)id);
			return NoContent();
		}
	}
}
