// ====================================================================
// Fichier      : Models/SlideElement.cs
// Responsable  : Membre 1
// Rôle         : Classe de base de tout objet sur la diapo : identifiant, position, taille, rotation, ordre Z
// ====================================================================
namespace WeakPoint.Core.Models;

public abstract class SlideElement
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = string.Empty;
    public long X { get; set; }
    public long Y { get; set; }
    public long Width { get; set; }
    public long Height { get; set; }
    public double Rotation { get; set; }
    public int ZOrder { get; set; }
    public bool IsVisible { get; set; } = true;
}
