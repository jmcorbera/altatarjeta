using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Credipaz.Comercio.Shared.Models
{
    public class CreditDetailModel
    {
        public int? Id { get; set; }
        public string Status { get; set; }
        public string Subsidiary { get; set; }
        public int? IdSeller { get; set; }
        public string Seller { get; set; }
        public decimal? Amount { get; set; }
        public int? Plan { get; set; }
        public int? IdQuota { get; set; }
        public int? Quota { get; set; }
        public decimal TaxQuota { get; set; } 
        public string Name { get; set; }
        public int? DNI { get; set; }
        public string CUIL { get; set; }
        public string Sex { get; set; }     
        public DateTime? BirthDate { get; set; }
        public int? IdMaritalStatus { get; set; }
        public string MaritalStatus { get; set; }
        public int? IdNationality { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string AddressNumber { get; set; }
        public string PostalCode { get; set; }
        //public string BetweenStreets { get; set; }
        public int? IdLocation { get; set; }
        public string Location { get; set; }
        public int? IdProvince { get; set; }
        public string Province { get; set; }
        public int? IdHousing { get; set; }
        public string Housing { get; set; }
        public string Email { get; set; }
        public string AreaCode1 { get; set; }
        public string Tel1 { get; set; }
        public int? IdTelType1 { get; set; }
        public string TelType1 { get; set; }
        public int? IdTelRel1 { get; set; }
        public string TelRel1 { get; set; }
        public string AreaCode2 { get; set; }
        public string Tel2 { get; set; }
        public int? IdTelType2 { get; set; }
        public string TelType2 { get; set; }
        public int? IdTelRel2 { get; set; }
        public string TelRel2 { get; set; }
        public string AreaCode3 { get; set; }
        public string Tel3 { get; set; }
        public int? IdTelType3 { get; set; }
        public string TelType3 { get; set; }
        public int? IdTelRel3 { get; set; }
        public string TelRel3 { get; set; }
        public int? IdOcupation { get; set; }
        public string Ocupation { get; set; }
        public string Company { get; set; }
        public string CUIT { get; set; }
        public string WorkFile { get; set; }
        public int? IdJobField { get; set; }
        public string JobField { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public decimal MontlyIncome { get; set; }
        public string WorkAreaCode { get; set; }
        public string WorkingTel { get; set; }
        public string WorkAddress { get; set; }
        public string WorkAddressNumber { get; set; }
        public string WorkPostalCode { get; set; }
        //public string WorkBetweenStreets { get; set; }
        public int? IdWorkLocation { get; set; }
        public string WorkLocation { get; set; }
        public int? IdWorkProvince { get; set; }
        public string WorkProvince { get; set; }
        public string Obs { get; set; }

    }
}
