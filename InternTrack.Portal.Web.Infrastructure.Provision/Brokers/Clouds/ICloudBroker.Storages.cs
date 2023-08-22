// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using InternTrack.Portal.Web.Infrastructure.Provision.Models.Storages;
using System.Threading.Tasks;

namespace InternTrack.Portal.Web.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<SqlServerResource> CreateSqlServerAsync(
            string sqlServerName,
            ResourceGroupResource resourceGroup);

        ValueTask<SqlDatabaseResource> CreateSqlDataBaseAsync(
            string sqlDatabaseName,
            SqlServerResource sqlServer);

        SqlDatabaseAccess GetAdminAccess();
    }
}
