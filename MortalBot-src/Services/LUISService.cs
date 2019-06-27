
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Extensions.Configuration;


namespace CoreBot.Services
{

    public class LUISService
    {
        private readonly LUISRuntimeClient _luisClient;
        private readonly string _endpointPredictionkey;
        private readonly ApiKeyServiceClientCredentials _credentials;

        private readonly string applicationIdppId;
        private readonly string timezoneOffset;
        private readonly string verbose;
        private readonly string spellCheck;
        private readonly string staging;
        private readonly string endpoint;
        private readonly string endpointApi;
        private readonly string endpointApiKey;

        private const string sectionConfig = "LuisServiceConfig";

        public LUISService(IConfiguration _config)
        {
            _endpointPredictionkey = _config.GetSection(sectionConfig).GetValue<string>("EndpointPredictionkey"); ;
            _credentials = new ApiKeyServiceClientCredentials(_endpointPredictionkey);

            _luisClient = new LUISRuntimeClient(_credentials, new System.Net.Http.DelegatingHandler[] { });
            _luisClient.Endpoint = _config.GetSection(sectionConfig).GetValue<string>("Endpoint");

            applicationIdppId = _config.GetSection(sectionConfig).GetValue<string>("ApplicationId");
            timezoneOffset = _config.GetSection(sectionConfig).GetValue<string>("TimezoneOffset");
            verbose = _config.GetSection(sectionConfig).GetValue<string>("Verbose");
            spellCheck = _config.GetSection(sectionConfig).GetValue<string>("SpellCheck");
            staging = _config.GetSection(sectionConfig).GetValue<string>("Staging");
            endpoint = _config.GetSection(sectionConfig).GetValue<string>("Endpoint");
            endpointApi = _config.GetSection(sectionConfig).GetValue<string>("EndpointApi");
            endpointApiKey = _config.GetSection(sectionConfig).GetValue<string>("endpointApiKey");
        }

        public async Task<LuisResult> GetIntentionsByText(string text)
        {

            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _endpointPredictionkey);

            // The "q" parameter contains the utterance to send to LUIS
            queryString["q"] = text;

            // These optional request parameters are set to their default values
            queryString["timezoneOffset"] = timezoneOffset;
            queryString["verbose"] = verbose;
            queryString["spellCheck"] = spellCheck;
            queryString["staging"] = staging;

            var endpointUri = endpoint + applicationIdppId + "?" + queryString;
            var response = await client.GetAsync(endpointUri);

            var strResponseContent = await response.Content.ReadAsStringAsync();
            var luisResult = Newtonsoft.Json.JsonConvert.DeserializeObject<LuisResult>(strResponseContent);

            return luisResult;


        }

        public async Task<string> GetAllIntentions()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", endpointApiKey);

            var endpointUri = endpointApi + applicationIdppId + "/versions/0.1/intents/";
            var response = await client.GetAsync(endpointUri);
            var strResponseContent = await response.Content.ReadAsStringAsync();

            return strResponseContent;
        }
    }
}