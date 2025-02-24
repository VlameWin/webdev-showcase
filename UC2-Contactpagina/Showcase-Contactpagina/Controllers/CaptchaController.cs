using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Showcase_Contactpagina.Controllers;

public class CaptchaController : Controller
{
    private IConfiguration Configuration { get; }
    private HttpRequest HttpRequest { get; }
    private HttpClient HttpClient { get; }
    private const string RecaptchaVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";
    public string? Error { get; set; }

    public CaptchaController(IConfiguration configuration, HttpClient httpClient, HttpRequest request)
    {
        Configuration = configuration;
        HttpClient = httpClient;
        HttpRequest = request;
    }

    public async Task<bool> ValidateCaptcha()
    {
        var recaptchaResponse = HttpRequest.Form["g-recaptcha-response"];
        if (string.IsNullOrEmpty(recaptchaResponse))
        {
            Error = "Please complete the reCAPTCHA.";
            return false;
        }

        // Check if the reCAPTCHA response is valid
        var captchaResponse = await HttpClient.PostAsync(
            $"{RecaptchaVerificationUrl}?secret={Configuration.GetSection("ReCaptchaSettings")
                .GetSection("PrivateKey").Value}&response={recaptchaResponse}",
            null
        );
        var capchaResponseContent = await captchaResponse.Content.ReadAsStringAsync();
        var capchaResponseResult =
            JsonConvert.DeserializeObject<RecaptchaVerificationResponse>(capchaResponseContent);

        if (!capchaResponseResult.Success)
        {
            Error = "reCAPTCHA failed.";
            return false;
        }

        return true;
    }
    
    public class RecaptchaVerificationResponse
    {
        [JsonProperty("success")] public bool Success { get; set; }
    }
}
