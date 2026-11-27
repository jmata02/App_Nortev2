/**
 * Logo de Norte: palmera blanca sobre una insignia con el degradado del atardecer rosado y
 * el nombre en letras gruesas rosa claro con contorno, estilo sticker. Los colores vienen de
 * tokens.css. `conNombre={false}` deja solo la insignia.
 */
export default function Logo({ conNombre = true }) {
  return (
    <span className="logo">
      <span className="logo__insignia" aria-hidden="true">
        <svg className="logo__palmera" viewBox="0 0 32 32">
          <g fill="none" style={{ stroke: 'var(--color-sobre-tornasol)' }} strokeLinecap="round" strokeLinejoin="round">
            <path d="M15 30 C15.5 24 16.5 18 18.5 12.5" strokeWidth="2.4" />
            <path d="M18.5 12.5 C15 8.5 9.5 8 4.5 11.5" strokeWidth="2.2" />
            <path d="M18.5 12.5 C16.5 6.5 12 3.5 7.5 3.5" strokeWidth="2.2" />
            <path d="M18.5 12.5 C19.5 6.5 23 3.5 27.5 4" strokeWidth="2.2" />
            <path d="M18.5 12.5 C22.5 9.5 27 10 30 14" strokeWidth="2.2" />
            <path d="M18.5 12.5 C18 8 19 5 21 2.5" strokeWidth="2" />
            <path d="M9 30.5 H23" strokeWidth="1.6" opacity=".7" />
          </g>
        </svg>
      </span>
      {conNombre && (
        <span className="logo__texto">
          <b className="logo__nombre">NORTE</b>
          <small className="logo__lema">finanzas</small>
        </span>
      )}
    </span>
  )
}
