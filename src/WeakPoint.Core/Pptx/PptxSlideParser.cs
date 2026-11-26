using DocumentFormat.OpenXml.Packaging;
using A = DocumentFormat.OpenXml.Drawing;
using P = DocumentFormat.OpenXml.Presentation;
using WeakPoint.Core.Models;

namespace WeakPoint.Core.Pptx;

public sealed class PptxSlideParser
{
    public Slide Parse(SlidePart slidePart)
    {
        ArgumentNullException.ThrowIfNull(slidePart);
        var slide = new Slide();
        var shapeTree = slidePart.Slide.CommonSlideData?.ShapeTree;
        if (shapeTree is null)
        {
            return slide;
        }

        var zOrder = 0;
        foreach (var shape in shapeTree.Elements<P.Shape>())
        {
            var transform = shape.ShapeProperties?.Transform2D;
            var offset = transform?.Offset;
            var extents = transform?.Extents;
            var name = shape.NonVisualShapeProperties?.NonVisualDrawingProperties?.Name?.Value ?? string.Empty;
            var description = shape.NonVisualShapeProperties?.NonVisualDrawingProperties?.Description?.Value;
            var text = shape.TextBody?.InnerText ?? string.Empty;

            SlideElement element;
            if (string.Equals(description, "WeakPoint.TextBox", StringComparison.Ordinal))
            {
                element = new TextBoxElement { Text = text };
            }
            else
            {
                element = new ShapeElement
                {
                    ShapeType = FromPresetGeometry(shape.ShapeProperties?.PresetGeometry?.Preset?.Value),
                    Text = text
                };
            }

            element.Name = name;
            element.X = offset?.X?.Value ?? 0L;
            element.Y = offset?.Y?.Value ?? 0L;
            element.Width = extents?.Cx?.Value ?? 0L;
            element.Height = extents?.Cy?.Value ?? 0L;
            element.Rotation = (transform?.Rotation?.Value ?? 0) / 60_000d;
            element.ZOrder = zOrder++;
            slide.Elements.Add(element);
        }

        return slide;
    }

    private static ShapeType FromPresetGeometry(A.ShapeTypeValues? preset)
    {
        return preset?.Value switch
        {
            "ellipse" => ShapeType.Ellipse,
            "triangle" => ShapeType.Triangle,
            "line" => ShapeType.Line,
            "rightArrow" => ShapeType.Arrow,
            _ => ShapeType.Rectangle
        };
    }
}
