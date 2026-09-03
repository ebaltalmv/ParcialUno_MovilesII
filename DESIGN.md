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
  background-dark: '#121212'
  on-background: '#1A1C1E'
  on-background-dark: '#FFFFFF'
  surface: '#FFFFFF'
  surface-dark: '#1E1E1E'
  on-surface: '#1A1C1E'
  on-surface-dark: '#FFFFFF'
  surface-variant: '#E7E0EC'
  surface-variant-dark: '#49454F'
  on-surface-variant: '#49454F'
  on-surface-variant-dark: '#CAC4D0'
  outline: '#79747E'
  outline-dark: '#938F99'
  surface-tint: '#E3350D'
  error: '#B3261E'
  error-dark: '#F2B8B5'
  on-error: '#FFFFFF'
  on-error-dark: '#601410'
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

## Colores (Para usar en Colors.xaml) y Modo Oscuro
Los colores están pensados para soportar `AppThemeBinding` para modo claro y oscuro:
- **Primary (Rojo Pokémon):** Para Action Buttons (Guardar, Buscar).
- **Secondary (Azul Pokémon):** Elementos de navegación y acentos.
- **Tertiary (Amarillo Eléctrico):** Destacar rarezas, tags (Shiny, Holo) y el indicador de Favoritos (★).
- **Backgrounds:** Fondos grises claros (`#F8F9FA`) en modo claro y grises oscuros (`#121212`) en modo oscuro.
- **Surfaces (Tarjetas):** Blancas (`#FFFFFF`) en modo claro y grises elevados (`#1E1E1E`) en oscuro.
- **Uso Obligatorio:** Es mandatorio usar `AppThemeBinding` (ej. `BackgroundColor="{AppThemeBinding Light={StaticResource Background}, Dark={StaticResource BackgroundDark}}"`) en páginas, textos y bordes para garantizar legibilidad.
- **AppShell:** La barra de navegación debe sobreescribir el color morado por defecto de MAUI aplicando explícitamente `Shell.BackgroundColor` y `Shell.TitleColor` en el archivo `AppShell.xaml`.

## Tipografía
Fuentes empaquetadas (ej. `OpenSans-Regular.ttf`, `OpenSans-Semibold.ttf`) o tipografía del sistema:
- `Title`: Bold, 24px-32px.
- `Body`: Regular, 14px-16px.
- Utiliza la propiedad `FontAttributes="Bold"` nativa de MAUI en controles `Label`.

## Componentes de MAUI Recomendados
- **Tarjetas de Pokémon:** Emplear el control `<Border>` en lugar del clásico `<Frame>`. El `<Border>` de MAUI permite especificar `StrokeShape="RoundRectangle 16"` (Nota: NO uses la propiedad `IsClippedToBounds` ya que causa error de compilación en MAUI 7/8; el Border recorta automáticamente). Utiliza la propiedad `Shadow` para profundidad.
- **Interacción en Listas:** En los `CollectionView`, usar SIEMPRE `SelectionMode="None"` para evitar que el color de fondo de selección nativo desborde las esquinas curvas. En su lugar, agrega un `<TapGestureRecognizer>` dentro del `Border` de la tarjeta para manejar la navegación o el toque.
- **Grillas y Listas (Responsividad):** Usar `<CollectionView>` con `GridItemsLayout`. Para hacer el diseño adaptativo entre móvil, tablet y escritorio, usa la extensión `OnIdiom`: `Span="{OnIdiom Phone=2, Tablet=4, Desktop=6}"` con `HorizontalItemSpacing="16"` y `VerticalItemSpacing="16"`.
- **Botones:** Utilizar `<Button>` con `CornerRadius="8"`. Altura mínima recomendada de `48` para asegurar un buen Touch Target móvil.
- **Tags de Tipo:** Pequeños `<Border>` circulares (`RoundRectangle 999`) con texto o íconos.
- **Favoritos:** Se utiliza el caracter de estrella (★ relleno para favorito, ☆ contorno para no favorito) junto a un `DataTrigger` o conversor, en lugar de mostrar textos booleanos como "True/False".

## Arquitectura de Layouts
Priorizar `Grid` para interfaces complejas y `VerticalStackLayout` / `HorizontalStackLayout` con `Spacing="16"` en lugar de aplicar `Margins` individuales extensos. El padding general de la pantalla o de la página raíz debe ser `Padding="16"`.
