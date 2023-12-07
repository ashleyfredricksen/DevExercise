using DeveloperExercise.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Linq;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeveloperExercise.Controllers
{
    public class PermitsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostPermitInfo(Permits permits)
        {
            Console.WriteLine(JsonSerializer.Serialize(permits));
            bool isValid = AddressValidationUSPS(permits.Address);

            if (!isValid)
            {
                TempData["invalid"] = "true";
                TempData["failMessage"] = "The address is not valid, please try again.";
            }
            else
            {
                TempData["successMessage"] = "Application submitted successfully. Thank you!";
                AddToDB(permits);
            }
            return RedirectToAction("Index");
        }

        public bool AddressValidationUSPS(Address address)
        {
            XDocument XMLDoc = new XDocument(
                new XElement("AddressValidateRequest",
                    new XAttribute("USERID", "7002SNA000681"),
                    new XElement("Revision", "1"),
                        new XElement("Address",
                            new XAttribute("ID", "0"),
                            new XElement("Address1", address.Address1),
                            new XElement("Address2", address.Address1),
                            new XElement("City", address.City),
                            new XElement("State", "FL"),
                            new XElement("Zip5", address.ZipCode),
                            new XElement("Zip4", "")
                         ))
                );

            HttpResponseMessage response;
            string message = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

                    var url = "https://secure.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + XMLDoc;
                    Console.WriteLine(url);

                    response = client.GetAsync(url).Result;
                    message = response.Content.ReadAsStringAsync().Result;
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            if (message.Contains("Error")) {
                return false;
            }
            else { return true; }
        }

        public static void AddToDB(Permits permits)
        {
            var db = new PermitContext();
            db.Add(new PermitSubmitter
            {
                FirstName = permits.FirstName,
                LastName = permits.LastName,
                Email = permits.Email,
                Phone = permits.Phone,
                PermitType = (int?)permits.PermitType,
                Address1 = permits.Address.Address1,
                Address2 = permits.Address.Address2,
                City = permits.Address.City,
                ZipCode = permits.Address.ZipCode,
                County = permits.Address.County,
                State = "FL"
            });
            db.SaveChanges();

            db.Add(new PermitApplicationInfo
            {
                PermitType = (int)permits.PermitType,
                ApplicationDate = DateTime.Now,
                County = permits.Address.County
            });
            db.SaveChanges();

        }
    }
}
