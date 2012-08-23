using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using TrelloNet;
using System.Linq;

namespace GiRello.Controllers
{
    public class CommentsController : ApiController
    {
        // POST api/comments
        public void Post([FromUri] string id)
        {
            var content = Request.Content.ReadAsStringAsync();
            content.Wait();
            var result = content.Result;
            result = HttpUtility.UrlDecode(result);
            result = result.Replace("payload=", "");
            dynamic payload = JsonConvert.DeserializeObject(result);
            foreach (dynamic commit in payload.commits)
            {
                string message = commit.message;
                string pattern = @"(?<=#)\d{1,}";
                foreach (Match match in Regex.Matches(message, pattern, RegexOptions.IgnoreCase))
                {
                    var trello = new Trello("c6163a4015c586e703e8ea98f94a89fa");
                    trello.Authorize("8c9152a54ff0b43f28d97966621ccb88242b703bfb4de005e3d2ab319da3ad54");
                    var boardId = new BoardId(id);
                    var card = trello.Cards.WithShortId(int.Parse(match.Value), boardId);
                    var cardId = new CardId(card.Id);
                    trello.Cards.AddComment(cardId, "Commit added: " + message);
                }
            }
        }

    }
}
