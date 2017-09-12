using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Wox.Plugin;

namespace SimpleWebSearch
{
    class Main : IPlugin
    {
        private PluginInitContext context;
        private string searchSuggestionsBaseUrl = "http://clients1.google.com/complete/search?hl=en&output=toolbar&q=";

        public void Init(PluginInitContext context)
        {
            this.context = context;
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            AddToResultsList(results, query.Search, 6); //I don't know how the score works, so a manual one is used

            AddSearchSuggestions(results, query);

            return results;
        }

        private void AddSearchSuggestions(List<Result> results, Query query)
        {
            var suggestionsList = GetGoogleSuggestionsList(query.Search);

            foreach (XmlNode suggestion in suggestionsList)
            {
                AddToResultsList(results, suggestion.Attributes[0].Value, 5); //lower score used here so that the users query is always at the top
            }
        }

        private XmlNodeList GetGoogleSuggestionsList(String query)
        {
            string url = searchSuggestionsBaseUrl + HttpUtility.UrlEncode(query);

            XmlDocument suggestionsDoc = new XmlDocument();
            suggestionsDoc.Load(url);

            XmlNodeList suggestionsList = suggestionsDoc.GetElementsByTagName("su" +
                                                                              "ggestion");
            return suggestionsList;
        }

        private void AddToResultsList(List<Result> results, string query, int score)
        {

            results.Add(new Result()
            {
                Title = $"{query}",
                SubTitle = $"Search Google for '{query}'",
                IcoPath = "Images\\google.png",  //relative path to your plugin directory
                Score = score,
                Action = e =>
                {
                    Process.Start("http://google.com/search?q=" + HttpUtility.UrlEncode(query.ToString()));
                    return true;
                }
            });
        }
    }
}
