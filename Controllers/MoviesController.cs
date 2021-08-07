using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity.Validation;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies
        private ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult New()
        {
            var genre = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel()
            {
                Genre = genre
            };
            return View("MovieForm",viewModel);
        }
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);
            if (movie == null)
                return HttpNotFound();
            var viewModel = new MovieFormViewModel(movie)
            {
                //Movie = movie,
                Genre = _context.Genres.ToList()
            };
            return View("MovieForm", viewModel);
        }
        public ActionResult Index()
        {
            var movies = _context.Movies.Include(c => c.Genre).ToList();
            return View(movies);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {                    
                    Genre = _context.Genres.ToList()
                };
                return View("MovieForm", viewModel);

            }
            if (movie.Id == 0)

            {
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);
            }
            else
            {
                var MovieInDb = _context.Movies.Single(x => x.Id == movie.Id);
                MovieInDb.Name = movie.Name;
                MovieInDb.GenreId = movie.GenreId;
                //MovieInDb.DateAdded = movie.DateAdded;
                MovieInDb.ReleaseDate = movie.ReleaseDate;
                MovieInDb.NumberInStock = movie.NumberInStock;
            }
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                Console.WriteLine(e);
            }
            return RedirectToAction("Index", "Movies");
        }
        [HttpPost]
        public ActionResult Create(MovieFormViewModel movieViewModel)
        {
            if(movieViewModel.Id == 0)
            {
                Movie movie = new Movie();
                //movie.Id = (byte)movieViewModel.Id;
                movie.Name = movieViewModel.Name;
                movie.GenreId = (byte)movieViewModel.GenreId;
                //movie.DateAdded = DateTime.Now;
                movie.ReleaseDate = (DateTime)movieViewModel.ReleaseDate;
                movie.NumberInStock = (byte)movieViewModel.NumberInStock;
                _context.Movies.Add(movie);
            }
            else
            {
                var MovieInDb = _context.Movies.Single(x=>x.Id == movieViewModel.Id);
                MovieInDb.Name = movieViewModel.Name;
                MovieInDb.GenreId = (byte)movieViewModel.GenreId;
                MovieInDb.DateAdded = DateTime.Now;
                MovieInDb.ReleaseDate = (DateTime)movieViewModel.ReleaseDate;

                MovieInDb.NumberInStock = (byte)movieViewModel.NumberInStock;

            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }
        public ActionResult Details(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);
            //var customer = GetCustomers().SingleOrDefault(c => c.id == id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }
        //public ActionResult Index()
        //{
        //    //var movies = new List<Movie>
        //    //{
        //    //     new Movie{Id=1,Name ="Movie 1"}
        //    //    ,new Movie{Id=2,Name ="Movie 2"}
        //    //};
        //    var movies = GetMovies();
        //    var viewModel1 = new MovieViewModel()
        //    {
        //        Movies = movies.ToList()
        //    };
        //    return View(viewModel1);
        //    //if (!pageIndex.HasValue)
        //    //    pageIndex = 1;
        //    //if (string.IsNullOrWhiteSpace(sortBy))
        //    //    sortBy = "Name";
        //    //return Content(string.Format("PageIndex = {0} and SortBy = {1}", pageIndex, sortBy));

        //}
        //public ActionResult Random()
        //{
        //    var movie = new Movie() { Name = "Sherk!"};
        //    var customers = new List<Customer>
        //    {
        //        new Customer{Name = "Customer 1"},
        //        new Customer{Name= "Customer 2"}
        //    };
        //    var viewModel = new RandomMovieViewModel
        //    {
        //        Movie = movie,
        //        Customers = customers
        //    };
        //    return View(viewModel);
        //}
        //public ActionResult Edit(int id)
        //{
        //    return Content("Id=" + id);
        //}
        //[Route("movies/released/{year}/{month:regex(\\d{2})}")]
        //public ActionResult ByReleasedDate(int year, int month)
        //{
        //    return Content( year+"/"+ month);
        //}
        //private IEnumerable<Movie> GetMovies()
        //{
        //    return new List<Movie>
        //    {
        //        new Movie{ Id = 1,Name = "Sherk"},
        //        new Movie{ Id = 2,Name = "ABCD"}
        //    };
        //}
    }
}