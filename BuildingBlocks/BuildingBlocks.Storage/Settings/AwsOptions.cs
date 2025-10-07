﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Storage.Settings
{
    public sealed class AwsOptions : IAwsOptions
    {
        public string Region { get; set; } = "";
        public string Bucket { get; set; } = "";
        public string? KeyPrefix { get; set; }
        public string? AccessKey { get; set; }

        public string? SecretKey { get; set; }

    }
}
