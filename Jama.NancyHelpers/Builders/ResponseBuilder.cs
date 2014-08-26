using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jama.NancyHelpers.Models;
using Nancy;
using Newtonsoft.Json;

namespace Jama.NancyHelpers.Builders
{
    public class ResponseBuilder : IAmendResponses
    {
        private Response _response;

        public ResponseBuilder()
        {
        }

        private ResponseBuilder(Response response)
        {
            _response = response;
        }

        public static IAmendResponses CreateBasicResponse()
        {
            var response = new Response()
            {
                ContentType = "application/json",
                StatusCode = HttpStatusCode.OK
            };

            var builder = new ResponseBuilder(response);

            return builder;
        }

        public static IAmendResponses CreateErrorResponse(string reasonPhrase, HttpStatusCode statusCode)
        {
            var response = new Response()
            {
                ContentType = "application/json",
                ReasonPhrase = reasonPhrase,
                StatusCode = statusCode
            };

            var builder = new ResponseBuilder(response);

            return builder;
        }

        public static IAmendResponses CreateErrorResponse(List<Error> errorList, HttpStatusCode statusCode)
        {
            var response = new Response()
            {
                ContentType = "application/json",
                ReasonPhrase = errorList.First().ErrorMessage,
                StatusCode = statusCode
            };

            var builder = new ResponseBuilder(response);

            return builder;
        }

        public IAmendResponses WithErrorReason(string reasonPhrase)
        {
            _response.ReasonPhrase = reasonPhrase;
            return this;
        }

        public IAmendResponses WithStatusCode(HttpStatusCode statusCode)
        {
            _response.StatusCode = statusCode;
            return this;
        }

        public IAmendResponses AddHeader(string key, string value)
        {
            _response.Headers.Add(key, value);
            return this;
        }

        public IAmendResponses WithContent(object model)
        {
            _response.Contents = stream =>
            {
                var serializer = new JsonSerializer();

                using (var sw = new StreamWriter(stream))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, model);
                }
            };

            return this;
        }

        public Response Build()
        {
            return _response;
        }

    }

    public interface IAmendResponses
    {
        IAmendResponses WithErrorReason(string reasonPhrase);
        IAmendResponses WithStatusCode(HttpStatusCode statusCode);
        IAmendResponses WithContent(object model);
        IAmendResponses AddHeader(string key, string value);
        Response Build();
    }
}
