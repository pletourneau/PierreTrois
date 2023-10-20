using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PierreTrois.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    [Required(ErrorMessage = "The treat must have a name")]
    public string Name { get; set; }
    
    public List <TreatFlavor> JoinEntities { get; set; }
    public ApplicationUser User { get; set; }
  }
}