using Credipaz.Comercio.Shared.Models;
using Credipaz.Library.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Credipaz.Comercio.Dal
{
    public class DBContext : DataContext
    {
        #region Private Values

        /// <summary> 
        /// Defines a private readonly connection string to the database. 
        /// </summary> 
        private static readonly string Cnn = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;

        #endregion

        #region Constructor
        public DBContext(string connection) : base(connection) { }

        #endregion

        #region Methods

        public static UserModel GetUserIdentification(int idUser)
        {
            using (var context = new DBContext(Cnn))
            {
                return context.QueryGetUserIdentification(idUser).FirstOrDefault();
            }
        }

        public static InfoModel GetUserInfo(int idUser)
        {
            using (var context = new DBContext(Cnn))
            {
                return context.QueryGetUserInfo(idUser).FirstOrDefault();
            }
        }

        //public static IEnumerable<GenericObject> GetType(string tableType, string filterParams)
        //{
        //    using (var context = new DBContext(Cnn))
        //    {
        //        try
        //        {
        //            var ret = (IEnumerable<GenericObject>)context.QueryGetType(tableType, filterParams);
        //            return ret.ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }

        //    }
        //}

        public static IEnumerable<CreditModel> GetCredits(int idUser)
        {
            using (var context = new DBContext(Cnn))
            {
                return context.QueryGetCredits(idUser).ToList();
            }
        }

        public static CreditDetailModel GetCreditDetail(int idLog)
        {
            try
            {
                using (var context = new DBContext(Cnn))
                {
                    return context.QueryGetCreditDetail(idLog).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //

        public static IEnumerable<GenericObject> GetCommerceSellers(int idUser)
        {
            using (var context = new DBContext(Cnn))
            {
                try
                {
                    return (IEnumerable<GenericObject>)context.QueryGetCommerceSellers(idUser).ToList();
                }
                catch (Exception ex)
                {
                    Trace.Error(string.Format("Error GetCommerceDealers : {0}", ex.Message));
                    return null;
                }
            }
        }

        public static IEnumerable<GenericObject> GetCommercePlans(int idUser)
        {
            using (var context = new DBContext(Cnn))
            {
                try
                {
                    return (IEnumerable<GenericObject>)context.QueryGetCommercePlans(idUser).ToList();
                }
                catch (Exception ex)
                {
                    Trace.Error(string.Format("Error GetCommercePlans : {0}", ex.Message));
                    return null;
                }
            }
        }

        public static IEnumerable<TaxModel> GetCommerceQuotas(int idUser)
        {
            using (var context = new DBContext(Cnn))
            {
                try
                {
                    return context.QueryGetCommerceQuotas(idUser).ToList();
                }
                catch (Exception ex)
                {
                    Trace.Error(string.Format("Error GetCommerceQuotas : {0}", ex.Message));
                    return null;
                }
            }
        }

        public static bool SaveForm(CreditDetailModel formData)
        {
            using (var context = new DBContext(Cnn))
            {
                try
                {
                    var d = formData;
                    return context.QuerySaveForm(d.Id,
                                                 d.Status,
                                                 d.IdSeller,
                                                 d.Amount,
                                                 d.IdQuota,
                                                 d.Quota,
                                                 d.TaxQuota,
                                                 d.IdMaritalStatus,
                                                 d.IdNationality,
                                                 d.Address,
                                                 d.AddressNumber,
                                                 d.PostalCode,
                                                 d.Location,
                                                 d.Province,
                                                 d.IdHousing,
                                                 d.Email,
                                                 d.AreaCode1,
                                                 d.Tel1,
                                                 d.IdTelType1,
                                                 d.IdTelRel1,
                                                 d.AreaCode2,
                                                 d.Tel2,
                                                 d.IdTelType2,
                                                 d.IdTelRel2,
                                                 d.AreaCode3,
                                                 d.Tel3,
                                                 d.IdTelType3,
                                                 d.IdTelRel3,
                                                 d.IdOcupation,
                                                 d.Company,
                                                 d.CUIT,
                                                 d.WorkFile,
                                                 d.IdJobField,
                                                 d.AdmissionDate,
                                                 d.WorkAreaCode,
                                                 d.WorkingTel,
                                                 d.WorkAddress,
                                                 d.WorkAddressNumber,
                                                 d.WorkPostalCode,
                                                 d.WorkLocation,
                                                 d.WorkProvince,
                                                 d.Obs) > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Trace.Error(string.Format("Error SaveForm : {0}", ex.Message));
                    return false;
                }
            }
        }

        public static IEnumerable<Condition> GetConditions()
        {
            using (var context = new DBContext(Cnn))
            {
                try
                {
                    return context.QueryGetConditions().ToList();
                }
                catch (Exception ex)
                {
                    Trace.Error(string.Format("Error GetCommerceQuotas : {0}", ex.Message));
                    return null;
                }
            }
        }

        #endregion

        #region Queries

        [Function(Name = "ComercioWeb.ObtenerIdentificacionUsuario")]
        private IEnumerable<UserModel> QueryGetUserIdentification
        (
            [Parameter(Name = "idUsuario")] int idUser
        )
        {
            var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), idUser);
            return (ISingleResult<UserModel>)result.ReturnValue;
        }


        [Function(Name = "ComercioWeb.ObtenerInformacionUsuario")]
        private IEnumerable<InfoModel> QueryGetUserInfo
        (
            [Parameter(Name = "idUsuario")] int idUser
        )
        {
            var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), idUser);
            return (ISingleResult<InfoModel>)result.ReturnValue;
        }

        //[Function(Name = "Common.ObtenerTipo")]
        //private IEnumerable<GenericObject> QueryGetType
        //(
        //    [Parameter(Name = "tabla")] string tableType,
        //    [Parameter(Name = "filtros")] string filterParams
        //)
        //{
        //    var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), tableType, filterParams);
        //    return (ISingleResult<GenericObject>)result.ReturnValue;
        //}


        [Function(Name = "ComercioWeb.ObtenerCreditosPorUsuario")]
        private IEnumerable<CreditModel> QueryGetCredits
        (
            [Parameter(Name = "idUsuario")] int idUser
        )
        {
            var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), idUser);
            return (ISingleResult<CreditModel>)result.ReturnValue;
        }

        [Function(Name = "ComercioWeb.ObtenerDetalleCreditos")]
        private IEnumerable<CreditDetailModel> QueryGetCreditDetail
        (
            [Parameter(Name = "idLog")] int idLog
        )
        {
            var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), idLog);
            return (ISingleResult<CreditDetailModel>)result.ReturnValue;
        }

        //

        [Function(Name = "ComercioWeb.ObtenerVendedoresPorComercio")]
        private IEnumerable<GenericObject> QueryGetCommerceSellers
        (
            [Parameter(Name = "idComerciante")] int idUser
        )
        {
            var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), idUser);
            return (ISingleResult<GenericObject>)result.ReturnValue;
        }

        [Function(Name = "ComercioWeb.ObtenerPlanesPorComercio")]
        private IEnumerable<GenericObject> QueryGetCommercePlans
        (
            [Parameter(Name = "idComerciante")] int idUser
        )
        {
            var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), idUser);
            return (ISingleResult<GenericObject>)result.ReturnValue;
        }

        [Function(Name = "ComercioWeb.ObtenerCuotasPorComercio")]
        private IEnumerable<TaxModel> QueryGetCommerceQuotas
        (
            [Parameter(Name = "idComerciante")] int idUser
        )
        {
            var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), idUser);
            return (ISingleResult<TaxModel>)result.ReturnValue;
        }

        [Function(Name = "ComercioWeb.GuardarCredito")]
        private int QuerySaveForm
        (
            [Parameter(Name = "id")] int? id,
            [Parameter(Name = "estado")] string status,
            [Parameter(Name = "idVendedor")] int? seller,
            [Parameter(Name = "montoAprobado")] decimal? amount,
            [Parameter(Name = "idQuota")] int? idquota,
            [Parameter(Name = "cuota")] int? quota,
            [Parameter(Name = "cuotaImporte")] decimal taxQuota,
            [Parameter(Name = "estadoCivil")] int? maritalStatus,
            [Parameter(Name = "nacionalidad")] int? nationality,
            [Parameter(Name = "domicilioCalle")] string address,
            [Parameter(Name = "domicilioNumero")] string addressNumber,
            [Parameter(Name = "codigoPostal")] string postalCode,
            [Parameter(Name = "localidad")] string location,
            [Parameter(Name = "provincia")] string province,
            [Parameter(Name = "tipoVivienda")] int? housing,
            [Parameter(Name = "correo")] string email,
            [Parameter(Name = "codigoArea1")] string areaCode1,
            [Parameter(Name = "tel1")] string tel1,
            [Parameter(Name = "tipoTel1")] int? telType1,
            [Parameter(Name = "relTel1")] int? terRel1,
            [Parameter(Name = "codigoArea2")] string areaCode2,
            [Parameter(Name = "tel2")] string tel2,
            [Parameter(Name = "tipoTel2")] int? telType2,
            [Parameter(Name = "relTel2")] int? terRel2,
            [Parameter(Name = "codigoArea3")] string areaCode3,
            [Parameter(Name = "tel3")] string tel3,
            [Parameter(Name = "tipoTel3")] int? telType3,
            [Parameter(Name = "relTel3")] int? terRel3,
            [Parameter(Name = "ocupacion")] int? ocupation,
            [Parameter(Name = "empresa")] string company,
            [Parameter(Name = "CUIT")] string CUIT,
            [Parameter(Name = "legajo")] string workFile,
            [Parameter(Name = "rubroLab")] int? jobField,
            [Parameter(Name = "fechaIngreso")] DateTime? admissionDate,
            [Parameter(Name = "codigoAreaLab")] string workAreaCode,
            [Parameter(Name = "telLab")] string workingTel,
            [Parameter(Name = "domiCalleLab")] string workAddress,
            [Parameter(Name = "domiNroLab")] string workAddressNumber,
            [Parameter(Name = "codPostalLab")] string workPostalCode,
            [Parameter(Name = "localidadLab")] string workLocation,
            [Parameter(Name = "provinciaLab")] string workProvince,
            [Parameter(Name = "obs")] string obs
        )
        {
            int ret = -1;
            Trace.Debug(string.Format("Datos del Formulario : {0}", id + ",'" + status + "'," + seller + "," + amount + "," + idquota + "," + quota + "," + taxQuota + "," + maritalStatus + "," + nationality + ",'" +
                                                                 address + "','" + addressNumber + "','" + postalCode + "','" + location + "," + province + "," + housing + ",'" + email + "','" + areaCode1 + "'," + "','" + tel1 + "'," + telType1 + "," +
                                                                 terRel1 + ",'" + "','" + areaCode2 + "'," + tel2 + "'," + telType2 + "," + terRel2 + ",'" + "','" + areaCode3 + "'," + tel3 + "'," + telType3 + "," + terRel3 + "," + ocupation + ",'" +
                                                                 company + "','" + CUIT + "','" + workFile + "'," + jobField + ",'" + admissionDate + "','" + "','" + workAreaCode + "'," + workingTel + "','" + workAddress + "','" +
                                                                 workAddressNumber + "','" + workPostalCode + "','" + workLocation + "," + workProvince + ",'" + obs + "'"));
            try
            {
                var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), id, status, seller, amount, idquota, quota, taxQuota, maritalStatus, nationality, address, addressNumber, postalCode, location, province, housing, email, tel1, telType1, terRel1, tel2, telType2, terRel2, tel3, telType3, terRel3, ocupation, company, CUIT, workFile, jobField, admissionDate, workingTel, workAddress, workAddressNumber, workPostalCode, workLocation, workProvince, obs);
                ret = (int)result.ReturnValue;
            }
            catch (Exception ex)
            {
                Trace.Error(string.Format("Error saving form : {0}", ex.Message));
            }

            return ret;
        }

        [Function(Name = "ComercioWeb.ObtenerCondiciones")]
        private IEnumerable<Condition> QueryGetConditions()
        {
            var result = ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod());
            return (ISingleResult<Condition>)result.ReturnValue;
        }

        #endregion

    }
}
