﻿using System;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Users;

namespace VVRestApi.Vault.CustomQueries
{
    public class CustomQueryManager : VVRestApi.Common.BaseApi
    {
        internal CustomQueryManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }
        /// <summary>
        /// returns the result of the saved query
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public JArray GetCustomQueryResults(string queryName, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            var queryString = string.Format("queryName={0}", UrlEncode(queryName));

            var results = HttpHelper.Get(VVRestApi.GlobalConfiguration.Routes.CustomQuery, queryString, options, GetUrlParts(), this.ApiTokens);
            var data = results.Value<JArray>("data");
            return data;
        }

        /// <summary>
        /// returns the result of a saved query
        /// </summary>
        /// <param name="queryId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public dynamic GetCustomQueryResults(Guid queryId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            var results = HttpHelper.Get(VVRestApi.GlobalConfiguration.Routes.CustomQueryId, "", options, GetUrlParts(), this.ApiTokens, queryId);
            var data = results.Value<JArray>("data");
            return data;
        }

    }
}