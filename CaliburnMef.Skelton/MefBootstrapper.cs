using Caliburn.Micro;
using CaliburnMef.Skelton.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows;

namespace CaliburnMef.Skelton
{
    /**
     * Add reference to Composition* from assemblies, not NuGet.
     */
    public class MefBootstrapper : BootstrapperBase
    {
        private CompositionContainer _container;

        public MefBootstrapper()
        {
            Initialize();
           
        }

        protected override void Configure()
        {

            /**I’m taking advantage of Silverlight’s CompositionHost to setup the CompositionContainer. 
             * You can just instantiate the container directly if you are working with .NET. 
             * Then, I’m creating an AggregateCatalog and populating it with AssemblyCatalogs; one for each Assembly in AssemblySource.Instance. 
             **/
            //container = CompositionHost.Initialize(
            //    new AggregateCatalog(
            //        AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
            //        )
            //    );

            /**
             * http://stackoverflow.com/questions/21139447/locating-view-for-a-viewmodel-both-in-an-external-assembly
             */
            //string pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            //if (!Directory.Exists(pluginPath))
            //    Directory.CreateDirectory(pluginPath);
            //var fi = new DirectoryInfo(pluginPath).GetFiles("*.dll");
            //AssemblySource.Instance.AddRange(fi.Select(fileInfo => Assembly.LoadFrom(fileInfo.FullName)));

            var catalog = new AggregateCatalog(
                AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
                );

            _container = new CompositionContainer(catalog);         
            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_container);

            _container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = _container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance as ComposablePart);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}