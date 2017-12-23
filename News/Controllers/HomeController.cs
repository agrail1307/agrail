using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace News.Controllers
{
    public class HomeController : Controller
    {
        public List<PravdaNews> list = new List<PravdaNews>();

        public ActionResult Index(string sort)
        {
            using (var client = new HttpClient())
            {
                var url = client.GetAsync("http://www.pravda.com.ua/").Result;
                var content = url.Content.ReadAsStringAsync().Result;

                var regex = "<div ((class=\"article article article_bold\")|(class=\"article article\")|(class=\"article article article_red\"))>.*?</a>";
                var time_regex = "[0-9]{2}:[0-9]{2}";
                var news_regex = "<a.*[^</a>]";

                var for_replace = "<a.*>";

                Match html_string = Regex.Match(content, regex);

                while (html_string.Success)
                {
                    Match time_string = Regex.Match(html_string.ToString(), time_regex);
                    Match news_string = Regex.Match(html_string.ToString(), news_regex);

                    var found_string = Regex.Replace(news_string.ToString(), for_replace, "");

                    list.Add(new PravdaNews(time_string.Value, found_string));
                    html_string = html_string.NextMatch();
                }
            }

            ViewBag.News = sort == "News" ? "News_desc" : "News";
            ViewBag.Time = sort == "Time" ? "Time_desc" : "Time";

            switch (sort)
            {
                case "News":
                    list.Sort((x, y) =>
                    {
                        return x.DayNews.CompareTo(y.DayNews);
                    });
                    break;
                case "News_desc":
                    list.Sort((x, y) =>
                    {
                        return x.DayNews.CompareTo(y.DayNews) * -1;
                    });
                    break;
                case "Time":
                    list.Sort((x, y) =>
                    {
                        return x.Time.CompareTo(y.Time);
                    });
                    break;
                case "Time_desc":
                    list.Sort((x, y) =>
                    {
                        return x.Time.CompareTo(y.Time) * -1;
                    });
                    break;
            }
            return View(list);
        }
    }
}