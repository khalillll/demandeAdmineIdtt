using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using demandeAdmineIdtt.Models;
using demandeAdmineIdtt.ViewModels;
using Microsoft.AspNet.Identity;

namespace demandeAdmineIdtt.Controllers
{
    public class RequestController : Controller
    {
        private AppUsersDbContext db = new AppUsersDbContext();

        // GET: Request
        public ActionResult Index()
        {
            var userAuthId = User.Identity.GetUserId();
            var userRequests = db.Requests.Where(x => x.User_Id == userAuthId);

            if (User.IsInRole("Admin"))
            {
                return View(db.Requests.OrderByDescending(x => x.Flag).ThenBy(y => y.RequestDate).ToList());
            }
            else
            {
                return View(userRequests.ToList());
            }

            
        }

        // GET: Request/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Request/Create
        public ActionResult Create()
        {
            List<SelectListItem> requestFlags = new List<SelectListItem>() {
                new SelectListItem {
                    Text = "Normal", Value = "Normal"
                },
                new SelectListItem {
                    Text = "Urgent", Value = "Urgent"
                }
            };
            ViewBag.userId = User.Identity.GetUserId();
            IEnumerable<Document> documentsList = db.Documents.Where(x => x.State == "Enabled").ToList();
            ViewBag.documentsList = new SelectList(documentsList, "Id", "Type");
            ViewBag.requestFlags = requestFlags;

            return View();
        }

        // POST: Request/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequestViewModel httpRequest)
        {
            // envoie du formulaire
            Request request = new Request();

            //Select user from databse where user = user in request
            var user = db.Users.Where(x => x.Id == httpRequest.userId).FirstOrDefault();


            //Creation de liste avec les ids du documents selectionnés
            List<int> selectedDocuments = httpRequest.Documents;
                
            if (ModelState.IsValid)
            {
                //Select document from db where documentsIds = listDocuments ids from request
                var requestedDocuments = db.Documents.Where(x => selectedDocuments.Contains(x.Id)).ToList();
                request.Documents = requestedDocuments;

                request.Title = httpRequest.Title;
                request.RequestDate = httpRequest.RequestDate;
                request.Flag = httpRequest.Flag;
                request.Status = "On Hold";
                request.CreatedAt = DateTime.Now;
                request.UpdatedAt = DateTime.Now;

                request.User = user; 
                db.Requests.Add(request);
                db.SaveChanges();
                //envoi de l'email
                string currentUserId = User.Identity.GetUserId();
                user = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                request.SendMail();
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // GET: Request/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            RequestViewModel request = db.Requests.Where(x => x.Id == id).
                              Select(y => new RequestViewModel
                              {
                                  Id = y.Id,
                                  Title = y.Title,
                                  Flag = y.Flag,
                                  RequestDate = y.RequestDate,
                                  CreatedAt = y.CreatedAt,
                                  UpdatedAt = y.UpdatedAt,
                                  Documents = y.Documents.Select(k => k.Id).ToList(),
                              }).FirstOrDefault();

            //create list of flags
            List<SelectListItem> requestFlags = new List<SelectListItem>() {
                new SelectListItem {
                    Text = "Normal", Value = "Normal"
                },
                new SelectListItem {
                    Text = "Urgent", Value = "Urgent"
                }
            };
            ViewBag.requestFlags = requestFlags;

            //Get all documents with state "enabled" from db.
            List<Document> SelectedList = new List<Document>();
            IEnumerable<Document> documentsListDb = db.Documents.Where(x => x.State == "Enabled");
            ViewBag.documentsListDb = documentsListDb.ToList();

            ViewBag.userId = User.Identity.GetUserId();

            //Get the chosen documents from the request.

            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Request/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Flag,RequestDate,UpdatedAt,CreatedAt,Documents")]  RequestViewModel request)
        {
            if (ModelState.IsValid)
            {
                var dbRequest = db.Requests.Where(y => y.Id == request.Id).Include(y => y.Documents).FirstOrDefault();
                List<int> selectedDocuments = request.Documents;
                dbRequest.Title = request.Title;
                dbRequest.Flag = request.Flag;
                dbRequest.RequestDate = request.RequestDate;
                dbRequest.UpdatedAt = DateTime.Now;
                var requestedDocuments = db.Documents.Where(x => selectedDocuments.Contains(x.Id)).ToList();
                dbRequest.Documents = requestedDocuments;

                db.SaveChanges();
                //envoi de l'email
                string currentUserId = User.Identity.GetUserId();
                dbRequest.User = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                dbRequest.SendMail();
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // GET: Request/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Request/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var dbRequest = db.Requests.Where(y => y.Id == id).Include(y => y.Documents).FirstOrDefault();
            db.Requests.Remove(dbRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
