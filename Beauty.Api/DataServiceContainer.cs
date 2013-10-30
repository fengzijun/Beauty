using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Beauty.API
{
    public class DataServiceContainer : WindsorContainer
    {
        #region Singleton Pattern

        private static DataServiceContainer instance;

        private DataServiceContainer()
        {
            RegisterComponents();
        }

        public static DataServiceContainer Self
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataServiceContainer();
                }
                return instance;
            }
        }

        #endregion

        public bool IsInstalled { get; set; }

        /// <summary>
        /// Get installer assembly name from web config. If not set this value in config file, use NodusServiceInstaller as default.
        /// These custom installer has to be placed aside the ControlBus.dll
        /// </summary>
        public string InstallerAssemblyName
        {
            get
            {
                object installerName = System.Configuration.ConfigurationManager.AppSettings["ServiceInstaller"];

                if (installerName != null)
                {
                    return installerName.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Register components for IoC container. 
        /// Installer class is configured in web.config. For customization project, developing new installer
        /// </summary>
        public void RegisterComponents()
        {
            if (!IsInstalled)
            {
                if (InstallerAssemblyName == string.Empty)
                {
                    this.Install(FromAssembly.This()); //Register installer in this assembly
                }
                else
                {

                    Assembly asm = Assembly.LoadFile(InstallerAssemblyName);
                    this.Install(FromAssembly.Instance(asm));
                }

                IsInstalled = true;
            }

        }

        /// <summary>
        /// Resolve service instance from IoC container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>()
        {
            T service = this.Resolve<T>();
            return service;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}