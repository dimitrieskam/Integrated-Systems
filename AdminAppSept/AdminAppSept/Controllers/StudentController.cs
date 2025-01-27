using AdminAppSept.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminAppSept.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
           HttpClient client = new HttpClient();
            string URL = "http://localhost:5066/API/Admin/GetAllStudents";
            HttpResponseMessage response = client.GetAsync(URL).Result;

            var data = response.Content.ReadAsAsync<List<Student>>().Result;
            return View(data);
        }
    }
}
