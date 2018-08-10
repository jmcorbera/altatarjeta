using Credipaz.Comercio.Dal;
using Credipaz.Comercio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credipaz.Comercio.Service.DataAccess
{
    internal static class DataContext
    {
        public static UserModel GetUserIdentification(int idUser)
        {
            return DBContext.GetUserIdentification(idUser);
        }

        public static InfoModel GetUserInfo(int idUser)
        {
            return DBContext.GetUserInfo(idUser);
        }

        //public static IEnumerable<GenericObject> GetType(string tableType, string filterParams)
        //{
        //    return DBContext.GetType(tableType, filterParams);
        //}

        public static IEnumerable<CreditModel> GetCredits(int idUser)
        {
            return DBContext.GetCredits(idUser);
        }

        public static CreditDetailModel GetCreditDetail(int idLog)
        {
            return DBContext.GetCreditDetail(idLog);
        }

        //

        public static IEnumerable<GenericObject> GetCommerceSellers(int idUser)
        {
            return DBContext.GetCommerceSellers(idUser);
        }

        public static IEnumerable<GenericObject> GetCommercePlans(int idUser)
        {
            return DBContext.GetCommercePlans(idUser);
        }

        public static IEnumerable<TaxModel> GetCommerceQuotas(int idUser)
        {
            return DBContext.GetCommerceQuotas(idUser);
        }

        public static bool SaveForm(CreditDetailModel formData)
        {
            return DBContext.SaveForm(formData);
        }

        public static IEnumerable<Condition> GetConditions()
        {
            return DBContext.GetConditions();
        }
    }
}