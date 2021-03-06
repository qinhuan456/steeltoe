﻿// Copyright 2017 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Steeltoe.Connector.Services;

namespace Steeltoe.Security.Authentication.CloudFoundry
{
    public static class CloudFoundryJwtBearerConfigurer
    {
        internal static void Configure(SsoServiceInfo si, JwtBearerOptions jwtOptions, CloudFoundryJwtBearerOptions options)
        {
            if (jwtOptions == null || options == null)
            {
                return;
            }

            if (si != null)
            {
                options.JwtKeyUrl = si.AuthDomain + CloudFoundryDefaults.JwtTokenUri;
            }

            jwtOptions.ClaimsIssuer = options.ClaimsIssuer;
            jwtOptions.BackchannelHttpHandler = CloudFoundryHelper.GetBackChannelHandler(options.ValidateCertificates);
            jwtOptions.TokenValidationParameters = CloudFoundryHelper.GetTokenValidationParameters(options.TokenValidationParameters, options.JwtKeyUrl, jwtOptions.BackchannelHttpHandler, options.ValidateCertificates);
            jwtOptions.SaveToken = options.SaveToken;
        }
    }
}
