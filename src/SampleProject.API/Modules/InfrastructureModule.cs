﻿using System.Reflection;
using Autofac;
using SampleProject.Domain.Customers.Orders;
using SampleProject.Domain.Products;
using SampleProject.Domain.SeedWork;
using SampleProject.Infrastructure;
using SampleProject.Infrastructure.Customers;
using SampleProject.Infrastructure.Products;
using SampleProject.Infrastructure.SeedWork;

namespace SampleProject.API.Modules
{
    public class InfrastructureModule : Autofac.Module
    {
        private readonly string _databaseConnectionString;

        public InfrastructureModule(string databaseConnectionString)
        {
            this._databaseConnectionString = databaseConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", _databaseConnectionString)
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventsDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CustomerRepository>()
                .As<ICustomerRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IDomainEventNotification<>).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IDomainEventNotification<>)).InstancePerDependency();
        }
    }
}