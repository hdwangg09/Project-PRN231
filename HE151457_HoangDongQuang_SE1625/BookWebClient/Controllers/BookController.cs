using BusinessObject.Modals;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace BookWebClient.Controllers
{
	public class BookController : Controller
	{
		private HttpClient _httpClient;
		private string PublisherApi = string.Empty;
		private string AuthorApi = string.Empty;
		private string BookApi = string.Empty;
		public BookController()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			BookApi = "http://localhost:5276/book";
			PublisherApi = "http://localhost:5276/publisher";
			AuthorApi = "http://localhost:5276/author";
		}
		public async Task<IActionResult> Index()
		{
			
			HttpResponseMessage response = await _httpClient.GetAsync(BookApi);
			string dataJson = await response.Content.ReadAsStringAsync();

			List<Book> books = JsonConvert.DeserializeObject<List<Book>>(dataJson);
			ViewData["Books"] = books;

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			if (!IsAdmin())
			{
				return RedirectToAction("Index");
			}
			HttpResponseMessage response = await _httpClient.GetAsync($"{BookApi}/details/{id}");
			string dataJson = await response.Content.ReadAsStringAsync();

			Book book = JsonConvert.DeserializeObject<Book>(dataJson);

			ViewData["Publisher"] = book.Pub;
			ViewData["AuthorOwnedBook"] = book.BookAuthors.Select(ba => ba.Author).ToList();
			return View(book);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			if (!IsAdmin())
			{
				return RedirectToAction("Index");
			}
			HttpResponseMessage responsePublisher = await _httpClient.GetAsync(PublisherApi);
			string dataJsonPublisher = await responsePublisher.Content.ReadAsStringAsync();
			List<Publisher> publishers = JsonConvert.DeserializeObject<List<Publisher>>(dataJsonPublisher);

			HttpResponseMessage responseAuthor = await _httpClient.GetAsync(AuthorApi);
			string dataJsonAuthor = await responseAuthor.Content.ReadAsStringAsync();
			List<Author> authors = JsonConvert.DeserializeObject<List<Author>>(dataJsonAuthor);

			ViewBag.Publishers = publishers;
			ViewBag.Authors = authors;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Book book)
		{
			if (!IsAdmin())
			{
				return RedirectToAction("Index");
			}
			List<string> IdAuthorsRaw = Request.Form["IdAuthor"].ToList();
			List<int> IdAuthors = IdAuthorsRaw.Select(x => int.Parse(x)).ToList();
			foreach (var authorId in IdAuthors)
			{
				book.BookAuthors.Add(new BookAuthor
				{
					AuthorId = authorId,
					RoyalityPercentage = 0,
					AuthorOrder = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
				});
			}

			// Gửi yêu cầu POST để tạo sách mới
			string jsonData = JsonConvert.SerializeObject(book);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync($"{BookApi}/add", content);

			if (response.IsSuccessStatusCode)
			{
				TempData["success"] = "Create book successfully!";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Create book failed!";
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
			// Lấy book ra
			HttpResponseMessage response = await _httpClient.GetAsync($"{BookApi}/details/{id}");
			string dataJson = await response.Content.ReadAsStringAsync();

			Book book = JsonConvert.DeserializeObject<Book>(dataJson);

			//lấy toàn bộ publisher ra
			HttpResponseMessage responsePublisher = await _httpClient.GetAsync(PublisherApi);
			string dataJsonPublisher = await responsePublisher.Content.ReadAsStringAsync();
			List<Publisher> publishers = JsonConvert.DeserializeObject<List<Publisher>>(dataJsonPublisher);

			//lấy toàn bộ author ra
			HttpResponseMessage responseAuthor = await _httpClient.GetAsync(AuthorApi);
			string dataJsonAuthor = await responseAuthor.Content.ReadAsStringAsync();
			List<Author> authors = JsonConvert.DeserializeObject<List<Author>>(dataJsonAuthor);

			ViewData["Publishers"] = publishers;
			ViewData["Authors"] = authors;
			ViewData["AuthorOwnedBook"] = book.BookAuthors.Select(ba => ba.Author).ToList();
			return View(book);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, Book book)
		{
			if (!IsAdmin())
			{
				return RedirectToAction("Index");
			}
			List<string> IdAuthorsRaw = Request.Form["IdAuthor"].ToList();
			List<int> IdAuthors = IdAuthorsRaw.Select(x => int.Parse(x)).ToList();
			foreach (var authorId in IdAuthors)
			{
				book.BookAuthors.Add(new BookAuthor
				{
					AuthorId = authorId,
					RoyalityPercentage = 0,
					AuthorOrder = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
				});
			}

			string jsonData = JsonConvert.SerializeObject(book);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PutAsync($"{BookApi}/update/{id}", content);

			if (response.IsSuccessStatusCode)
			{
				TempData["success"] = "Update book successfully!";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Update book failed!";
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
			HttpResponseMessage response = await _httpClient.GetAsync($"{BookApi}/details/{id}");
            string dataJson = await response.Content.ReadAsStringAsync();

            Book book = JsonConvert.DeserializeObject<Book>(dataJson);

            ViewData["Publisher"] = book.Pub;
            ViewData["AuthorOwnedBook"] = book.BookAuthors.Select(ba => ba.Author).ToList();
            return View(book);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
			if (!IsAdmin())
			{
				return RedirectToAction("Index");
			}
			HttpResponseMessage response = await _httpClient.DeleteAsync($"{BookApi}/delete/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Delete book failed!";
            }
            TempData["success"] = "Delete book successfuly!";
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
