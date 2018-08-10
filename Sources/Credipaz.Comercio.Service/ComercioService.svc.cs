using Credipaz.Comercio.Service.DataAccess;
using Credipaz.Comercio.Shared.Interfaces;
using Credipaz.Comercio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Credipaz.Comercio.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ComercioService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ComercioService.svc or ComercioService.svc.cs at the Solution Explorer and start debugging.
    public class ComercioService : IComercioService
    {
        public UserModel GetUserIdentification(int idUser)
        {
            return DataContext.GetUserIdentification(idUser);
        }

        public InfoModel GetUserInfo(int idUser)
        {
            return DataContext.GetUserInfo(idUser);
        }

        //public IEnumerable<GenericObject> GetType(string tableType, string filterParams)
        //{
        //    return DataContext.GetType(tableType, filterParams);
        //}

        public IEnumerable<CreditModel> GetCredits(int idUser)
        {
            return DataContext.GetCredits(idUser);
        }

        public CreditDetailModel GetCreditDetail(int idLog)
        {
            return DataContext.GetCreditDetail(idLog);
        }

        //

        public IEnumerable<GenericObject> GetCommerceSellers(int idUser)
        {
            return DataContext.GetCommerceSellers(idUser);
        }

        public IEnumerable<GenericObject> GetCommercePlans(int idUser)
        {
            return DataContext.GetCommercePlans(idUser);
        }

        public IEnumerable<TaxModel> GetCommerceQuotas(int idUser)
        {
            return DataContext.GetCommerceQuotas(idUser);
        }

        public bool SaveForm(CreditDetailModel formData)
        {
            return DataContext.SaveForm(formData);
        }

        public IEnumerable<Condition> GetConditions()
        {
            return DataContext.GetConditions();
        }
    }
}
