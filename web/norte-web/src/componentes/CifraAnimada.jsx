import { useEffect, useState } from 'react'
import { formatearColones, prefiereMenosMovimiento } from '../utilidades/formato.js'

const DURACION_MS = 1400

/**
 * Muestra una cifra que "cuenta" desde cero hasta su valor. El valor final viene del API;
 * los números intermedios son solo animación de pantalla, no un cálculo de dinero.
 */
export default function CifraAnimada({ valor, formato = formatearColones, className = '' }) {
  const sinAnimacion = prefiereMenosMovimiento()
  const [mostrado, setMostrado] = useState(0)

  useEffect(() => {
    if (sinAnimacion) return undefined
    let cuadro
    const inicio = performance.now()
    const avanzar = (ahora) => {
      const progreso = Math.min((ahora - inicio) / DURACION_MS, 1)
      const suavizado = 1 - Math.pow(1 - progreso, 4)
      setMostrado(progreso === 1 ? valor : valor * suavizado)
      if (progreso < 1) cuadro = requestAnimationFrame(avanzar)
    }
    cuadro = requestAnimationFrame(avanzar)
    return () => cancelAnimationFrame(cuadro)
  }, [valor, sinAnimacion])

  return <span className={`num ${className}`}>{formato(sinAnimacion ? valor : mostrado)}</span>
}
