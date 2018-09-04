using DevExpress.Utils.About;
using DevExpressGrid3.Controllers;
using DevExpressGrid3.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevExpressGrid3.Helpers
{
    public static class RtCrmDataProvider
    {
        const string RtCrmDataContextKey = "DXRtCrmDataContext";
        //Models.RTCRM db = new Models.RTCRM();
        public static RTCRM Db
        {
            get
            {
                if (HttpContext.Current.Items[RtCrmDataContextKey] == null)
                    HttpContext.Current.Items[RtCrmDataContextKey] = new RTCRM();
                return (RTCRM)HttpContext.Current.Items[RtCrmDataContextKey];
            }
        }

        static double CalculateAveragePrice(int categoryID)
        {
            //return (double)(from product in DB.Products where product.CategoryID == categoryID select product).Average(s => s.UnitPrice);
            return Double.MinValue;
        }

        static double CalculateMinPrice(int categoryID)
        {
            //return (double)(from product in DB.Products where product.CategoryID == categoryID select product).Min(s => s.UnitPrice);
            return Double.MinValue;
        }

        static double CalculateMaxPrice(int categoryID)
        {
            //return (double)(from product in DB.Products where product.CategoryID == categoryID select product).Max(s => s.UnitPrice);
            return Double.MinValue;
        }
        public static IEnumerable GetProducts() //GetCategories for combobox later
        {
            var query = from product in Db.new_contract_plan_productBase
                        select new
                        {
                            ProductId = product.new_link_product_id,
                            CategoryName = product.new_name
                        };
            return query.ToList();
        }
        public static Product GetProductByID(int categoryID)
        {
            //return (from product in DB.Categories where category.CategoryID == categoryID select category).SingleOrDefault<Category>();
            return new Product();
        }

        internal static void InsertContract(EditableContract contract)
        {
            var editContract = new EditableContract()
            {
                ContrGUID = contract.ContrGUID,
                ProductGroupProduct = contract.ProductGroupProduct,
                Product = contract.Product,
                Service1Quarter = contract.Service1Quarter,
                Service2Quarter = contract.Service2Quarter,
                Service3Quarter = contract.Service3Quarter,
                Service4Quarter = contract.Service4Quarter,
                Consult1Quarter = contract.Consult1Quarter,
                Consult2Quarter = contract.Consult2Quarter,
                Consult3Quarter = contract.Consult3Quarter,
                Consult4Quarter = contract.Consult4Quarter,
                NewServiceYear = contract.NewServiceYear,
                NewConsultYear = contract.NewConsultYear,
                NewProductTotalConsult = contract.NewProductTotalConsult,
                NewProductTotalService = contract.NewProductTotalService,
                NewYearTotal = contract.NewYearTotal
            };
            GetEditableContracts().Add(editContract);
        }

        public static void UpdateProduct(EditableContract contract)
        {
            EditableContract editProduct = GetEditableContract(contract.ContrGUID);
        }

        public static EditableContract GetEditableContract(Guid contrGUID)
        {

            return ContractQueryAll().Where(contract => contract.ContrGUID == contrGUID).FirstOrDefault();
        }

        public static void DeleteProduct(Guid contrGUID)
        {
            var contract = GetEditableContract(contrGUID);

            if (contract != null)
            {
                GetEditableContracts().Remove(contract);
            }
        }

        internal static IList<EditableContract> GetEditableContracts()
        {
            var contracts = (IList<EditableContract>)HttpContext.Current.Session["Contracts"];

            if (contracts == null)
            {
                IQueryable<EditableContract> query = ContractQueryAll();
                contracts = query.ToList();
                HttpContext.Current.Session["Contracts"] = contracts;
            }
            return contracts;
        }

        private static IQueryable<EditableContract> ContractQueryAll()
        {
            return from main in Db.new_contract_plan_productBase
                   join a in Db.new_d_product_catalogBase on main.new_link_product_id equals a.new_d_product_catalogId
                   join b in Db.new_d_product_groupsBase on main.new_link_product_group_id equals b.new_d_product_groupsId
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
        }
    }

    public class Product
    {
    }
}