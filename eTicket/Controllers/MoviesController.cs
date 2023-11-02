using eTicket.Data;
using eTicket.Data.Services;
using eTicket.Data.Static;
using eTicket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace eTicket.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;

        public MoviesController(IMoviesService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);
            return View(allMovies);
        }

        //Search
        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filterResult = allMovies.Where(n => n.Name.ToUpper().Contains(searchString.ToUpper()) || n.Description.ToUpper().Contains(searchString.ToUpper())).ToList();
                return View("Index", filterResult);
            }
            return View("Index", allMovies);
        }

        //GET: movies/details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var movieDetails = await _service.GetMoviesByIdAsync(id);
            return View(movieDetails);
        }

        //GET: movies/create
        public async Task<IActionResult> Create()
        {
            var moviesDropdownData = await _service.GetNewMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(moviesDropdownData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(moviesDropdownData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(moviesDropdownData.Actors, "Id", "FullName");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM movie)
        {
            if (!ModelState.IsValid)
            {
                var moviesDropdownData = await _service.GetNewMovieDropdownsValues();

                ViewBag.Cinemas = new SelectList(moviesDropdownData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(moviesDropdownData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(moviesDropdownData.Actors, "Id", "FullName");

                return View(movie); 
            }
            await _service.AddNewMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }


        //GET: movies/edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var movieDetails = await _service.GetMoviesByIdAsync(id);
            if (movieDetails == null) return View("NotFound");

            var response = new NewMovieVM()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                StartDate = movieDetails.StartDate,
                EndDate = movieDetails.EndDate,
                ImageURL = movieDetails.ImageURL,
                MovieCategory = movieDetails.MovieCategory,
                CinemaId = movieDetails.CinemaId,
                ProducerId = movieDetails.ProducerId,
                ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList(),
            };

            var moviesDropdownData = await _service.GetNewMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(moviesDropdownData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(moviesDropdownData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(moviesDropdownData.Actors, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMovieVM movie)
        {
            if (id != movie.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var moviesDropdownData = await _service.GetNewMovieDropdownsValues();

                ViewBag.Cinemas = new SelectList(moviesDropdownData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(moviesDropdownData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(moviesDropdownData.Actors, "Id", "FullName");

                return View(movie);
            }
            await _service.UpdateMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        //Remove Movie
        public async Task<IActionResult> Remove(int id)
        {
            var movieDetails = await _service.GetByIdAsync(id);
            if (movieDetails == null) return View("NotFound");

            await _service.DeleteMovieAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
