// ====================================================================
// Fichier      : ViewModels/SlideViewModel.cs
// Responsable  : Membre 1
// Rôle         : Représente la diapositive affichée et ses éléments pour la vue
// ====================================================================
namespace WeakPoint.UI.ViewModels;

public class SlideViewModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Title { get; set; } = "Nouvelle diapositive";
}
