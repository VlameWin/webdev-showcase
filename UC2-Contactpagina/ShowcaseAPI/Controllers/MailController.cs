using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using ShowcaseAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShowcaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private IConfiguration Configuration { get; }
    
    // POST api/<MailController>
    [HttpPost]
    public ActionResult Post([Bind("FirstName, LastName, Email, Phone")] Contactform form)
    {
        //Op brightspace staan instructies over hoe je de mailfunctionaliteit werkend kunt maken:
        //Project Web Development > De showcase > Week 2: contactpagina (UC2) > Hoe verstuur je een mail vanuit je webapplicatie met Mailtrap?

        //Send emails using MailTrap
        var client = new SmtpClient(
            Configuration.GetSection("MailTrapSettings").GetSection("Host").Value,
            Int32.Parse(Configuration.GetSection("MailTrapSettings").GetSection("Port").Value)
            ) 
        {
            Credentials = new NetworkCredential(
                Configuration.GetSection("MailTrapSettings").GetSection("Username").Value,
                Configuration.GetSection("MailTrapSettings").GetSection("Password").Value
                ),
            EnableSsl = true
        };
        client.Send(
            "example@example.com",
            form.Email,
            form.Subject,
            $"Naam: {form.FirstName} {form.LastName} \n Email: {form.Email} \n Telefoonnummer: {form.Phone} \n Bericht: {form.Message}");

        return Ok();
    }
}
