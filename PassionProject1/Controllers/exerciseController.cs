using PassionProject1.Migrations;
using PassionProject1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using exercise = PassionProject1.Models.exercise;

namespace PassionProject1.Controllers
{
    public class exerciseController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static exerciseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44357/api/exercisesData/");
        }

        // https://localhost:44357/Exercise/List
        // GET: exercise/List
        public ActionResult List()
        {
            //OBJECTIVE: communicate with our exercises api to retrieve a list of exercises
            // curl https://localhost:44357/api/exercisesData/ListExercise
            // https://localhost:44357/api/exercisesData/listexercise
            string url = "ListExercise";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<exercise> exercises = response.Content.ReadAsAsync<IEnumerable<exercise>>().Result;
            Debug.WriteLine(message:"Number of exercises recieved: ");
            Debug.WriteLine(exercises.Count());

            return View(exercises);
        }

        // GET: exercise/Details/5
        public ActionResult Details(int id)
        {
            //OBJECTIVE: communicate with our eercises api to retrieve one exercises
            // curl https://localhost:44357/api/exercisesData/FindExercise/{id}

            string url = "FindExercise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            exercise selectedExercise = response.Content.ReadAsAsync<exercise>().Result;
            Debug.WriteLine("Name of Exercise: ");
            Debug.WriteLine(selectedExercise.exerciseName);

            return View(selectedExercise);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: exercise/New
        public ActionResult New()
        {
            return View();
        }

        // POST: exercise/Create
        [HttpPost]
        public ActionResult Create(exercise exercises)
        {
            Debug.WriteLine("The jsonpayload is:");
            //Debug.WriteLine(exercise.excerciseName);
            //OBJECTIVE: add a new exercise into our system using the API
            //curl -d @exercise.json -H "Content-type:application/json" https://localhost:44357/api/exerciseData/AddExercise
            string url = "AddExercise";


            string jsonpayload = jss.Serialize(exercises);

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

        // GET: exercise/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "FindExercise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            exercise selectedExercise = response.Content.ReadAsAsync<exercise>().Result;
            return View(selectedExercise);
        }

        // POST: exercise/Update/5
        [HttpPost]
        public ActionResult Update(int id, exercise exercise)
        {
            string url = "UpdateExercises/" + id;
            string jsonpayload = jss.Serialize(exercise);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: exercise/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindExercise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            exercise selectedExercise = response.Content.ReadAsAsync<exercise>().Result;
            return View(selectedExercise);
        }

        // POST: exercise/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteExercise/" + id;
            HttpContent content = new StringContent("");
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
    }
}
