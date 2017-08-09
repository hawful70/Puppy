﻿#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> HangfireConfig.cs </Name>
//         <Created> 17/07/17 11:03:19 PM </Created>
//         <Key> f148fc57-541b-428a-a489-c909d8c2dca3 </Key>
//     </File>
//     <Summary>
//         HangfireConfig.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Puppy.Hangfire
{
    public static class HangfireConfig
    {
        /// <summary>
        ///     Hangfire Dashboard Url. Ex: /developers/job, if this is <c> empty </c> then disable dashboard
        /// </summary>
        /// <remarks> Start with <c> "/" </c> but end with <c> empty </c> </remarks>
        public static string DashboardUrl { get; set; }

        /// <summary>
        ///     Access Key read from URI 
        /// </summary>
        /// <remarks> Empty is allow <c> Anonymous </c> </remarks>
        public static string AccessKey { get; set; } = string.Empty;

        /// <summary>
        ///     Query parameter via http request 
        /// </summary>
        /// <remarks> Empty is allow <c> Anonymous </c> </remarks>
        public static string AccessKeyQueryParam { get; set; } = "key";
    }
}