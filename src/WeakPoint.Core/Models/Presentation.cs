// ====================================================================
// Fichier      : Models/Presentation.cs
// Responsable  : Membre 1
// Rôle         : Modèle racine : liste des diapositives, taille de page (16:9 / 4:3), métadonnées
// ====================================================================
namespace WeakPoint.Core.Models;

public class Presentation
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public long SlideWidth { get; set; } = 12_192_000;
    public long SlideHeight { get; set; } = 6_858_000;
    public List<Slide> Slides { get; } = new();

    public Slide AddSlide(Slide? slide = null)
    {
        var newSlide = slide ?? new Slide();
        Slides.Add(newSlide);
        return newSlide;
    }
}
