using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Webbutveckling_med_.NET___Moment_2.Models;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Webbutveckling_med_.NET___Moment_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            //View data
            ViewData["Banner"] = "Adopt a dog. Save a life. Gain a friend.";

            //Session text
            string sessionText = "Hello from session!";

            //Set session
            HttpContext.Session.SetString("session", sessionText);

            return View();
        }

        [HttpPost("/Dogs")]
        public IActionResult CreateDog(CreateDog newDog)
        {
            //Name of json file 
            var fileName = "data.json";

            //Filepath of json file
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, fileName);

            //Read json file
            var json = System.IO.File.ReadAllText(filePath);

            //Deserialize json into a list
            var dogs = JsonSerializer.Deserialize<List<Dog>>(json);

            //Initialize model
            var dog = new Dog();

            //Check if picture is uploaded
            if (newDog.Pic != null)
            {
                //Set filepath for pics folder in wwwroot
                var picsFilePath = Path.Combine(webHostEnvironment.WebRootPath, "pics");
                //Set picture path
                var picture = Path.Combine(picsFilePath, newDog.Pic.FileName);
                //Check if uploaded picture already exists
                if (!System.IO.File.Exists(picture))
                {
                    //Saves picture if upload doesn't exists
                    newDog.Pic.CopyTo(new FileStream(picture, FileMode.Create));
                }

                //Set the picture i model
                dog.Pic = newDog.Pic.FileName;
            }
            else
            {
                //If no picture is uploaded set default picture
                dog.Pic = "default.png";
            }

            //Set model
            dog.Name = newDog.Name;
            dog.Gender = newDog.Gender;
            dog.Breed = newDog.Breed;
            dog.Age = newDog.Age;            

            //Add model to list
            dogs.Add(dog);

            //Save list
            Save(dogs, filePath);

            //Return view and list
            return View("Dogs", dogs);
        }
        

        [HttpGet("/Dogs")]
        public IActionResult Dogs()
        {
            //Name of json file
            var fileName = "data.json";

            //Filepath of json file
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, fileName);

            //Initialize new list
            var dogs = new List<Dog>();
            
            //Check if json file exists
            if (System.IO.File.Exists(filePath))
            {
                //Read json
                var json = System.IO.File.ReadAllText(filePath);

                //Deserialize json into list
                dogs = JsonSerializer.Deserialize<List<Dog>>(json);
            } else
            {
                //Save and/or create json
                Save(dogs, filePath);
            }
             
            //Return view and list
            return View(dogs);
        }

        [HttpGet("/AddNewDog")]
        public IActionResult Add()
        {
            //Return view
            return View();
        }

        [HttpGet("/About")]
        public IActionResult About()
        {
            //Viewbag
            ViewBag.text = "We are an organisation that work diligently for every dog to have a loving home.";

            //Get session
            string sessionHello = HttpContext.Session.GetString("session");

            //Viewbag session
            ViewBag.session = sessionHello;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //Save method
        private void Save(List<Dog> dogs, string fileName)
        {
            //Serialize json
            var jsonString = JsonSerializer.Serialize(dogs);

            //Saves to file
            System.IO.File.WriteAllText(fileName, jsonString);
        }

    }
}