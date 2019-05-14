using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_Bestelbons.Models;
using WPF_Bestelbons.ViewModels;

namespace WPF_Bestelbons
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;



        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IDataServiceLeveranciers, DataServiceLeveranciers>();
            _container.RegisterSingleton(typeof(Leveranciers),null,typeof(Leveranciers));
            _container.RegisterSingleton(typeof(Bestelbon), null, typeof(Bestelbon));
            _container.RegisterSingleton(typeof(PDFCreator), null, typeof(PDFCreator));
            _container.Singleton<ShellViewModel>();
            _container.Singleton<LeveranciersViewModel>();
            _container.Singleton<BestelbonsViewModel>();
            _container.Singleton<BestelbonOpmaakViewModel>();
            _container.Singleton<AddLeverancierViewModel>();
            _container.Singleton<DialogViewModel>();
            _container.Singleton<SelectUserViewModel>();
            _container.Singleton<LeveringsvoorwaardenViewModel>();
            _container.Singleton<UsersViewModel>();
            _container.Singleton<EditLeverancierViewModel>();

        }


        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetInstance(service, key);
            if (instance != null)
                return instance;
            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }





    }
}
