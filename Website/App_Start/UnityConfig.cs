using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.AspNet.Identity.EntityFramework;
using Core.Identity.Models;
using Microsoft.AspNet.Identity;
using Core.Identity;
using Microsoft.AspNet.Identity.Owin;
using AutoMapper.Unity;
using Core.Data.UnitOfWork;
using Core.Data.EF;
using Core.Services.Interface;
using Core.Services.Service;
using Microsoft.Owin.Security;
using System.Web;
namespace WebMembership.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here 

            #region register identity

            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<ApplicationRoleManager>();
            container.RegisterType<Core.Identity.EmailService>();

            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                new InjectionConstructor(typeof(ApplicationDbContext)));
            container.RegisterType<IRoleStore<ApplicationRole,string>, RoleStore<ApplicationRole>>(
                new InjectionConstructor(typeof(ApplicationDbContext)));


            #endregion

            #region register Services
            container.RegisterType<DI_WK_TEMPEntities>(new PerRequestLifetimeManager());
            container.RegisterType<WKTempUnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<DCTUnitOfWork>(new PerRequestLifetimeManager());

            container.RegisterType<ITokenStatus, TokenStatusService>(new PerRequestLifetimeManager());
            container.RegisterType<IServiceStatus,ServiceStatus>(new PerRequestLifetimeManager());
            container.RegisterType<IFolderStatus, FolderStatus>(new PerRequestLifetimeManager());
            container.RegisterType<IServerStatus, ServerStatus>(new PerRequestLifetimeManager());
            container.RegisterType<IPOControl, POControlService>(new PerRequestLifetimeManager());
            container.RegisterType<INavbar, NavbarService>(new PerRequestLifetimeManager());
            container.RegisterType<IIcon3Integration, Icon3IntegrationService>(new PerRequestLifetimeManager());
            container.RegisterType<Core.Services.Interface.IRole, RoleService>(new PerRequestLifetimeManager());
            container.RegisterType<Core.Services.Interface.IUser, UserService>(new PerRequestLifetimeManager());
            container.RegisterType<Core.Services.Interface.IEmail, Core.Services.Service.EmailService>(new PerRequestLifetimeManager());
            container.RegisterType<IActivity, ActivityService>(new PerRequestLifetimeManager());
            container.RegisterType<IMember, MemberService>(new PerRequestLifetimeManager());
            container.RegisterType<IRegister, RegisterService>(new PerRequestLifetimeManager());
            #endregion

            #region register =>Configuration Dto to DB model
            container.RegisterMappingProfile<Core.Services.Configuration.SystemViewModelProfile>();
            container.RegisterMappingProfile<Core.Services.Configuration.VisibilityViewModelProfile>();
            container.RegisterMappingProfile<Core.Services.Configuration.AdministrationViewModelProfile>();
            container.RegisterMapper(); 
            #endregion
        }
    }
}
