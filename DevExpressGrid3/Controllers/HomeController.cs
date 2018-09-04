using DevExpress.Web.Mvc;
using DevExpressGrid3.Helpers;
using DevExpressGrid3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;

namespace DevExpressGrid3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to DevExpress Extensions for ASP.NET MVC!";

            //ViewBag.BatchEditingOptions = new BatchEditingDemoOptions();//change latte for other settings from Url
            return View(); //goes to straight to gridviewpartial
        }

        RTCRM Db = new RTCRM();
        //https://documentation.devexpress.com/AspNet/14760/ASP-NET-MVC-Extensions/Grid-View/Concepts/Binding-to-Data/Binding-to-Large-Data-Database-Server-Mode


        [ValidateInput(false)]
        public ActionResult BatchEditingPartial()
        {
            #region old 
            //var query = from main in db.new_contract_plan_productBase
            //            join a in db.new_d_product_catalogBase on main.new_link_product_id equals a.new_d_product_catalogId
            //            join b in db.new_d_product_groupsBase on main.new_link_product_group_id equals b.new_d_product_groupsId
            //            select new EditableContract
            //            {
            //                ContrGUID = main.new_contract_plan_productId,
            //                ProductGroupProduct = a.new_name,
            //                Product = b.new_name,
            //                Service1Quarter = main.new_service_1_quarter,
            //                Service2Quarter = main.new_service_2_quarter,
            //                Service3Quarter = main.new_service_3_quarter,
            //                Service4Quarter = main.new_service_4_quarter,
            //                Consult1Quarter = main.new_consulting_1_quarter,
            //                Consult2Quarter = main.new_consulting_2_quarter,
            //                Consult3Quarter = main.new_consulting_3_quarter,
            //                Consult4Quarter = main.new_consulting_4_quarter,
            //                NewServiceYear = main.new_service_year,
            //                NewConsultYear = main.new_year_sum,
            //                NewProductTotalConsult = main.new_product_sum_consulting,
            //                NewProductTotalService = main.new_product_sum_service,
            //                NewYearTotal = main.new_year_sum
            //            };
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
            #endregion
            /* var model = db.new_contract_plan_productBase;*/
            //var model = RtCrmDataProvider.GetEditableContracts();
            var model = GetEditableContracts();

            return PartialView("BatchEditingPartial", model);
            //return PartialView("_GridViewPartial", model);

        }

        // Apply all changes made on the client side to a data source.

        [HttpPost, ValidateInput(false)]
        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<EditableContract, Guid> updateValues)
        {
            // var model = RtCrmDataProvider.GetEditableContracts();
             var model = GetEditableContracts();

            // Insert all added values. 
            foreach (var contract in updateValues.Insert)
            {
                if (updateValues.IsValid(contract))
                {
                    try
                    {
                        model.Add(contract);
                        //RtCrmDataProvider.Db.SaveChanges();
                        Db.SaveChanges();

                    }
                    catch (Exception e)
                    {
                        updateValues.SetErrorText(contract, e.Message);
                    }
                }
            }
            // Update all edited values. 
            foreach (var contract in updateValues.Update)
            {
                if (updateValues.IsValid(contract))
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.ContrGUID == contract.ContrGUID);
                        if (modelItem != null)
                        {
                            this.UpdateModel(modelItem);
                            //this.UpdateModel(contract);
                            //RtCrmDataProvider.Db.SaveChanges();
                            Db.SaveChanges();

                        }
                    }
                    catch (Exception e)
                    {
                        updateValues.SetErrorText(contract, e.Message);
                    }
                }
            }

            // Delete all values that were deleted on the client side from the data source. 
            foreach (var ContrGUID in updateValues.DeleteKeys)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ContrGUID == ContrGUID);
                    if (item != null) model.Remove(item);
                    //RtCrmDataProvider.Db.SaveChanges();
                    Db.SaveChanges();

                }
                catch (Exception e)
                {
                    updateValues.SetErrorText(ContrGUID, e.Message);
                }
            }
            //return PartialView("BatchEditingPartial", model.ToList());
            return BatchEditingPartial();
        }

        internal  IList<EditableContract> GetEditableContracts()
        {
            //var contracts = (IList<EditableContract>)HttpContext.Current.Session["Contracts"];

            //if (contracts == null)
            //{
            //    IQueryable<EditableContract> query = ContractQueryAll();
            //    contracts = query.ToList();
            //    HttpContext.Current.Session["Contracts"] = contracts;
            //}
            IQueryable<EditableContract> query = ContractQueryAll();
          var  contracts = query.ToList();
            return contracts;
        }

        private IQueryable<EditableContract> ContractQueryAll()
        {
            var query = Db.new_contract_plan_productBase
                //.Include(table => table.new_d_product_catalogBase.new_name)
                //.Include(table => table.new_d_product_groupsBase.new_name)
                .Select(x => new EditableContract
                {
                    ContrGUID = x.new_contract_plan_productId,
                    ProductGroupProduct = x.new_d_product_groupsBase.new_name,
                    Product = x.new_d_product_catalogBase.new_name,
                    Service1Quarter = x.new_service_1_quarter,
                    Service2Quarter = x.new_service_2_quarter,
                    Service3Quarter = x.new_service_3_quarter,
                    Service4Quarter = x.new_service_4_quarter,
                    Consult1Quarter = x.new_consulting_1_quarter,
                    Consult2Quarter = x.new_consulting_2_quarter,
                    Consult3Quarter = x.new_consulting_3_quarter,
                    Consult4Quarter = x.new_consulting_4_quarter,
                    NewServiceYear = x.new_service_year,
                    NewConsultYear = x.new_year_sum,
                    NewProductTotalConsult = x.new_product_sum_consulting,
                    NewProductTotalService = x.new_product_sum_service,
                    NewYearTotal = x.new_year_sum

                });

            //return from main in Db.new_contract_plan_productBase
            //       join a in Db.new_d_product_catalogBase on main.new_link_product_id equals a.new_d_product_catalogId
            //       join b in Db.new_d_product_groupsBase on main.new_link_product_group_id equals b.new_d_product_groupsId
            //       select new EditableContract
            //       {
            //           ContrGUID = main.new_contract_plan_productId,
            //           ProductGroupProduct = a.new_name,
            //           Product = b.new_name,
            //           Service1Quarter = main.new_service_1_quarter,
            //           Service2Quarter = main.new_service_2_quarter,
            //           Service3Quarter = main.new_service_3_quarter,
            //           Service4Quarter = main.new_service_4_quarter,
            //           Consult1Quarter = main.new_consulting_1_quarter,
            //           Consult2Quarter = main.new_consulting_2_quarter,
            //           Consult3Quarter = main.new_consulting_3_quarter,
            //           Consult4Quarter = main.new_consulting_4_quarter,
            //           NewServiceYear = main.new_service_year,
            //           NewConsultYear = main.new_year_sum,
            //           NewProductTotalConsult = main.new_product_sum_consulting,
            //           NewProductTotalService = main.new_product_sum_service,
            //           NewYearTotal = main.new_year_sum
            //       };
        }
    }


    //[ValidateInput(false)]
    //public ActionResult BatchEditingPartial(BatchEditingDemoOptions options)
    //{
    //    ViewBag.BatchEditingOptions = options;
    //    return PartialView("BatchEditingPartial", RtCrmDataProvider.GetEditableContracts());
    //}

    //[HttpPost, ValidateInput(false)]
    //public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<EditableContract, int> updateValues, BatchEditingDemoOptions options)
    //{
    //    foreach (var product in updateValues.Insert)
    //    {
    //        if (updateValues.IsValid(product))
    //            InsertContract(product, updateValues);
    //    }
    //    foreach (var product in updateValues.Update)
    //    {
    //        if (updateValues.IsValid(product))
    //            UpdateContract(product, updateValues);
    //    }
    //    foreach (var productID in updateValues.DeleteKeys)
    //    {
    //        DeleteProduct(productID, updateValues);
    //    }
    //    return BatchEditingPartial(options);
    //}

    //private void DeleteProduct(int productID, MVCxGridViewBatchUpdateValues<EditableContract, int> updateValues)
    //{
    //    try
    //    {
    //        RtCrmDataProvider.DeleteProduct(productID);
    //    }
    //    catch (Exception e)
    //    {
    //        updateValues.SetErrorText(productID, e.Message);
    //    }
    //}

    //private void UpdateContract(EditableContract product, MVCxGridViewBatchUpdateValues<EditableContract, int> updateValues)
    //{
    //    try
    //    {
    //        RtCrmDataProvider.UpdateProduct(product);
    //    }
    //    catch (Exception e)
    //    {
    //        updateValues.SetErrorText(product, e.Message);
    //    }
    //}

    //protected void InsertContract(EditableContract contract, MVCxGridViewBatchUpdateValues<EditableContract, int> updateValues)
    //{
    //    try
    //    {
    //        RtCrmDataProvider.InsertContract(contract);
    //    }
    //    catch (Exception e)
    //    {
    //        updateValues.SetErrorText(contract, e.Message);
    //    }
    //}

    //[HttpPost, ValidateInput(false)]
    //public ActionResult GridViewPartialAddNew(DevExpressGrid3.Models.new_contract_plan_productBase item)
    //{
    //    var model = db.new_contract_plan_productBase;
    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            model.Add(item);
    //            db.SaveChanges();
    //        }
    //        catch (Exception e)
    //        {
    //            ViewData["EditError"] = e.Message;
    //        }
    //    }
    //    else
    //        ViewData["EditError"] = "Please, correct all errors.";
    //    return PartialView("_GridViewPartial", model.ToList());
    //}

    //[HttpPost, ValidateInput(false)]
    //public ActionResult GridViewPartialUpdate(DevExpressGrid3.Models.new_contract_plan_productBase item)
    //{
    //    var model = db.new_contract_plan_productBase;
    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            var modelItem = model.FirstOrDefault(it => it.new_contract_plan_productId == item.new_contract_plan_productId);
    //            if (modelItem != null)
    //            {
    //                this.UpdateModel(modelItem);
    //                db.SaveChanges();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            ViewData["EditError"] = e.Message;
    //        }
    //    }
    //    else
    //        ViewData["EditError"] = "Please, correct all errors.";
    //    return PartialView("_GridViewPartial", model.ToList());
    //}

    //[HttpPost, ValidateInput(false)]
    //public ActionResult GridViewPartialDelete(System.Guid new_contract_plan_productId)
    //{
    //    var model = db.new_contract_plan_productBase;
    //    if (new_contract_plan_productId != null)
    //    {
    //        try
    //        {
    //            var item = model.FirstOrDefault(it => it.new_contract_plan_productId == new_contract_plan_productId);
    //            if (item != null)
    //                model.Remove(item);
    //            db.SaveChanges();
    //        }
    //        catch (Exception e)
    //        {
    //            ViewData["EditError"] = e.Message;
    //        }
    //    }
    //    return PartialView("_GridViewPartial", model.ToList());
    //}
}
