#!/usr/bin/env pwsh
param(
    [Parameter(Mandatory=$true, Position=0)]
    [string]$AppNameImage,
    
    [switch]$NoBuild,
    [switch]$NoPush,
    [switch]$Rm
)

# Configurar para que el script termine en caso de error
$ErrorActionPreference = "Stop"

$IMAGE = "${AppNameImage}:latest"

# Construir imagen (si no se especifica --no-build)
if (-not $NoBuild) {
    Write-Host "🛠️ Construyendo imagen $IMAGE" -ForegroundColor Green
    docker build ./src/mait.stats/ -t $IMAGE
} else {
    Write-Host "⏭️ Omitiendo construcción de imagen" -ForegroundColor Gray
}

# Subir imagen (si no se especifica --no-push)
if (-not $NoPush) {
    # Verificar que la imagen existe antes de intentar subirla
    $imageExists = docker images --format "table {{.Repository}}:{{.Tag}}" | Select-String -Pattern "^$([regex]::Escape($IMAGE))$" -Quiet
    
    if ($imageExists) {
        Write-Host "📤 Subiendo imagen a $IMAGE" -ForegroundColor Yellow
        docker push $IMAGE
    } else {
        Write-Host "❌ Error: La imagen $IMAGE no existe localmente. Use -NoBuild para omitir la construcción solo si la imagen ya existe." -ForegroundColor Red
        Write-Host "💡 Sugerencia: Ejecute sin -NoBuild para construir la imagen primero." -ForegroundColor Yellow
        exit 1
    }
} else {
    Write-Host "⏭️ Omitiendo subida de imagen" -ForegroundColor Gray
}

# Eliminar imagen local (solo si se especifica -Rm)
if ($Rm) {
    # Verificar que la imagen existe antes de intentar eliminarla
    $imageExists = docker images --format "table {{.Repository}}:{{.Tag}}" | Select-String -Pattern "^$([regex]::Escape($IMAGE))$" -Quiet
    
    if ($imageExists) {
        Write-Host "🧹 Eliminando imagen local $IMAGE" -ForegroundColor Cyan
        try {
            docker rmi $IMAGE
        } catch {
            Write-Host "⚠️ No se pudo eliminar la imagen local (esto es normal si está en uso)" -ForegroundColor Yellow
        }
    } else {
        Write-Host "⚠️ La imagen $IMAGE no existe localmente, no se puede eliminar" -ForegroundColor Yellow
    }
} else {
    Write-Host "⏭️ Manteniendo imagen local (use -Rm para eliminarla)" -ForegroundColor Gray
}

Write-Host "✅ Proceso completado exitosamente" -ForegroundColor Green

# FORMA DE USO
# .\build-local.ps1 <nombre_imagen> [opciones]
# 
# Opciones disponibles:
#   -NoBuild    : No construye la imagen Docker (requiere que la imagen ya exista)
#   -NoPush     : No sube la imagen al registry
#   -Rm         : Elimina la imagen local después de procesarla


# Ejemplos:
# .\build-local.ps1 registry.archivo.pub/mait-stats
# .\build-local.ps1 registry.archivo.pub/mait-stats -Rm
# .\build-local.ps1 registry.archivo.pub/mait-stats -NoPush
# .\build-local.ps1 registry.archivo.pub/mait-stats -NoBuild
