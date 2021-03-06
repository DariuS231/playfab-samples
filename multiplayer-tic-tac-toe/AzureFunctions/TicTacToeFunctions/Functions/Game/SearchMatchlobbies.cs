using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using PlayFab.Plugins.CloudScript;
using System.Net.Http;
using System.Threading.Tasks;
using TicTacToeFunctions.Helpers;
using TicTacToeFunctions.Models;
using TicTacToeFunctions.Models.Service.Requests;
using TicTacToeFunctions.Util;

namespace TicTacToeFunctions.Functions.Game
{
    public static class SearchMatchLobbies
    {
        [FunctionName("SearchMatchLobbies")]
        public static async Task<Wrapper<MatchLobby>> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage req,
            [CosmosDB(ConnectionStringSetting = "PlayFabTicTacToeCosmosDB")] DocumentClient cosmosDBClient
        )
        {
            var context = await FunctionContext<SearchMatchLobbiesRequest>.Create(req);
            var lobbyListRequest = context.FunctionArgument;

            var result = await MatchLobbyUtil.GetMatchLobbyInfoAsync(
                cosmosDBClient,
                ExpressionUtils.GetSearchMatchLobbiesExpression(lobbyListRequest.SearchTerm));

            return new Wrapper<MatchLobby>
            {
                Items = result
            };
        }
    }
}
