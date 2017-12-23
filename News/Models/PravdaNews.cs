using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace News.Models
{
    public class PravdaNews
    {
        public string Time { get; set; }

        public string DayNews { get; set; }

        public PravdaNews(string time, string news)
        {
            Time = time; DayNews = news;
        }
    }
}