using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ButterMove.Controllers;

[ApiController]
[Route("[controller]")]
public class EstimateController : ControllerBase
{
    private readonly ILogger<EstimateController> _logger;
    private enum EnumHeaderValues { State = 0, Estimate, Kilometers, Amount };

    public EstimateController(ILogger<EstimateController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetEstimate()
    {
        const string HeaderKeyName = "ip-header";
        if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue))
        {
            return BadRequest();
        }

        string valueState = string.Empty;
        AmountCalculator.EnumEstimateType valueEstimate;
        int valueKilometers = 0;
        decimal valueAmount = 0;

        try
        {
            // Header values
            string[] pars = headerValue[0].Trim().Split(",");
            valueState = pars[((int)EnumHeaderValues.State)];
            valueEstimate = pars[((int)EnumHeaderValues.Estimate)].ToLower().Equals("normal") ? AmountCalculator.EnumEstimateType.Normal : AmountCalculator.EnumEstimateType.Premium;
            valueKilometers = int.Parse(pars[((int)EnumHeaderValues.Kilometers)]);
            valueAmount = decimal.Parse(pars[((int)EnumHeaderValues.Amount)]);
        }
        catch
        {
            return BadRequest();
        }

        try
        {
            // Calculator
            var calc = new AmountCalculator(valueState, valueEstimate, valueKilometers, valueAmount);

            var result = new
            {
                amount = calc.GetAmount(),
                processed = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK")
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

