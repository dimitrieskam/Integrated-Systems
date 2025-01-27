using AdminAppSept.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminAppSept.Controllers
{
    public class SemesterController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5066/API/Admin/GetAllSemesters";
            HttpResponseMessage response = client.GetAsync(URL).Result;

            var data = response.Content.ReadAsAsync<List<Semester>>().Result;
            return View(data);
        }
    }
}
