/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:NetworkScanner"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace NetworkScanner.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<View1ViewModel>();
            SimpleIoc.Default.Register<View2ViewModel>();
            //SimpleIoc.Default.Register<AppearanceViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public View1ViewModel View1
        {
            get
            {
                return ServiceLocator.Current.GetInstance<View1ViewModel>();
            }
        }

        public View2ViewModel View2
        {
            get
            {
                return ServiceLocator.Current.GetInstance<View2ViewModel>();
            }
        }

        //public AppearanceViewModel AVM
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<AppearanceViewModel>();
        //    }
        //}

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}