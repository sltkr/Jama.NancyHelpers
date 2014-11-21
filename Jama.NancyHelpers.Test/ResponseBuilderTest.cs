using System;
using Jama.NancyHelpers.Builders;
using Nancy;
using Nancy.Responses;
using Nancy.Testing;
using Xunit;

namespace Jama.NancyHelpers.Test
{

    public class TestModule : NancyModule
    {
        public TestModule()
        {
            this.Get["/withcontent"] = _ => ResponseBuilder.CreateBasicResponse()
                .WithContent("OK")
                .Build();

            this.Get["/withcontentjson"] = _ => ResponseBuilder.CreateBasicResponse()
                .WithContentAsJson("OK")
                .Build();

            this.Get["/withcontentjson_object"] = _ => ResponseBuilder.CreateBasicResponse()
                .WithContentAsJson(
                new
                    {
                       Test = "Test" 
                    })
                .Build();
        }
    }

    public class ResponseBuilderTest
    {
        [Fact]
        public void CreatingResponse_with_AsJson_returns_JsonResponse()
        {
            var sut = new ResponseBuilder();
            var response = sut.WithContentAsJson(new object()).Build();
            Assert.IsAssignableFrom(typeof(JsonResponse<Object>), response);
        }

        [Fact]
        public void WhenCheckingBody_IGetAnException_StreamClosed()
        {
            var sut = new Browser(with => with.Module<TestModule>());

            Assert.Throws(typeof(ObjectDisposedException), () => sut.Get("/withcontent").Body.AsString());
        }

        [Fact]
        public void ICanCheckBody_WhenUsingAsJsonOption()
        {
            var sut = new Browser(with => with.Module<TestModule>());

            var response = sut.Get("/withcontentjson");
            var body = response.Body.AsString();

            Assert.True(!string.IsNullOrEmpty(body));
            Assert.True(body.Contains("OK"));
        }

        [Fact]
        public void WhenUsingAsJsonOption_IGetAProperJsonFormat()
        {
            var sut = new Browser(with => with.Module<TestModule>());

            var response = sut.Get("/withcontentjson_object");
            var body = response.Body.AsString();

            Assert.True(!string.IsNullOrEmpty(body));
            Assert.True(body.Contains("{\"test\":\"Test\"}"));
        }
    }
}
