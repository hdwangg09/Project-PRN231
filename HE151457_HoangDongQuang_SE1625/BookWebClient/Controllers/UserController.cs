using BusinessObject.Modals;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace BookWebClient.Controllers
{
    public class UserController : Controller
    {
        private HttpClient _httpClient;
        private string ProductApi = string.Empty;
        private static int checkLogin = 0;
        public UserController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApi = "http://localhost:5276/user";
        }
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([Bind("EmailAddress,Password")] User userLogin)
        {
            var jsonData = JsonConvert.SerializeObject(userLogin);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{ProductApi}/login", content);

            HttpResponseMessage response2 = await _httpClient.PostAsync($"{ProductApi}/existEmail", content);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(data);

                HttpContext.Session.SetString("role", user.Role.RoleId.ToString());
                HttpContext.Session.SetString("username", user.FristName + " " + user.LastName);
                HttpContext.Session.SetString("userid", user.UserId.ToString());

                return RedirectToAction("Index", "Home");
            }
            else
            {
                if ((int)response2.StatusCode == 200)
                {
                    checkLogin += 1;
                    if (checkLogin == 3)
                    {
                        if (checkLogin == 3)
                        {
                            checkLogin = 0;
                        }
                        return RedirectToAction("ResetPassword", "User");
                    }
                }
                TempData["error"] = "Email or Password incorrect!";
                return View(userLogin);
            }


        }

        [HttpGet]
        [Route("ResetPassword")]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([Bind("EmailAddress,Password,FristName,LastName")] User userRegister)
        {

            var jsonData = JsonConvert.SerializeObject(userRegister);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{ProductApi}/register", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "SignUp account successfuly!";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["error"] = "Email already used!";
                return View(userRegister);
            }
        }
        [HttpGet("~/Logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeProfile(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userid"))
                || int.Parse(HttpContext.Session.GetString("userid")) != id)
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage response = await _httpClient.GetAsync($"{ProductApi}/details/{id}");
            string dataJson = await response.Content.ReadAsStringAsync();

            User user = JsonConvert.DeserializeObject<User>(dataJson);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeProfile([Bind("UserId,FristName,LastName")] User user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{ProductApi}/updateProfile", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Edit profile failed!";
            }
            else
            {
                TempData["success"] = "Edit profile successfuly!";
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userid"))
                || int.Parse(HttpContext.Session.GetString("userid")) != id)
            {
                return RedirectToAction("Index");
            }
            ViewData["Hello"] = "Hello";
            ViewData["UserIdToChangePass"] = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string userIDToChangePass, string NewPassword, string OldPassword)
        {

            HttpResponseMessage response = await _httpClient.PostAsync($"{ProductApi}/isPasswordUser?userId={userIDToChangePass}&oldPassword={OldPassword}", null);

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Password Không đúng!!";
            }
            else
            {
                if (userIDToChangePass != null)
                {

                    User userChangePass = new User();
                    userChangePass.UserId = int.Parse(userIDToChangePass);
                    userChangePass.Password = NewPassword;

                    var jsonData = JsonConvert.SerializeObject(userChangePass);

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage resChangePassword = await _httpClient.PostAsync($"{ProductApi}/changePassword", content);
                    if (resChangePassword != null)
                    {
                        TempData["success"] = "Change password successfuly!";
                    }
                }
            }
            return RedirectToAction("Login", "User");
        }
        public bool IsAdmin()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("role")) || HttpContext.Session.GetString("role") != "1")
            {
                return false;
            }
            return true;
        }

    }
}
