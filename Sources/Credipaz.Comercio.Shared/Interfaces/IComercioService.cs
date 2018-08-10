using Credipaz.Comercio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Credipaz.Tarjeta.Shared.Interfaces
{
    /// <summary>
    /// Defines the behavior of Pre-Authorization Contract  Service
    /// </summary>
    [ServiceContract]
    public interface IComercioService
    {
        [OperationContract]
        UserModel GetUserIdentification(int idUser);

        [OperationContract]
        InfoModel GetUserInfo(int idUser);

        //[OperationContract]
        //IEnumerable<GenericObject> GetType(string tableType, string filterParams);

        [OperationContract]
        IEnumerable<CreditModel> GetCredits(int idUser);

        [OperationContract]
        CreditDetailModel GetCreditDetail(int idLog);

        //form select

        [OperationContract]
        IEnumerable<GenericObject> GetCommerceSellers(int idUser);

        [OperationContract]
        IEnumerable<GenericObject> GetCommercePlans(int idUser);

        [OperationContract]
        IEnumerable<TaxModel> GetCommerceQuotas(int idUser);

        [OperationContract]
        bool SaveForm(CreditDetailModel formData);

        [OperationContract]
        IEnumerable<Condition> GetConditions();

    }
}
