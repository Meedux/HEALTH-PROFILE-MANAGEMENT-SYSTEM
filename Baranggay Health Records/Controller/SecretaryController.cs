using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Baranggay_Health_Records.Controller
{
    public class SecretaryController : ControllerContext
    {
        // GET: SecretaryController
        public ActionResult Index()
        {
            return null;
        }

        // GET: SecretaryController/Details/5
        public ActionResult Details(int id)
        {
            return null;
        }

        // GET: SecretaryController/Create
        public ActionResult Create()
        {
            return null;
        }

        // POST: SecretaryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return null;
            }
            catch
            {
                return null;
            }
        }

        // GET: SecretaryController/Edit/5
        public ActionResult Edit(int id)
        {
            return null;
        }

        // POST: SecretaryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return null;
            }
            catch
            {
                return null;
            }
        }

        // GET: SecretaryController/Delete/5
        public ActionResult Delete(int id)
        {
            return null;
        }

        // POST: SecretaryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
