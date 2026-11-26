// ====================================================================
// Fichier      : Models/TextBoxElement.cs
// Responsable  : Membre 1
// Rôle         : Zone de texte : paragraphes et segments de texte mis en forme
// ====================================================================
namespace WeakPoint.Core.Models;

public class TextBoxElement : SlideElement
{
    public string Text { get; set; } = string.Empty;
    public TextFormat Format { get; set; } = new();
}
