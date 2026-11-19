using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Dtos
{
    public class BidListDto // Ce que l'API renvoie au client, c'est un DTO (Data Transfer Object) qui est une version simplifiée de l'entité BidList. Il ne contient que les propriétés nécessaires pour l'affichage ou la manipulation des données côté client.
    {
        public int BidListId { get; set; }
        public string Account { get; set; } = string.Empty;
        public string BidType { get; set; } = string.Empty;
        public double? BidQuantity { get; set; }
    }

    public class BidListInputDto // Ce que le client envoie pour créer ou modifier
    {
        [Required(ErrorMessage = "Le compte est obligatoire.")]
        public string Account { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le type de l'offre est obligatoire.")]
        public string BidType { get; set; } = string.Empty;

        public double? BidQuantity { get; set; }
    }
}