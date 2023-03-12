using Microsoft.AspNetCore.Mvc;
using OgrenciProje.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace OgrenciProje.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-LH9JGSC;Initial Catalog=ogrenci;Integrated Security=True");
        public String errorMessage = "";
        public String successMesage = "";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
       public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string name, string surname, int stdNumber )
        {
            Ogrenci yeniOgrenci = new Ogrenci();
            yeniOgrenci.name = name;
            yeniOgrenci.surname = surname;
            yeniOgrenci.stdNumber = stdNumber;
            
            conn.Open();
            String query = "INSERT INTO ogrenci (name, surname, stdNumber) " +
                "VALUES (@name, @surname, @stdNumber)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", yeniOgrenci.name);
            cmd.Parameters.AddWithValue("@surname", yeniOgrenci.surname);
            cmd.Parameters.AddWithValue("@stdNumber", yeniOgrenci.stdNumber);

            cmd.ExecuteNonQuery();
            Response.Redirect("/");
                
            return View();
        }

        public IActionResult List()
        {
            String query = "SELECT * FROM ogrenci";
            SqlCommand cmd = new SqlCommand(query, conn);
            List<Ogrenci> ogrenci = new List<Ogrenci>();
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ogrenci.Add(
                    new Ogrenci
                    {
                        id = (int)dr["id"],
                        name = (string)dr["name"],
                        surname = (string)dr["surname"],
                        stdNumber = (int)dr["stdNumber"],

                    });
            }
            conn.Close();


            return View(ogrenci);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}