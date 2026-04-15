# Changelog
All notable changes to this package will be documented in this file.

## [1.0.1] - 2026-04-15
### Changed
- Converted `StatType` from an enum to a serializable class to allow extensibility without modifying the package.

### Added
- `StatTypeRegistry` to manage and track all registered `StatType` instances.
- `StatTypeDrawer` custom property drawer for Inspector support.
- `StatTypeEditorInitializer` to ensure all `StatType` registrations are initialized before the Editor draws them.

## [1.0.0] - 2026-03-24
### Added
- Initial release of the package with base implementation.