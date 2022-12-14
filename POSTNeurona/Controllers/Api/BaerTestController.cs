using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using POSTNeurona.Models;
using System.Diagnostics;

namespace POSTNeurona.Controllers.Api
{
    /**
     * Clase para simular la API que retorna el Token (API LALO)
     * 
     * 
     */
    public class BaerTestController : ApiController
    {
        //Atributo para declarar Log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /**
         * Metodo para recibir la solicutd POST de la web
         * Parametros: request (Parametros provinientes de la web)
         * Retorna: HttpResponseMessage (200 en caso exitoso, 400 en caso de ERROR)
         */
        [HttpPost]
        // POST: api/BaerTestController
        public HttpResponseMessage Post(HttpRequestMessage request)
        {

            //Posee los datos de TokenDevLalo
            var urlToken = request.RequestUri.AbsoluteUri;
            var authTK = request.Headers.Authorization.ToString();
            var nameKey = "Authorization";

            var re = Request;
            var headers = re.Headers;

            string token;

            if (headers.Contains(nameKey))
            {
                token = headers.GetValues(nameKey).First();
                if (authTK != token)
                {
                    return request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, bool>()
                {

                    { "Ok", false },

                }, Configuration.Formatters.JsonFormatter);
                }
            }
            else
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, bool>()
                {

                    { "Ok", false },

                }, Configuration.Formatters.JsonFormatter);
            }
            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            //while (true)
            //{
            //    if (sw.ElapsedMilliseconds > 6000) return request.CreateResponse(HttpStatusCode.BadRequest, new Dictionary<string, bool>()
            //    {

            //        { "Ok", false },

            //    }, Configuration.Formatters.JsonFormatter); ;
            //}
            

            return request.CreateResponse(HttpStatusCode.OK, new Dictionary<string, string>()
            {
                
                    { "access_token", "1cef685d-32fa-459a-b7fc-82f4dae6fb37" },
                
            }, Configuration.Formatters.JsonFormatter);
        }
    }
}
