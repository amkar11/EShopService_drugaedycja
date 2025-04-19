using Microsoft.AspNetCore.Mvc;
using EShop.Application;
using EShop.Domain.CreditCardProvider;
using System.Net;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopService_drugaedycja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        public readonly ICreditCardService _creditCardService;
        public CreditCardController(ICreditCardService creditCardService)
        {
            _creditCardService = creditCardService;
        }

        // GET api/<CreditCardController>/5
        [HttpGet]
        public IActionResult Get(string cardNumber)
        {
            try 
            {
                _creditCardService.ValidateCard(cardNumber);
                return Ok(new { message = _creditCardService.GetCardType(cardNumber) });
            }
            catch (CreditNumberTooShortException ex)
            {
                return BadRequest(new { error = $"{ex.Message}", code = HttpStatusCode.BadRequest });
            }
            catch (CreditNumberTooLongException ex)
            {
                return BadRequest(new { error = $"{ex.Message}", code = HttpStatusCode.BadRequest });
            }
            catch (CardNumberInvalidException ex)
            {
                return StatusCode(406, new { message = $"{ex.Message}" });
            }
        }


        // POST api/<CreditCardController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CreditCardController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CreditCardController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
