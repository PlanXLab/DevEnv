#!/bin/bash
# launcher.sh — DevEnv macOS portable launcher
# https://github.com/PlanXLab/DevEnv
#
# Portable: ALL paths are derived from this script's location.
# Move the entire folder to a new path or another Mac and it works.
#
# Usage:
#   ./launcher.sh                  — launch VS Code (update check throttled to 1h)
#   ./launcher.sh --update         — force update check, then launch VS Code
#   ./launcher.sh --no-launch      — update only, skip VS Code launch
#   ./launcher.sh [vscode-args]    — extra arguments forwarded to VS Code
#
# Matches Windows launcher.exe behaviour:
#   - Portable mode (--extensions-dir / --user-data-dir)
#   - Auto-update on every launch (Windows: --check-versions-internal)
#     Throttled to once per hour to avoid hitting GitHub API rate limits.

[ -z "$BASH_VERSION" ] && exec bash "$0" "$@"

# ---------------------------------------------------------------------------
# Portable paths (all relative to this script's directory)
# ---------------------------------------------------------------------------
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
VSCODE_APP="$SCRIPT_DIR/Visual Studio Code.app"
VSCODE_CLI="$VSCODE_APP/Contents/Resources/app/bin/code"
VSCODE_DATA="$SCRIPT_DIR/data"
VSCODE_EXTENSIONS="$VSCODE_DATA/extensions"
VSCODE_USER_DATA="$VSCODE_DATA/user-data"
VSCODE_PYTHON="$VSCODE_DATA/lib/python"
VSCODE_PYTHON_BIN="$VSCODE_PYTHON/bin/python3"
PVS_INFO="$SCRIPT_DIR/pvs.info"
UPDATE_STAMP="$SCRIPT_DIR/.last_update"

# ---------------------------------------------------------------------------
# Parse flags
# ---------------------------------------------------------------------------
DO_LAUNCH=true
DO_UPDATE=false
VSCODE_ARGS=()
for _arg in "$@"; do
  case "$_arg" in
    --update)    DO_UPDATE=true ;;
    --no-launch) DO_LAUNCH=false ;;
    *)           VSCODE_ARGS+=("$_arg") ;;
  esac
done

# ---------------------------------------------------------------------------
# Load pvs.info (PYTHON_VERSION, PYTHON_PATH, PLATFORM, ...)
# ---------------------------------------------------------------------------
PYTHON_VERSION=""
[[ -f "$PVS_INFO" ]] && source "$PVS_INFO"

# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------
step() { echo ""; echo "==> $*"; }
info() { echo "    $*"; }
warn() { echo "    [!] $*"; }

# Update or insert a key=value pair in pvs.info
pvs_set() {
  local key="$1" val="$2"
  if grep -q "^${key}=" "$PVS_INFO" 2>/dev/null; then
    sed -i '' "s|^${key}=.*|${key}=${val}|" "$PVS_INFO"
  else
    echo "${key}=${val}" >> "$PVS_INFO"
  fi
}

# Returns 0 (true) if an update check should run.
# Eager (every-launch) like Windows launcher.exe, throttled to 1 hour
# so we don't hit the GitHub API rate limit on rapid relaunches.
need_update_check() {
  if [[ "$DO_UPDATE" == "true" ]]; then return 0; fi
  if [[ ! -f "$UPDATE_STAMP" ]]; then return 0; fi
  local last elapsed
  last=$(cat "$UPDATE_STAMP" 2>/dev/null || echo 0)
  elapsed=$(( $(date +%s) - last ))
  if [[ $elapsed -gt 3600 ]]; then return 0; fi
  return 1
}

# ---------------------------------------------------------------------------
# Install python-build-standalone for PY_VERSION into TARGET_DIR
# Fully portable: no compilation, pre-built binary
# ---------------------------------------------------------------------------
install_python_portable() {
  local py_version="$1" target_dir="$2"
  local arch py_arch
  arch=$(uname -m)
  [[ "$arch" == "arm64" ]] && py_arch="aarch64-apple-darwin" || py_arch="x86_64-apple-darwin"
  local pattern="cpython-${py_version}+.*-${py_arch}-install_only\\.tar\\.gz"

  info "Searching python-build-standalone for Python $py_version ($py_arch)..."
  local asset_url
  asset_url=$("$VSCODE_PYTHON_BIN" -c "
import urllib.request, json, re, sys
try:
    req = urllib.request.Request(
        'https://api.github.com/repos/astral-sh/python-build-standalone/releases',
        headers={'Accept': 'application/vnd.github.v3+json', 'User-Agent': 'DevEnv/1.0'})
    with urllib.request.urlopen(req, timeout=15) as r:
        releases = json.loads(r.read())
    pat = re.compile(sys.argv[1])
    for release in releases[:5]:
        for asset in release.get('assets', []):
            if pat.match(asset['name']):
                print(asset['browser_download_url'])
                sys.exit(0)
    sys.exit(1)
except Exception as e:
    print(f'Error: {e}', file=sys.stderr)
    sys.exit(1)
" "$pattern" 2>/dev/null) || { warn "Asset not found for Python $py_version ($py_arch)"; return 1; }

  local tmptar="/tmp/devenv-python-update.tar.gz"
  local tmpdir="/tmp/devenv-python-extract"
  info "Downloading Python $py_version..."
  curl -fsSL -L "$asset_url" -o "$tmptar" || { warn "Download failed"; rm -f "$tmptar"; return 1; }
  info "Extracting..."
  rm -rf "$tmpdir" && mkdir -p "$tmpdir"
  tar -xzf "$tmptar" -C "$tmpdir" && rm -f "$tmptar"

  # Atomic-ish swap: new → active → old (removed)
  rm -rf "${target_dir}.new"
  mv "$tmpdir/python" "${target_dir}.new"
  rm -rf "${target_dir}.old"
  [[ -d "$target_dir" ]] && mv "$target_dir" "${target_dir}.old"
  mv "${target_dir}.new" "$target_dir"
  rm -rf "${target_dir}.old" "$tmpdir"
  info "Python $py_version installed at $target_dir"
}

# ---------------------------------------------------------------------------
# Update logic
# ---------------------------------------------------------------------------
do_update() {

  # ── VS Code ──────────────────────────────────────────────────────────────
  step "Checking VS Code"
  if [[ -x "$VSCODE_CLI" ]]; then
    local current latest
    current=$("$VSCODE_CLI" --version 2>/dev/null | head -1 || echo "unknown")
    latest=$(curl -fsSL \
      "https://api.github.com/repos/microsoft/vscode/releases/latest" \
      -H "User-Agent: DevEnv/1.0" 2>/dev/null \
      | "$VSCODE_PYTHON_BIN" -c \
        "import json,sys; print(json.load(sys.stdin).get('tag_name',''))" 2>/dev/null \
      || echo "")
    if [[ -n "$latest" && "$current" != "$latest" ]]; then
      info "Updating VS Code: $current → $latest"
      local tmpzip="/tmp/devenv-vscode-update.zip"
      if curl -fsSL -L \
          "https://update.code.visualstudio.com/latest/darwin-universal/stable" \
          -o "$tmpzip"; then
        rm -rf "${VSCODE_APP}.old"
        mv "$VSCODE_APP" "${VSCODE_APP}.old"
        if unzip -q "$tmpzip" -d "$SCRIPT_DIR" && [[ -d "$VSCODE_APP" ]]; then
          rm -rf "${VSCODE_APP}.old"
          xattr -dr com.apple.quarantine "$VSCODE_APP" 2>/dev/null || true
          # Persist new version to pvs.info (matches Windows UpdateVersionsInInfo)
          local new_ver
          new_ver=$("$VSCODE_CLI" --version 2>/dev/null | head -1 || echo "$latest")
          pvs_set "VSCODE_VERSION" "$new_ver"
          info "VS Code updated to $new_ver"
        else
          [[ -d "${VSCODE_APP}.old" ]] && mv "${VSCODE_APP}.old" "$VSCODE_APP"
          warn "VS Code update failed, restored previous version"
        fi
        rm -f "$tmpzip"
      else
        warn "VS Code download failed"
      fi
    else
      info "VS Code is up to date ($current)"
    fi
  else
    warn "VS Code CLI not found: $VSCODE_CLI"
  fi

  # ── Python ───────────────────────────────────────────────────────────────
  if [[ -n "$PYTHON_VERSION" && -x "$VSCODE_PYTHON_BIN" ]]; then
    step "Checking Python"
    local minor="${PYTHON_VERSION%.*}"
    local latest_py
    latest_py=$("$VSCODE_PYTHON_BIN" -c "
import urllib.request, json, sys
try:
    with urllib.request.urlopen(
        'https://endoflife.date/api/python/' + sys.argv[1] + '.json', timeout=10
    ) as r:
        print(json.loads(r.read())['latest'])
except Exception:
    print(sys.argv[2])
" "$minor" "$PYTHON_VERSION" 2>/dev/null || echo "$PYTHON_VERSION")
    if [[ "$latest_py" != "$PYTHON_VERSION" ]]; then
      info "Updating Python: $PYTHON_VERSION → $latest_py"
      if install_python_portable "$latest_py" "$VSCODE_PYTHON"; then
        pvs_set "PYTHON_VERSION" "$latest_py"
        pvs_set "PYTHON_PATH"    "$VSCODE_PYTHON/bin/python3"
        PYTHON_VERSION="$latest_py"
        info "Python updated to $latest_py"
      fi
    else
      info "Python is up to date ($PYTHON_VERSION)"
    fi
  fi

  # Record timestamp so next auto-check is in 7 days
  date +%s > "$UPDATE_STAMP"
}

# ---------------------------------------------------------------------------
# Main
# ---------------------------------------------------------------------------
if need_update_check; then
  do_update
fi

if [[ "$DO_LAUNCH" == "true" ]]; then
  if [[ ! -x "$VSCODE_CLI" ]]; then
    echo "ERROR: VS Code not found at:" >&2
    echo "  $VSCODE_APP" >&2
    echo "" >&2
    echo "Run devenv-setup.sh to reinstall, or check the install path." >&2
    exit 1
  fi
  # Launch VS Code detached (returns immediately, terminal is free)
  nohup "$VSCODE_CLI" \
    --extensions-dir "$VSCODE_EXTENSIONS" \
    --user-data-dir  "$VSCODE_USER_DATA"  \
    ${VSCODE_ARGS[@]+"${VSCODE_ARGS[@]}"} \
    >/dev/null 2>&1 &
  disown
  echo "VS Code launched (portable)"
  echo "  Extensions : $VSCODE_EXTENSIONS"
  echo "  User data  : $VSCODE_USER_DATA"
fi
