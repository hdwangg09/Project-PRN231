using BusinessObject.Modals;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BookWebClient.Controllers
{
    public class PublisherController : Controller
    {
        private HttpClient _httpClient;
        private string ProductApi = string.Empty;
        public PublisherController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApi = "http://localhost:5276/publisher";
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(ProductApi);
            string dataJson = await response.Content.ReadAsStringAsync();

            List<Publisher> publisher = JsonConvert.DeserializeObject<List<Publisher>>(dataJson);
            ViewData["Publisher"] = publisher;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage response = await _httpClient.GetAsync($"{ProductApi}/details/{id}");
            string dataJson = await response.Content.ReadAsStringAsync();

            Publisher publisher = JsonConvert.DeserializeObject<Publisher>(dataJson);

            return View(publisher);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("PiblisherName,City,Country")] Publisher publisher)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index");
            }
            var content = new StringContent(JsonConvert.SerializeObject(publisher), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{ProductApi}/add", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Add publisher failed!";
            }
            else
            {
                TempData["success"] = "Add publisher successfully!";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage response = await _httpClient.GetAsync($"{ProductApi}/details/{id}");
            string dataJson = await response.Content.ReadAsStringAsync();

            Publisher publisher = JsonConvert.DeserializeObject<Publisher>(dataJson);

            return View(publisher);
        }

        [HttpPost]
        public async Task<IActionResult> Update([Bind("PibId,PiblisherName,State,City,Country")] Publisher publisher)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index");
            }
            var content = new StringContent(JsonConvert.SerializeObject(publisher), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{ProductApi}/update/{publisher.PibId}", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Update publisher failed!";
            }
            else
            {
                TempData["success"] = "Update publisher successfully!";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage response = await _httpClient.GetAsync($"{ProductApi}/details/{id}");
            string dataJson = await response.Content.ReadAsStringAsync();

            Publisher publisher = JsonConvert.DeserializeObject<Publisher>(dataJson);

            return View(publisher);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ComfirmDelete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{ProductApi}/delete/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Delete publisher failed!";
            }
            else
            {
                TempData["success"] = "Delete publisher successfully!";
            }
            return RedirectToAction("Index");
        }
        public bool IsAdmin()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("role")) || HttpContext.Session.GetString("role") == "2")
            {
                return false;
            }
            return true;
        }
    }
}
