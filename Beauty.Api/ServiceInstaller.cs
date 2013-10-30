
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Beauty.API
{
    using Beauty.InterFace;
    using Beauty.Service;

    public class ServiceInstaller : IWindsorInstaller
    {
        
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(
                Component.For<IFirstPage>().ImplementedBy<FirstPageService>().LifeStyle.Transient,
                Component.For<ILike>().ImplementedBy<LIkeService>().LifeStyle.Transient,
                Component.For<IMoney>().ImplementedBy<MoneyService>().LifeStyle.Transient,
                Component.For<IPrice>().ImplementedBy<PriceService>().LifeStyle.Transient,
                Component.For<ISetting>().ImplementedBy<SettingService>().LifeStyle.Transient,
                Component.For<IShare>().ImplementedBy<ShareService>().LifeStyle.Transient,
                Component.For<ITask>().ImplementedBy<TaskService>().LifeStyle.Transient,
                Component.For<IUser>().ImplementedBy<UserService>().LifeStyle.Transient,
                Component.For<IUserSetting>().ImplementedBy<UserSettingService>().LifeStyle.Transient,
                Component.For<IUserStore>().ImplementedBy<UserStoreService>().LifeStyle.Transient,
                Component.For<IUserToken>().ImplementedBy<UserTokenService>().LifeStyle.Transient,
                Component.For<IBady>().ImplementedBy<BadyService>().LifeStyle.Transient,
                Component.For<IGroup>().ImplementedBy<GroupService>().LifeStyle.Transient,
                Component.For<IFirstPageArg>().ImplementedBy<FirstPageArgService>().LifeStyle.Transient,
                Component.For<ILog>().ImplementedBy<LogService>().LifeStyle.Transient,
                Component.For<INotice>().ImplementedBy<NoticeService>().LifeStyle.Transient,
                Component.For<IUserLoginCount>().ImplementedBy<UserLoginCountService>().LifeStyle.Transient,
                Component.For<IUserAccount>().ImplementedBy<UserAccountService>().LifeStyle.Transient
             
                );
         
        }
    }
}