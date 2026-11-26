// Íconos de línea propios (sin librería). Heredan el color del texto (currentColor).
const TRAZOS = {
  dashboard: (
    <>
      <rect x="3" y="3" width="7" height="9" rx="2" />
      <rect x="14" y="3" width="7" height="5" rx="2" />
      <rect x="14" y="12" width="7" height="9" rx="2" />
      <rect x="3" y="16" width="7" height="5" rx="2" />
    </>
  ),
  movimientos: <path d="M7 4v16M7 20l-3-3M7 20l3-3M17 20V4M17 4l-3 3M17 4l3 3" />,
  deudas: (
    <>
      <rect x="3" y="5" width="18" height="14" rx="3" />
      <path d="M3 10h18M7 15h4" />
    </>
  ),
  presupuestos: (
    <>
      <path d="M12 3a9 9 0 1 0 9 9h-9z" />
      <path d="M15 3.5A9 9 0 0 1 20.5 9H15z" />
    </>
  ),
  metas: (
    <>
      <circle cx="12" cy="12" r="9" />
      <circle cx="12" cy="12" r="5" />
      <circle cx="12" cy="12" r="1.5" />
    </>
  ),
  categorias: (
    <>
      <path d="M4 4h7v7H4zM13 4h7v7h-7zM4 13h7v7H4z" />
      <circle cx="16.5" cy="16.5" r="3.5" />
    </>
  ),
  subir: <path d="M12 19V5M5 12l7-7 7 7" />,
  bajar: <path d="M12 5v14M5 12l7 7 7-7" />,
  tarjeta: (
    <>
      <rect x="3" y="6" width="18" height="12" rx="2" />
      <path d="M3 10h18" />
    </>
  ),
  diana: (
    <>
      <circle cx="12" cy="12" r="8" />
      <circle cx="12" cy="12" r="3" />
    </>
  ),
}

export default function Icono({ nombre, className = 'icono' }) {
  return (
    <svg className={className} viewBox="0 0 24 24" aria-hidden="true">
      {TRAZOS[nombre]}
    </svg>
  )
}
