using System.Net;
using Newtonsoft.Json;

namespace ScreechrDemo.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next
            , ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var errorResult = new ErrorInfo()
                {
                    Reason = $"Message: {ex.Message}, Source: {ex.Source}, Method: {ex.TargetSite}"
                    ,
                    ErrorCode = HttpStatusCode.InternalServerError.ToString()
                }.ToString();

                _logger.LogError(errorResult);

                await httpContext.Response.WriteAsync(errorResult);
            }
        }


    }

    public class ErrorInfo
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
