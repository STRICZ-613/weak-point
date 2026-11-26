// ====================================================================
// Fichier      : PptxRoundTripTests.cs
// Responsable  : Membre 1
// Rôle         : Tests : écrire -> relire un .pptx, validation avec OpenXmlValidator
// ====================================================================
using WeakPoint.Core.Pptx;
using Xunit;

namespace WeakPoint.Tests;

public class PptxRoundTripTests
{
    [Fact]
    public void WidescreenTemplateContainsOneSlide()
    {
        var presentation = PptxTemplate.CreatePresentation();

        Assert.Single(presentation.Slides);
        Assert.Equal(12_192_000, presentation.SlideWidth);
        Assert.Equal(6_858_000, presentation.SlideHeight);
    }

    [Fact]
    public void StandardTemplateUsesFourByThreeSize()
    {
        var presentation = PptxTemplate.CreatePresentation(wideScreen: false);

        Assert.Equal(9_144_000, presentation.SlideWidth);
        Assert.Equal(6_858_000, presentation.SlideHeight);
    }
}
