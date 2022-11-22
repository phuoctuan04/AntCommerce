namespace AntCommerce.Module.GraphQL.Controllers
{
    using GraphQL;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/graphql")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class GraphQLController : ControllerBase
    {
        private readonly IProductQueryService _productQueryService;
        private readonly IProductCommandService _productCommandService;
        private readonly IMediator _mediator;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Query query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Variables = query.Variables,
                ComplexityConfiguration = new GraphQL.Validation.Complexity.ComplexityConfiguration { MaxDepth = 15, }
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}