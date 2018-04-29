using System.Linq;
using AspNetCore21Showcase.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore21Showcase.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ValuesController : Controller {

        //ActionResult<ValuesReponse> aiuta la documentazione
        //Swagger a capire che tipo di dato verrà restituito dall'action
        [HttpPost, Route("")]
        public ActionResult<ValuesResponse> Get([FromBody] ValuesRequest request) {

            //Non ho bisogno di controllare ModelState.IsValid perché viene fatto automaticamente
            //dato che ho usato l'attributo ApiController
            //if (!ModelState.IsValid) {
            //    return BadRequest();
            //}

            //Posso quindi dare per scontato che l'oggetto ValuesRequest abbia valori
            //conformi alle data annotation che sono state poste sulle sue proprietà
            var response = new ValuesResponse {
                Length = request.Text.Trim().Length,
                UniqueLetters = request.Text.Trim().Distinct().Count()
            };
            return Ok(response);
        }
    }
}