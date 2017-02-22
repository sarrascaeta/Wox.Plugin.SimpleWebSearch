using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Wox.Plugin;

namespace SimpleWebSearch
{
    class Main : IPlugin
    {
        private PluginInitContext context;

        public void Init(PluginInitContext context)
        {
            this.context = context;
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            results.Add(new Result()
            {
                Title = $"{query}",
                SubTitle = $"Search Google for '{query}'",
                IcoPath = "Images\\google.png",  //relative path to your plugin directory
                Score = 5,
                Action = e =>
                {
                    Process.Start("http://google.com/search?q=" + HttpUtility.UrlEncode(query.ToString()));
                    return true;
                }
            });

            return results;
        }

    }
}
