using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevExpressGrid3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to DevExpress Extensions for ASP.NET MVC!";

            return View();
        }

        DevExpressGrid3.Models.RTCRM db = new DevExpressGrid3.Models.RTCRM();
        //https://documentation.devexpress.com/AspNet/14760/ASP-NET-MVC-Extensions/Grid-View/Concepts/Binding-to-Data/Binding-to-Large-Data-Database-Server-Mode


        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var query = from main in db.new_contract_plan_productBase
                        join a in db.new_d_product_catalogBase on main.new_link_product_id equals a.new_d_product_catalogId
                        join b in db.new_d_product_groupsBase on main.new_link_product_group_id equals b.new_d_product_groupsId
                        select new EditableContract
                        {
                            ContrGUID = main.new_contract_plan_productId,
                           ProductGroupProduct = a.new_name,
                            Product = b.new_name,
                            Service1Quarter = main.new_service_1_quarter,
                            Service2Quarter = main.new_service_2_quarter,
                            Service3Quarter = main.new_service_3_quarter,
                            Service4Quarter = main.new_service_4_quarter,
                            Consult1Quarter = main.new_consulting_1_quarter,
                            Consult2Quarter = main.new_consulting_2_quarter,
                            Consult3Quarter = main.new_consulting_3_quarter,
                            Consult4Quarter = main.new_consulting_4_quarter,
                            NewServiceYear = main.new_service_year,
                            NewConsultYear = main.new_year_sum,
                            NewProductTotalConsult = main.new_product_sum_consulting,
                            NewProductTotalService = main.new_product_sum_service,
                            NewYearTotal = main.new_year_sum
                        };


            //var query = db.new_contract_plan_productBase
            //    //.Include(table => table.new_d_product_catalogBase.new_name)
            //    //.Include(table => table.new_d_product_groupsBase.new_name)
            //    .Select(x => new EditableContract
            //    {
            //        ContrGUID = x.new_contract_plan_productId,
            //        ProductGroupProduct = x.new_d_product_catalogBase.new_name,
            //        Product = x.new_d_product_groupsBase.new_name,
            //        //Service1Quarter = x.new_service_1_quarter.HasValue ? x.new_service_1_quarter.Value:
            //        //Service1Quarter = x.new_service_1_quarter.GetValueOrDefault(0m)
            //        Service1Quarter = x.new_service_1_quarter ?? Decimal.MinValue

                        //    });

                        /* var model = db.new_contract_plan_productBase;*/
            var retVal = query.ToList();
            return PartialView("_GridViewPartial", retVal);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew(DevExpressGrid3.Models.new_contract_plan_productBase item)
        {
            var model = db.new_contract_plan_productBase;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate(DevExpressGrid3.Models.new_contract_plan_productBase item)
        {
            var model = db.new_contract_plan_productBase;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.new_contract_plan_productId == item.new_contract_plan_productId);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.Guid new_contract_plan_productId)
        {
            var model = db.new_contract_plan_productBase;
            if (new_contract_plan_productId != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.new_contract_plan_productId == new_contract_plan_productId);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridViewPartial", model.ToList());
        }
    }

    public class EditableContract
    {
        public Guid ContrGUID { get; set; }
        public string Product { get; set; }
        public string ProductGroupProduct { get; set; }

        public decimal? Service1Quarter { get; set; }
        public decimal? Service2Quarter { get; set; }
        public decimal? Service3Quarter { get; set; }
        public decimal? Service4Quarter { get; set; }
        public decimal? Consult1Quarter { get; set; }
        public decimal? Consult2Quarter { get; set; }
        public decimal? Consult3Quarter { get; set; }
        public decimal? Consult4Quarter { get; set; }
        public decimal? NewServiceYear { get; set; }
        public decimal? NewConsultYear { get; set; }
        public decimal? NewProductTotalService { get; set; }
        public decimal? NewProductTotalConsult { get; set; }
        public decimal? NewYearTotal { get; set; }


    }
}