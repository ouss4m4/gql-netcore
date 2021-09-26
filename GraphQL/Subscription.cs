using gql_netcore.Models;
using HotChocolate;
using HotChocolate.Types;

namespace gql_netcore.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic]
        public Platform OnPlatformAdded([EventMessage] Platform platform) => platform;
    }
}