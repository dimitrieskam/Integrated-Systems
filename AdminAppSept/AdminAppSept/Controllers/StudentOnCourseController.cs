using AdminAppSept.Models;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System.Text;

namespace AdminAppSept.Controllers
{
    public class StudentOnCourseController : Controller
    {
        public StudentOnCourseController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5066/API/Admin/GetStudentOnCourses";
            HttpResponseMessage response = client.GetAsync(URL).Result;

            var data = response.Content.ReadAsAsync<List<StudentOnCourse>>().Result;
            return View(data);
        }

        public IActionResult Details(Guid Id)
        {
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5066/API/Admin/GetDetailsForStudentOnCourse";
            var model = new
            {
                Id = Id,
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var data = response.Content.ReadAsAsync<StudentOnCourse>().Result;

            return View(data);
        }
        public FileContentResult CreateInvoice(Guid Id)
        {
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5066/API/Admin/GetDetailsForStudentOnCourse";
            var model = new
            {
                Id = Id
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var data = response.Content.ReadAsAsync<StudentOnCourse>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);
            document.Content.Replace("{{CourseCode}}", data.Course.CourseCode);
            document.Content.Replace("{{CourseName}}", data.Course.CourseName);
            
            document.Content.Replace("{{StudentIndex}}",data.Student.StudentIndex);
            document.Content.Replace("{{StudentName}}", data.Student.FirstName);
            document.Content.Replace("{{SemesterCode}}", data.Semester.SemesterCode);

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportedInvoice.pdf");

        }
    }
}
