using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PierreTrois.Models
{
  public class Flavor
  {
    public int FlavorId { get; set; }
    [Required(ErrorMessage = "Your flavor needs a name!")]
    public string Name { get; set; }

    public List<TreatFlavor> JoinEntities { get; set; }
  }
}