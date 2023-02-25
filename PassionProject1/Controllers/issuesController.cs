using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PassionProject1.Models;

namespace PassionProject1.Controllers
{
    public class issuesController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static issuesController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44357/api/");
        }

        // GET: issues/List
        public ActionResult List()
        {
            //OBJECTIVE: communicate with our issues api to retrieve a list of issues
            // curl https://localhost:44357/api/issuesData/ListIssues

            string url = "issuesData/ListIssues";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<issues> issues = response.Content.ReadAsAsync<IEnumerable<issues>>().Result;
            Debug.WriteLine("Number of issues recieved: ");
            Debug.WriteLine(issues.Count());

            return View(issues);
        }

        // GET: issues/Details/5
        public ActionResult Details(int id)
        {
            //OBJECTIVE: communicate with our issues api to retrieve one issue
            // curl https://localhost:44357/api/issuesData/FindIssues/{id}

            string url = "issuesData/FindIssues/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            issues selectedIssues = response.Content.ReadAsAsync<issues>().Result;
            Debug.WriteLine("Name of issue: ");
            Debug.WriteLine(selectedIssues.issueName);

            return View(selectedIssues);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: issues/New
        public ActionResult New()
        {
            return View();
        }

        // POST: issues/Create
        [HttpPost]
        public ActionResult Create(issues issue)
        {
            Debug.WriteLine("The jsonpayload is:");
            //Debug.WriteLine(issue.issueName);
            //OBJECTIVE: add a new issue into our system using the API
            //curl -d @issue.json -H "Content-type:application/json" https://localhost:44357/api/issuesData/AddIssue
            string url = "issuesData/AddIssue";

            
            string jsonpayload = jss.Serialize(issue);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            { 
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: issues/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "issuesData/FindIssues/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            issues selectedIssue = response.Content.ReadAsAsync<issues>().Result;
            return View(selectedIssue);
        }

        // POST: issues/Update/5
        [HttpPost]
        public ActionResult Update(int id, issues issue)
        {
            string url = "issuesData/UpdateIssues/" + id;
            string jsonpayload = jss.Serialize(issue);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: issues/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "issuesData/FindIssues/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            issues selectedIssue = response.Content.ReadAsAsync<issues>().Result;
            return View(selectedIssue);
        }

        // POST: issues/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "issuesData/DeleteIssues/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url,content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
