
using AdminApplication.Models;
using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace AdminApplication.Controllers
{
    public class OrderController : Controller
    {
        public OrderController() {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        }

        //Intstall Newtonsoft and Client
        public IActionResult Index()
        {
           HttpClient client = new HttpClient();
            string URL = "http://localhost:5054/api/Admin/GetAllOrders";

            HttpResponseMessage response=client.GetAsync(URL).Result;
            var data=response.Content.ReadAsAsync<List<Order>>().Result;
            return View(data);
        }

        public IActionResult Details(string id)
        {
            HttpClient client=new HttpClient();
            string URL = "http://localhost:5054/api/Admin/GetDetailsForOrder";
            var model = new
            {
                Id = id
            };

            HttpContent content=new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            
            HttpResponseMessage response=client.PostAsync(URL, content).Result;

            var result=response.Content.ReadAsAsync<Order>().Result;

            return View(result);
        }
        public FileContentResult CreateInvoice(Guid Id)
        {
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5054/api/Admin/GetDetailsForOrder";
            var model = new
            {
                Id = Id
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var data = response.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);
            document.Content.Replace("{{OrderNumber}}", data.Id.ToString());
            document.Content.Replace("{{UserName}}", data.Owner.UserName);
            StringBuilder sb = new StringBuilder();
            var total = 0.0;
            foreach (var item in data.ProductInOrders)
            {
                sb.Append("Product " + item.OrderedProduct.Concert.ConcertName + " with quantity " + item.Quantity + " with price " + item.OrderedProduct.Price + "$");
                total += (item.Quantity * item.OrderedProduct.Price);
            }
            document.Content.Replace("{{ProductList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", total.ToString() + "$");

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportedInvoice.pdf");

        }

        [HttpGet]
        public FileContentResult ExportOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            using(var workbook=new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Orders");

                worksheet.Cell(1, 1).Value = "Order ID";
                worksheet.Cell(1, 2).Value = "Customer Username";

                HttpClient client=new HttpClient();
                string URL = "http://localhost:5054/api/Admin/GetAllOrders";
                HttpResponseMessage response=client.GetAsync(URL).Result;

                var data = response.Content.ReadAsAsync<List<Order>>().Result;

                for (int i=0;i<data.Count();i++) {
                    var order = data[i];
                    worksheet.Cell(i + 2, 1).Value = order.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = order.Owner.UserName;

                    for (int j=0;j<order.ProductInOrders.Count();j++)
                    {
                        worksheet.Cell(1, j + 3).Value = "Product - " + (j + 1);
                        worksheet.Cell(i + 2, j + 3).Value = order.ProductInOrders.ElementAt(j).OrderedProduct.Concert.ConcertName;
                    }
                }

                using (var stream=new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }

            }
        }
    }
}
