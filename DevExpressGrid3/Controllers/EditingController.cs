//using DevExpress.Web.Mvc;
//using DevExpressGrid3.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace DevExpressGrid3.Controllers
//{
//    public class EditingController : Controller
//    {
//        public ActionResult BatchEditing(BatchEditingDemoOptions options) //the options header in DX demos not need to implement
//        {
//            ViewBag.BatchEditingOptions = options;
//            return View("BatchEditing", RtCrmDataProvider.GetEditableContracts());
//        }

//        [ValidateInput(false)]
//        public ActionResult BatchEditingPartial(BatchEditingDemoOptions options)
//        {
//            ViewBag.BatchEditingOptions = options;
//            return PartialView("BatchEditingPartial", RtCrmDataProvider.GetEditableContracts());
//        }

//        [HttpPost, ValidateInput(false)]
//        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<EditableContract, int> updateValues, BatchEditingDemoOptions options)
//        {
//            foreach (var contract in updateValues.Insert)
//            {
//                if (updateValues.IsValid(contract))
//                    InsertContract(contract, updateValues);
//            }
//            foreach (var contract in updateValues.Update)
//            {
//                if (updateValues.IsValid(contract))
//                    UpdateContract(contract, updateValues);
//            }
//            foreach (var contract in updateValues.DeleteKeys)
//            {
//                DeleteProduct(contract., updateValues);
//            }
//            return BatchEditingPartial(options);
//        }

        
//        private void InsertContract(EditableContract contract, MVCxGridViewBatchUpdateValues<EditableContract, int> updateValues)
//        {
//            try
//            {
//                RtCrmDataProvider.InsertContract(contract);
//            }
//            catch (Exception e)
//            {
//                updateValues.SetErrorText(contract, e.Message);
//            }
//        }
//        private void UpdateContract(EditableContract contract, MVCxGridViewBatchUpdateValues<EditableContract, int> updateValues)
//        {
//            try
//            {
//                RtCrmDataProvider.UpdateProduct(contract);
//            }
//            catch (Exception e)
//            {
//                updateValues.SetErrorText(contract, e.Message);
//            }
//        }

//        private void DeleteProduct(Guid contractID, MVCxGridViewBatchUpdateValues<EditableContract, int> updateValues)
//        {
//            try
//            {
//                RtCrmDataProvider.DeleteProduct(contractID);
//            }
//            catch (Exception e)
//            {
//                updateValues.SetErrorText(contractID, e.Message);
//            }
//        }

//    }
//}