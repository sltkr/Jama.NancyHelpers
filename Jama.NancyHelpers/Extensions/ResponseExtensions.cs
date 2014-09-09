using Nancy;

namespace Jama.NancyHelpers.Extensions
{
    public static class ResponseExtensions
    {
        public static Response AddGlobalCors(this Response response)
        {
            return response
                .WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
        }
    }
}
