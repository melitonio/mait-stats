#!/bin/bash
set -e

APP_NAME="$1"
TAG="$2"
IMAGE="registry.mait.gq/${APP_NAME}:${TAG}"

echo "🛠️ Construyendo imagen $IMAGE"
docker build ./src/mait.stats/ -t $IMAGE

echo "📤 Subiendo imagen a $IMAGE"
docker push $IMAGE

echo "🧹 Eliminando imagen local $IMAGE"
docker rmi $IMAGE || true
