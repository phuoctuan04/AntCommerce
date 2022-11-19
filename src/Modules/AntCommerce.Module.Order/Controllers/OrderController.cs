using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AntCommerce.Module.Order.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public OrderController( ILogger<OrderController> logger, 
            IHttpContextAccessor httpContextAccessor) {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Get all data");
            return Ok("data");
        }

        [HttpGet]
        [Route("{orderId:int}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            _logger.LogInformation("Get product by id");
            // var product = await _productQueryService.FindByIdAsync(orderId);
            // if (product is null)
            // {
            //     _logger.LogError($"Product with Id {orderId} not found");
            //     return NotFound();
            // }

            return Ok("product");
        }

        // [Route("search")]
        // [HttpGet]
        // public async Task<IActionResult> Search([FromQuery] ProductQueryModel model)
        // {
        //     // var cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        //     // if (cancellationToken.IsCancellationRequested)
        //     // {
        //     //     return new ObjectResult(StatusCodes.Status408RequestTimeout);
        //     // }
        //     // // TODO
        //     // var result = await _mediator.Send(new QueryProductCommand() { Model = model }, cancellationToken);
        //     return Ok("ok");
        // }

        // [HttpPost]
        // [Route("")]
        // public async Task<ActionResult> Post([FromBody] ProductModel productModel)
        // {
        //     // var productModelValidator = new ProductModelValidator();
        //     // var validatorResult = await productModelValidator.ValidateAsync(productModel);

        //     // if (validatorResult.IsValid)
        //     // {
        //     //     var cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        //     //     if (cancellationToken.IsCancellationRequested)
        //     //     {
        //     //         return new ObjectResult(StatusCodes.Status408RequestTimeout);
        //     //     }

        //     //     var result = await _mediator.Send(new CreateProductCommand() { Model = productModel },
        //     //         cancellationToken);
        //     //     return Created("", result);
        //     // }
        //     // _logger.LogWarning("BadRequest");
        //     return BadRequest("validatorResult");
        // }

        // [HttpPut]
        // [Route("{orderId:int}")]
        // public async Task<IActionResult> Put(int orderId, [FromBody] ProductModel productModel)
        // {
        //     // var product = await _productQueryService.FindByIdAsync(orderId);
        //     // if (product is null)
        //     // {
        //     //     return NotFound();
        //     // }

        //     // var productModelValidator = new ProductModelValidator();
        //     // var validatorResult = await productModelValidator.ValidateAsync(productModel);

        //     // if (validatorResult.IsValid)
        //     // {
        //     //     var cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        //     //     if (cancellationToken.IsCancellationRequested)
        //     //     {
        //     //         return new ObjectResult(StatusCodes.Status408RequestTimeout);
        //     //     }

        //     //     productModel.Id = orderId;
        //     //     var result = await _mediator.Send(new EditProductCommand() { Model = productModel },
        //     //         cancellationToken);
        //     //     return Ok(result);
        //     // }
        //     // _logger.LogWarning("BadRequest");
        //     return BadRequest("validatorResult");
        // }

        [HttpDelete]
        [Route("{orderId:int}")]
        public async Task<IActionResult> Delete(int orderId)
        {
            // var product = await _productQueryService.FindByIdAsync(orderId);
            // if (product is null)
            // {
            //     return NotFound();
            // }

            // var deleteResult = await _productCommandService.DeleteAsync(orderId);
            // if (deleteResult > 0)
            // {
            //     return NoContent();
            // }
            // _logger.LogWarning("BadRequest");
            return BadRequest("Cannot delete order");
        }
    }
}