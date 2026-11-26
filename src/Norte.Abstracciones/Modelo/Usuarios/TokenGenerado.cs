namespace Norte.Abstracciones.Modelo.Usuarios;

/// <summary>Token JWT firmado y el momento (UTC) en que vence.</summary>
public record TokenGenerado(string Token, DateTime ExpiraUtc);
