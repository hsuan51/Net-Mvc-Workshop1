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
            List<SelectListItem> bookClassList = SelectBookClassList();
            ViewBag.BOOK_CLASS_ID = bookClassList;
            List<SelectListItem> bookKeeperList = SelectBookKeeperList();
            ViewBag.BOOK_KEEPER = bookKeeperList;
            List<SelectListItem> bookStatusList = SelectBookStatusList();
            ViewBag.BOOK_STATUS = bookStatusList;
            bookStatusList.Insert(0,new SelectListItem() //加入一筆空值
            {
                Value = "",
                Text = "",
                Selected = true
            });
            bookClassList.Insert(0,new SelectListItem()  //加入一筆空值
            {
                Value = "",
                Text = "",
                Selected = true
            });
            return View();
        }

        public ActionResult Create()
        {
            List<SelectListItem> bookClassList = SelectBookClassList();
            ViewBag.BOOK_CLASS_ID = bookClassList;
            List<SelectListItem> bookStatusList = SelectBookStatusList();
            ViewBag.BOOK_STATUS = bookStatusList;
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
            List<SelectListItem> bookStatusList = SelectBookStatusList();
            ViewBag.BOOK_STATUS = bookStatusList;
            List<SelectListItem> bookKeeperList = SelectBookKeeperList();
            ViewBag.BOOK_KEEPER = bookKeeperList;
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(BOOK_DATA bookData)
        {
            int BOOK_ID = bookData.BOOK_ID;
            var book = db.BOOK_DATA.Where(m => m.BOOK_ID == BOOK_ID).FirstOrDefault();
            book.BOOK_NAME = bookData.BOOK_NAME;
            book.BOOK_CLASS_ID = bookData.BOOK_CLASS_ID;
            book.BOOK_AUTHOR = bookData.BOOK_AUTHOR;
            book.BOOK_PUBLISHER = bookData.BOOK_PUBLISHER;
            book.BOOK_NOTE = bookData.BOOK_NOTE;
            book.BOOK_BOUGHT_DATE = bookData.BOOK_BOUGHT_DATE;
            book.BOOK_STATUS = bookData.BOOK_STATUS;
            if(bookData.BOOK_KEEPER == null)
            {
                book.BOOK_KEEPER = "";
            }
            else
            {
                book.BOOK_KEEPER = bookData.BOOK_KEEPER;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Search(BOOK_DATA searchBookData)
        {
            var books = db.BOOK_DATA.OrderBy(m => m.BOOK_ID).ToList();
            List<BOOK_DATA> resultBooks = new List<BOOK_DATA>();
            if (searchBookData.BOOK_NAME == null)
            {
                searchBookData.BOOK_NAME = "";
            }
            if (searchBookData.BOOK_CLASS_ID == null)
            {
                searchBookData.BOOK_CLASS_ID = "";
            }
            if (searchBookData.BOOK_KEEPER == null)
            {
                searchBookData.BOOK_KEEPER = "";
            }
            if (searchBookData.BOOK_STATUS == null)
            {
                searchBookData.BOOK_STATUS = "";
            }
            foreach (var item in books)
            {
                if (item.BOOK_NAME.Contains(searchBookData.BOOK_NAME) && item.BOOK_CLASS_ID.Contains(searchBookData.BOOK_CLASS_ID) && item.BOOK_KEEPER.Contains(searchBookData.BOOK_KEEPER) && item.BOOK_STATUS.Contains(searchBookData.BOOK_STATUS))
                {
                    resultBooks.Add(item);
                }
            }
            return View(resultBooks);
        }

        public List<SelectListItem> SelectBookClassList()  //將BOOK_CLASS置換成中文
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

        public List<SelectListItem> SelectBookKeeperList()  //將BOOK_KEEPER置換成中文
        {
            var member = db.MEMBER_M.ToList();
            List<SelectListItem> memberList = new List<SelectListItem>();
            memberList.Add(new SelectListItem()
            {
                Value = "",
                Text = "",
                Selected = true
            });
            foreach (var item in member)
            {
                memberList.Add(new SelectListItem()
                {
                    Value = item.USER_ID,
                    Text = item.USER_CNAME
                });
            }
            return memberList;
        }

        public List<SelectListItem> SelectBookStatusList()  //將BOOK_STATUS置換成中文
        {
            var bookCode = db.BOOK_CODE.ToList();
            List<SelectListItem> bookCodeList = new List<SelectListItem>();
            foreach (var item in bookCode)
            {
                bookCodeList.Add(new SelectListItem()
                {
                    Value = item.CODE_ID,
                    Text = item.CODE_TYPE
                });
            }
            return bookCodeList;
        }
    }
}