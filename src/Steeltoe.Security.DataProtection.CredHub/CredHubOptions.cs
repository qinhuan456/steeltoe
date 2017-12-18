﻿// Copyright 2017 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Steeltoe.Security.DataProtection.CredHub
{
    /// <summary>
    /// Configured CredHub client
    /// </summary>
    public class CredHubOptions
    {
        /// <summary>
        /// Routable address of CredHub server
        /// </summary>
        public string CredHubUrl { get; set; } = "https://credhub.service.cf.internal:8844";

        /// <summary>
        /// UAA user with necessary permissions to perform desired CredHub interactions
        /// </summary>
        public string CredHubUser { get; set; }

        /// <summary>
        /// UAA User's password
        /// </summary>
        public string CredHubPassword { get; set; }

        /// <summary>
        /// Validate server certificates for UAA and/or CredHub servers
        /// </summary>
        public bool ValidateCertificates { get; set; } = true;
    }
}
