﻿namespace VVRestApi.Administration.Customers
{
    using System;

    using Newtonsoft.Json.Linq;

    using VVRestApi.Common;
    using VVRestApi.Common.Messaging;

    public class CustomerManager : VVRestApi.Common.BaseApi
    {
        internal CustomerManager(AdministrationApi api)
        {
            base.Populate(api.CurrentToken);
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="name">Name of the customer</param>
        /// <param name="alias">Alias of the customer. This will be used in the URL and for logging into the VaultApi</param>
        /// <param name="databaseAlias">Alias to give to the initial database that will be created for the customer. This will be used in the URL and for logging into the VaultApi.</param>
        /// <param name="newAdminUsername">A new unique username for the Vault.Access account on the administrator</param>
        /// <param name="newAdminPassword">A password for the new user.</param>
        /// <param name="newAdminEmailAddress">A valid e-mail address for the new user account. Password reset requests will be sent to this account.</param>
        /// <param name="databaseCount">The number of databases to license to this customer. At minimum, 1 database license will be granted.</param>
        /// <param name="userCount">The number of user accounts to license to the customer. At minimum, 5 user licenses will be granted.</param>
        /// <param name="addCurrentUser">If set to true, the current user will be added as a vault.access user.</param>
        /// <returns></returns>
        public Customer CreateCustomer(string name, string alias, string databaseAlias, string newAdminUsername, string newAdminPassword, string newAdminEmailAddress, int databaseCount, int userCount, bool addCurrentUser)
        {
            return HttpHelper.Post<Customer>(GlobalConfiguration.Routes.Customers, string.Empty, this.CurrentToken, new { name = name, alias = alias, databaseAlias = databaseAlias, newAdminUsername = newAdminUsername, newAdminPassword = newAdminPassword, newAdminEmailAddress = newAdminEmailAddress, databaseCount = databaseCount, userCount = userCount, addCurrentUser = addCurrentUser });
        }
    }
}