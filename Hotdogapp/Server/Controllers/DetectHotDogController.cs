using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Hotdogapp.Shared.Models;
using Hotdogapp.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hotdogapp.Server.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/DetectHotDog")]
    public class DetectHotDogController : ControllerBase
    {
        private readonly PredictionEnginePool<InMemoryImageData, ImagePrediction> _predictionEnginePool;
        private readonly ILogger _logger;
        public DetectHotDogController(ILogger<DetectHotDogController> logger, PredictionEnginePool<InMemoryImageData, ImagePrediction> predictionEnginePool)
        {
            _logger = logger;
            _predictionEnginePool = predictionEnginePool;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<HotDogResultModel> DetectisHotDogAsync()
        {
            string imageBase64;
            InMemoryImageData imageInputforModel = new InMemoryImageData();
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    imageBase64 = await reader.ReadToEndAsync();
                }
                imageInputforModel.Image = DataURLServices.ToBytes(imageBase64);
                if (!imageInputforModel.Image.IsValidImage())
                {
                    throw new ArgumentException("File was not an image file type");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to read image", ex);
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var prediction = _predictionEnginePool.Predict(imageInputforModel);
            HotDogResultModel hotDogResult = new HotDogResultModel
            {
                accuracy = prediction.Score.Max().ToString(),
                isHotDog = prediction.PredictedLabel.Equals("hot_dog")
            };
            return hotDogResult;
        }
    }
}
