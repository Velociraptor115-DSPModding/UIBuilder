using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

namespace DysonSphereProgram.Modding.UI;

public interface IProperties<T> where T : Component
{
  void Apply(T component);
}

public abstract record GraphicProperties : IProperties<Graphic>
{
  public Material material { get; init; } = null;
  public Color color { get; init; } = Color.white;
  public bool raycastTarget { get; init; } = true;

  public void Apply(Graphic component)
  {
    if (material != null)
      component.material = material;
    component.color = color;
    component.raycastTarget = raycastTarget;
  }
}

public abstract record MaskableGraphicProperties : GraphicProperties, IProperties<MaskableGraphic>
{
  public bool maskable { get; init; } = true;

  public void Apply(MaskableGraphic component)
  {
    base.Apply(component);
    component.maskable = maskable;
  }
}

public record RawImageProperties : MaskableGraphicProperties, IProperties<RawImage>
{
  public Texture texture { get; init; } = null;
  public Rect uvRect { get; init; } = new Rect(0.0f, 0.0f, 1f, 1f);

  public void Apply(RawImage component)
  {
    base.Apply(component);
    if (texture != null)
      component.texture = texture;
    component.uvRect = uvRect;
  }
}

public record ImageProperties : MaskableGraphicProperties, IProperties<Image>
{
  public Sprite sprite { get; init; } = null;
  public Sprite overrideSprite { get; init; } = null;
  public Image.Type type { get; init; } = Image.Type.Simple;
  public bool preserveAspect { get; init; } = false;

  public bool fillCenter { get; init; } = true;
  public Image.FillMethod fillMethod { get; init; } = Image.FillMethod.Radial360;
  public bool fillClockwise { get; init; } = true;
  public int fillOrigin { get; init; } = 0;
  public float alphaHitTestMinimumThreshold { get; init; } = 0;
  public bool useSpriteMesh { get; init; } = false;

  public void Apply(Image component)
  {
    base.Apply(component);
    if (sprite != null)
      component.sprite = sprite;
    if (overrideSprite != null)
      component.overrideSprite = overrideSprite;
    component.type = type;
    component.preserveAspect = preserveAspect;
    component.fillCenter = fillCenter;
    component.fillMethod = fillMethod;
    component.fillClockwise = fillClockwise;
    component.fillOrigin = fillOrigin;
    component.alphaHitTestMinimumThreshold = alphaHitTestMinimumThreshold;
    component.useSpriteMesh = useSpriteMesh;
  }
}

public static class GraphicPropertiesBuilder
{
  public static readonly ImageProperties ImagePropertiesDefault = new ImageProperties();
  public static readonly TranslucentImageProperties TranslucentImagePropertiesDefault = new TranslucentImageProperties();

  public static T WithRaycast<T>(this T record, bool raycastTarget) where T : GraphicProperties =>
    record with { raycastTarget = raycastTarget };
  public static T WithColor<T>(this T record, Color color) where T : GraphicProperties => record with { color = color };
  public static T WithMaterial<T>(this T record, Material material) where T : GraphicProperties => record with { material = material};

  public static T WithSprite<T>(this T record, Sprite sprite, Sprite overrideSprite = null, Image.Type? type = null)
    where T : ImageProperties
    => record with
    {
      sprite = sprite
      , overrideSprite = overrideSprite ? overrideSprite : record.overrideSprite
      , type = type.HasValue ? type.Value : record.type
    };
  public static T WithSlicedSprite<T>(this T record, Sprite sprite, Sprite overrideSprite = null)
    where T : ImageProperties
    => WithSprite(record, sprite, overrideSprite, Image.Type.Sliced);
}

public record TranslucentImageProperties : ImageProperties, IProperties<TranslucentImage>
{
  public TranslucentImageSource source { get; init; } = null;
  public float vibrancy { get; init; } = 1f;
  public float brightness { get; init; } = 0;
  public float flatten { get; init; } = 0.1f;
  public bool updating { get; init; } = true;
  public bool lateUpdating { get; init; } = true;
  public float spriteBlending { get; init; } = 0.65f;

  public void Apply(TranslucentImage component)
  {
    base.Apply(component);
    component.vibrancy = vibrancy;
    component.brightness = brightness;
    component.flatten = flatten;
    component.updating = updating;
    component.lateUpdating = lateUpdating;
    component.spriteBlending = spriteBlending;
  }
}

public record TextProperties : MaskableGraphicProperties, IProperties<Text>
{
  public Font font { get; init; } = null;
  public bool supportRichText { get; init; } = true;
  public bool resizeTextForBestFit { get; init; } = false;
  public int resizeTextMinSize { get; init; } = 14;
  public int resizeTextMaxSize { get; init; } = 40;
  public TextAnchor alignment { get; init; } = TextAnchor.UpperLeft;
  public bool alignByGeometry { get; init; } = false;
  public int fontSize { get; init; } = 14;
  public HorizontalWrapMode horizontalOverflow { get; init; } = HorizontalWrapMode.Wrap;
  public VerticalWrapMode verticalOverflow { get; init; } = VerticalWrapMode.Truncate;
  public float lineSpacing { get; init; } = 1f;
  public FontStyle fontStyle { get; init; } = FontStyle.Normal;

  public void Apply(Text component)
  {
    base.Apply(component);
    if (font != null)
      component.font = font;
    component.supportRichText = supportRichText;
    component.resizeTextForBestFit = resizeTextForBestFit;
    component.resizeTextMinSize = resizeTextMinSize;
    component.resizeTextMaxSize = resizeTextMaxSize;
    component.alignment = alignment;
    component.alignByGeometry = alignByGeometry;
    component.fontSize = fontSize;
    component.horizontalOverflow = horizontalOverflow;
    component.verticalOverflow = verticalOverflow;
    component.lineSpacing = lineSpacing;
    component.fontStyle = fontStyle;
  }
}

public record SelectableProperties : IProperties<Selectable>
{
  public Selectable.Transition? transition { get; init; } = null;
  public ColorBlock? colors { get; init; } = null;
  public SpriteState? spriteState { get; init; } = null;

  public void Apply(Selectable component)
  {
    if (transition.HasValue)
      component.transition = transition.Value;
    if (colors.HasValue)
      component.colors = colors.Value;
    if (spriteState.HasValue)
      component.spriteState = spriteState.Value;
  }
}

public record ScrollRectProperties : IProperties<ScrollRect>
{
  public ScrollRect.MovementType movementType { get; init; } = ScrollRect.MovementType.Elastic;
  public float elasticity { get; init; } = 0.1f;
  public bool inertia { get; init; } = true;
  public float decelerationRate { get; init; } = 0.135f;
  public float scrollSensitivity { get; init; } = 1f;
  public float horizontalScrollbarSpacing { get; init; } = 0;
  public float verticalScrollbarSpacing { get; init; } = 0;


  public void Apply(ScrollRect component)
  {
    component.movementType = movementType;
    component.elasticity = elasticity;
    component.inertia = inertia;
    component.decelerationRate = decelerationRate;
    component.scrollSensitivity = scrollSensitivity;
    component.horizontalScrollbarSpacing = horizontalScrollbarSpacing;
    component.verticalScrollbarSpacing = verticalScrollbarSpacing;
  }
}
