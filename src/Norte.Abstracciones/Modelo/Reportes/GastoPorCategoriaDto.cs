namespace Norte.Abstracciones.Modelo.Reportes;

/// <summary>Cuánto se gastó en una categoría y qué porcentaje es del gasto total del mes.</summary>
public record GastoPorCategoriaDto(Guid CategoriaId, string Categoria, decimal Total, decimal Porcentaje);
