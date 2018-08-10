using Credipaz.IdentityValidator.View.Shared;
using Credipaz.IdentityValidator.View.Shared.Models;
using Credipaz.Library.Log;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Credipaz.Tarjeta.Web.CommerceService;
using Credipaz.Tarjeta.Web.Helpers;
using Credipaz.Tarjeta.Web.Models;
using Credipaz.Tarjeta.Web.PreDatoRService;
using Credipaz.Tarjeta.Web.ScoringService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using Credipaz.Tarjeta.Web.CommonService;

namespace Credipaz.Tarjeta.Web.Controllers
{
    [ApplicationAuthorize]
    public class HomeController : Controller
    {
        //private static CreditDetailModel formData;

        private CommonServiceClient commonService = new CommonServiceClient();
        private ComercioServiceClient commerceService = new ComercioServiceClient();
        private PreDatoRServiceClient preDatoRService = new PreDatoRServiceClient();
        private ServicioClient scoringService = new ServicioClient();
        private BusinessLogic businessLogic = new BusinessLogic();

        public ActionResult Index()
        {
            var UserName = HttpContext.User.Identity.Name;

            try
            {
                var info = this.commerceService.GetUserInfo(Convert.ToInt32(UserName));

                Session["idCommerce"] = info.IdCommerce;
                Session["description"] = info.Description;
                Session["cuit"] = info.CUIT;
                Session["address"] = info.Address;
                Session["location"] = info.Location;
                Session["province"] = info.Province;
                Session["subsidiary"] = info.Subsidiary;
                Session["company"] = info.Company;
                Session["companyFullName"] = info.CompanyFullName;
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
            }

            return View();
        }

        public ActionResult Credits(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult NewCredit(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Info(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public JsonResult GetType(string type)
        {
            try
            {
                var list = this.commonService.GetTypes(type, null);
                return Json(new { success = true, error = "", list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = string.Format("Error al obtener tipo {0} :" + ex.Message, type) }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSellers()
        {
            try
            {
                var list = this.commerceService.GetCommerceSellers(Convert.ToInt32(HttpContext.User.Identity.Name));
                return Json(new { success = true, error = "", list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Getplans()
        {
            try
            {
                var list = this.commerceService.GetCommercePlans(Convert.ToInt32(HttpContext.User.Identity.Name));
                return Json(new { success = true, error = "", list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetQuotas()
        {
            try
            {
                var list = this.commerceService.GetCommerceQuotas(Convert.ToInt32(HttpContext.User.Identity.Name));
                return Json(new { success = true, error = "", list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAreaCodes()
        {
            try
            {
                var list = this.commonService.GetAreaCodes();

                return Json(new { success = true, error = "", list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

        public JsonResult GetCredits()
        {
            var UserName = HttpContext.User.Identity.Name;

            //Thread.Sleep(10000);

            try
            {
                var list = this.commerceService.GetCredits(Convert.ToInt32(UserName));

                if (list != null)
                {
                    return Json(new { success = true, error = "", list }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, error = "Error. No existen creditos del Cliente" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCreditDetail(int idLog)
        {

            try
            {
                var list = this.commerceService.GetCreditDetail(idLog);

                Session["idLog"] = idLog;

                if (list != null)
                {
                    return Json(new { success = true, error = "", list }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, error = "Error. No existen detalles de creditos del Cliente" });
                }
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetAuthorization(CreditModel creditModel)
        {
            try
            {
                var idApp = Convert.ToInt32(ConfigurationManager.AppSettings.Get("idApp"));

                var dict = new Dictionary<string, string>();

                dict.Add("Documento", creditModel.DniNumber.ToString());
                dict.Add("Sexo", creditModel.Sex.Substring(0, 1));
                dict.Add("Ocupacion", creditModel.Ocupation);
                dict.Add("Ingreso", creditModel.MonthlyIncome);
                dict.Add("MontoSolicitado", creditModel.RequestedAmount);
                dict.Add("IdComerciante", HttpContext.User.Identity.Name);

                var xmlDoc = preDatoRService.OneDataToCheck(dict, idApp);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlDoc.ToString());

                var resultado = ParseQueryResult(xml, "//Resultado");
                var obs = ParseQueryResult(xml, "//Observacion");
                var idLog = ParseQueryResult(xml, "//Id");

                if (resultado == "Aprobado")
                {
                    var param = new ParametrosEntradaScoring();

                    param.sValor = creditModel.DniNumber.ToString();
                    param.nIDComerciante = Convert.ToInt32(HttpContext.User.Identity.Name);
                    param.nIngresoMensual = Convert.ToInt32(creditModel.MonthlyIncome);
                    param.nLimiteCuotas = 0;
                    param.nDisponible = 0;
                    param.sLKOcupacion = creditModel.Ocupation;
                    param.nMontoSolicitado = Convert.ToDecimal(creditModel.RequestedAmount);
                    param.sTipo = "d";
                    param.sCanal = "2";
                    param.sLugarConsulta = "2";
                    param.sProducto = "C";

                    var scoring = ((DataTable)scoringService.ObtenerScoring(param)).AsEnumerable();

                    var formData = new CreditDetailModel();

                    formData.Id = Convert.ToInt32(idLog);

                    var PreAuthorizedAmount = Convert.ToInt32(ConfigurationManager.AppSettings.Get("preAuthorizedAmount"));

                    if (scoring != null || scoring.Count() > 0)
                    {
                        if (Convert.ToInt32(creditModel.RequestedAmount) < PreAuthorizedAmount)
                        {
                            formData.Status = "PCO";
                            var ret = commerceService.SaveForm(formData);
                        }
                        else
                        {
                            formData.Status = "PEN";
                            var ret = commerceService.SaveForm(formData);
                        }
                        return Json(new { success = true, error = "", idLog }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        formData.Status = "REC";
                        var ret = commerceService.SaveForm(formData);

                        return Json(new { success = false, error = "Credito Rechazado", idLog, status = "REC" }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { success = false, error = obs, status = "REC" });
                }

            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetIdentification(EntryData entryData)
        {
            var idApplication = Convert.ToInt32(ConfigurationManager.AppSettings.Get("idApp"));

            string cuil = string.Empty;
            string name = string.Empty;

            try
            {
                var saveQuestionsModel = businessLogic.GetIdentification(entryData, idApplication, out cuil, out name);

                if (saveQuestionsModel != null)
                {
                    return Json(new { success = true, error = "", name, cuil, saveQuestionsModel }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (cuil != string.Empty)
                    {
                        if (name.Substring(0, 8) == "SUCESION")
                        {
                            return Json(new { success = false, error = "Observacion, Sucesion de Individuo", name, cuil }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = true, error = "", name, cuil, saveQuestionsModel }, JsonRequestBehavior.AllowGet);
                        }                        
                    }
                    else
                    {
                        return Json(new { success = false, error = "Error al Conectarse con el Servicio", name, cuil, saveQuestionsModel }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message });
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetQuestions(long cuil)
        {
            var configId = Convert.ToInt32(ConfigurationManager.AppSettings.Get("idConfig"));
            var idApplication = Convert.ToInt32(ConfigurationManager.AppSettings.Get("idApp"));
            var entryData = new EntryData() { Cuil = cuil, ConfigId = configId };

            try
            {
                var queryModel = businessLogic.GetQuestions(entryData, idApplication);

                if (queryModel != null)
                {
                    return Json(new { success = true, error = "", queryModel }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, error = "Error. No se pudieron Obtener preguntas" });
                }
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SaveQuestions(ExitData exitData)
        {
            var idApplication = Convert.ToInt32(ConfigurationManager.AppSettings.Get("idApp"));

            try
            {
                var saveQuestionsModel = businessLogic.SaveQuestions(exitData, idApplication);

                if (saveQuestionsModel != null)
                {
                    return Json(new { success = true, error = "", saveQuestionsModel }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, error = "Error. No se obtener respuesta del formulario" });
                }
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message });
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SaveForm(CreditDetailModel formData)
        {
            var idApplication = Convert.ToInt32(ConfigurationManager.AppSettings.Get("idApp"));

            try
            {
                var ret = commerceService.SaveForm(formData);

                if (ret)
                {
                    return Json(new { success = true, info = Resources.Language.CorrectSaveData, error = "" });
                }
                else
                {
                    return Json(new { success = false, error = Resources.Language.InCorrectSaveData });
                }

            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

        public ActionResult PrintForm()
        {
            try
            {
                var formData = this.commerceService.GetCreditDetail((int)Session["idLog"]); ;

                var workStream = CreatePDF(formData);

                return new FileStreamResult(workStream, "application/pdf");
            }
            catch (Exception ex)
            {
                Trace.Error(ex.Message);
                return null;
            }
        }

        protected MemoryStream CreatePDF(CreditDetailModel formData)
        {
            try
            {

                var GC = this.commerceService.GetConditions();

                string sMontoEnLetras = "";

                sMontoEnLetras = formData.Amount != null ? this.ConvertNumberToString(Convert.ToDecimal(formData.Amount)) : "";

                byte[] byteInfo;

                using (MemoryStream msObj = new MemoryStream())
                {
                    //MemoryStream msObj = new MemoryStream();

                    using (Document document = new Document(PageSize.A4, 0f, 0f, 5f, 0f))
                    {
                        //Document document = new Document(PageSize.A4, 0, 0, 5, 0);

                        using (PdfWriter pwObj = PdfWriter.GetInstance(document, msObj))
                        {

                            //PdfWriter.GetInstance(document, msObj).CloseStream = false;

                            PdfPTable ptTabla1 = new PdfPTable(3);
                            PdfPTable ptTabla2 = new PdfPTable(3);
                            PdfPTable ptTabla3 = new PdfPTable(5);
                            PdfPTable ptTabla4 = new PdfPTable(4);
                            PdfPTable ptTabla5 = new PdfPTable(1);
                            PdfPTable ptTabla6 = new PdfPTable(3);
                            PdfPTable ptTabla7 = new PdfPTable(5);
                            PdfPTable ptTabla8 = new PdfPTable(3);
                            PdfPTable ptTabla9 = new PdfPTable(4);
                            PdfPTable ptTabla10 = new PdfPTable(3);
                            PdfPTable ptTabla11 = new PdfPTable(3);
                            PdfPTable ptTabla12 = new PdfPTable(1);
                            PdfPTable ptTabla13 = new PdfPTable(3);
                            PdfPTable ptTabla14 = new PdfPTable(5);
                            PdfPTable ptTabla15 = new PdfPTable(4);
                            PdfPTable ptTabla16 = new PdfPTable(5);

                            PdfPTable ptTabla1C = new PdfPTable(1);
                            PdfPTable ptTabla2C = new PdfPTable(2);
                            PdfPTable ptTabla3C = new PdfPTable(3);

                            PdfPCell pcCelda = new PdfPCell();

                            string sCadena = "";
                            string sCadenaBlanca = "                                                 ";
                            Phrase phrase = new Phrase();

                            document.Open();

                            var fuentePequeña = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10);                        
                            var fuenteChica = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9);
                            var fuenteMuyChica = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8);
                            var fuenteMinima = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 6);

                            var fuenteChicaBold = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, Font.BOLD);

                            //document.Open();

                            //string sLogoUrl = "C:\\Users\\jcorbera\\Documents\\Sources\\WebComercio\\Sources\\Credipaz.Comercio.Web\\Images\\Logo_01.jpg";
                            string sLogoUrl = Server.MapPath(Url.Content("~/Images/Logo_01.jpg")); 

                            iTextSharp.text.Image iLogo = iTextSharp.text.Image.GetInstance(sLogoUrl);
                            //Resize image depend upon your need
                            //iLogo.ScaleToFit(140f, 30f);

                            ptTabla1.TotalWidth = 900f;

                            float[] widths = new float[] { 300, 300, 300 };
                            ptTabla1.SetWidths(widths);

                            sCadena = " ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.VerticalAlignment = 1; // Centro
                            pcCelda.Border = 0;
                            ptTabla1.AddCell(pcCelda);

                            sCadena = "SOLICITUD DE CREDITO\nY DE TARJETA\nCondiciones Particulares";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 1; // Centro
                            pcCelda.Border = 0;
                            ptTabla1.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Nro. de Tarjeta: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[..............................]\n", fuenteChica));
                            phrase.Add(new Chunk("Nro. de Crédito: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[..............................]\n", fuenteChica));
                            phrase.Add(new Chunk("Nro. de Pre-autorización: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[ S ]", fuenteChica));
                            //sCadena = "Nro. de Tarjeta: [..............................]\nNro. de Crédito: [..............................]\nNro. de Pre-autorización: S                      ";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla1.AddCell(pcCelda);

                            document.Add(ptTabla1);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla2.TotalWidth = 900f;

                            float[] widths2 = new float[] { 600, 150, 150 };
                            ptTabla2.SetWidths(widths2);

                            sCadena = "Las presentes condiciones particulares complementan las condiciones generales suscriptas"
                                    + "en el día de la fecha entre las partes contratantes y en consecuencia forman parte integrantes"
                                    + "de las mismas, de conformidad con lo establecido en la Ley Nro. 25.065 ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteMuyChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase = new Phrase();phrase.Add(new Chunk("C.F.\n", fuenteChicaBold));
                            phrase.Add(new Chunk("[..............................]", fuenteChica));
                            //sCadena = "C.F.\n[..............................]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha de Solicitud\n", fuenteChicaBold));
                            phrase.Add(new Chunk("___/___/_______ ", fuenteChica));
                            //sCadena = "Fecha de Solicitud\n___/___/_______ ";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2.AddCell(pcCelda);

                            document.Add(ptTabla2);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla3.TotalWidth = 900f;

                            //sCadena = "Sucursal " + Session["company"].ToString();
                            sCadena = "Sucursal " + Session["company"] != null ? Session["company"].ToString() : "";
                            
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla3.AddCell(pcCelda);

                            sCadena = "Importe de venta";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla3.AddCell(pcCelda);

                            sCadena = "Tipo de Plan";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla3.AddCell(pcCelda);

                            sCadena = "Cant. Cuotas";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla3.AddCell(pcCelda);

                            sCadena = "Importe Cuota";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla3.AddCell(pcCelda);

                            sCadena = formData.Subsidiary;
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3.AddCell(pcCelda);

                            sCadena = formData.Amount.ToString();
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3.AddCell(pcCelda);

                            sCadena = formData.Plan.ToString();
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3.AddCell(pcCelda);

                            sCadena = formData.Quota.ToString();
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3.AddCell(pcCelda);

                            sCadena = formData.TaxQuota.ToString();
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3.AddCell(pcCelda);

                            document.Add(ptTabla3);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla4.TotalWidth = 900f;

                            float[] widths4 = new float[] { 225, 275, 200, 200 };
                            ptTabla4.SetWidths(widths4);

                            sCadena = "Tarjeta";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla4.AddCell(pcCelda);

                            sCadena = "Apellido y nombre del Vendedor";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla4.AddCell(pcCelda);

                            sCadena = "Doc. Nro.";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla4.AddCell(pcCelda);

                            sCadena = "Posee Tarjeta Puntos Plus";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla4.AddCell(pcCelda);

                            //sCadena = Session["description"].ToString() + " - " + Session["idCommerce"].ToString();
                            var sCadenaDescription = Session["description"] != null ? Session["description"].ToString() : "";
                            var sCadenaIdCommerce = Session["idCommerce"] != null ? Session["idCommerce"].ToString() : "";
                            sCadena = sCadenaDescription + " - " + sCadenaIdCommerce;

                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla4.AddCell(pcCelda);

                            sCadena = formData.Seller;
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla4.AddCell(pcCelda);

                            sCadena = "[........................................]";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla4.AddCell(pcCelda);

                            sCadena = "[........................................]";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla4.AddCell(pcCelda);

                            document.Add(ptTabla4);

                            //document.Add(new Paragraph(sCadenaBlanca));

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla5.TotalWidth = 900f;

                            sCadena = "DATOS PARTICULARES DEL SOLICITANTE";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla5.AddCell(pcCelda);

                            document.Add(ptTabla5);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla6.TotalWidth = 900f;

                            float[] widths6 = new float[] { 500, 200, 200 };
                            ptTabla6.SetWidths(widths6);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Apellido y Nombres: ",  fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Name, fuenteChica));
                            //sCadena = "Apellido y Nombres: " + formData.Name;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla6.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Nro. Doc.: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.DNI.ToString(), fuenteChica));
                            //sCadena = "Nro. Doc.: " + formData.DNI.ToString();
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla6.AddCell(pcCelda);

                            var fnac = formData.BirthDate == null ? "" : Convert.ToDateTime(formData.BirthDate).ToShortDateString();
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha Nac.: ", fuenteChicaBold));
                            phrase.Add(new Chunk(fnac, fuenteChica));
                            //sCadena = "Fecha de Nacimiento: " + fnac;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla6.AddCell(pcCelda);

                            document.Add(ptTabla6);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla7.TotalWidth = 900f;

                            float[] widths7 = new float[] { 300, 200, 100, 100, 200 };
                            ptTabla7.SetWidths(widths7);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Domicilio : ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Address, fuenteChica));
                            //sCadena = "Domicilio : " + formData.Address;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla7.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Nro.: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.AddressNumber, fuenteChica));
                            //sCadena = "Nro.: " + formData.AddressNumber;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla7.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Piso: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[.......]", fuenteChica));
                            //sCadena = "Piso: [...............]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla7.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Dpto.: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[.......]", fuenteChica));
                            //sCadena = "Dpto.: [...............]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla7.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Código Postal: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.PostalCode, fuenteChica));
                            //sCadena = "Código Postal: " + formData.PostalCode;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla7.AddCell(pcCelda);

                            document.Add(ptTabla7);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla8.TotalWidth = 900f;

                            //float[] widths8 = new float[] { 300, 300, 300 };
                            //ptTabla8.SetWidths(widths8);

                            //phrase = new Phrase();
                            //phrase.Add(new Chunk("Entre Calles: ", fuenteChicaBold));
                            //phrase.Add(new Chunk(formData.BetweenStreets, fuenteChica));
                            ////sCadena = "Entre Calles: " + formData.BetweenStreets;
                            ////pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            //pcCelda = new PdfPCell(phrase);
                            //pcCelda.HorizontalAlignment = 0; // Izquierda
                            //pcCelda.Border = 0;
                            //ptTabla8.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Localidad: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Location, fuenteChica));
                            //sCadena = "Localidad: " + formData.Location;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla8.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Provincia: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Province, fuenteChica));
                            //sCadena = "Provincia: " + formData.Province;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla8.AddCell(pcCelda);

                            //phrase = new Phrase();
                            //phrase.Add(new Chunk("Barrio: ", fuenteChicaBold));
                            //phrase.Add(new Chunk("[.....................]", fuenteChica));
                            ////sCadena = "Barrio: [..............]";
                            ////pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            //pcCelda = new PdfPCell(phrase);
                            //pcCelda.HorizontalAlignment = 0; // Izquierda
                            //pcCelda.Border = 0;
                            //ptTabla8.AddCell(pcCelda);

                            document.Add(ptTabla8);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla9.TotalWidth = 900f;

                            float[] widths9 = new float[] { 225, 250, 225, 200 };
                            ptTabla9.SetWidths(widths9);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Nacionalidad: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Nationality, fuenteChica));
                            //sCadena = "Nacionalidad: " + formData.Nationality;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla9.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Vivienda: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Housing, fuenteChica));
                            //sCadena = "Vivienda: " + formData.Housing;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla9.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Estado Civil: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.MaritalStatus, fuenteChica));
                            //sCadena = "Estado Civil: " + formData.MaritalStatus;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla9.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Cant. de Hijos: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[..........]", fuenteChica));
                            //sCadena = "Cant. de Hijos: [..........]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla9.AddCell(pcCelda);

                            document.Add(ptTabla9);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla10.TotalWidth = 900f;

                            float[] widths10 = new float[] { 225, 250, 425 };
                            ptTabla10.SetWidths(widths10);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Teléfono: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Tel1, fuenteChica));
                            //sCadena = "Teléfono: " + formData.Tel1;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla10.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("C.U.I.L.: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.CUIL, fuenteChica));
                            //sCadena = "C.U.I.L.: " + formData.CUIL;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla10.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("E-mail: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Email, fuenteChica));
                            //sCadena = "E-mail: " + formData.Email;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla10.AddCell(pcCelda);

                            document.Add(ptTabla10);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla11.TotalWidth = 900f;

                            float[] widths11 = new float[] { 475, 175, 250 };
                            ptTabla11.SetWidths(widths11);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Ref. 1 - Apellido y Nombre: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[.........................................................]", fuenteChica));
                            //sCadena = "Ref. 1 - Apellido y Nombre: [.............................................................]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla11.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Teléfono: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Tel2, fuenteChica));
                            //sCadena = "Teléfono: " + formData.Tel2;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla11.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Relación : ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.TelRel2, fuenteChica));
                            //sCadena = "Relación Títular: " + formData.TelRel2;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla11.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Ref. 2 - Apellido y Nombre: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[.........................................................]", fuenteChica));
                            //sCadena = "Ref. 2 - Apellido y Nombre: [.............................................................]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla11.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Teléfono: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Tel3, fuenteChica));
                            //sCadena = "Teléfono: " + formData.Tel3;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla11.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Relación : ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.TelRel3, fuenteChica));
                            //sCadena = "Relación Títular: " + formData.TelRel3;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla11.AddCell(pcCelda);

                            document.Add(ptTabla11);

                            //document.Add(new Paragraph(sCadenaBlanca));

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla12.TotalWidth = 900f;

                            sCadena = "DATOS DE ACTIVIDAD";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            pcCelda.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            ptTabla12.AddCell(pcCelda);

                            document.Add(ptTabla12);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla13.TotalWidth = 900f;

                            float[] widths13 = new float[] { 300, 200, 200 };
                            ptTabla13.SetWidths(widths13);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Empresa: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.Company, fuenteChica));
                            //sCadena = "Empresa: " + formData.Company;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla13.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("C.U.I.T.: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.CUIT, fuenteChica));
                            //sCadena = "C.U.I.T.:" + formData.CUIT;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla13.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Rubro: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.JobField, fuenteChica));
                            //sCadena = "Rubro: " + formData.JobField;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla13.AddCell(pcCelda);

                            document.Add(ptTabla13);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla14.TotalWidth = 900f;

                            float[] widths14 = new float[] { 300, 200, 100, 100, 200 };
                            ptTabla14.SetWidths(widths14);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Domicilio Legal: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.WorkAddress, fuenteChica));
                            //sCadena = "Domicilio Legal: " + formData.WorkAddress;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla14.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Nro.: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.WorkAddressNumber, fuenteChica));
                            //sCadena = "Nro.: " + formData.WorkAddressNumber;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla14.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Piso: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[.......]", fuenteChica));
                            //sCadena = "Piso: [.......]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla14.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Dpto.: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[......]", fuenteChica));
                            //sCadena = "Dpto.: [......]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla14.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Código Postal: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.WorkPostalCode, fuenteChica));
                            //sCadena = "Código Postal: " + formData.WorkPostalCode;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla14.AddCell(pcCelda);

                            document.Add(ptTabla14);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla15.TotalWidth = 900f;

                            float[] widths15 = new float[] { 300, 300, 180, 120 };
                            ptTabla15.SetWidths(widths15);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Localidad: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.WorkLocation, fuenteChica));
                            //sCadena = "Localidad: " + formData.WorkLocation;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla15.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Provincia: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.WorkProvince, fuenteChica));
                            //sCadena = "Provincia: " + formData.WorkProvince;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla15.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Tel. Lab.: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.WorkingTel, fuenteChica));
                            //sCadena = "Tel. Lab.: " + formData.WorkingTel;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla15.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Interno: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[........]", fuenteChica));
                            //sCadena = "Interno: [........]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla15.AddCell(pcCelda);

                            document.Add(ptTabla15);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla16.TotalWidth = 900f;

                            float[] widths16 = new float[] { 200, 150, 150, 200, 200 };
                            ptTabla16.SetWidths(widths16);

                            var fIn = formData.AdmissionDate == null ? "" : Convert.ToDateTime(formData.AdmissionDate).ToShortDateString();
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Fecha Ingreso: ", fuenteChicaBold));
                            phrase.Add(new Chunk(fIn, fuenteChica));
                            //sCadena = "Fecha de Ingreso: " + fIn;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla16.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Sueldo: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.MontlyIncome.ToString(), fuenteChica));
                            //sCadena = "Sueldo: " + formData.MontlyIncome;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla16.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Legajo: ", fuenteChicaBold));
                            phrase.Add(new Chunk(formData.WorkFile, fuenteChica));
                            //sCadena = "Legajo: " + formData.WorkFile;
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla16.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Sección: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[...........................]", fuenteChica));
                            //sCadena = "Sección: [.........................]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla16.AddCell(pcCelda);

                            phrase = new Phrase();
                            phrase.Add(new Chunk("Cargo: ", fuenteChicaBold));
                            phrase.Add(new Chunk("[.............................]", fuenteChica));
                            //sCadena = "Cargo: [...........................]";
                            //pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda = new PdfPCell(phrase);
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla16.AddCell(pcCelda);

                            document.Add(ptTabla16);

                            ptTabla1C.TotalWidth = 900f;

                            sCadena = "\n";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteMinima));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla1C.AddCell(pcCelda);

                            document.Add(ptTabla1C);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------
                            ptTabla1C.DeleteBodyRows();

                            ptTabla1C.TotalWidth = 900f;

                            sCadena = "Límite de crédito Inicial: [....................] "
                                    + "Fecha de cierre mensual: Día 25 ó Hábil Posterior " 
                                    + "Teléfono para comunicar denuncia, extravíos y/o hurtos 4319-2552 - Teléfono para obtener información sobre saldos y tasas 4451-5454 "
                                    + "Persona autorizada para retirar la(s) Tarjeta(s) [.............................................................................] "
                                    + "Tipo y Nro. de DOC. [....................................] ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla1C.AddCell(pcCelda);

                            document.Add(ptTabla1C);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla1C.DeleteBodyRows();

                            ptTabla1C.TotalWidth = 900f;

                            sCadena = "\n";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteMinima));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla1C.AddCell(pcCelda);

                            document.Add(ptTabla1C);

                            ptTabla3C.TotalWidth = 900f;

                            float[] widths3C = new float[] { 350, 250, 300 };
                            ptTabla3C.SetWidths(widths3C);

                            sCadena = "CONDICION GENERAL\nConcepto (Artículo - Inciso)";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 1; // Centro
                            pcCelda.BorderWidthTop = 1;
                            pcCelda.BorderWidthLeft = 1;
                            pcCelda.BorderWidthRight = 0;
                            pcCelda.BorderWidthBottom = 1;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = "CONDICIONES GENERALES\nImporte";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 1; // Centro
                            pcCelda.BorderWidthTop = 1;
                            pcCelda.BorderWidthLeft = 1;
                            pcCelda.BorderWidthRight = 0;
                            pcCelda.BorderWidthBottom = 1;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = "CONDICIONES PARTICULARES\n(Iniciales)";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChicaBold));
                            pcCelda.HorizontalAlignment = 1; // Centro
                            pcCelda.BorderWidthTop = 1;
                            pcCelda.BorderWidthLeft = 1;
                            pcCelda.BorderWidthRight = 1;
                            pcCelda.BorderWidthBottom = 1;
                            ptTabla3C.AddCell(pcCelda);

                            document.Add(ptTabla3C);

                            if (GC != null)
                            {
                                if (GC.Count > 0)
                                {
                                    ptTabla3C.DeleteBodyRows();

                                    foreach (var obj in GC)
                                    {
                                        sCadena = obj.Concept;
                                        pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                                        pcCelda.HorizontalAlignment = 0; // Izquierda
                                        pcCelda.BorderWidthTop = 0;
                                        pcCelda.BorderWidthLeft = 1;
                                        pcCelda.BorderWidthRight = 0;
                                        pcCelda.BorderWidthBottom = 1;
                                        ptTabla3C.AddCell(pcCelda);

                                        sCadena = obj.Amount;
                                        pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                                        pcCelda.HorizontalAlignment = 2; // Derecha
                                        pcCelda.BorderWidthTop = 0;
                                        pcCelda.BorderWidthLeft = 1;
                                        pcCelda.BorderWidthRight = 0;
                                        pcCelda.BorderWidthBottom = 1;
                                        ptTabla3C.AddCell(pcCelda);

                                        sCadena = obj.Initial.ToString();
                                        pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                                        pcCelda.HorizontalAlignment = 2; // Derecha
                                        pcCelda.BorderWidthTop = 0;
                                        pcCelda.BorderWidthLeft = 1;
                                        pcCelda.BorderWidthRight = 1;
                                        pcCelda.BorderWidthBottom = 1;
                                        ptTabla3C.AddCell(pcCelda);
                                    }

                                    document.Add(ptTabla3C);
                                }
                            }

                            //document.Add(new Paragraph(sCadenaBlanca));
                            ptTabla1C.DeleteBodyRows();
                            ptTabla1C.TotalWidth = 900f;

                            sCadena = "\n";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteMinima));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla1C.AddCell(pcCelda);

                            document.Add(ptTabla1C);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla3C.DeleteBodyRows();

                            ptTabla3C.TotalWidth = 900f;

                            widths = new float[] { 445, 2, 445 };
                            ptTabla3C.SetWidths(widths);

                            //sCadena = "Autorizo a debitar las cuotas de este crédito en mi tarjeta CABAL " + Session["company"].ToString() + " y acepto las "
                            var sCadenaCompany = Session["company"] != null ? Session["company"].ToString() : "";
                            sCadena = "Autorizo a debitar las cuotas de este crédito en mi tarjeta CABAL " + sCadenaCompany + " y acepto las "                             
                                    + "condiciones particulares y los coeficientes para compras en cuotas ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteMuyChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.BorderWidthTop = 1;
                            pcCelda.BorderWidthLeft = 1;
                            pcCelda.BorderWidthRight = 1;
                            pcCelda.BorderWidthBottom = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = " ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.VerticalAlignment = 1; // Centro
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = "Manifestamos en carácter de declaración jurada que los datos consignados "
                                    + "en la presente son correctos, que hemos cumplimentado los requisitos de control "
                                    + "y verificación, especialmente con el documento de identidad y el recibo de "
                                    + "haberes, y que las firmas del solicitante y garante fueron expuestas en presencia nuestra.- ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteMuyChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.BorderWidthTop = 1;
                            pcCelda.BorderWidthLeft = 1;
                            pcCelda.BorderWidthRight = 1;
                            pcCelda.BorderWidthBottom = 0;
                            ptTabla3C.AddCell(pcCelda);

                            document.Add(ptTabla3C);

                            ptTabla3C.DeleteBodyRows();

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla3C.TotalWidth = 900f;

                            widths = new float[] { 445, 2, 445 };
                            ptTabla3C.SetWidths(widths);

                            sCadena = "\n\n\n\n"
                                    + "..........................................................................................\n"
                                    + "FIRMA SOLICITANTE";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 1; // Centro
                            pcCelda.BorderWidthTop = 1;
                            pcCelda.BorderWidthLeft = 1;
                            pcCelda.BorderWidthRight = 1;
                            pcCelda.BorderWidthBottom = 1;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = " ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.VerticalAlignment = 1; // Centro
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = "\n\n\n\n"
                                    + "..........................................................................................\n"
                                    + "FIRMA Y SELLO DEL COMERCIO";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 1; // Centro
                            pcCelda.BorderWidthTop = 1;
                            pcCelda.BorderWidthLeft = 1;
                            pcCelda.BorderWidthRight = 1;
                            pcCelda.BorderWidthBottom = 1;
                            ptTabla3C.AddCell(pcCelda);

                            document.Add(ptTabla3C);

                            ptTabla3C.DeleteBodyRows();

                            //document.Add(new Paragraph(sCadenaBlanca));
                            //document.Add(new Paragraph(sCadenaBlanca));
                            //document.Add(new Paragraph(sCadenaBlanca));

                            //--------------------------------------------------------------------------------------------------
                            // Pagare
                            //--------------------------------------------------------------------------------------------------

                            ptTabla2C.TotalWidth = 900f;

                            widths = new float[] { 600, 300 };
                            ptTabla2C.SetWidths(widths);

                            pcCelda = new PdfPCell(iLogo);
                            sCadena = " ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.VerticalAlignment = 1; // Centro
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "Nro. de Cuenta  [......................................]\n"
                                    + "Nro. de Crédito [......................................]\n"
                                    + "Importe:        " + formData.Amount;

                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0 ; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            document.Add(ptTabla2C);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla1C.TotalWidth = 900f;

                            ptTabla1C.DeleteBodyRows();

                            sCadena = "........ de ................................. de ................";

                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 2; // Derecha
                            pcCelda.Border = 0;
                            ptTabla1C.AddCell(pcCelda);

                            document.Add(ptTabla1C);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla1C.TotalWidth = 900f;

                            ptTabla1C.DeleteBodyRows();

                            var sCadenaCompanyFullName = Session["companyFullName"] != null ? Session["companyFullName"].ToString() : "";
                            sCadena = "Pagaremos a la vista de " + sCadenaCompanyFullName + " "     
                                    + ", o a su orden, la cantidad de pesos "
                                    + sMontoEnLetras
                                    + " "
                                    + "por igual valor recibido a mi entera satisfacción, sín protesto (Art. 50 Dec. Ley 5965/63). Este pagaré podrá ser presentado para el pago dentro del plazo "
                                    + "de 10 años a partir de la fecha de emisión, debiendo presentarse para el pago en ..................................";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla1C.AddCell(pcCelda);

                            document.Add(ptTabla1C);

                            //document.Add(new Paragraph(sCadenaBlanca));
                            ptTabla1C.DeleteBodyRows();
                            ptTabla1C.TotalWidth = 900f;

                            sCadena = "\n";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteMuyChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla1C.AddCell(pcCelda);

                            document.Add(ptTabla1C);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla2C.DeleteBodyRows();

                            ptTabla2C.TotalWidth = 900f;

                            widths = new float[] { 200, 700 };
                            ptTabla2C.SetWidths(widths);

                            sCadena = "Firma del Deudor";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "..................................................................................................................................................................";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "Aclaración de firma";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "..................................................................................................................................................................";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "Documento";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "..................................................................................................................................................................";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "Domicilio";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "..................................................................................................................................................................";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "Localidad";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "..................................................................................................................................................................";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "Pcia.";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            sCadena = "..................................................................................................................................................................";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla2C.AddCell(pcCelda);

                            document.Add(ptTabla2C);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla1C.TotalWidth = 900f;

                            ptTabla1C.DeleteBodyRows();

                            sCadena = "-------------------------------------------------------------------------------------------------------------------------------------------------------------";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla1C.AddCell(pcCelda);

                            sCadena = "TALON PARA EL CLIENTE";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla1C.AddCell(pcCelda);

                            document.Add(ptTabla1C);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            ptTabla3C.TotalWidth = 900f;

                            widths = new float[] { 370, 350, 180 };
                            ptTabla3C.SetWidths(widths);

                            ptTabla3C.DeleteBodyRows();

                            sCadena = "Títular:......................................................................";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = "Fecha de solicitud: ____/____/______";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = " ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = "D.N.I.:.........................................................................";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = " ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = " ";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = "Importe Cuotas:...................... Cantidad Cuotas:......";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = "Fecha de primer vencimiento: ____/____/______";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 0; // Izquierda
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            sCadena = "......................................\nFirma y Sello del Tarjeta";
                            pcCelda = new PdfPCell(new Paragraph(sCadena, fuenteChica));
                            pcCelda.HorizontalAlignment = 1; // Centro
                            pcCelda.Border = 0;
                            ptTabla3C.AddCell(pcCelda);

                            document.Add(ptTabla3C);

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------

                            document.Close();
                        }
                    }

                    byteInfo = msObj.ToArray();
                }

                return new MemoryStream(byteInfo);

            }
            catch (Exception ex)
            {
                Trace.Error(ex.InnerException.Message);
                return new MemoryStream();
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Credits", "Home");
        }

        private static string ParseQueryResult(XmlDocument xmlDoc, string tag)
        {
            string value = string.Empty;

            XmlNodeList nodes = xmlDoc.SelectNodes(tag);

            if (nodes.Count > 0)
            {
                value = nodes.Item(0).InnerText;
            }

            return value;
        }

        public string ConvertNumberToString(decimal nNumero)
        {
            string sResultado = "";
            string sDecimal = "";

            Int64 nEntero;

            int nDecimales;

            nEntero = Convert.ToInt64(Math.Truncate(nNumero));

            nDecimales = Convert.ToInt32(Math.Round((nNumero - nEntero) * 100, 2));

            if (nDecimales > 0)
            {
                sDecimal = " CON " + string.Format("{0:00}", nDecimales) + "/100";
            }
            else
            {
                sDecimal = " CON " + string.Format("{0:00}", nDecimales) + "/00";
            }

            sResultado = toText(Convert.ToDouble(nEntero)) + sDecimal;

            return sResultado;
        }

        private string toText(double value)
        {
            string Num2Text = "";

            value = Math.Truncate(value);

            if (value == 0) Num2Text = "CERO";

            else if (value == 1) Num2Text = "UNO";

            else if (value == 2) Num2Text = "DOS";

            else if (value == 3) Num2Text = "TRES";

            else if (value == 4) Num2Text = "CUATRO";

            else if (value == 5) Num2Text = "CINCO";

            else if (value == 6) Num2Text = "SEIS";

            else if (value == 7) Num2Text = "SIETE";

            else if (value == 8) Num2Text = "OCHO";

            else if (value == 9) Num2Text = "NUEVE";

            else if (value == 10) Num2Text = "DIEZ";

            else if (value == 11) Num2Text = "ONCE";

            else if (value == 12) Num2Text = "DOCE";

            else if (value == 13) Num2Text = "TRECE";

            else if (value == 14) Num2Text = "CATORCE";

            else if (value == 15) Num2Text = "QUINCE";

            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);

            else if (value == 20) Num2Text = "VEINTE";

            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);

            else if (value == 30) Num2Text = "TREINTA";

            else if (value == 40) Num2Text = "CUARENTA";

            else if (value == 50) Num2Text = "CINCUENTA";

            else if (value == 60) Num2Text = "SESENTA";

            else if (value == 70) Num2Text = "SETENTA";

            else if (value == 80) Num2Text = "OCHENTA";

            else if (value == 90) Num2Text = "NOVENTA";

            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);

            else if (value == 100) Num2Text = "CIEN";

            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);

            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";

            else if (value == 500) Num2Text = "QUINIENTOS";

            else if (value == 700) Num2Text = "SETECIENTOS";

            else if (value == 900) Num2Text = "NOVECIENTOS";

            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);

            else if (value == 1000) Num2Text = "MIL";

            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);

            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";

                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";

            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);

            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";

                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";

            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";

                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            }

            return Num2Text;
        }
    }
}
