using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using gql_netcore.Data;
using gql_netcore.GraphQL.Commands;
using gql_netcore.GraphQL.Platforms;
using gql_netcore.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Subscriptions;

namespace gql_netcore.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddPlatformPayload> AddPlatformAsync(
            AddPlatformInput input,
            [ScopedService] AppDbContext context,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancelToken)
        {
            var platform = new Platform
            {
                Name = input.Name
            };

            context.Platforms.Add(platform);
            await context.SaveChangesAsync(cancelToken);
            await eventSender.SendAsync(nameof(Subscription.OnPlatformAdded), platform, cancelToken);

            return new AddPlatformPayload(platform);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddCommandPayload> AddCommandAsync(
            AddCommandInput input,
            [ScopedService] AppDbContext context
        )
        {
            var command = new Command
            {
                HowTo = input.HowTo,
                CommandLine = input.CommandLine,
                PlatformId = input.PlatformId
            };

            context.Commands.Add(command);
            await context.SaveChangesAsync();
            return new AddCommandPayload(command);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddCommandPayload> EditCommandAsync(EditCommandInput input,
        [ScopedService] AppDbContext context)
        {

            var entity = context.Commands.FirstOrDefault(item => item.Id == input.Id);
            if (entity != null)
            {
                Console.WriteLine($"Entity found {entity}");

                entity.HowTo = input.HowTo;
                entity.CommandLine = input.CommandLine;

                await context.SaveChangesAsync();

                return new AddCommandPayload(entity);
            }
            else
            {
                Console.WriteLine("Entity Not found");
                return null;
            }
        }

    }
}