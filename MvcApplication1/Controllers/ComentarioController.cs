using MvcApplication1.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class ComentarioController : Controller
    {
        private MyConnectionString db = new MyConnectionString();

        //
        // GET: /Comentario/

        public ActionResult Index()
        {
            var comentario = db.comentario.Include(c => c.restaurante);
            return View(comentario.ToList());
        }

        //
        // GET: /Comentario/Details/5

        public ActionResult Details(int id = 0)
        {
            comentario comentario = db.comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            return View(comentario);
        }

        //
        // GET: /Comentario/Create

        public ActionResult Create()
        {
            ViewBag.cod_restaurante = new SelectList(db.restaurante, "cod", "nome");
            return View();
        }

        //
        // POST: /Comentario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(comentario comentario)
        {
            if (ModelState.IsValid)
            {
                db.comentario.Add(comentario);
                db.SaveChanges();
            }

            ViewBag.cod_restaurante = new SelectList(db.restaurante, "cod", "nome", comentario.cod_restaurante);

            // Variável do tipo TemData utilizada para armazenar o valor temporário entre os controllers
            int idreturn = (int)TempData["id1"];

            //var id = db.restaurante.AsParallel(new SelectList(db.restaurante, "cod");
            return RedirectToAction("Details", "Home", new { id = idreturn });
        }

        //
        // GET: /Comentario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            comentario comentario = db.comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            ViewBag.cod_restaurante = new SelectList(db.restaurante, "cod", "nome", comentario.cod_restaurante);
            return View(comentario);
        }

        //
        // POST: /Comentario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(comentario comentario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comentario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cod_restaurante = new SelectList(db.restaurante, "cod", "nome", comentario.cod_restaurante);
            return View(comentario);
        }

        //
        // GET: /Comentario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            comentario comentario = db.comentario.Find(id);

            if (comentario == null)
            {
                return HttpNotFound();
            }
            db.comentario.Remove(comentario);
            db.SaveChanges();
            //return RedirectToAction("Details", "Home");
            return RedirectToAction("Index");
        }

        //
        // POST: /Comentario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            comentario comentario = db.comentario.Find(id);
            db.comentario.Remove(comentario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}