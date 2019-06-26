using CoreBot.Models;
using Microsoft.Azure.CognitiveServices.Language.SpellCheck;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot.Services
{
    public class MortalBotService
    {
        private readonly SpellCheckClient _spellCheckClient;

        private readonly string apiKey;
        private readonly string countryCode;
        private readonly string market;
        private readonly string mode;

        private const string sectionConfig = "SpellCheckConfig";

        SearchIndexClient indexClient;
        string searchServiceName;
        string queryApiKey;
        string searchServiceIndex;

        public MortalBotService(IConfiguration _config)
        {

            //////////////////////////////////////////// AZURE SPEEL CHECK

            apiKey = _config.GetSection(sectionConfig).GetValue<string>("ApiKey");
            countryCode = _config.GetSection(sectionConfig).GetValue<string>("CountryCode");
            market = _config.GetSection(sectionConfig).GetValue<string>("Market");
            mode = _config.GetSection(sectionConfig).GetValue<string>("Mode");



            ApiKeyServiceClientCredentials _credentials = new ApiKeyServiceClientCredentials(apiKey);
            _spellCheckClient = new SpellCheckClient(_credentials);


            //////////////////////////////////////////// AZURE SEARCH


            searchServiceName = _config.GetSection("AzureSearchConnection").GetSection("SearchServiceName").Value;
            queryApiKey = _config.GetSection("AzureSearchConnection").GetSection("SearchServiceQueryApiKey").Value;
            searchServiceIndex = _config.GetSection("AzureSearchConnection").GetSection("SearchServiceIndex").Value;
            indexClient = new SearchIndexClient(searchServiceName, searchServiceIndex, new SearchCredentials(queryApiKey));
        }


        //TODO: AZURE SPEEL CHECK
        public string ValidateSpellCheck(string text)
        {
            var spellCheckResult = _spellCheckClient
                   .SpellCheckerAsync(text, setLang: market, market: market, countryCode: countryCode, mode: mode);



            foreach (var check in spellCheckResult.Result.FlaggedTokens)
            {
                var maxScore = check.Suggestions.Where(p => p.Score == check.Suggestions.Max(m => m.Score)).SingleOrDefault();
                text = text.Replace(check.Token, maxScore.Suggestion);
            };



            return text;
        }




        //TODO: AZURE SEARCH
        public IList<Acciones> SearchByText(string text)
        {
            DocumentSearchResult<Acciones> search;
            search = indexClient.Documents.Search<Acciones>(text);



            var result = (from r in search.Results where r.Score > 0.1 select r.Document).ToList();
            return result;
        }
    }
}
