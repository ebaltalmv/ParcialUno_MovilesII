---
name: MAUI PokeCollection Modern
colors:
  primary: '#E3350D'
  on-primary: '#FFFFFF'
  primary-container: '#FFDAD3'
  on-primary-container: '#400000'
  secondary: '#3168B1'
  on-secondary: '#FFFFFF'
  secondary-container: '#D6E3FF'
  on-secondary-container: '#001B3D'
  tertiary: '#FFCC00'
  on-tertiary: '#332900'
  tertiary-container: '#FFE873'
  on-tertiary-container: '#4D3E00'
  background: '#F8F9FA'
  on-background: '#1A1C1E'
  surface: '#FFFFFF'
  on-surface: '#1A1C1E'
  surface-variant: '#E7E0EC'
  on-surface-variant: '#49454F'
  outline: '#79747E'
  surface-tint: '#E3350D'
  error: '#B3261E'
  on-error: '#FFFFFF'
typography:
  display:
    fontFamily: 'OpenSans-Bold'
    fontSize: 32px
    fontWeight: '800'
  headline-md:
    fontFamily: 'OpenSans-Semibold'
    fontSize: 24px
    fontWeight: '600'
  body-lg:
    fontFamily: 'OpenSans-Regular'
    fontSize: 16px
    fontWeight: '400'
  body-sm:
    fontFamily: 'OpenSans-Regular'
    fontSize: 14px
    fontWeight: '400'
  label-bold:
    fontFamily: 'OpenSans-Semibold'
    fontSize: 12px
    fontWeight: '600'
rounded:
  sm: 4px
  md: 8px
  lg: 16px
  xl: 24px
  full: 9999px
spacing:
  xs: 4px
  sm: 8px
  md: 16px
  lg: 24px
  xl: 32px
---

# MAUI PokéCollection Design System

## Concepto Visual
Este sistema de diseño está optimizado para construirse **únicamente usando .NET MAUI con C# y XAML**. El diseño abandona efectos web complejos como el "glassmorphism" en favor de una **UI táctil, limpia y nativa**, basada en sombras sutiles (Shadow) y bordes redondeados (Border). La paleta está inspirada en los colores clásicos de la franquicia pero afinada para interfaces modernas.

## Colores (Para usar en Colors.xaml)
Los colores están pensados para soportar `AppThemeBinding` fácilmente:
- **Primary (Rojo Pokémon):** Para Action Buttons (Guardar, Buscar).
- **Secondary (Azul Pokémon):** Elementos de navegación y acentos.
- **Tertiary (Amarillo Eléctrico):** Destacar rarezas y tags (Shiny, Holo).
- **Backgrounds:** Fondos grises claros (`#F8F9FA`) con tarjetas puramente blancas (`#FFFFFF`) para generar contraste.

## Tipografía
Fuentes empaquetadas (ej. `OpenSans-Regular.ttf`, `OpenSans-Semibold.ttf`) o tipografía del sistema:
- `Title`: Bold, 24px-32px.
- `Body`: Regular, 14px-16px.
- Utiliza la propiedad `FontAttributes="Bold"` nativa de MAUI en controles `Label`.

## Componentes de MAUI Recomendados
- **Tarjetas de Pokémon:** Emplear el control `<Border>` en lugar del clásico `<Frame>`. El `<Border>` de MAUI permite especificar `StrokeShape="RoundRectangle 16"` y manejar recortes (`IsClippedToBounds`) correctamente. Utiliza la propiedad `Shadow` para profundidad.
- **Grillas y Listas:** Usar `<CollectionView>` con `GridItemsLayout` a 2 o 3 columnas, definiendo `HorizontalItemSpacing="16"` y `VerticalItemSpacing="16"`.
- **Botones:** Utilizar `<Button>` con `CornerRadius="8"`. Altura mínima recomendada de `48` para asegurar un buen Touch Target móvil.
- **Tags de Tipo:** Pequeños `<Border>` circulares (`RoundRectangle 999`) con texto o íconos.

## Arquitectura de Layouts
Priorizar `Grid` para interfaces complejas y `VerticalStackLayout` / `HorizontalStackLayout` con `Spacing="16"` en lugar de aplicar `Margins` individuales extensos. El padding general de la pantalla o de la página raíz debe ser `Padding="16"`.
