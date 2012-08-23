using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrelloNet;
using GiRello.Models;

namespace GiRello.Controllers
{
    public class CommentsController : ApiController
    {
        // POST api/comments
        public void Post(string id, [FromBody]CommitPayload payload)
        {
            var trello = new Trello("c6163a4015c586e703e8ea98f94a89fa");
            trello.Authorize("8c9152a54ff0b43f28d97966621ccb88242b703bfb4de005e3d2ab319da3ad54");
            var boardId = new BoardId(id);
            var card = trello.Cards.WithShortId(133, boardId);
            var cardId = new CardId(card.Id);
            trello.Cards.AddComment(cardId, "Test from GiRello");
        }

    }
}
