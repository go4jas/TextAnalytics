using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TextAnalytics.Service;

namespace TextAnalytics.Models
{
    public class TextAnalyticsService
    {
        /// <summary>
        /// container for subscription credentials. Make sure to enter 
        /// </summary>
        class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("Ocp-Apim-Subscription-Key", Constants.API_KEY);
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }

        public IList<string> AnalyzeKeyPhrases(string id, string text, string language)
        {
            ITextAnalyticsAPI client = new TextAnalyticsAPI(new ApiKeyServiceClientCredentials());
            client.AzureRegion = AzureRegions.Westcentralus;

            KeyPhraseBatchResult result = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput(language, id, text)
                        })).Result;

            return result.Documents.FirstOrDefault().KeyPhrases;
        }
        public double? AnalyzeSentiment(string id, string text, string language)
        {
            ITextAnalyticsAPI client = new TextAnalyticsAPI(new ApiKeyServiceClientCredentials());
            client.AzureRegion = AzureRegions.Westcentralus;

            SentimentBatchResult result = client.SentimentAsync(
                    new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput(language, id, text)
                        })).Result;

            return result.Documents.FirstOrDefault().Score;
        }
        public DetectedLanguage AnalyzeLanguage(string id, string text)
        {
            // Create a client.
            ITextAnalyticsAPI client = new TextAnalyticsAPI(new ApiKeyServiceClientCredentials());
            client.AzureRegion = AzureRegions.Westcentralus;

            var result = client.DetectLanguageAsync(new BatchInput(
                    new List<Input>()
                        {
                          new Input(id, text)
                    })).Result;

           return result.Documents.FirstOrDefault().DetectedLanguages.FirstOrDefault();
        }
    }
}