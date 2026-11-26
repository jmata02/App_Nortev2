import { useId } from 'react'

/** Logo de Norte: palmera neón y el nombre. `conNombre={false}` deja solo la palmera. */
export default function Logo({ conNombre = true }) {
  const idDegradado = useId()

  return (
    <span className="logo">
      <svg className="logo__palmera" viewBox="0 0 32 32" aria-hidden="true">
        <defs>
          <linearGradient id={idDegradado} x1="0" y1="0" x2="0" y2="1">
            <stop offset="0" style={{ stopColor: 'var(--color-ambar)' }} />
            <stop offset=".45" style={{ stopColor: 'var(--color-magenta)' }} />
            <stop offset="1" style={{ stopColor: 'var(--color-violeta)' }} />
          </linearGradient>
        </defs>
        <g fill="none" stroke={`url(#${idDegradado})`} strokeLinecap="round" strokeLinejoin="round">
          <path d="M15 30 C15.5 24 16.5 18 18.5 12.5" strokeWidth="2.4" />
          <path d="M18.5 12.5 C15 8.5 9.5 8 4.5 11.5" strokeWidth="2.2" />
          <path d="M18.5 12.5 C16.5 6.5 12 3.5 7.5 3.5" strokeWidth="2.2" />
          <path d="M18.5 12.5 C19.5 6.5 23 3.5 27.5 4" strokeWidth="2.2" />
          <path d="M18.5 12.5 C22.5 9.5 27 10 30 14" strokeWidth="2.2" />
          <path d="M18.5 12.5 C18 8 19 5 21 2.5" strokeWidth="2" />
          <path d="M9 30.5 H23" strokeWidth="1.6" opacity=".7" />
        </g>
      </svg>
      {conNombre && <b className="logo__nombre">NORTE</b>}
    </span>
  )
}
