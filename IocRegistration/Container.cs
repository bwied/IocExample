using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace IocRegistration
{
    public class Container
    {
        public readonly WindsorContainer IocContainer;
        public static Container Instance { get; } = new Container();

        private Container()
        {
            IocContainer = new WindsorContainer();
            IocContainer.Register(Component.For<IReadOnlyDictionary<string, string>>().ImplementedBy<Dictionary<string, string>>().LifeStyle.Transient);
        }
    }
}
