namespace AntCommerce.Module.Product.Controllers
{
    using AntCommerce.Module.Product.Commands;
    using AntCommerce.Module.Product.DTOs;
    using AntCommerce.Module.Product.Services;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/product")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductController : ControllerBase
    {
       private readonly IProductQueryService _productQueryService;
       private readonly IProductCommandService _productCommandService;
       private readonly IMediator _mediator;
       
       private readonly IHttpContextAccessor _httpContextAccessor;
       private readonly ILogger _logger;

        public ProductController(IProductQueryService productQueryService,
            IProductCommandService productCommandService,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ProductController> logger)
       {
           _productQueryService = productQueryService;
           _productCommandService = productCommandService;
            _mediator = mediator;
           _httpContextAccessor= httpContextAccessor;
           _logger = logger;
       }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Get all data");
            var data = await _productQueryService.FindAllAsync();
            return Ok(data);
        }

        [HttpGet]
        [Route("{productId:int}")]
        public async Task<IActionResult> GetById(int productId)
        {
            _logger.LogInformation("Get product by id");
            var product = await _productQueryService.FindByIdAsync(productId);
            if (product is null)
            {
                _logger.LogError($"Product with Id {productId} not found");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("search")]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] ProductQueryModel model)
        {
            var cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

            if (cancellationToken.IsCancellationRequested)
            {
                return new ObjectResult(StatusCodes.Status408RequestTimeout);
            }
            // TODO
            var result = await _mediator.Send(new QueryProductCommand() { Model = model }, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Post([FromBody] ProductModel productModel)
        {
            var productModelValidator = new ProductModelValidator();
            var validatorResult = await productModelValidator.ValidateAsync(productModel);

            if (validatorResult.IsValid)
            {
                var cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

                if (cancellationToken.IsCancellationRequested)
                {
                    return new ObjectResult(StatusCodes.Status408RequestTimeout);
                }

                var result = await _mediator.Send(new CreateProductCommand() { Model = productModel },
                    cancellationToken);
                return Created("", result);
            }
            _logger.LogWarning("BadRequest");
            return BadRequest(validatorResult);
        }

        [HttpPut]
        [Route("{productId:int}")]
        public async Task<IActionResult> Put(int productId, [FromBody] ProductModel productModel)
        {
            var product = await _productQueryService.FindByIdAsync(productId);
            if (product is null)
            {
                return NotFound();
            }

            var productModelValidator = new ProductModelValidator();
            var validatorResult = await productModelValidator.ValidateAsync(productModel);

            if (validatorResult.IsValid)
            {
                var cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

                if (cancellationToken.IsCancellationRequested)
                {
                    return new ObjectResult(StatusCodes.Status408RequestTimeout);
                }

                productModel.Id = productId;
                var result = await _mediator.Send(new EditProductCommand() { Model = productModel },
                    cancellationToken);
                return Ok(result);
            }
            _logger.LogWarning("BadRequest");
            return BadRequest(validatorResult);
        }

        [HttpDelete]
        [Route("{productId:int}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var product = await _productQueryService.FindByIdAsync(productId);
            if (product is null)
            {
                return NotFound();
            }

            var deleteResult = await _productCommandService.DeleteAsync(productId);
            if (deleteResult > 0)
            {
                return NoContent();
            }
            _logger.LogWarning("BadRequest");
            return BadRequest("Cannot delete product");
        }
    }
}
