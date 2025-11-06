# Makefile to build DeclaredAgeRangeWrapper.xcframework via archive (with dSYMs)

# Configuration
PROJECT := DeclaredAgeRangeWrapper.xcodeproj
SCHEME  := DeclaredAgeRangeWrapper
FW_NAME := DeclaredAgeRangeWrapper
CONFIG  := Release

# Derived paths
BUILD_DIR := $(CURDIR)/build
ARCHIVES_DIR := $(BUILD_DIR)/archives

# Archive paths
IOS_ARCHIVE   := $(ARCHIVES_DIR)/ios
SIM_ARCHIVE   := $(ARCHIVES_DIR)/ios-simulator
MACOS_ARCHIVE := $(ARCHIVES_DIR)/macos

# Framework paths within archives
IOS_FW_PATH   := $(IOS_ARCHIVE).xcarchive/Products/Library/Frameworks/$(FW_NAME).framework
SIM_FW_PATH   := $(SIM_ARCHIVE).xcarchive/Products/Library/Frameworks/$(FW_NAME).framework
MACOS_FW_PATH := $(MACOS_ARCHIVE).xcarchive/Products/Library/Frameworks/$(FW_NAME).framework

# dSYM paths
IOS_DSYM_PATH   := $(IOS_ARCHIVE).xcarchive/dSYMs/$(FW_NAME).framework.dSYM
SIM_DSYM_PATH   := $(SIM_ARCHIVE).xcarchive/dSYMs/$(FW_NAME).framework.dSYM
MACOS_DSYM_PATH := $(MACOS_ARCHIVE).xcarchive/dSYMs/$(FW_NAME).framework.dSYM

# Final output
XCFRAMEWORK := $(CURDIR)/$(FW_NAME).xcframework

.PHONY: all xcframework archive-ios archive-sim archive-macos clean init

all: xcframework

init:
	@mkdir -p "$(ARCHIVES_DIR)"

xcframework: clean_build_dirs init archive-ios archive-sim archive-macos
	@echo "Creating $(FW_NAME).xcframework (with dSYMs)..."
	@rm -rf "$(XCFRAMEWORK)"
	@xcodebuild -create-xcframework \
		-framework "$(IOS_FW_PATH)" -debug-symbols "$(IOS_DSYM_PATH)" \
		-framework "$(SIM_FW_PATH)" -debug-symbols "$(SIM_DSYM_PATH)" \
		-framework "$(MACOS_FW_PATH)" -debug-symbols "$(MACOS_DSYM_PATH)" \
		-output "$(XCFRAMEWORK)"
	@echo "Built: $(XCFRAMEWORK)"

# Archive iOS (device)
archive-ios:
	@echo "Archiving $(SCHEME) for iOS (iphoneos) with dSYMs..."
	@xcodebuild archive \
		-project "$(PROJECT)" \
		-scheme "$(SCHEME)" \
		-configuration "$(CONFIG)" \
		-destination 'generic/platform=iOS' \
		-archivePath "$(IOS_ARCHIVE)" \
		SKIP_INSTALL=NO \
		BUILD_LIBRARY_FOR_DISTRIBUTION=YES \
		CODE_SIGNING_ALLOWED=NO CODE_SIGNING_REQUIRED=NO \
		DEBUG_INFORMATION_FORMAT=dwarf-with-dsym \
		| sed -E 's/^/\[archive-ios\] /'

# Archive iOS Simulator
archive-sim:
	@echo "Archiving $(SCHEME) for iOS Simulator (iphonesimulator) with dSYMs..."
	@xcodebuild archive \
		-project "$(PROJECT)" \
		-scheme "$(SCHEME)" \
		-configuration "$(CONFIG)" \
		-destination 'generic/platform=iOS Simulator' \
		-archivePath "$(SIM_ARCHIVE)" \
		SKIP_INSTALL=NO \
		BUILD_LIBRARY_FOR_DISTRIBUTION=YES \
		CODE_SIGNING_ALLOWED=NO CODE_SIGNING_REQUIRED=NO \
		DEBUG_INFORMATION_FORMAT=dwarf-with-dsym \
		| sed -E 's/^/\[archive-sim\] /'

# Archive macOS
archive-macos:
	@echo "Archiving $(SCHEME) for macOS (macosx) with dSYMs..."
	@xcodebuild archive \
		-project "$(PROJECT)" \
		-scheme "$(SCHEME)" \
		-configuration "$(CONFIG)" \
		-destination 'generic/platform=macOS' \
		-archivePath "$(MACOS_ARCHIVE)" \
		SKIP_INSTALL=NO \
		BUILD_LIBRARY_FOR_DISTRIBUTION=YES \
		CODE_SIGNING_ALLOWED=NO CODE_SIGNING_REQUIRED=NO \
		DEBUG_INFORMATION_FORMAT=dwarf-with-dsym \
		| sed -E 's/^/\[archive-macos\] /'

.PHONY: clean_build_dirs
clean_build_dirs:
	@rm -rf "$(BUILD_DIR)"

clean:
	@rm -rf "$(BUILD_DIR)" "$(XCFRAMEWORK)"
	@echo "Cleaned build products and xcframework."
