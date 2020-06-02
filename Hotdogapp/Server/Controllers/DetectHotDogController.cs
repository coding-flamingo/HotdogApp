using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Hotdogapp.Shared.Models;
using Hotdogapp.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hotdogapp.Server.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/DetectHotDog")]
    public class DetectHotDogController : ControllerBase
    {
        ILogger _logger;
        public DetectHotDogController(ILogger<DetectHotDogController> logger)
        {
            _logger = logger;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<HotDogResultModel> DetectisHotDogAsync()
        {
            string imageBase64;
            byte[] imageBytes;
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    imageBase64 = await reader.ReadToEndAsync();
                }
                imageBytes = DataURLServices.ToBytes(imageBase64);
                if (!imageBytes.IsValidImage())
                {
                    throw new ArgumentException("File was not an image file type");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to read image", ex);
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            
            HotDogResultModel hotDogResult = new HotDogResultModel();
            hotDogResult.accuracy = "99%";
            hotDogResult.isHotDog = false;
            return hotDogResult;
        }
    }
}
