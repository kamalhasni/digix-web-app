using DigiX.Web.App.Adapters;
using DigiX.Web.Global.Objects;
using DigiX.Web.Global.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DigiX.Web.App.Controllers
{
    public class HomeController : Controller
    {
        private Lazy<ProductService> _productService = new Lazy<ProductService>();
        private Lazy<CheckOutService> _checkOutService = new Lazy<CheckOutService>();

        public ActionResult Index()
        {
            try
            {
                var jsonResponseString = string.Empty;
                var xmlResponseString = string.Empty;
                using (var restAdapter = new RestAdapter(string.Format(RestAdapter.URL_FORMAT, "products")))
                {
                    restAdapter.ProceedToGet();
                    jsonResponseString = restAdapter.ResponseString;

                    restAdapter.Accepts = "text/xml";
                    restAdapter.ProceedToGet();
                    xmlResponseString = restAdapter.ResponseString;

                    ViewBag.JsonContentLength = jsonResponseString.Length;
                    ViewBag.XmlContentLength = xmlResponseString.Length;
                }

                return View(JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonResponseString));
            }
            catch { }

            return View(_productService.Value.GetProducts());
        }

        public ActionResult CheckOut()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CheckOut(string itemSkus)
        {
            if (!string.IsNullOrEmpty(itemSkus))
            {
                var orders = _checkOutService.Value.GetCheckoutOrders(itemSkus.Split(',').ToList());

                if (orders != null && orders.Any())
                    return View("CheckOut", orders);
            }

            return View(_productService.Value.GetProducts());
        }


    }
}