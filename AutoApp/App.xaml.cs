using AutoApp.Contexts;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AutoApp
{
    public partial class App : Application
    {
        public App()
        {
            SimpleIoc.Default.Register<ApplicationContext>();
        }
    }
}
