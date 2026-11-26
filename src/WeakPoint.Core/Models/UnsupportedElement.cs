// ====================================================================
// Fichier      : Models/UnsupportedElement.cs
// Responsable  : Membre 1
// Rôle         : Objet non géré (graphique, SmartArt...) : XML brut conservé pour ne rien perdre à l'enregistrement
// ====================================================================
namespace WeakPoint.Core.Models;

public class UnsupportedElement : SlideElement
{
    public string RawXml { get; set; } = string.Empty;
}
