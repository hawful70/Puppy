﻿#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Puppy </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Puppy </Project>
//     <File>
//         <Name> LoggerException.cs </Name>
//         <Created> 10/08/17 11:20:21 PM </Created>
//         <Key> 141ffe67-ca45-4c4c-8234-c97f287b66aa </Key>
//     </File>
//     <Summary>
//         LoggerException.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;

namespace Puppy.Logger.Core
{
    [Serializable]
    [DesignerCategory(nameof(Puppy))]
    public class LoggerException
    {
        // TODO Log Request Information: Endpoint with Param values

        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public DateTimeOffset CreatedOn { get; } = DateTimeOffset.Now;

        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel Level { get; set; } = LogLevel.Error;

        public string Message { get; set; }

        public SerializableException Exception { get; }

        public LoggerException()
        {
        }

        public LoggerException(string message, LogLevel level) : this()
        {
            Message = message;
            Level = level;
        }

        public LoggerException(Exception ex, LogLevel level, string message = null) : this(message, level)
        {
            Exception = new SerializableException(ex);
        }

        public LoggerException(ExceptionContext context, LogLevel level, string message = null) : this(message, level)
        {
            Exception = new SerializableException(context.Exception);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}