using System.Linq;
using gql_netcore.Data;
using gql_netcore.Models;
using HotChocolate;
using HotChocolate.Types;

namespace gql_netcore.GraphQL.Platforms
{
    public class PlatformType : ObjectType<Platform>
    {
        protected override void Configure(IObjectTypeDescriptor<Platform> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Description("Represents any SDK that has a CLI - Command Line Interface");
            descriptor.Field(p => p.LicenseKey).Ignore();
            descriptor
                .Field(p => p.Commands)
                .ResolveWith<Resolvers>(p => p.GetCommands(default!, default!))
                .UseDbContext<AppDbContext>()
                .Description("A List of available commands for this platform");


        }

        private class Resolvers
        {
            public IQueryable<Command> GetCommands(Platform platform, [ScopedService] AppDbContext context)
            {
                return context.Commands.Where(p => p.PlatformId == platform.Id);
            }
        }
    }
}