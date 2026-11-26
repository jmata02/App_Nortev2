using Norte.Abstracciones.Servicios;

namespace Norte.Pruebas.Unitarias.Fakes;

/// <summary>Hash falso y predecible: basta para probar que el Flujo nunca guarda la contraseña en claro.</summary>
public class HasherContrasenaFake : IHasherContrasena
{
    public string Hashear(string contrasena) => "hash:" + contrasena;

    public bool Verificar(string hash, string contrasena) => hash == Hashear(contrasena);
}
