using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using A = DocumentFormat.OpenXml.Drawing;
using P = DocumentFormat.OpenXml.Presentation;
using ModelPresentation = WeakPoint.Core.Models.Presentation;

namespace WeakPoint.Core.Pptx;

public static class PptxTemplate
{
    public static ModelPresentation CreatePresentation(bool wideScreen = true)
    {
        var presentation = new ModelPresentation
        {
            SlideWidth = wideScreen ? 12_192_000 : 9_144_000,
            SlideHeight = 6_858_000
        };
        presentation.AddSlide();
        return presentation;
    }

    public static void CreateFile(string filePath, bool wideScreen = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        new PptxWriter().Write(CreatePresentation(wideScreen), filePath);
    }

    internal static PresentationPart CreatePackage(PresentationDocument document, long width, long height)
    {
        var presentationPart = document.AddPresentationPart();
        var masterPart = presentationPart.AddNewPart<SlideMasterPart>();
        var layoutPart = masterPart.AddNewPart<SlideLayoutPart>();
        var masterId = presentationPart.GetIdOfPart(masterPart);

        layoutPart.SlideLayout = new P.SlideLayout(
            new P.CommonSlideData(CreateShapeTree()),
            new P.ColorMapOverride(new P.MasterColorMapping()))
        {
            Type = P.SlideLayoutValues.Blank,
            Preserve = true
        };

        masterPart.SlideMaster = new P.SlideMaster(
            new P.CommonSlideData(CreateShapeTree()),
            new P.ColorMap
            {
                Background1 = A.ColorSchemeIndexValues.Light1,
                Text1 = A.ColorSchemeIndexValues.Dark1,
                Background2 = A.ColorSchemeIndexValues.Light2,
                Text2 = A.ColorSchemeIndexValues.Dark2,
                Accent1 = A.ColorSchemeIndexValues.Accent1,
                Accent2 = A.ColorSchemeIndexValues.Accent2,
                Accent3 = A.ColorSchemeIndexValues.Accent3,
                Accent4 = A.ColorSchemeIndexValues.Accent4,
                Accent5 = A.ColorSchemeIndexValues.Accent5,
                Accent6 = A.ColorSchemeIndexValues.Accent6,
                Hyperlink = A.ColorSchemeIndexValues.Hyperlink,
                FollowedHyperlink = A.ColorSchemeIndexValues.FollowedHyperlink
            },
            new P.SlideLayoutIdList(
                new P.SlideLayoutId
                {
                    Id = 1U,
                    RelationshipId = masterPart.GetIdOfPart(layoutPart)
                }),
            new P.TextStyles(new P.TitleStyle(), new P.BodyStyle(), new P.OtherStyle()));

        presentationPart.Presentation = new P.Presentation(
            new P.SlideIdList(),
            new P.SlideSize
            {
                Cx = width,
                Cy = height,
                Type = width == 9_144_000 ? P.SlideSizeValues.Screen4x3 : P.SlideSizeValues.Wide
            },
            new P.NotesSize { Cx = 6_858_000, Cy = 9_144_000 });

        presentationPart.Presentation.SlideMasterIdList = new P.SlideMasterIdList(
            new P.SlideMasterId { Id = 2U, RelationshipId = masterId });

        layoutPart.SlideLayout.Save();
        masterPart.SlideMaster.Save();
        presentationPart.Presentation.Save();
        return presentationPart;
    }

    internal static P.ShapeTree CreateShapeTree()
    {
        return new P.ShapeTree(
            new P.NonVisualGroupShapeProperties(
                new P.NonVisualDrawingProperties { Id = 1U, Name = string.Empty },
                new P.NonVisualGroupShapeDrawingProperties(),
                new P.ApplicationNonVisualDrawingProperties()),
            new P.GroupShapeProperties(
                new A.TransformGroup(
                    new A.Offset { X = 0L, Y = 0L },
                    new A.Extents { Cx = 0L, Cy = 0L },
                    new A.ChildOffset { X = 0L, Y = 0L },
                    new A.ChildExtents { Cx = 0L, Cy = 0L })));
    }
}
