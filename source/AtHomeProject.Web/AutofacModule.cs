using AtHomeProject.Data.Interfaces;
using AtHomeProject.Domain.Interfaces;
using Autofac;

namespace AtHomeProject.Web
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IShippingService).Assembly, typeof(IUnitOfWork).Assembly)
                .Where(t => t.Name.EndsWith("Service") || t.Name.Equals("UnitOfWork"))
                .AsImplementedInterfaces();
        }
    }
}
