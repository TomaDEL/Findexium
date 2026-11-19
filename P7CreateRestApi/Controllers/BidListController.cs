using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Dtos;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController] // Vérifie automatiquement les [Required] du DTO et renvoie une erreur 400 détaillée si le client envoie des données invalides.
    [Route("bids")]
    public class BidListController : ControllerBase
    {
        private readonly IRepository<BidList> _repository;
        private readonly ILogger<BidListController> _logger;

        public BidListController(IRepository<BidList> repository, ILogger<BidListController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Récupérer : GET /bids  → toute la liste
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidListDto>>> GetAll()
        {
            _logger.LogInformation("GET /bids : consultation de la liste des offres.");
            var bids = await _repository.GetAllAsync();
            return Ok(bids.Select(ToDto)); // 200
        }

        // Récupérer : GET /bids/3  → une seule offre
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BidListDto>> GetById(int id)
        {
            _logger.LogInformation("GET /bids/{Id} : consultation d'une offre.", id);
            var bid = await _repository.GetByIdAsync(id);
            if (bid == null) return BidNotFound(id);
            return Ok(ToDto(bid)); // 200
        }

        // Créer : POST /bids
        [HttpPost]
        public async Task<ActionResult<BidListDto>> Create([FromBody] BidListInputDto input)
        {
            _logger.LogInformation("POST /bids : création d'une offre pour le compte {Account}.", input.Account);
            var bid = new BidList
            {
                Account = input.Account,
                BidType = input.BidType,
                BidQuantity = input.BidQuantity,
                CreationDate = DateTime.UtcNow // date posée par le serveur, pas par le client
            };
            await _repository.AddAsync(bid);
            // 201 Created + adresse de la nouvelle ressource dans l'en-tête "Location"
            return CreatedAtAction(nameof(GetById), new { id = bid.BidListId }, ToDto(bid));
        }

        // Mettre à jour : PUT /bids/3
        [HttpPut("{id:int}")]
        public async Task<ActionResult<BidListDto>> Update(int id, [FromBody] BidListInputDto input)
        {
            _logger.LogInformation("PUT /bids/{Id} : mise à jour d'une offre.", id);
            var bid = await _repository.GetByIdAsync(id);
            if (bid == null) return BidNotFound(id);

            bid.Account = input.Account;
            bid.BidType = input.BidType;
            bid.BidQuantity = input.BidQuantity;
            bid.RevisionDate = DateTime.UtcNow;
            await _repository.UpdateAsync(bid);
            return Ok(ToDto(bid));  // 200
        }

        // Supprimer : DELETE /bids/3
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /bids/{Id} : suppression d'une offre.", id);
            var bid = await _repository.GetByIdAsync(id);
            if (bid == null) return BidNotFound(id);

            await _repository.DeleteAsync(bid);
            return NoContent(); // 204 : réussi, rien à renvoyer
        }

        private static BidListDto ToDto(BidList bid) => new()
        {
            BidListId = bid.BidListId,
            Account = bid.Account,
            BidType = bid.BidType,
            BidQuantity = bid.BidQuantity
        };

        // Erreur 404 avec un message détaillé
        private ActionResult BidNotFound(int id) =>
            Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Offre introuvable",
                detail: $"Aucune offre n'existe avec l'identifiant {id}.");
    }
}