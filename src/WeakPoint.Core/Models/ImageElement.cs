// ====================================================================
// Fichier      : Models/ImageElement.cs
// Responsable  : Membre 1
// Rôle         : Image insérée : octets de l'image, format, recadrage éventuel
// ====================================================================
namespace WeakPoint.Core.Models;

public class ImageElement : SlideElement
{
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
    public string MimeType { get; set; } = "image/png";
    public string AlternativeText { get; set; } = string.Empty;
}
