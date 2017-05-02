﻿#region	License

//------------------------------------------------------------------------------------------------
// <Auto-generated>
//     <Author> Top Nguyen (http://topnguyen.net) </Author>
//     <Project> TopCore.Framework.Core </Project>
//     <File> 
//         <Name> ConfigHelper.cs </Name>
//         <Created> 28 03 2017 10:49:18 AM </Created>
//         <Key> FA2C1290-B6C6-4535-A050-E7E67AFDA713 </Key>
//     </File>
//     <Summary>
//         ConfigHelper
//     </Summary>
// </Auto-generated>
//------------------------------------------------------------------------------------------------

#endregion License

using Microsoft.Extensions.Configuration;

namespace TopCore.Framework.Core
{
    public static class ConfigHelper
    {
        public static string GetValue(string configFileFullPath, string sectionQuery, bool isOptionalConfigFile = false, bool isReloadOnChange = true)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(configFileFullPath, isOptionalConfigFile, isReloadOnChange)
                .Build();

            return config.GetSection(sectionQuery).Value;
        }
    }
}