#!/bin/bash
set -e


APP_NAME="$1"
TAG="$2"
IMAGE="registry.mait.gq/${APP_NAME}:${TAG}"

# Crear o usar el builder existente
if ! docker buildx inspect mybuilder > /dev/null 2>&1; then
    docker buildx create --use --name mybuilder
else
    docker buildx use mybuilder
fi

docker buildx inspect --bootstrap
docker buildx build --platform linux/amd64 --push -t $IMAGE ./src/mait.stats/

echo "Imagen construida y subida: $IMAGE"
