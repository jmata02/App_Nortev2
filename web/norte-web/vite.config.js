import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// En Docker (ver docker-compose.yml) los cambios de los archivos de Windows no llegan como
// eventos al contenedor: hay que revisarlos por sondeo y escuchar fuera del contenedor.
const enDocker = process.env.NORTE_EN_DOCKER === 'true'

// El puerto 5173 es el que permite la política CORS del API (Cors:OrigenSpa).
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    strictPort: true,
    host: enDocker ? '0.0.0.0' : 'localhost',
    watch: enDocker ? { usePolling: true, interval: 300 } : undefined,
  },
})
