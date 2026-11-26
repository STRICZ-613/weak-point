// ====================================================================
// Fichier      : Models/Slide.cs
// Responsable  : Membre 1
// Rôle         : Une diapositive : éléments, fond, disposition, notes, transition
// ====================================================================
namespace WeakPoint.Core.Models;

public class Slide
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public SlideBackground Background { get; set; } = new();
    public SlideLayout Layout { get; set; } = new();
    public SlideTransition? Transition { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<SlideElement> Elements { get; } = new();

    public T AddElement<T>(T element) where T : SlideElement
    {
        ArgumentNullException.ThrowIfNull(element);
        element.ZOrder = Elements.Count;
        Elements.Add(element);
        return element;
    }
}
