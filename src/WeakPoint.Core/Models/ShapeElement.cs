// ====================================================================
// Fichier      : Models/ShapeElement.cs
// Responsable  : Membre 1
// Rôle         : Forme géométrique (rectangle, ellipse, flèche...) avec remplissage, contour, texte éventuel
// ====================================================================
namespace WeakPoint.Core.Models;

public class ShapeElement : SlideElement
{
    public ShapeType ShapeType { get; set; } = ShapeType.Rectangle;
    public string Text { get; set; } = string.Empty;
    public FillStyle Fill { get; set; } = new();
    public StrokeStyle Stroke { get; set; } = new();
}
