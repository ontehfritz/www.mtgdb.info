using System;
using Nancy;
using Nancy.TinyIoc;
using Nancy.Bootstrapper;
using Nancy.Authentication.Forms;
using SuperSimple.Auth;

namespace mtgdb.info
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            StaticConfiguration.DisableErrorTraces = false;
            StaticConfiguration.EnableRequestTracing = true;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            SuperSimpleAuth ssa = 
                new SuperSimpleAuth ("mtgdb.info", 
                    "4e5844a9-6444-415d-b06d-6f29f52fbd0e");

            container.Register<SuperSimpleAuth>(ssa);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, 
            NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<IUserMapper, NancyUserMapper>();
        }

        protected override void RequestStartup(TinyIoCContainer container, 
            IPipelines pipelines, 
            NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var formsAuthConfiguration = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/logon",
                UserMapper = container.Resolve<IUserMapper>(),
            };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
    }
}

