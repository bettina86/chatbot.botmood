/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;

namespace BotLib
{

    /// <summary>
    /// Implementation of https://www.microsoft.com/cognitive-services/en-us/text-analytics/documentation
    /// </summary>
    public class TextAnalysisService
    {
        private string ApiKey = WebConfigurationManager.AppSettings["MicrosoftTextAnalysisApiKey"];

        #region Requests
        private class TextRequest
        {
            public TextRequest()
            {
                Documents = new List<TextDocument>();
            }
            [JsonProperty("documents")]
            public List<TextDocument> Documents { get; set; }
        }

        private class TextDocument
        {
            public TextDocument(string text, string language)
            {
                Id = Guid.NewGuid().ToString();
                Language = language;
                Text = text;
            }
            [JsonProperty("language")]
            public string Language { get; private set; }
            [JsonProperty("id")]
            public string Id { get; private set; }
            [JsonProperty("text")]
            public string Text { get; private set; }
        }
        #endregion

        private readonly HttpClient _httpClient;
        /// <summary>
        /// Cognitive Text service endpoint
        /// </summary>
        private string serviceEndpoint = WebConfigurationManager.AppSettings["MicrosoftTextAnalysisServiceEndpoint"];

        public TextAnalysisService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ApiKey);
        }


        /* AQUI TITO ESTÁ COM ERRO
        public async Task<List<string>> DetectLanguage(string text)
        {
            if (string.IsNullOrEmpty("pt") || string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }
            var request = new TextRequest();
            request.Documents.Add(new TextDocument(text, "pt"));
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync($"{serviceEndpoint}languages", content).ConfigureAwait(false);
            var response = JObject.Parse(await result.Content.ReadAsStringAsync());
            CatchAndThrow(response);
            return response["documents"].Children().First().Value<JArray>("detectedLanguages").ToObject<List<string>>();
        }
        */


        public async Task<List<string>> KeyPhrases(string language, string text)
        {
            if (string.IsNullOrEmpty(language) || string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }
            var request = new TextRequest();
            request.Documents.Add(new TextDocument(text, language));
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync($"{serviceEndpoint}keyPhrases", content).ConfigureAwait(false);
            var response = JObject.Parse(await result.Content.ReadAsStringAsync());
            CatchAndThrow(response);
            return response["documents"].Children().First().Value<JArray>("keyPhrases").ToObject<List<string>>();
        }

        public async Task<double> Sentiment(string language, string text)
        {
            if (string.IsNullOrEmpty(language) || string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }
            var request = new TextRequest();
            request.Documents.Add(new TextDocument(text, language));
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync($"{serviceEndpoint}sentiment", content).ConfigureAwait(false);
            var response = JObject.Parse(await result.Content.ReadAsStringAsync());
            CatchAndThrow(response);
            return response["documents"].Children().First().Value<double>("score");
        }

        private void CatchAndThrow(JObject response)
        {
            if (response["errors"] != null && response["errors"].Children().Any())
            {
                throw new Exception(response["errors"].Children().First().Value<string>("message"));
            }
            if (response["code"] != null && response["message"] != null)
            {
                throw new Exception(response["message"].Value<string>());
            }
        }
    }

}