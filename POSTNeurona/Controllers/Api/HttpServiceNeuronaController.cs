using Newtonsoft.Json;
using POSTNeurona.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace POSTNeurona.Controllers.Api
{
    /**
     *  API para realizar RESQUEST a enpoints 
     * 
     */
    public class HttpServiceNeuronaController : ApiController
    {
        //Atributo para declarar Log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /**
         * Metodo para recibir la solicutd POST de la web
         * Parametros: body (Cuerpo de la solicitud provinientes de la web), request (Parametros provinientes de la web)
         * Retorna: HttpResponseMessage (200 en caso exitoso, 400 en caso de ERROR)
         */
        [HttpPost]
        // POST: api/HttpServiceNeurona
        public HttpResponseMessage Post([FromBody] NeuronaRequest body, HttpRequestMessage request)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            if (body != null && ModelState.IsValid) //Debibo a que puede mandar un objeto vacío
            {
                
                //DEBO PASARLE COMO PARAMETRO EL TIPO DE METODO
                Dictionary<string, string> accessTkn = PostToken(body.MethodApi, body.SubMethodApi);
                if (accessTkn.ContainsKey("OK"))
                {
                    if (accessTkn["OK"] == "true")
                    {
                        //METODO PARA OBTENER EL RESULTADO DE LOS REQUEST
                        resultado = GetResult(body.Url, body.Method, accessTkn["msg"], body.Body, body.MethodApi, body.SubMethodApi);
                        log.Info($"Conexion finalizada en el Endpoint (GetResult) {body.Url}");
                        if (resultado["OK"] == "false")
                        {
                            return request.CreateResponse(HttpStatusCode.BadRequest, resultado, Configuration.Formatters.JsonFormatter);
                        }

                    }
                    else
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, accessTkn, Configuration.Formatters.JsonFormatter);
                    }

                }
                else
                {
                    log.Error("ERROR: La solicitud no fue procesada.");
                    resultado.Add("OK", "false");
                    resultado.Add("msg", "ERROR: La solicitud no fue procesada.");

                    return request.CreateResponse(HttpStatusCode.BadRequest, resultado, Configuration.Formatters.JsonFormatter);
                }

                return request.CreateResponse(HttpStatusCode.OK, resultado, Configuration.Formatters.JsonFormatter);


            }
            else
            {
                log.Error("ERROR: La solicitud no fue procesada. Debido a que el cuerpo de la solicitud esta vacío");
                resultado.Add("OK", "false");
                resultado.Add("msg", "ERROR: La solicitud no fue procesada. Debido a que el cuerpo de la solicitud esta vacío");

                return request.CreateResponse(HttpStatusCode.NotAcceptable, resultado, Configuration.Formatters.JsonFormatter);


            }
        }

        /**
         * Metodo para obtener TOKEN de acceso (En caso de fallo intenta 3 veces)
         * Parametros (URL es el endpoint para obtener el token, authTK es el valor del header de la API,nameKey es la clave para el header de la API)
         * Retorna Dictionary con la respuesta de la solicitud al enpoint para obtener token de acceso
        */
        public Dictionary<string, string> PostToken(string apiMethod, string subMetodo = "")
        {
            string urlToken = "", authTk = "", nameKey = "", responseTK = "", sufijo = "";
            Dictionary<string, string> resultado = new Dictionary<string, string>();

            sufijo = GetSufix(apiMethod, subMetodo);

            //Posee los datos de Token
            //DEPENDIENDO DEL TIPO DE AUTENTICACION LLAMAR LA API QUE ELIGIO
            urlToken = ConfigurationManager.AppSettings["urlToken" + sufijo];//URL Endpoint para obtener Token
            if (subMetodo == "")
            {
                authTk = ConfigurationManager.AppSettings["authTK" + sufijo];//VALOR header
                nameKey = ConfigurationManager.AppSettings["nameKey" + sufijo];// Key header
            }
            responseTK = ConfigurationManager.AppSettings["responseTK" + sufijo]; //nombre dado en el header para identificar el token

            int maxRetryReq = Int32.Parse(ConfigurationManager.AppSettings["maxRetryReq"]);
            bool redo = false;
            int retries = 0;
            do
            {
                try
                {
                    log.Info($"Conectando al Endpoint (PostToken) {urlToken}");
                    var request = (HttpWebRequest)WebRequest.Create(urlToken);
                    request.Timeout = 5000;
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                    request.ContentLength = 0;
                    if (subMetodo == "")
                    {
                        request.Headers[nameKey] = authTk;
                    }                    

                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream strReader = response.GetResponseStream())
                        {
                            if (strReader == null)
                            {
                                log.Error("ERROR (400) BAD REQUEST (PostToken): La solicitud no fue válida.");
                                resultado.Add("OK", "false");
                                resultado.Add("msg", "ERROR (400) BAD REQUEST (Obtener Token): La solicitud no fue válida.");
                            }
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string res = objReader.ReadToEnd();
                                dynamic jsonObj = JsonConvert.DeserializeObject(res);
                                res = jsonObj[responseTK].ToString();
                                log.Info($"Respuesta 200 (PostToken): Respuesta exitosa.");
                                resultado.Add("OK", "true");
                                resultado.Add("msg", res);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    redo = true;
                    if (retries == 2)
                    {
                        resultado.Add("OK", "false");
                        resultado.Add("msg", $"EXCEPTION (Obtener Token): {ex.Message} INTENTOS {retries + 1}");
                    }
                    log.Error($"EXCEPTION (PostToken): {ex.Message} INTENTO {retries + 1}");
                    ++retries;

                }
            } while (redo && retries < maxRetryReq);
            return resultado;
        }

        public string GetSufix(string apiMethod, string subMetodo = "")
        {
            string sufijo = "";
            switch (apiMethod)
            {
                case "1":
                    sufijo = "_ahorro";
                    break;
                case "2":
                    //Blindaje Total
                    sufijo = "_bt";
                    break;
                case "3":
                    //ciberseguridad
                    switch (subMetodo)
                    {
                        case "0":
                            sufijo = "_ciber_cat";
                            break;
                        case "1":
                            sufijo = "_ciber_cot";
                            break;
                        case "2":
                            sufijo = "_ciber_cum";
                            break;
                        case "3":
                            sufijo = "_ciber_doc";
                            break;
                        case "4":
                            sufijo = "_ciber_rfc";
                            break;
                        default:
                            break;
                    }
                    break;
                case "4":
                    sufijo = "_fenix";
                    break;
                case "5":
                    sufijo = "_gmm";
                    break;
                case "6":
                    //hogar protegido
                    sufijo = "_hp";
                    break;
                case "7":
                    //no autos
                    sufijo = "_middleware";
                    break;
                case "8":
                    //no autos
                    sufijo = "_na";
                    break;
            }
            return sufijo;
        }


        /**
         * Metodo para realizar la solicitud al enpoint requerido por el cliente (En caso de fallo intenta 3 veces)
         * Parametros (URL, METODO, TOKEN de acceso, BODY en caso de que enpoint lo requiera)
         * Retorna Dictionary con la respuesta de la solicitud al enpoint requerido por el cliente
        */
        public Dictionary<string, string> GetResult(string url, string method, string accessTkn, string body, string apiMethod, string subMetodo = "")
        {
            string sufijo = GetSufix(apiMethod, subMetodo);

            Dictionary<string, string> resultado = new Dictionary<string, string>();

            string responseTK = ConfigurationManager.AppSettings["responseTK" + sufijo];
            int maxRetryReq = Int32.Parse(ConfigurationManager.AppSettings["maxRetryReq"]);
            bool redo = false;
            int retries = 0;
            do
            {
                try
                {
                    log.Info($"Conectando al Endpoint (GetResult): {url}");
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    //EL--> Se asigna el tipo de metodo
                    switch (method)
                    {
                        case "1":
                            request.Method = "GET";
                            break;
                        case "2":
                            request.Method = "POST";
                            break;
                        case "3":
                            request.Method = "PUT";
                            break;
                        default:
                            break;
                    }
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                    request.Headers["access_token"] = accessTkn;
                    if (method == "2" || method == "3")
                    {
                        string json = body;
                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                        {
                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                    }

                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream strReader = response.GetResponseStream())
                        {
                            if (strReader == null)
                            {
                                log.Error("ERROR (400) BAD REQUEST (GetResult): La solicitud no fue válida.");
                                resultado.Add("OK", "false");
                                resultado.Add("msg", "ERROR (400) BAD REQUEST (Solicitud endpoint): La solicitud no fue válida.");
                            }
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string res = objReader.ReadToEnd();
                                log.Info($"Respuesta 200: Respuesta exitosa.");
                                resultado.Add("OK", "true");
                                resultado.Add("msg", res);
                                resultado.Add("id", Guid.NewGuid().ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    redo = true;
                    if (retries == 2)
                    {
                        resultado.Add("OK", "false");
                        resultado.Add("msg", $"EXCEPTION (Solicitud endpoint): {ex.Message} INTENTOS {retries + 1}");
                    }
                    log.Error($"EXCEPTION (GetResult): {ex.Message} INTENTO {retries + 1}");
                    ++retries;
                }
            } while (redo && retries < maxRetryReq);
            return resultado;
        }
    }
}
