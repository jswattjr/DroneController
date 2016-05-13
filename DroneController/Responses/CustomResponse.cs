using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DroneController.Responses
{
    public class CustomResponse : IHttpActionResult
    {
        public string message { get; private set; }
        public HttpStatusCode status { get; private set; }
        public CustomResponse(string message, HttpStatusCode status)
        {
            this.message = message;
            this.status = status;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(status);
            response.Content = new StringContent(message); // Put the message in the response body (text/plain content).
            return response;
        }
    }
}