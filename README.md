# SlideStudio

Réplique simplifiée de PowerPoint en C# (WPF) : créer des diapositives, les éditer, ouvrir et enregistrer des fichiers `.pptx`.

Projet scolaire — Keyce College, Informatique et Intelligence Artificielle.

## Prérequis
- Windows 10/11
- .NET SDK 10 (LTS) : vérifier avec `dotnet --version`
- VS Code + extension « C# Dev Kit »
- Git

## Commandes utiles
```
dotnet restore                              # télécharge les paquets NuGet
dotnet build                                # compile toute la solution
dotnet test                                 # lance les tests
dotnet run --project src/SlideStudio.UI     # lance l'application
```

## Documentation
Voir `docs/Documentation_Projet.txt` (fonctionnalités, répartition des tâches, structure, règles Git).

## Équipe
| Membre | Nom | GitHub | Rôle |
|---|---|---|---|
| M1 | | STRICZ-613 | Chef technique, modèle, format PPTX |
| M2 | | | Moteur d'édition |
| M3 | | | Mise en forme, vignettes, diaporama |
| M4 | | | Services, dialogues, tests, documentation |
