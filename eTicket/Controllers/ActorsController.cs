using eTicket.Data;
using eTicket.Data.Services;
using eTicket.Data.Static;
using eTicket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTicket.Controllers
{
    
    public class ActorsController : Controller
    {
        private readonly IActorsService _service;

        public ActorsController(IActorsService service)
        {
            _service = service;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get: Actors/Create
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName", "ProfilePictureURL", "Bio")] Actor actor)
        {
            if (!ModelState.IsValid)
            {
                 return View("test",actor);
            }
            await _service.AddAsync(actor);
            return RedirectToAction(nameof(Index));
        }

        //Get: Actors/Details/1
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var actorDetails= await _service.GetByIdAsync(id);

            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        //Get: Actors/Edit/1
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);

            if (actorDetails == null) return View("NotFound");

            return View(actorDetails);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("FullName", "Id", "ProfilePictureURL", "Bio")] Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);

            }
            await _service.UpdateAsync(id, actor);
            return RedirectToAction(nameof(Index));
        }

        //Get: Actors/Delete/1
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);

            if (actorDetails == null) return View("NotFound");

            return View(actorDetails);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actionDetails = await _service.GetByIdAsync(id);
            if (actionDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
