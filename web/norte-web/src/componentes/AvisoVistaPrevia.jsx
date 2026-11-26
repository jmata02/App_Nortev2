/**
 * Marca las pantallas que todavía muestran datos de ejemplo. Cada responsable lo quita
 * cuando conecta su módulo al API.
 */
export default function AvisoVistaPrevia({ responsable, texto }) {
  return (
    <span className="aviso-previa">
      {texto ?? `Vista previa del diseño · datos de ejemplo · responsable: ${responsable}`}
    </span>
  )
}
