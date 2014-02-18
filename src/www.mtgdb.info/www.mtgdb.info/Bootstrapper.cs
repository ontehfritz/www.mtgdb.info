using System;
using Nancy;
using Nancy.TinyIoc;
using Nancy.Bootstrapper;
using Nancy.Authentication.Forms;
using SuperSimple.Auth;
using MtgDb.Info;

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

            //this must be the same key also in MonoRepository.cs
            SuperSimpleAuth ssa = 
                new SuperSimpleAuth ("mtgdb.info", 
                    "5e61fe35-1f96-4cf8-8f4b-54ba43e79903");


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

