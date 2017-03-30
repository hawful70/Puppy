﻿#region	License

//------------------------------------------------------------------------------------------------
// <Auto-generated>
//     <Author> Top Nguyen (http://topnguyen.net) </Author>
//     <Project> TopCore.Data.EF </Project>
//     <File> 
//         <Name> TopCoreContextFactory.cs </Name>
//         <Created> 28 03 2017 05:50:31 PM </Created>
//         <Key> 0679F181-B40B-49BF-A6A6-1AFA54A83376 </Key>
//     </File>
//     <Summary>
//         TopCoreContextFactory
//     </Summary>
// </Auto-generated>
//------------------------------------------------------------------------------------------------

#endregion License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TopCore.Framework.Core;

namespace TopCore.Data.EF
{
    public class TopCoreContextFactory : IDbContextFactory<TopCoreContext>
    {
        public TopCoreContext Create(DbContextFactoryOptions options)
        {
            var connectionString = GetConnectionString(options);
            return CreateCoreContext(connectionString);
        }

        /// <summary>
        /// Get connection from DbContextFactoryOptions Environment
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private string GetConnectionString(DbContextFactoryOptions options)
        {
            var connectionString = ConfigHelper.GetValue("appsettings.json", $"ConnectionStrings:{options.EnvironmentName}");
            return connectionString;
        }

        private static TopCoreContext CreateCoreContext(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<TopCoreContext>();
            builder.UseSqlServer(connectionString);
            return new TopCoreContext(builder.Options);
        }
    }
}