﻿using System;
using NewgroundsIODotNet.Enums;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels
{
    /// <summary>
    /// A cry of help from the server indicating something's wrong.
    /// </summary>
    public struct NgioServerError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        public NgioErrorCode ErrorCode =>
            (Enum.IsDefined(typeof(NgioErrorCode), Code) ? (NgioErrorCode) Code : NgioErrorCode.Unknown);
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}