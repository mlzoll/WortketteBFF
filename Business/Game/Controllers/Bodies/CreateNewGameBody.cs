using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Game.Controllers.Bodies;

public class CreateNewGameBody
{
    [MaxLength(50)]
    [MinLength(1)]
    public string FirstWord { get; set; } = "";

    [MaxLength(50)]
    [MinLength(1)]
    public string Topic { get; set; } = "";
}