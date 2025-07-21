using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elasticsearch.Net;
using Nest;

namespace TetraTech.TTProjetPlus
{
    public class ConnectionToEs
    {
        #region Connection string to connect with Elasticsearch  

        public ElasticClient EsClient()
        {
            var nodes = new Uri[]
            {
                new Uri("http://localhost:9200/"),
            };

            //var connectionPool = new StaticConnectionPool(nodes);
            //var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming();
            //var elasticClient = new ElasticClient(connectionSettings);

            var setting = new ConnectionSettings(nodes[0]);
            var client = new ElasticClient(setting);
            return client;
        }

        #endregion Connection string to connect with Elasticsearch  
    }
}