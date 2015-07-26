using MvcApplication1.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        private MyConnectionString db = new MyConnectionString();

        //
        // GET: /Home/

        // Autocomplete Textbox Using Database Return Value in ASP.NET Mvc3/Mvc4 Razor With Jquery
        public JsonResult AutoComplete(string Nome)
        {
            //private SchoolContext db = new SchoolContext();
            //var sugestao = from s in db.restaurante
            //               select s.nome;
            //var listaNomes = sugestao.Where(n => n.ToLower().StartsWith(Nome.ToLower()));

            //return Json(listaNomes, JsonRequestBehavior.AllowGet);

            var result = (from r in db.restaurante
                          where r.nome.ToLower().Contains(Nome.ToLower())
                          select new { r.nome }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            //return View(db.restaurante.ToList());
            var data = db.restaurante.ToList();
            return View(data);
        }

        // Index com filtro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string Nome)
        {
            if (Nome == null || Nome == "")
            {
                var data = db.restaurante.ToList();
                return View(data);
            }
            else
            {
                var data = (from c in db.restaurante
                            where c.nome.Contains(Nome)
                            select c);
                return View(data);
            }

            //If Not String.IsNullOrEmpty(strCriterio) Then
            //    estudantes = estudantes.
            //                 Where(Function(est) est.SobreNome.ToUpper().Contains(strCriterio.ToUpper()) _
            //                 OrElse est.Nome.ToUpper().Contains(strCriterio.ToUpper()))
            //End If

            //return View(db.restaurante.ToList());
            //var data = db.restaurante.ToList();
        }

        private restaurante reg = new restaurante();

        public FileContentResult RecuperaImagem(int id)
        {
            reg = db.restaurante.FirstOrDefault(i => i.cod == id);
            if (reg.imagem != null)
            {
                return File(reg.imagem, "image/jpeg");
            }
            else
            {
                return null;
            }
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id = 0)
        {
            restaurante restaurante = db.restaurante
              .Include(i => i.comentario)
              .SingleOrDefault(x => x.cod == id);

            TempData["id1"] = (int)id;

            //restaurante restaurante = db.restaurante.Find(id);
            if (restaurante == null)
            {
                return HttpNotFound();
            }
            return View(restaurante);
        }

        //
        // GET: /Home/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create
        [HttpPost]
        // PROTEGE CONTRA REQUISIÇÕES INDEVIDAS
        [ValidateAntiForgeryToken]
        public ActionResult Create(restaurante rest) //, HttpPostedFileBase file
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    int tamanho = (int)Request.Files[0].InputStream.Length;
                    byte[] arquivo = new byte[tamanho];
                    Request.Files[0].InputStream.Read(arquivo, 0, tamanho);
                    byte[] arquivoAR = arquivo;
                    rest.imagem = arquivoAR;
                }

                db.restaurante.Add(rest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rest);
        }

        //
        // GET: /Home/Edit/5
        public ActionResult Edit(int id = 0)
        {
            restaurante restaurante = db.restaurante.Find(id);
            if (restaurante == null)
            {
                return HttpNotFound();
            }
            return View(restaurante);
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(restaurante restaurante)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    int tamanho = (int)Request.Files[0].InputStream.Length;
                    byte[] arquivo = new byte[tamanho];
                    Request.Files[0].InputStream.Read(arquivo, 0, tamanho);
                    byte[] arquivoAR = arquivo;
                    restaurante.imagem = arquivoAR;
                }

                db.Entry(restaurante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(restaurante);
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id = 0)
        {
            restaurante restaurante = db.restaurante.Find(id);
            if (restaurante == null)
            {
                return HttpNotFound();
            }
            return View(restaurante);
        }

        //
        // POST: /Home/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            restaurante restaurante = db.restaurante.Find(id);
            db.restaurante.Remove(restaurante);
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