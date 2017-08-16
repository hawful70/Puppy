﻿#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Puppy </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Puppy </Project>
//     <File>
//         <Name> SerializableException.cs </Name>
//         <Created> 10/08/17 5:58:52 PM </Created>
//         <Key> 69f64980-151f-44db-ba2a-d05775526df7 </Key>
//     </File>
//     <Summary>
//         SerializableException.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;
using System.ComponentModel;

namespace Puppy.Logger.Core.Models
{
    [Serializable]
    [DesignerCategory(nameof(Puppy))]
    public class SerializableException : Serializable
    {
        public string HelpLink { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        public string TypeName { get; set; }

        public string BaseTypeName { get; set; }

        public SerializableException InternalException { get; set; }

        public SerializableException()
        {
        }

        public SerializableException(string message) : this()
        {
            Message = message;
        }

        public SerializableException(Exception ex) : this(ex.Message)
        {
            HelpLink = ex.HelpLink;
            Source = ex.Source;
            StackTrace = ex.StackTrace;
            TypeName = ex.GetType()?.FullName;
            BaseTypeName = ex.GetBaseException()?.GetType()?.FullName;
            InternalException = ex.InnerException != null ? new SerializableException(ex.InnerException) : null;
        }
    }
}