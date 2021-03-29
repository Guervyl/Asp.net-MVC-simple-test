using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Core.EntityFramework;

namespace Test1.Controllers
{
    public class VentasController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Ventas
        public ActionResult Index(string fecha = null)
        {
            List<Venta> ventas;

            if (fecha != null)
            {
                DateTime dt;
                if (!DateTime.TryParse(fecha, out dt))
                {
                    return new HttpNotFoundResult("Formato de fecha incorrecto.");
                }

                ventas = db.Venta.Where(v => v.fecha >= dt).ToList();
            }
            else
            {
                ventas = db.Venta.Include(v => v.proveedor).ToList();
            }

            return View(ventas);
        }

        // GET: Ventas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // GET: Ventas/Create
        public ActionResult Create(string nombreProveedor)
        {
            // Retornar 404 si no no existe este proveedor
            proveedor pv;
            var rp = ThrowIfNotFoundProveedor(nombreProveedor, out pv);
            if (rp != null)
                return rp;

            ViewBag.proveedor_nombre = pv.nombre;

            return View();
        }

        /// <summary>
        /// Buscar el proveedor por su nombre y retornar 404 si no existe.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="pv"></param>
        /// <returns></returns>
        private ActionResult ThrowIfNotFoundProveedor(string nombre, out proveedor pv)
        {
            pv = db.proveedor.Where(p => p.nombre == nombre).FirstOrDefault();

            if (pv == null)
            {
                return new HttpNotFoundResult("No existe este proveedor.");
            }

            return null;
        }

        // POST: Ventas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fecha,nombre")] Venta venta, string nombreProveedor)
        {
            proveedor pv;
            var rp = ThrowIfNotFoundProveedor(nombreProveedor, out pv);
            if (rp != null)
                return rp;

            // Validar el nombre de la venta
            if (String.IsNullOrEmpty(venta.nombre))
            {
                ModelState.AddModelError("nombre", "Por favor ingrese el nombre de la venta.");
            }

            if (ModelState.IsValid)
            {
                venta.proveedor = pv;

                db.Venta.Add(venta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.proveedor_nombre = pv.nombre;

            return View(venta);
        }

        // GET: Ventas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.proveedor_id = new SelectList(db.proveedor, "id", "nombre", venta.proveedor_id);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,proveedor_id,fecha,nombre")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.proveedor_id = new SelectList(db.proveedor, "id", "nombre", venta.proveedor_id);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Venta venta = db.Venta.Find(id);
            db.Venta.Remove(venta);
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
