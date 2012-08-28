using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using TrelloNet;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace GiRello.Controllers
{
    public class CommitController : ApiController
    {
        private Models.GiRelloContext db = new Models.GiRelloContext();
        private Trello trello = new Trello("cba437c57ff37b1d42536e654657490d");

        // POST api/comments
        public HttpResponseMessage Post([FromUri] string id, [FromBody] Models.GitPost gitPost)
        {
            HttpResponseMessage response;
            if (ModelState.IsValid && gitPost != null)
            {
                response = Request.CreateResponse(HttpStatusCode.NoContent);
                JObject payload = JObject.Parse(gitPost.payload);
                foreach (var commit in payload["commits"])
                {
                    string username;
                    string service;
                    string token;
                    var count = commit["author"].Children().Count();
                    if (commit["author"].Children().Count() == 0)
                    {
                        service = "bitbucket";
                        username = (string)commit["author"];
                        try
                        {
                            token = db.Auths.First(c => c.BitbucketUser == username).Token;
                        }
                        catch
                        {
                            username = (string)payload["repository"]["owner"];
                            token = db.Auths.First(c => c.BitbucketUser == username).Token;
                        }
                    }
                    else
                    {
                        service = "github";
                        username = (string)commit["author"]["username"];
                        try
                        {
                            token = db.Auths.First(c => c.GithubUser == username).Token;
                        }
                        catch
                        {
                            username = (string)payload["repository"]["owner"]["name"];
                            token = db.Auths.First(c => c.GithubUser == username).Token;
                        }
                    }
                    string message = (string)commit["message"];
                    string pattern = @"(?<=#)\d{1,}";
                    foreach (Match match in Regex.Matches(message, pattern, RegexOptions.IgnoreCase))
                    {
                        addComment(id, int.Parse(match.Value), message, token);
                        response = Request.CreateResponse(HttpStatusCode.Created);
                    }
                }
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return response;
        }



        private bool addComment(string boardId, int cardNumber, string comment, string userToken)
        {
            trello.Authorize(userToken);
            var bId = new BoardId(boardId);
            var card = trello.Cards.WithShortId(cardNumber, bId);
            var cardId = new CardId(card.Id);
            trello.Cards.AddComment(cardId, "Commit added: " + comment);
            return true;
        }

    }
}
