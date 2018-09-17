using System;

using Unity;
using VehicleManagementApp.BLL;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Repository;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<IOrganaizationManager, OrganaizationManager>();
            container.RegisterType<IOrganaizationRepository, OrganaizationRepository>();

            container.RegisterType<IDesignationManager, DesignationManager>();
            container.RegisterType<IDesignationRepository, DesignationRepository>();

            container.RegisterType<IVehicleTypeManager, VehicleTypeManager>();
            container.RegisterType<IVehicleTypeRepository, VehicleTypeRepository>();

            container.RegisterType<IVehicleManager, VehicleManager>();
            container.RegisterType<IVehicleRepository, VehicleRepository>();

            container.RegisterType<IDepartmentManager, DepartmentManager>();
            container.RegisterType<IDepartmentRepository, DepartmentRepository>();

            container.RegisterType<IRoleManager, RoleManager>();
            container.RegisterType<IRoleRepository, RoleRepository>();

            container.RegisterType<IUserManager, UserManager>();
            container.RegisterType<IUserRepository, UserRepository>();

            container.RegisterType<IEmployeeManager, EmployeeManager>();
            container.RegisterType<IEmployeeRepository, EmployeeRepository>();

            container.RegisterType<IRequsitionStatusManager, RequsitionStatusManager>();
            container.RegisterType<IRequsitionStatusRepoisitory, RequsitionStatusRepository>();

            container.RegisterType<ICommentManager, CommentManager>();
            container.RegisterType<ICommentRepository, CommentRepository>();

            container.RegisterType<IRequsitionManager, RequsitionManager>();
            container.RegisterType<IRequsitionRepository, RequsistionRepository>();

            container.RegisterType<IDivisionManager, DivisionManager>();
            container.RegisterType<IDivisionRepository, DivisionRepository>();

            container.RegisterType<IDistrictManager, DistrictManager>();
            container.RegisterType<IDristictRepository, DistrictRepository>();
            
            container.RegisterType<IThanaManager, ThanaManager>();
            container.RegisterType<IThanaRepository, ThanaRepository>();

            container.RegisterType<IManagerManager, ManagerManager>();
            container.RegisterType<IManagerRepository, ManagerRepository>();
        }
    }
}