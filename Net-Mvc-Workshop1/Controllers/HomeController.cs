using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Net_Mvc_Workshop1.Models;
namespace Net_Mvc_Workshop1.Controllers
{
    public class HomeController : Controller
    {
        dbBookEntities db = new dbBookEntities();
        // GET: Home
        public ActionResult Index()
        {
            var books = db.BOOK_DATA.OrderBy(m => m.BOOK_ID).ToList();
            return View(books);
        }

        public ActionResult Create()
        {
            List<SelectListItem> bookClassList = SelectBookClassList();
            ViewBag.BOOK_CLASS_ID = bookClassList;
            return View();
        }

        [HttpPost]
        public ActionResult Create(BOOK_DATA bookData)
        {
            bookData.BOOK_STATUS = "A";
            db.BOOK_DATA.Add(bookData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int BOOK_ID)
        {
            var book = db.BOOK_DATA.Where(m => m.BOOK_ID == BOOK_ID).FirstOrDefault();
            db.BOOK_DATA.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int BOOK_ID)
        {
            var book = db.BOOK_DATA.Where(m => m.BOOK_ID == BOOK_ID).FirstOrDefault();
            List<SelectListItem> bookClassList = SelectBookClassList();
            ViewBag.BOOK_CLASS_ID = bookClassList;
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(BOOK_DATA bookData)
        {
            int BOOK_ID = bookData.BOOK_ID;
            var book = db.BOOK_DATA.Where(m => m.BOOK_ID == BOOK_ID).FirstOrDefault();
            book.BOOK_NAME = bookData.BOOK_NAME;
            book.BOOK_CLASS_ID = bookData.BOOK_CLASS_ID;
            book.BOOK_STATUS = bookData.BOOK_STATUS;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<SelectListItem> SelectBookClassList()
        {
            var bookClass = db.BOOK_CLASS.ToList();
            List<SelectListItem> bookClassList = new List<SelectListItem>();
            foreach (var item in bookClass)
            {
                bookClassList.Add(new SelectListItem()
                {
                    Value = item.BOOK_CLASS_ID,
                    Text = item.BOOK_CLASS_NAME
                });
            }
            return bookClassList;
        }
    }
}