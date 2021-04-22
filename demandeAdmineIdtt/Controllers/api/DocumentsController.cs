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
using demandeAdmineIdtt.Models;

namespace demandeAdmineIdtt.Controllers.api
{
    public class DocumentsController : ApiController
    {
        private AppUsersDbContext db = new AppUsersDbContext();

        // GET: api/Documents
        public IQueryable<Document> GetDocuments()
        {
            return db.Documents;
        }

        // GET: api/Documents/5
        [ResponseType(typeof(Document))]
        public IHttpActionResult GetDocument(int id)
        {
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        // PUT: api/Documents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDocument(int id, Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.Id)
            {
                return BadRequest();
            }

            db.Entry(document).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Documents
        [ResponseType(typeof(Document))]
        public IHttpActionResult PostDocument(Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Documents.Add(document);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = document.Id }, document);
        }

        // DELETE: api/Documents/5
        [ResponseType(typeof(Document))]
        public IHttpActionResult DeleteDocument(int id)
        {
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            db.Documents.Remove(document);
            db.SaveChanges();

            return Ok(document);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DocumentExists(int id)
        {
            return db.Documents.Count(e => e.Id == id) > 0;
        }
    }
}