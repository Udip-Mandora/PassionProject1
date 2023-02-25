using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject1.Models;

namespace PassionProject1.Controllers
{
    public class exercisesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all exercises in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all exercises in the system
        /// </returns>
        /// <example>
        /// GET: api/exercisesData/ListExercise
        /// </example>
        [HttpGet]
        public IQueryable<exercise> ListExercise()
        {
            return db.exercise;
        }

        /// <summary>
        /// Find particular exercises in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all exercises in the system.
        /// </returns>
        /// <example>
        /// GET: api/exercisesData/FindExercise/5
        /// </example>
        [ResponseType(typeof(exercise))]
        [HttpGet]
        public IHttpActionResult FindExercise(int id)
        {
            exercise exercise = db.exercise.Find(id);
            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        /// <summary>
        /// Update exercise in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all the exercises included in database.
        /// </returns>
        /// <example>
        /// POST: api/exercisesData/UpdateExercises/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateExercises(int id, exercise exercise)
        {
            Debug.WriteLine("I have reached the update issues method.");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != exercise.exerciseId)
            {
                Debug.WriteLine("Id mismatched");
                return BadRequest();
            }

            db.Entry(exercise).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!exerciseExists(id))
                {
                    Debug.WriteLine("Exercise does not exist");
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
        /// Add exercise in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: enter exercise name.
        /// </returns>
        /// <example>
        /// POST: api/exercisesData/AddExercise
        /// </example>
        [ResponseType(typeof(exercise))]
        [HttpPost]
        public IHttpActionResult AddExercise(exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.exercise.Add(exercise);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = exercise.exerciseId }, exercise);
        }

        /// <summary>
        /// Delete a particular exercise in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Exercise name in the system.
        /// </returns>
        /// <example>
        /// POST: api/exercisesData/DeleteExercise/5
        /// </example>
        [ResponseType(typeof(exercise))]
        [HttpPost]
        public IHttpActionResult DeleteExercise(int id)
        {
            exercise exercise = db.exercise.Find(id);
            if (exercise == null)
            {
                return NotFound();
            }

            db.exercise.Remove(exercise);
            db.SaveChanges();

            return Ok(exercise);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool exerciseExists(int id)
        {
            return db.exercise.Count(e => e.exerciseId == id) > 0;
        }
    }
}