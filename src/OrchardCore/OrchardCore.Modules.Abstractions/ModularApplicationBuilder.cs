﻿using System;
using Microsoft.AspNetCore.Builder;

namespace OrchardCore.Modules
{
    public class ModularApplicationBuilder
    {
        private IApplicationBuilder _app;

        public ModularApplicationBuilder(IApplicationBuilder app)
        {
            _app = app;
        }

        public ModularApplicationBuilder Configure(Action<IApplicationBuilder> configuration)
        {
            configuration(_app);
            return this;
        }
    }
}