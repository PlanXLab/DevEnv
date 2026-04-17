# DevEnv

DevEnv is a portable Windows development environment installer and updater. The current repository is built around two WinForms executables:

- `devenv-setup.exe`: initial installation
- `launcher.exe`: launch, version checks, and upgrades

The final distributable is `devenv-setup.exe` only. `launcher.exe` is embedded into the installer and extracted into the target folder during installation.

## Download

- Latest release page: https://github.com/PlanXLab/DevEnv/releases/latest
- Latest `devenv-setup.exe`: https://github.com/PlanXLab/DevEnv/releases/latest/download/devenv-setup.exe

## Current Architecture

### Installer

`devenv-setup.exe` installs the latest available versions of the following components:

- VS Code
- Python Embedded
- PowerShell 7
- Oh My Posh
- PowerShell modules: `Terminal-Icons`, `PSFzf`, `modern-unix-win`
- Fonts: `0xProto Nerd Font`, `DalseoHealing`
- VS Code extensions:
  - `teabyii.ayu`
  - `zhuangtongfa.material-theme`
  - `ms-python.python`
  - `ms-toolsai.jupyter`
  - `ms-vscode-remote.remote-ssh`
  - `KevinRose.vsc-python-indent`
  - `usernamehw.errorlens`
  - `Gerrnperl.outline-map`

Installer UI behavior:

- Default installation path is `C:\VSCode`
- The Python version field can be left empty to install the latest supported version
- Pressing `Enter` in the Python version field starts installation immediately
- The UI provides overall progress, per-component progress, and a filtered Key Events log

### Launcher

`launcher.exe` is the runtime entry point for an installed environment.

- Launches `Code.exe` in portable mode
- Checks component versions in the background
- Creates `upgrade_*` flag files when updates are available
- Shows the upgrade UI on the next run and upgrades only the required individual components

Upgradeable components:

- PowerShell 7
- Oh My Posh
- Terminal-Icons
- PSFzf
- modern-unix-win
- VS Code

### Version and State Files

The installation root contains the following runtime files:

- `pvs.info`: installation path and installed component versions
- `pvs.log`: installation log
- `upgrade.log`: upgrade log
- `upgrade_*`: launcher-generated upgrade flag files

## Installed Folder Layout

A typical installed layout looks like this:

```text
<install-root>/
├─ Code.exe
├─ launcher.exe
├─ pvs.info
├─ pvs.log
├─ upgrade.log
├─ data/
│  ├─ extensions/
│  ├─ user-data/
│  └─ lib/
│     ├─ fonts/
│     ├─ pwsh/
│     ├─ python/
│     └─ origin/
└─ resources/
```

## Usage

### End Users

1. Download `devenv-setup.exe` from GitHub Releases.
2. Run it and choose the installation path and Python version.
3. After installation, launch VS Code through `launcher.exe` or the created shortcut.
4. From that point on, `launcher.exe` handles background version checks and upgrade entry.

### Maintenance Mode

`launcher.exe --init` runs the internal maintenance routine.

- Synchronizes the VS Code extension set
- Restores configuration and origin files
- Cleans the Python environment
- Reinstalls fonts and recreates shortcuts
- Cleans user-data and launches VS Code

## Build

Requirements:

- Windows
- .NET Framework 4.7.2 build environment
- `csc.exe`

Build command:

```powershell
./build
```

Build outputs:

- Intermediate artifact: `dist/launcher.exe`
- Final artifact: `dist/devenv-setup.exe`

`build.ps1` builds `launcher.exe` first, embeds it into `devenv-setup.exe`, and leaves only `devenv-setup.exe` as the final distribution artifact.

## Git and Release Policy

This repository does not commit build artifacts.

- `dist/` is excluded from Git tracking
- Only source code is pushed to GitHub
- The final executable is distributed through GitHub Release assets only

Release upload workflow for this repository:

```powershell
./publish-release.ps1 -Tag v2026.04.17
```

Once at least one release already exists, you can upload to the latest release without specifying a tag:

```powershell
./publish-release.ps1
```

The script performs the following:

- Verifies that `dist/devenv-setup.exe` exists
- Queries the latest release when needed
- Replaces the asset if the release already exists
- Creates the first release and uploads the asset if no release exists yet

## Implementation Notes

- VS Code core installation uses direct archive extraction
- VS Code extensions are downloaded as the latest VSIX packages in parallel and extracted directly
- PowerShell 7 and VS Code download/extraction paths are kept as lightweight as possible
- The upgrade UI keeps user-facing Key Events concise while preserving full diagnostic logs in files
