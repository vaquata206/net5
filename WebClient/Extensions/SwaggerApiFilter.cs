using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebClient.Extensions
{
    /// <summary>
    /// Swagger api filter.
    /// </summary>
    public class SwaggerApiFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var apiDescription in context.ApiDescriptions)
            {
                if (!apiDescription.RelativePath.StartsWith("api/"))
                {
                    var path = "/" + apiDescription.RelativePath;
                    swaggerDoc.Paths.Remove(path);
                }
            }
        }
    }
}
