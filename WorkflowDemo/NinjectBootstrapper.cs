using Caliburn.Micro;
using Ninject;
using System;
using System.Collections.Generic;
using System.Windows;
using WorkflowDemo.Infra;

namespace WorkflowDemo.Model
{
    /**
     * https://github.com/Caliburn-Micro/Caliburn.Micro/blob/master/samples/Caliburn.Micro.Hello/Caliburn.Micro.Hello/HelloBootstrapper.cs
     */
    public class NinjectBootstrapper : BootstrapperBase
    {
        public NinjectBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ApplicationControllerViewModel>();
        }

        /**
         * http://putridparrot.com/blog/caliburn-micro-and-inversion-of-control-using-ninject/
         */
        private IKernel _kernel;
        protected override void Configure()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            _kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();

            _kernel.Bind<ITransitionMap>().To<TransitionMap>().InSingletonScope();

        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _kernel.Dispose();
            base.OnExit(sender, e);
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            return _kernel.Get(service);
        }
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _kernel.GetAll(service);
        }
        protected override void BuildUp(object instance)
        {
            _kernel.Inject(instance);
        }

    }
}
