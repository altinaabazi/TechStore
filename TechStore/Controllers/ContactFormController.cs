using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Models;


namespace TechStore.Controllers
{
    public class ContactController : Controller
    {
        // Kjo është një listë e thjeshtë që do të përdorim për shembujt e tanishëm
        private static List<ContactFormModel> contactForms = new List<ContactFormModel>();

        // Get action për të shfaqur formën për krijim
        // GET: Contact/Create
        public IActionResult Create()
        {
            // Inicializoni një model të ri
            var model = new ContactFormModel();
            return View(model);
        }


        // Post action për të ruajtur të dhënat e kontaktit të reja
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(ContactFormModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        contactForms.Add(model); // Shtoni në listë
        //        TempData["SuccessMessage"] = "Mesazhi u krijua me sukses!";
        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                // Inicializo `Id` si unik
                model.Id = contactForms.Count > 0 ? contactForms.Max(c => c.Id) + 1 : 1;
                contactForms.Add(model); // Shto në listë
                TempData["SuccessMessage"] = "Mesazhi u krijua me sukses!";
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Authorize]
        public IActionResult Edit(int id)
        {
            var model = contactForms.FirstOrDefault(c => c.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(int id, ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                var contactToUpdate = contactForms.FirstOrDefault(c => c.Id == id);
                if (contactToUpdate != null)
                {
                    contactToUpdate.Name = model.Name;
                    contactToUpdate.Email = model.Email;
                    contactToUpdate.Message = model.Message;
                }
                TempData["SuccessMessage"] = "Mesazhi u përditësua me sukses!";
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
            var model = contactForms.FirstOrDefault(c => c.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var contactToDelete = contactForms.FirstOrDefault(c => c.Id == id);
            if (contactToDelete != null)
            {
                contactForms.Remove(contactToDelete);
                TempData["SuccessMessage"] = "Mesazhi u fshi me sukses!";
            }
            else
            {
                TempData["ErrorMessage"] = "Mesazhi nuk u gjet!";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var model = contactForms.FirstOrDefault(c => c.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }

        // Get action për të shfaqur detajet e një kontakti
     

       

        // Get action për të shfaqur listën e të gjithë kontakteve
        public IActionResult Index()
        {
            return View(contactForms);
        }
    }
}
