using AdminPortal.Models;
using AdminSalesPortal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
namespace AdminPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController()
        {
            _httpClient = new HttpClient();

            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Auth/Adminlogin");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Auth/Testing");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/GetAcivitiesByAgentId");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/GetAcivitiesByAppoinmentId");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/Search");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/GetFormLabel");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/GetUserList");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/GetUser");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/GetAllTeam");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/InsertTeam");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/GetTEam");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/UpdateUser");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/GetDropdownTeam");
            _httpClient.BaseAddress = new Uri("https://salestrackingmanagement.msainfotech.in/Form/InsertUser");
            _httpClient.BaseAddress = new Uri("https://dummyjson.com/posts");

        }
        [Authorize]
        public async Task<ActionResult> Index()
        {
            try
            {

                using (HttpClient httpClient = new HttpClient())
                {
                    // Add headers to bypass ModSecurity, if necessary
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Call the API endpoint asynchronously to get activities
                    HttpResponseMessage response = await httpClient.GetAsync("https://salestrackingmanagement.msainfotech.in/Form/GetAcivitiesByAgentId");

                    // Call the API endpoint asynchronously to get dropdown team
                    HttpResponseMessage responseTeam = await httpClient.GetAsync("https://salestrackingmanagement.msainfotech.in/Form/GetDropdownTeam");

                    if (response.IsSuccessStatusCode && responseTeam.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        List<AgentActivityModel> activities = JsonConvert.DeserializeObject<List<AgentActivityModel>>(jsonContent);

                        string jsonContentTeam = await responseTeam.Content.ReadAsStringAsync();
                        DropdownModel dropdownModel = JsonConvert.DeserializeObject<DropdownModel>(jsonContentTeam);

                        // Pass the activities and teams to the view
                        ViewBag.Activities = activities;
                        ViewBag.Teams = dropdownModel.Teams;
                        ViewBag.AgentNames = dropdownModel.Agents;

                        return View();
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        return View("Forbidden");
                    }
                    else
                    {
                        return new HttpStatusCodeResult(response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return View("NetworkError");
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            try
            {
                var loginData = new
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var jsonContent = JsonConvert.SerializeObject(loginData);

                // Create an HttpClient instance with custom headers
                using (var httpClient = new HttpClient())
                {
                    // Add headers to bypass ModSecurity, if necessary
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync("https://salestrackingmanagement.msainfotech.in/Auth/Adminlogin", stringContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string accessToken = await response.Content.ReadAsStringAsync();

                        // Store user authentication using Forms Authentication
                        FormsAuthentication.SetAuthCookie(model.Email, false);

                        return RedirectToAction("Index");
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();

                        if (errorMessage == "You are not an admin.")
                        {
                            ViewBag.ShowPopupMessage = true;
                            return View();
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Invalid email or password.";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "An error occurred while processing your request.";
                        return View();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = "A network error occurred while processing your request.";
                return View();
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Viewdataform(int id)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Add headers to bypass ModSecurity, if necessary
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                    // Call the API endpoint asynchronously
                    HttpResponseMessage response = await httpClient.GetAsync($"https://salestrackingmanagement.msainfotech.in/Form/ViewDataForm?appointmentID={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        //string jsonResponse = await responses.Content.ReadAsStringAsync();

                        // Deserialize JSON into the corresponding models
                        List<Field> activities = JsonConvert.DeserializeObject<List<Field>>(jsonContent);

                        return View(activities);
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        // Handle 403 Forbidden response
                        // For example, return a specific view or message indicating access is forbidden
                        return View("Forbidden");
                    }
                    else
                    {
                        // Handle other error responses
                        return new HttpStatusCodeResult(response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions
                // For example, return a specific view or message indicating a network error
                return View("NetworkError");
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Search(SearchRequestModel searchRequestModel)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Add headers to bypass ModSecurity, if necessary
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    // Add headers to bypass ModSecurity, if necessary
                    //httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                    //httpClient.DefaultRequestHeaders.Accept.Clear();
                    //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    searchRequestModel.Internet = searchRequestModel.Internet == "All" ? null : searchRequestModel.Internet;
                    searchRequestModel.CreditCheck = searchRequestModel.CreditCheck == "All" ? null : searchRequestModel.CreditCheck;
                    searchRequestModel.AgentName = searchRequestModel.AgentName == "Select Agent" ? null : searchRequestModel.AgentName;
                    searchRequestModel.TeamName = searchRequestModel.TeamName == "Select Team" ? null : searchRequestModel.TeamName;
                    HttpContent input = new StringContent(JsonConvert.SerializeObject(searchRequestModel), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync($"https://salestrackingmanagement.msainfotech.in/Form/Search", input);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as JSON string
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON string to a list of appointment objects
                        List<AgentActivityModel> appointments = JsonConvert.DeserializeObject<List<AgentActivityModel>>(jsonContent);

                        // Return the list of appointments as JSON to the client
                        return Json(appointments, JsonRequestBehavior.AllowGet); // Allow GET requests
                    }
                    else
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        // If the API request fails, return an empty list to the client
                        return Json(new List<AgentActivitiesResponseModel>(), JsonRequestBehavior.AllowGet); // Allow GET requests
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle the exception if the API request fails
                return Json(new List<AppointmentLAbel>(), JsonRequestBehavior.AllowGet); // Allow GET requests
            }
        }



        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        [HttpGet]
        public ActionResult EditForm(int id)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Add headers to bypass ModSecurity, if necessary
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    // Call the API endpoint asynchronously
                    HttpResponseMessage response = httpClient.GetAsync($"https://salestrackingmanagement.msainfotech.in/Form/GetAcivitiesByAppoinmentId?appointmentID={id}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        List<FieldModel> team = JsonConvert.DeserializeObject<List<FieldModel>>(data);

                        // Check if the list has any elements before accessing the first one
                        if (team != null && team.Count > 0)
                        {
                            // Pass the list to the view
                            return View(team);
                        }
                        else
                        {
                            // If the list is empty, handle it appropriately (e.g., display a message)
                            return View("NoData");
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        // Handle 403 Forbidden response
                        // For example, return a specific view or message indicating access is forbidden
                        return View("Forbidden");
                    }
                    else
                    {
                        // Handle other error responses
                        return new HttpStatusCodeResult(response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions
                // For example, return a specific view or message indicating a network error
                return View("NetworkError");
            }
        }
        [HttpPost]
        public async Task<ActionResult> EditForm(List<FieldModel> model)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add headers to bypass ModSecurity, if necessary
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                // Convert the list of FieldModel objects to JSON
                string jsonContent = JsonConvert.SerializeObject(model);

                // Create the request content
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                // Call the API endpoint asynchronously
                HttpResponseMessage response = await httpClient.PostAsync($"https://salestrackingmanagement.msainfotech.in/Form/Updatedata", content);

                if (response.IsSuccessStatusCode)
                {
                    // If the request is successful, redirect to the action that lists the teams
                    return RedirectToAction("Index");
                }
                else
                {
                    string json = await response.Content.ReadAsStringAsync();
                    // If the request is not successful, return to the create view with an error
                    return View("Error");
                }
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> UserList()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Add headers to bypass ModSecurity, if necessary
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await httpClient.GetAsync("https://salestrackingmanagement.msainfotech.in/Form/GetUserList");

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        List<UserModel> users = JsonConvert.DeserializeObject<List<UserModel>>(data);
                        return View(users);
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle network errors
                return View("NetworkError");
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ViewUser(int id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add headers to bypass ModSecurity, if necessary
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = httpClient.GetAsync($"https://salestrackingmanagement.msainfotech.in/Form/GetUser?usermasterID={id}").Result;
                HttpResponseMessage responseDropdown = await httpClient.GetAsync("https://salestrackingmanagement.msainfotech.in/Form/GetDropdownTeam");
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    List<UserModel> team = JsonConvert.DeserializeObject<List<UserModel>>(data);
                    return View(team.First());
                }
                else
                {
                    return View("Error");
                }
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> TeamList()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add headers to bypass ModSecurity, if necessary
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                // Call the API endpoint asynchronously
                HttpResponseMessage response = await httpClient.GetAsync($"https://salestrackingmanagement.msainfotech.in/Form/GetAllTeam");
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    List<TeamModel> users = JsonConvert.DeserializeObject<List<TeamModel>>(data);
                    return View(users);
                }
                else
                {
                    return View("Error");
                }
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult CreateTeam()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<ActionResult> CreateTeam(TeamModel model)
        //{
        //    HttpClient httpClient = new HttpClient();

        //    // Convert the TeamModel object to JSON
        //    string json = JsonConvert.SerializeObject(model);

        //    // Create the request content
        //    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

        //    // Call the API endpoint asynchronously
        //    HttpResponseMessage response = await httpClient.PostAsync("http://192.168.1.34:8088/Form/InsertTeam", content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        // If the request is successful, redirect to the action that lists the teams
        //        return RedirectToAction("ListTeams");
        //    }
        //    else
        //    {
        //        // If the request is not successful, return to the create view with an error
        //        return View("Error");
        //    }
        //}
        [Authorize]
        [HttpPost]
        public ActionResult CreateTeam(TeamModel model)
        {
            if (ModelState.IsValid)
            {
                model.dtCreatedDate = DateTime.Now;
                model.dtUpdatedDAte = DateTime.Now;
                using (HttpClient httpClient = new HttpClient())
                {
                    // Add headers to bypass ModSecurity, if necessary
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    // Convert the TeamModel object to JSON
                    string json = JsonConvert.SerializeObject(model);

                    // Create the request content
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Call the API endpoint synchronously
                    HttpResponseMessage response = httpClient.PostAsync("https://salestrackingmanagement.msainfotech.in/Form/InsertTeam", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // If the request is successful, redirect to the action that lists the teams
                        return RedirectToAction("TeamList");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "This Team Name is Already exists");


                    }
                }
            }
                return View(model);
            
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> CreateUser()
        {
            
            using (HttpClient httpClient = new HttpClient())
            {
                // Add headers to bypass ModSecurity, if necessary
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                   
                    HttpResponseMessage responseTeam = await httpClient.GetAsync("https://salestrackingmanagement.msainfotech.in/Form/GetDropdownTeam");

                    // Check if the response for dropdown is successful
                    if (responseTeam.IsSuccessStatusCode)
                    {
                        string jsonContentTeam = await responseTeam.Content.ReadAsStringAsync();
                        DropdownModel dropdownModel = JsonConvert.DeserializeObject<DropdownModel>(jsonContentTeam);
                        ViewBag.Roles = dropdownModel.Roles;
                        ViewBag.Teams = dropdownModel.Teams;
                       
                        ViewBag.Managers = dropdownModel.Managers;
                    }
                    else
                    {
                        // Initialize ViewBag items with empty lists if the API call fails
                        ViewBag.Roles = new List<RoleDropdownItem>(); // Replace 'Role' with your actual model type
                        ViewBag.Teams = new List<TeamDropdownItem>(); // Replace 'Team' with your actual model type
                        ViewBag.Managers = new List<ManagerDropdownItem>(); // Replace 'Agent' with your actual model type
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    ViewBag.Roles = new List<RoleDropdownItem>(); // Replace 'Role' with your actual model type
                    ViewBag.Teams = new List<TeamDropdownItem>(); // Replace 'Team' with your actual model type
                    ViewBag.Agents = new List<AgentDropdownItem>();  // Replace 'Agent' with your actual model type
                }
            }

            return View();
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateUser(UserModel model)
        {
            if (ModelState.IsValid)
            {
                model.dtCreatedDate = DateTime.Now;
                model.dtLastUpdatedDate = DateTime.Now;

                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string json = JsonConvert.SerializeObject(model);
                        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await httpClient.PostAsync("https://salestrackingmanagement.msainfotech.in/Form/InsertUser", content);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("UserList");
                        }
                        //else if (response.StatusCode ) 
                        //{
                        //    string errorMessage = await response.Content.ReadAsStringAsync();
                        //    ModelState.AddModelError(string.Empty, errorMessage);
                        //}
                        else
                        {
                            ModelState.AddModelError(string.Empty, "This Email is Already exists");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error: " + ex.Message;
                }
            }

            // If ModelState is not valid or if an exception occurred, retrieve dropdown data and return to the view
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseDropdown = await httpClient.GetAsync("https://salestrackingmanagement.msainfotech.in/Form/GetDropdownTeam");

                if (responseDropdown.IsSuccessStatusCode)
                {
                    string dropdownData = await responseDropdown.Content.ReadAsStringAsync();
                    DropdownModel dropdownModel = JsonConvert.DeserializeObject<DropdownModel>(dropdownData);
                    ViewBag.Roles = dropdownModel.Roles;
                    ViewBag.Teams = dropdownModel.Teams;
                    ViewBag.Managers = dropdownModel.Managers;
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to retrieve dropdown data.";
                }
            }

            return View(model);
        }



        [Authorize]
        [HttpGet]
        public ActionResult EditTeam(int id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add headers to bypass ModSecurity, if necessary
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                // Call the API endpoint synchronously
                //HttpResponseMessage response = httpClient.GetAsync("http://192.168.1.34:8088/Form/GetTEam?teamID={id}").Result;
                HttpResponseMessage response = httpClient.GetAsync($"https://salestrackingmanagement.msainfotech.in/Form/GetTEam?teamID={id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    List<TeamModel> team = JsonConvert.DeserializeObject<List<TeamModel>>(data);
                    return View(team.First());
                }
                else
                {
                    // If the request is not successful, return to the create view with an error
                    return View("Error");
                }
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditTeam(TeamModel model)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add headers to bypass ModSecurity, if necessary
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                // Convert the TeamModel object to JSON
                string json = JsonConvert.SerializeObject(model);

                // Create the request content
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Call the API endpoint synchronously
                HttpResponseMessage response = httpClient.PostAsync("https://salestrackingmanagement.msainfotech.in/Form/UpdateTeam", content).Result;


                if (response.IsSuccessStatusCode)
                {
                    // If the request is successful, redirect to the action that lists the teams
                    return RedirectToAction("TeamList");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This Team Name is Already exists");
                    return View(model);
                }
            }
        }
        [Authorize]
        public ActionResult ViewTeam(int id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add headers to bypass ModSecurity, if necessary
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                // Call the API endpoint synchronously
                //HttpResponseMessage response = httpClient.GetAsync("http://192.168.1.34:8088/Form/GetTEam?teamID={id}").Result;
                HttpResponseMessage response = httpClient.GetAsync($"https://salestrackingmanagement.msainfotech.in/Form/GetTEam?teamID={id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    List<TeamModel> team = JsonConvert.DeserializeObject<List<TeamModel>>(data);
                    return View(team.First());
                }
                else
                {
                    // If the request is not successful, return to the create view with an error
                    return View("Error");
                }
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> EditUser(int id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseUser = await httpClient.GetAsync($"https://salestrackingmanagement.msainfotech.in/Form/GetUser?usermasterID={id}");
                HttpResponseMessage responseDropdown = await httpClient.GetAsync("https://salestrackingmanagement.msainfotech.in/Form/GetDropdownTeam");

                if (responseUser.IsSuccessStatusCode && responseDropdown.IsSuccessStatusCode)
                {
                    string userData = await responseUser.Content.ReadAsStringAsync();
                    List<UserModel> userList = JsonConvert.DeserializeObject<List<UserModel>>(userData);

                    // Assuming you only expect one user in the list, you can take the first one
                    UserModel user = userList.FirstOrDefault();

                    string dropdownData = await responseDropdown.Content.ReadAsStringAsync();
                    DropdownModel dropdownModel = JsonConvert.DeserializeObject<DropdownModel>(dropdownData);

                    // Convert Roles to SelectListItem collection
                    //var rolesSelectList = dropdownModel.Roles.Select(r => new SelectListItem
                    //{
                    //    Value = r.Id.ToString(), // Assuming RoleDropdownItem has an Id property
                    //    Text = r.Name, // Assuming RoleDropdownItem has a Name property
                    //    Selected = r.Id == user.iRoleMasterID // Set Selected to true if the Id matches the user's rolemasterid
                    //});

                    //ViewBag.Roles = rolesSelectList;

                    //// Similarly, convert Teams and Agents to SelectListItem collections

                    //var teamsSelectList = dropdownModel.Teams.Select(t => new SelectListItem
                    //{
                    //    Value = t.Id.ToString(), // Assuming TeamDropdownItem has an Id property
                    //    Text = t.Name, // Assuming TeamDropdownItem has a Name property
                    //    Selected = t.Id == user.TeamMasterId // Set Selected to true if the Id matches the user's teamid
                    //});

                    //ViewBag.Teams = teamsSelectList;

                    //var agentsSelectList = dropdownModel.Agents.Select(a => new SelectListItem
                    //{
                    //    Value = a.Id.ToString(), // Assuming AgentDropdownItem has an Id property
                    //    Text = a.Name, // Assuming AgentDropdownItem has a Name property
                    //    Selected = a.Id == user.ManagerId // Set Selected to true if the Id matches the user's agentid
                    //});

                    //ViewBag.Agents = agentsSelectList;
                    ViewBag.Roles = dropdownModel.Roles;
                    ViewBag.Teams = dropdownModel.Teams;
                    ViewBag.Managers = dropdownModel.Managers;

                    return View(user);
                }



                else
                {
                    return View("Error");
                }
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> EditUser(UserModel model)
        {
            if (ModelState.IsValid)
            {
                // Update the user data
                model.dtLastUpdatedDate = DateTime.Now;

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string json = JsonConvert.SerializeObject(model);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    //try
                    //{
                    HttpResponseMessage response = await httpClient.PostAsync("https://salestrackingmanagement.msainfotech.in/Form/UpdateUser", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("UserList");
                    }
                    else
                    {
                        HttpResponseMessage responseDropdown = await httpClient.GetAsync("https://salestrackingmanagement.msainfotech.in/Form/GetDropdownTeam");
                        string dropdownData = await responseDropdown.Content.ReadAsStringAsync();
                        DropdownModel dropdownModel = JsonConvert.DeserializeObject<DropdownModel>(dropdownData);
                        ViewBag.Roles = dropdownModel.Roles;
                        ViewBag.Teams = dropdownModel.Teams;
                        ViewBag.Managers = dropdownModel.Managers;
                        ModelState.AddModelError(string.Empty, "This Email is Already exists");
                        return View(model);
                    }
                }
            }
            else
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Add headers to bypass ModSecurity, if necessary
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Convert the TeamModel object to JSON
                    string json = JsonConvert.SerializeObject(model);

                    // Create the request content
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseDropdown = await httpClient.GetAsync("https://salestrackingmanagement.msainfotech.in/Form/GetDropdownTeam");
                    string dropdownData = await responseDropdown.Content.ReadAsStringAsync();
                    DropdownModel dropdownModel = JsonConvert.DeserializeObject<DropdownModel>(dropdownData);
                    ViewBag.Roles = dropdownModel.Roles;
                    ViewBag.Teams = dropdownModel.Teams;
                    ViewBag.Managers = dropdownModel.Managers;
                    return View(model);
                }
            }
            //}
            //catch (HttpRequestException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return View(model);
            //}
        }
        [HttpGet]
        public async Task<ActionResult> DownloadFile(string filePath, string fileName)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourCustomUserAgent");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await httpClient.GetAsync($"https://salestrackingmanagement.msainfotech.in/Form/DownloadFile?filePath={filePath}&fileName={fileName}");

                if (!response.IsSuccessStatusCode)
                {

                    return View("Error");
                }

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var GetfileName = response.Content.Headers.ContentDisposition.FileNameStar;

                return File(fileBytes, "application/octet-stream", GetfileName);

            }
        }
    }

    
}
