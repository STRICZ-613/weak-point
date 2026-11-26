using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using A = DocumentFormat.OpenXml.Drawing;
using P = DocumentFormat.OpenXml.Presentation;
using WeakPoint.Core.Models;
using ModelPresentation = WeakPoint.Core.Models.Presentation;

namespace WeakPoint.Core.Pptx;

public sealed class PptxWriter
{
    public void Write(ModelPresentation presentation, string filePath)
    {
        ArgumentNullException.ThrowIfNull(presentation);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        if (presentation.SlideWidth <= 0 || presentation.SlideHeight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(presentation), "La taille des diapositives doit être positive.");
        }

        using var document = PresentationDocument.Create(filePath, PresentationDocumentType.Presentation);
        var presentationPart = PptxTemplate.CreatePackage(document, presentation.SlideWidth, presentation.SlideHeight);
        document.PackageProperties.Title = presentation.Title;
        document.PackageProperties.Creator = presentation.Author;

        var slideIds = presentationPart.Presentation.SlideIdList!;
        uint slideId = 256;
        foreach (var modelSlide in presentation.Slides)
        {
            var slidePart = presentationPart.AddNewPart<SlidePart>();
            slidePart.AddPart(presentationPart.SlideMasterParts.First().SlideLayoutParts.First());

            var shapeTree = PptxTemplate.CreateShapeTree();
            uint shapeId = 2;
            foreach (var element in modelSlide.Elements.OrderBy(item => item.ZOrder))
            {
                if (!element.IsVisible)
                {
                    continue;
                }

                switch (element)
                {
                    case TextBoxElement textBox:
                        shapeTree.Append(CreateShape(textBox, shapeId++, true, textBox.Text));
                        break;
                    case ShapeElement shape:
                        shapeTree.Append(CreateShape(shape, shapeId++, false, shape.Text));
                        break;
                    case ImageElement:
                        throw new NotSupportedException("L'écriture des images sera ajoutée avec le mapper d'images.");
                    case UnsupportedElement:
                        throw new NotSupportedException("La conservation des objets non pris en charge sera ajoutée dans une phase ultérieure.");
                    default:
                        throw new NotSupportedException($"Type d'élément non pris en charge : {element.GetType().Name}.");
                }
            }

            slidePart.Slide = new P.Slide(
                new P.CommonSlideData(shapeTree),
                new P.ColorMapOverride(new P.MasterColorMapping()));
            slidePart.Slide.Save();
            slideIds.Append(new P.SlideId
            {
                Id = slideId++,
                RelationshipId = presentationPart.GetIdOfPart(slidePart)
            });
        }

        presentationPart.Presentation.Save();
    }

    private static P.Shape CreateShape(SlideElement element, uint id, bool isTextBox, string text)
    {
        var preset = element is ShapeElement shape
            ? ToPresetGeometry(shape.ShapeType)
            : A.ShapeTypeValues.Rectangle;
        var name = string.IsNullOrWhiteSpace(element.Name) ? (isTextBox ? "Zone de texte" : "Forme") : element.Name;
        var nonVisualProperties = new P.NonVisualShapeProperties(
            new P.NonVisualDrawingProperties
            {
                Id = id,
                Name = name,
                Description = isTextBox ? "WeakPoint.TextBox" : null
            },
            new P.NonVisualShapeDrawingProperties(),
            new P.ApplicationNonVisualDrawingProperties());

        var shapeProperties = new P.ShapeProperties(
            new A.Transform2D(
                new A.Offset { X = element.X, Y = element.Y },
                new A.Extents { Cx = Math.Max(1L, element.Width), Cy = Math.Max(1L, element.Height) }),
            new A.PresetGeometry(new A.AdjustValueList()) { Preset = preset },
            isTextBox
                ? new A.NoFill()
                : new A.SolidFill(new A.RgbColorModelHex { Val = "4472C4" }),
            new A.Outline(
                new A.SolidFill(new A.RgbColorModelHex { Val = "000000" }),
                new A.PresetDash { Val = A.PresetLineDashValues.Solid }));

        shapeProperties.Transform2D!.Rotation = (int)Math.Round(element.Rotation * 60_000d);

        var body = new P.TextBody(
            new A.BodyProperties(),
            new A.ListStyle(),
            new A.Paragraph(
                new A.Run(
                    new A.RunProperties { Language = "fr-FR" },
                    new A.Text(text ?? string.Empty)),
                new A.EndParagraphRunProperties { Language = "fr-FR" }));

        return new P.Shape(nonVisualProperties, shapeProperties, body);
    }

    private static A.ShapeTypeValues ToPresetGeometry(ShapeType shapeType)
    {
        return shapeType switch
        {
            ShapeType.Ellipse => A.ShapeTypeValues.Ellipse,
            ShapeType.Triangle => A.ShapeTypeValues.Triangle,
            ShapeType.Line => A.ShapeTypeValues.Line,
            ShapeType.Arrow => A.ShapeTypeValues.RightArrow,
            _ => A.ShapeTypeValues.Rectangle
        };
    }
}
