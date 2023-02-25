using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject1.Models;
using System.Diagnostics;
using System.Web.Services.Description;
using System.Runtime.CompilerServices;


namespace PassionProject1.Controllers
{
    public class issuesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all issues in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all issues in the database.
        /// </returns>
        /// <example>
        /// GET: api/issuesData/ListIssues 
        /// </example>
        [HttpGet]
        public IQueryable<issues> ListIssues()
        {
            return db.issues;
        }

        /// <summary>
        /// Returns particular issue in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: contains a specific issue and its details.
        /// </returns>
        /// <example>
        /// GET: api/issuesData/FindIssues/5
        /// </example>
        [ResponseType(typeof(issues))]
        [HttpGet]
        public IHttpActionResult FindIssues(int id)
        {
            issues issues = db.issues.Find(id);
            if (issues == null)
            {
                return NotFound();
            }

            return Ok(issues);
        }


        /// <summary>
        /// Returns exercise related to the issue in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: give exercise to particular issue
        /// </returns>
        /// <example>
        /// GET: api/issuesData/ListIssuesForExercise/5
        // </example>
        [ResponseType(typeof(issues))]
        [HttpGet]
        public IHttpActionResult ListIssuesForExercise(int id)
        {
            List<issues> issues = db.issues.Where(i => i.exercise.Any(e => e.exerciseId == id)).ToList();
            List<issues> issue = new List<issues>();
            issues.ForEach(i => issues.Add(new issues()
            {
                issueId = i.issueId,
                issueName = i.issueName,
                issueDescription = i.issueDescription
            }));

            return Ok(issues);
        }

        /// <summary>
        /// Update an issue at a time in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: issue name and issue description
        /// </returns>
        /// <example>
        /// POST: api/issuesData/UpdateIssues/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateIssues(int id, issues issues)
        {
            Debug.WriteLine("I have reached the update issues method.");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != issues.issueId)
            {
                Debug.WriteLine("Id mismatch");
                Debug.WriteLine("GET parameter:" + id);
                Debug.WriteLine("POST parameter:" + issues.issueId);
                Debug.WriteLine("POST parameter:" + issues.issueName);
                Debug.WriteLine("POST parameter:" + issues.issueDescription);
                return BadRequest();
            }

            db.Entry(issues).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!issuesExists(id))
                {
                    Debug.WriteLine("Issue does not exist");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add an issue in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Add an issue name and issue description in the system.
        /// </returns>
        /// <example>
        /// POST: api/issuesData/AddIssue
        /// </example>
        [ResponseType(typeof(issues))]
        [HttpPost]
        public IHttpActionResult AddIssue(issues issues)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.issues.Add(issues);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = issues.issueId }, issues);
        }

        /// <summary>
        /// Deletes an issue from the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: an issue with its name and description
        /// </returns>
        /// <example>
        /// POST: api/issuesData/DeleteIssues/5
        /// </example>
        [ResponseType(typeof(issues))]
        [HttpPost]
        public IHttpActionResult Deleteissues(int id)
        {
            issues issues = db.issues.Find(id);
            if (issues == null)
            {
                return NotFound();
            }

            db.issues.Remove(issues);
            db.SaveChanges();

            return Ok(issues);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool issuesExists(int id)
        {
            return db.issues.Count(e => e.issueId == id) > 0;
        }
    }
}