# Scope Mirror
A set of tools to duplicate the clipped area of the desktop.

See [documents on Wiki](https://github.com/sakapon/Scope-Mirror/wiki).

## Single
For standalone, shows the desktop to a secondary display.

### Setup
- [Download the app](https://github.com/sakapon/Scope-Mirror/raw/master/Downloads/ClipMirror.Single-1.0.1.zip)

### Usage
- Execute `ClipMirror.Single.exe`

### System Requirements
- .NET Framework 4.5

### Release Notes
- **v1.0.1** The first release.

## Lightning
Shows the desktop to a remote computer on network.

### Setup
- [Download the app for a host computer](https://github.com/sakapon/Scope-Mirror/raw/master/Downloads/ScopeMirror.Lightning.Host-1.0.1.zip)
- [Download the app for users](https://github.com/sakapon/Scope-Mirror/raw/master/Downloads/ScopeMirror.Lightning.Guest-1.0.2.zip)

### Usage
- Execute `ScopeMirror.Lightning.Host.exe`, on a host computer
- Edit `ScopeMirror.Lightning.Guest.exe.config` and set the IP address of the host computer
- Execute `ScopeMirror.Lightning.Guest.exe`, on guest computers

### System Requirements
- .NET Framework 4.5

### Release Notes
- **v1.0.1** The first release.
- **v1.0.2** Reduce big images.
