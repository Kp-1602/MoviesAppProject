using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesAppProjectFSD.Models;
using MoviesAppProjectFSD.Models.DB;
using Newtonsoft.Json;
using X.PagedList;


namespace MoviesAppProjectFSD.Controllers
{
    public class MovieController : Controller
    {
        private readonly FSWD22_sing5458_FSWD22ProjectContext _context;

        public MovieController(FSWD22_sing5458_FSWD22ProjectContext context)
        {
            _context = context;
        }

        //// GET: Movie
        //public async Task<IActionResult> Index()
        //{
        //    var fSWD22_sing5458_FSWD22ProjectContext = _context.Movies.Include(m => m.Cinema).Include(m => m.Producer);
        //    return View(await fSWD22_sing5458_FSWD22ProjectContext.ToListAsync());
        //}

        public IActionResult Index(string searchString, int? page, string sortOrder)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var pageNumber = page ?? 1; // if no page was specified in the querystring, deafult 
                                        //to the first page 

            string sql = "SELECT * FROM Movies WHERE Name LIKE @p0 "; // Contains An     
            string wrapString = "%" + searchString + "%";
            switch (sortOrder)
            {
                case "name_desc":
                    sql = sql + "ORDER BY Name DESC";
                    break;
                case "name_asc":
                    sql = sql + "ORDER BY Name ASC";
                    break;
            }

            List<Movie> movies = _context.Movies.FromSqlRaw(sql, wrapString).Include(m => m.Cinema).Include(m => m.Producer).ToList();
            return View(movies.ToPagedList(pageNumber, 6));

        }

        public string IndexAJAX(string searchString)
        {
            string sql = "SELECT * FROM Movies WHERE Name LIKE @p0"; // Contains An     
            string wrapString = "%" + searchString + "%";
            List<Movie> movies = _context.Movies.FromSqlRaw(sql, wrapString).ToList();
            string jason = JsonConvert.SerializeObject(movies);
            return jason;
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? Id)
        {
            //if (Id == null)
            //{
            //    return NotFound();
            //}

            var movie = await _context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == Id);



            var UrlList = (from Act in _context.Actors
                          join Acm in _context.ActorMovies
                          on Act.Id equals Acm.ActorId
                          join Mov in _context.Movies
                          on Acm.MovieId equals Mov.Id
                          where Mov.Id == Id
                          select new ActorMovieVM
                          {
                              PciURL = Act.ProfilePictureUrl,
                              ActorID = Act.Id,
                              ActorName = Act.FullName
                          }).SingleOrDefault();

            ViewBag.UrlList = UrlList.PciURL;
            ViewBag.ActorID = UrlList.ActorID;
            ViewBag.ActorName = UrlList.ActorName;



            //if (movie == null)
            //{
            //    return NotFound();
            //}

            return View(movie);
        }



        [Authorize(Roles = "Admin")]
        // GET: Movie/Create
        public IActionResult Create()
        {
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Id");
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id");
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,ImageUrl,StartDate,EndDate,CinemaId,ProducerId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Id", movie.CinemaId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Id", movie.CinemaId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }
        [Authorize(Roles = "Admin")]
        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,ImageUrl,StartDate,EndDate,CinemaId,ProducerId")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Id", movie.CinemaId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        [Authorize(Roles = "Admin")]
        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

    }
}