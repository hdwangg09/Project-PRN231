using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using BusinessObject.Modals;
using System.Net.Http.Json;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Nodes;
using System.Text;

namespace BookWebClient.Controllers
{
	public class AuthorController : Controller
	{
		private HttpClient _httpClient;
		private string ProductApi = string.Empty;
		public AuthorController()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			ProductApi = "http://localhost:5276/author";
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			HttpResponseMessage response = await _httpClient.GetAsync(ProductApi);
			string dataJson = await response.Content.ReadAsStringAsync();

			List<Author> authors = JsonConvert.DeserializeObject<List<Author>>(dataJson);
			ViewData["Authors"] = authors;

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			HttpResponseMessage response = await _httpClient.GetAsync($"{ProductApi}/details/{id}");
			string dataJson = await response.Content.ReadAsStringAsync();

			Author author = JsonConvert.DeserializeObject<Author>(dataJson);

			return View(author);
		}

		[HttpPost]
		public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,City,EmailAddress,Phone,Address")] Author author)
		{
			var content = new StringContent(JsonConvert.SerializeObject(author), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync($"{ProductApi}/add", content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["error"] = "Add author failed!";
			}
			else
			{
				TempData["success"] = "Add author successfully!";
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

			Author author = JsonConvert.DeserializeObject<Author>(dataJson);

			return View(author);
		}

		[HttpPost]
		public async Task<IActionResult> Update([Bind("AuthorId,FirstName,LastName,City,EmailAddress,Phone,Address")] Author author)
		{

			var content = new StringContent(JsonConvert.SerializeObject(author), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PutAsync($"{ProductApi}/update/{author.AuthorId}", content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["error"] = "Update author failed!";
			}
			else
			{
				TempData["success"] = "Update author successfully!";
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

			Author author = JsonConvert.DeserializeObject<Author>(dataJson);

			return View(author);
		}
		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> ComfirmDelete(int id)
		{
			HttpResponseMessage response = await _httpClient.DeleteAsync($"{ProductApi}/delete/{id}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["error"] = "Delete author failed!";
			}
			else
			{
				TempData["success"] = "Delete author successfully!";
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
