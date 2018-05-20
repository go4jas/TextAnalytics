using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextAnalytics.Models;
using TextAnalytics.Service;

namespace TextAnalytics.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult RunAnalytics(string inputData)
        {
            //return Json(inputData, JsonRequestBehavior.AllowGet);
            //var sentimentsDetected = SentimentService.AnalyzeSentiment("123456789", inputData, "");
            //var detectedLanguage = SentimentService.AnalyzeLanguage("1234567899", inputData);

            var service = new TextAnalyticsService();

            var detectedLanguage = service.AnalyzeLanguage("1", inputData);

            var languageData = String.Format("{0}({1}) with {2} % confidence", detectedLanguage.Name, 
                detectedLanguage.Iso6391Name, detectedLanguage.Score * 100);
            var languageConfidence = detectedLanguage.Score * 100;

            var keyPhrases = service.AnalyzeKeyPhrases("1", inputData, detectedLanguage.Iso6391Name);
            var sentiment = service.AnalyzeSentiment("1", inputData, detectedLanguage.Iso6391Name);

            var joinedKeyPhrases = string.Join(", ", keyPhrases);

            sentiment = (sentiment != null) ? sentiment * 100 : 0;
            return Json(new {language = languageData, sentiments = sentiment, phrases = joinedKeyPhrases }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}