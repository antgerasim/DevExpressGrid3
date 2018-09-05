using DevExpress.Web.Mvc;
using DevExpressGrid3.Models;
using DevExpressGrid3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DevExpressGrid3.Controllers
{
    public class HomeController : Controller
    {
        RTCRM Db = new RTCRM();
        //https://documentation.devexpress.com/AspNet/14760/ASP-NET-MVC-Extensions/Grid-View/Concepts/Binding-to-Data/Binding-to-Large-Data-Database-Server-Mode
        public ActionResult Index(string parenttableid, string contractstatus)
        {
            //http://localhost:50895/?parenttableid=3D317583-36B0-E811-80D3-DBE78F6B8753&contractstatus=2
            //ViewBag.Message = "Welcome to DevExpress Extensions for ASP.NET MVC!";
            var ptid = parenttableid == null ? "3D317583-36B0-E811-80D3-DBE78F6B8753" : parenttableid;
            var cstatus = contractstatus == null ? "1" : contractstatus;
            Session["parenttableid"] = ptid;
            //Session["contractstatus"] = cstatus;
            return View(new IndexViewModel(ptid));
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingPartial(string parenttableid)
        {
            //3D317583 - 36B0 - E811 - 80D3 - DBE78F6B8753 //my choice 359C8C83-74AC-E811-80D3-DBE78F6B8753
            var model = GetEditableViewModelContracts(new Guid(parenttableid));
            Session["contractstatus"] = model.FirstOrDefault().StatusCode;

            var urlTest = Request.RawUrl;
            if (Request.IsAjaxRequest())
            {
                var ptid = Session["parenttableid"] as string;
                var cstatus = Session["contractstatus"] as string;
                var ajaxmodel = GetEditableViewModelContracts(new Guid(ptid));

                return PartialView("BatchEditingPartial", ajaxmodel);
            }

            
            return PartialView("BatchEditingPartial", model);
        }

        // Apply all changes made on the client side to a data source.
        [HttpPost, ValidateInput(false)]
        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<EditableContract, Guid> updateValues)
        {
            var dataContextModelContracts = GetDataContextModelContracts();
            // Insert all added values. 
            foreach (var contract in updateValues.Insert)
            {
                if (updateValues.IsValid(contract))
                {
                    try
                    {
                        InsertContract(contract, dataContextModelContracts);
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
                        UpdateContract(contract);
                        Db.SaveChanges();
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
                    DeleteContract(ContrGUID, dataContextModelContracts);
                    Db.SaveChanges();
                }
                catch (Exception e)
                {
                    updateValues.SetErrorText(ContrGUID, e.Message);
                }
            }
            var parenttableid = Session["parenttableid"] as string;
            var contractstatus = Session["contractstatus"] as string;
            return BatchEditingPartial(parenttableid, contractstatus);
        }

        private void DeleteContract(Guid contrGuid, IList<new_contract_plan_productBase> dataContextModelContracts)
        {
            var dataContract = GetDataContextModelContract(contrGuid);
            if (dataContract != null)
            {
                GetDataContextModelContracts().Remove(dataContract);
            }
        }

        private void InsertContract(EditableContract editContract, IList<new_contract_plan_productBase> dataContextModelContracts)
        {
            var dataContextModelContract = new new_contract_plan_productBase();
            //initialize childs for null reference errors
            dataContextModelContract.new_d_product_groupsBase = new new_d_product_groupsBase();
            dataContextModelContract.new_d_product_catalogBase = new new_d_product_catalogBase();

            dataContextModelContract.new_contract_plan_productId = Guid.NewGuid(); //todo check if EF or db add guid automatically
            dataContextModelContract.new_d_product_groupsBase.new_name = editContract.ProductGroupProduct;
            dataContextModelContract.new_d_product_catalogBase.new_name = editContract.Product;
            dataContextModelContract.new_service_1_quarter = editContract.Service1Quarter;
            dataContextModelContract.new_consulting_1_quarter = editContract.Consult1Quarter;
            //dataContextModelContracts.Add(dataContextModelContract);
            Db.new_contract_plan_productBase.ToList().Add(dataContextModelContract);
        }

        private void UpdateContract(EditableContract editContract)
        {
            var dataContract = Db.new_contract_plan_productBase.FirstOrDefault(it => it.new_contract_plan_productId == editContract.ContrGuid);

            if (dataContract != null)
            {
                /*mapping from viewmodel to dataModel*/
                dataContract.new_d_product_groupsBase.new_name = editContract.ProductGroupProduct ?? ""; //null check 
                dataContract.new_d_product_catalogBase.new_name = editContract.Product ?? ""; // null check
                dataContract.new_service_1_quarter = editContract.Service1Quarter;
                dataContract.new_consulting_1_quarter = editContract.Consult1Quarter;
            }
        }

        private IList<EditableContract> GetEditableViewModelContracts(Guid filterId)
        {
            #region HttpSessionExample
            //var contracts = (IList<EditableContract>)HttpContext.Current.Session["Contracts"];

            //if (contracts == null)
            //{
            //    IQueryable<EditableContract> query = ContractQueryAll();
            //    contracts = query.ToList();
            //    HttpContext.Current.Session["Contracts"] = contracts;
            //}
            #endregion
            #region oldjoins
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
            #endregion
            //IQueryable<EditableContract> query = Db.new_contract_plan_productBase.Where(x=> x.new_link_contract_plan_year_id == filterId)
            IQueryable<EditableContract> query = Db.new_contract_plan_productBase
                //.Where(x => x.new_link_contract_plan_year_id == filterId)
                .Select(dataContextModelContract => new EditableContract
                {
                    ContrGuid = dataContextModelContract.new_contract_plan_productId,
                    ProductGroupProduct = dataContextModelContract.new_d_product_groupsBase.new_name,
                    Product = dataContextModelContract.new_d_product_catalogBase.new_name,
                    Service1Quarter = dataContextModelContract.new_service_1_quarter,
                    Service2Quarter = dataContextModelContract.new_service_2_quarter,
                    Service3Quarter = dataContextModelContract.new_service_3_quarter,
                    Service4Quarter = dataContextModelContract.new_service_4_quarter,
                    Consult1Quarter = dataContextModelContract.new_consulting_1_quarter,
                    Consult2Quarter = dataContextModelContract.new_consulting_2_quarter,
                    Consult3Quarter = dataContextModelContract.new_consulting_3_quarter,
                    Consult4Quarter = dataContextModelContract.new_consulting_4_quarter,
                    NewServiceYear = dataContextModelContract.new_service_year,
                    NewConsultYear = dataContextModelContract.new_year_sum,
                    NewProductTotalConsult = dataContextModelContract.new_product_sum_consulting,
                    NewProductTotalService = dataContextModelContract.new_product_sum_service,
                    NewYearTotal = dataContextModelContract.new_year_sum,
                    FirstQuartalTotal = dataContextModelContract.new_service_1_quarter + dataContextModelContract.new_consulting_1_quarter,
                    SecondQuartalTotal = dataContextModelContract.new_service_2_quarter + dataContextModelContract.new_consulting_2_quarter,
                    ThirdQuartalTotal = dataContextModelContract.new_service_3_quarter + dataContextModelContract.new_consulting_3_quarter,
                    FourthQuartalTotal = dataContextModelContract.new_service_4_quarter + dataContextModelContract.new_consulting_4_quarter,
                    StatusCode = dataContextModelContract.statuscode
                });
            var contracts = query.ToList();
            return contracts;
        }

        private IList<new_contract_plan_productBase> GetDataContextModelContracts()
        {
            return Db.new_contract_plan_productBase.ToList(); ;
        }

        private new_contract_plan_productBase GetDataContextModelContract(Guid contrGuid)
        {
            return Db.new_contract_plan_productBase.FirstOrDefault(contract => contract.new_contract_plan_productId == contrGuid);
        }

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
}
