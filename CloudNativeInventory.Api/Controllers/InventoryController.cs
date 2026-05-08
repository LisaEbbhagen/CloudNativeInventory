using CloudNativeInventory.Api.Data;
using CloudNativeInventory.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudNativeInventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly InventoryDbContext _context;
    private readonly IConfiguration _configuration;

    public InventoryController(InventoryDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    // This endpoint is used to prove that the app has successfully retrieved the secret integration key (from Azure Key Vault in production)
    [HttpGet("system/verify-integration")]
    public IActionResult VerifyExternalIntegration()
    {
        var apiKey = _configuration["ExternalServices:VendorApiKey"];

        if (string.IsNullOrEmpty(apiKey) || apiKey == "LOCAL_DEV_SECRET_12345_DO_NOT_DEPLOY")
        {
            return StatusCode(500, new { Status = "Unsecured", Message = "Running with local (or missing) secret!" });
        }

        return Ok(new { Status = "Secured", Message = "Secret successfully loaded via secure configuration." });
    }
}