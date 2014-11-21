using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jama.NancyHelpers.Models;
using Nancy;
using Nancy.Responses;
using Newtonsoft.Json;

namespace Jama.NancyHelpers.Builders
{
    /// <summary>
    /// Response builder to help create Nancy Responses to help in general purpose, and 
    /// error scenarios.
    /// </summary>
    public class ResponseBuilder : IAmendResponses
    {
        private Response _response;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ResponseBuilder()
        {
        }

        private ResponseBuilder(Response response)
        {
            _response = response;
        }

        /// <summary>
        /// Creates a standard reponse with an initial OK (200) status code
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a Json error response allowing for specification of status code to return
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static IAmendResponses CreateErrorResponse(Exception ex, HttpStatusCode statusCode)
        {
            var response = new Response()
            {
                ContentType = "application/json",
                ReasonPhrase = ex.Message + " " + ex.StackTrace,
                StatusCode = statusCode
            };

            var builder = new ResponseBuilder(response);

            return builder;
        }

        /// <summary>
        /// Creates a Json error response allowing for specification of status code to return
        /// </summary>
        /// <param name="reasonPhrase"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a Json error response using a list of errors
        /// </summary>
        /// <param name="errorList"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Set the error message for the response 
        /// </summary>
        /// <param name="reasonPhrase"></param>
        /// <returns></returns>
        public IAmendResponses WithErrorReason(string reasonPhrase)
        {
            _response.ReasonPhrase = reasonPhrase;
            return this;
        }

        /// <summary>
        /// Set the status code for the response that is returned
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public IAmendResponses WithStatusCode(HttpStatusCode statusCode)
        {
            _response.StatusCode = statusCode;
            return this;
        }

        /// <summary>
        /// Add custom headers to the response
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IAmendResponses AddHeader(string key, string value)
        {
            _response.Headers.Add(key, value);
            return this;
        }

        /// <summary>
        /// Add an object to the response and serialize to Json format using the default serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public IAmendResponses WithContentAsJson<T>(T model)
        {
            var serializer = new DefaultJsonSerializer();

            _response = new JsonResponse<T>(model, serializer);

            return this;
        }

        /// <summary>
        /// Add an object to the response with your own custom serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public IAmendResponses WithContentAsJson<T>(T model, ISerializer serializer)
        {
            _response = new JsonResponse<T>(model, serializer);

            return this;
        }

        /// <summary>
        /// Add an object to the response using the Json.Net serializer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Obsolete("This method is deprecated, please consider using WithContentAsJson")]
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

        /// <summary>
        /// The build step that returns a Nancy response
        /// </summary>
        /// <returns>Returns a Nancy response</returns>
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
        IAmendResponses WithContentAsJson<T>(T model);
        IAmendResponses AddHeader(string key, string value);
        Response Build();
    }
}
