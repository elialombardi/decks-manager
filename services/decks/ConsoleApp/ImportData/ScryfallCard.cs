// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
namespace ConsoleApp.ImportData
{
  public class ImageUris
  {
    // public string Small { get; set; }
    public required string Normal { get; set; }
    // public string Large { get; set; }
    // public string Png { get; set; }
    // public string Art_crop { get; set; }
    // public string Border_crop { get; set; }
  }

  // public class Legalities
  // {
  //     public string Standard { get; set; }
  //     public string Future { get; set; }
  //     public string Historic { get; set; }
  //     public string Timeless { get; set; }
  //     public string Gladiator { get; set; }
  //     public string Pioneer { get; set; }
  //     public string Explorer { get; set; }
  //     public string Modern { get; set; }
  //     public string Legacy { get; set; }
  //     public string Pauper { get; set; }
  //     public string Vintage { get; set; }
  //     public string Penny { get; set; }
  //     public string Commander { get; set; }
  //     public string Oathbreaker { get; set; }
  //     public string Standardbrawl { get; set; }
  //     public string Brawl { get; set; }
  //     public string Alchemy { get; set; }
  //     public string Paupercommander { get; set; }
  //     public string Duel { get; set; }
  //     public string Oldschool { get; set; }
  //     public string Premodern { get; set; }
  //     public string Predh { get; set; }
  // }

  // public class Prices
  // {
  //     public object Usd { get; set; }
  //     public object Usd_foil { get; set; }
  //     public object Usd_etched { get; set; }
  //     public object Eur { get; set; }
  //     public object Eur_foil { get; set; }
  //     public object Tix { get; set; }
  // }

  // public class PurchaseUris
  // {
  //     public string Tcgplayer { get; set; }
  //     public string Cardmarket { get; set; }
  //     public string Cardhoarder { get; set; }
  // }

  // public class RelatedUris
  // {
  //     public string Tcgplayer_infinite_articles { get; set; }
  //     public string Tcgplayer_infinite_decks { get; set; }
  //     public string Edhrec { get; set; }
  // }

  public class ScryfallCard
  {
    public required string Id { get; set; }
    // public required string Oracle_id { get; set; }
    // public required List<object> Multiverse_ids { get; set; }
    // public int Tcgplayer_id { get; set; }
    public required string Name { get; set; }
    // public required string Lang { get; set; }
    // public required string Released_at { get; set; }
    // public required string Uri { get; set; }
    // public required string Scryfall_uri { get; set; }
    // public required string Layout { get; set; }
    // public bool Highres_image { get; set; }
    // public required string Image_status { get; set; }
    // public required ImageUris? Image_uris { get; set; }
    // public string? Mana_cost { get; set; }
    // public double Cmc { get; set; }
    // public required string Type_line { get; set; }
    // public required string Oracle_text { get; set; }
    // public required List<object> Colors { get; set; }
    // public required List<object> Color_identity { get; set; }
    // public required List<object> Keywords { get; set; }
    // public Legalities? Legalities { get; set; }
    // public required List<string> Games { get; set; }
    // public bool Reserved { get; set; }
    // public bool Foil { get; set; }
    // public bool Nonfoil { get; set; }
    // public required List<string> Finishes { get; set; }
    // public bool Oversized { get; set; }
    // public bool Promo { get; set; }
    // public bool Reprint { get; set; }
    // public bool Variation { get; set; }
    // public required string Set_id { get; set; }
    // public required string Set { get; set; }
    // public required string Set_name { get; set; }
    // public required string Set_type { get; set; }
    // public required string Set_uri { get; set; }
    // public required string Set_search_uri { get; set; }
    // public required string Scryfall_set_uri { get; set; }
    // public required string Rulings_uri { get; set; }
    // public required string Prints_search_uri { get; set; }
    // public required string Collector_number { get; set; }
    // public bool Digital { get; set; }
    // public required string Rarity { get; set; }
    // public required string Flavor_text { get; set; }
    // public required string Card_back_id { get; set; }
    // public required string Artist { get; set; }
    // public required List<string> Artist_ids { get; set; }
    // public required string Illustration_id { get; set; }
    // public required string Border_color { get; set; }
    // public required string Frame { get; set; }
    // public bool Full_art { get; set; }
    // public bool Textless { get; set; }
    // public bool Booster { get; set; }
    // public bool Story_spotlight { get; set; }
    // public int Edhrec_rank { get; set; }
    // public Prices? Prices { get; set; }
    // public RelatedUris? Related_uris { get; set; }
    // public PurchaseUris? Purchase_uris { get; set; }
  }
}