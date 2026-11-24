#!/usr/bin/env bash
# Recorre el ciclo de vida completo de un despliegue contra la API en marcha.
# Uso:  ./scripts/demo-api.sh [URL_BASE]      (por defecto http://localhost:8080)
set -euo pipefail

BASE="${1:-http://localhost:8080}"
API="$BASE/api/deployments"

pretty() { if command -v jq >/dev/null; then jq .; else cat; echo; fi; }
titulo() { printf '\n\033[1m── %s\033[0m\n' "$1"; }

titulo "GET /health"
curl -s "$BASE/health"; echo

titulo "POST /api/deployments  (registrar)"
RESPUESTA=$(curl -s -X POST "$API" -H 'Content-Type: application/json' \
  -d '{"applicationName":"CloudForge.Api","version":"1.4.0","environment":"Staging","commitSha":"a1b2c3d","branch":"main"}')
echo "$RESPUESTA" | pretty
ID=$(echo "$RESPUESTA" | sed -E 's/.*"id":"([^"]+)".*/\1/')

titulo "POST /api/deployments/{id}/succeed  (sin haber empezado → 409)"
curl -s -X POST "$API/$ID/succeed" | pretty

titulo "POST /api/deployments/{id}/start"
curl -s -X POST "$API/$ID/start" | pretty

sleep 1

titulo "POST /api/deployments/{id}/succeed"
curl -s -X POST "$API/$ID/succeed" | pretty

titulo "GET /api/deployments?environment=Staging"
curl -s "$API?environment=Staging" | pretty
