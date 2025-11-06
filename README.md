# Declared Age Range Sample

A sample project demonstrating how to use Apple's Declared Age Range API in .NET for iOS and macOS applications. This project shows how to request age information from users to deliver age-appropriate experiences in your app.

## Overview

This sample demonstrates:
- How to create Swift wrapper bindings for the Declared Age Range framework
- How to build an XCFramework that works across iOS, iOS Simulator, and macOS
- How to create .NET bindings for iOS and macOS
- How to request age range information from users in a .NET iOS app
- How to handle age range responses including self-declared and guardian-declared ages

## Project Structure

```
DeclaredAgeRangeSample/
├── DeclaredAgeRangeWrapper/              # Swift wrapper framework
│   ├── DeclaredAgeRangeWrapper/
│   │   └── DeclaredAgeRangeWrapper.swift # Swift bridge for Objective-C interop
│   ├── Makefile                          # Build XCFramework for all platforms
│   └── DeclaredAgeRangeWrapper.xcodeproj
├── DeclaredAgeRangeWrapperBinding.iOS/   # .NET iOS bindings
│   ├── ApiDefinition.cs                  # Objective-C binding definitions
│   └── StructsAndEnums.cs                # Enums and structures
├── DeclaredAgeRangeWrapperBinding.macOS/ # .NET macOS bindings
└── iOSSampleApp/                         # Sample iOS application
    └── SceneDelegate.cs                  # Demo implementation
```

## Requirements

- **macOS**: Required for building the XCFramework and running the sample
- **Xcode 26.0+**: Required for Swift 6 and Declared Age Range API support
- **.NET 9 or 10**: Required for .NET iOS/macOS development
- **iOS 26.0+** or **macOS 26.0+**: Target platforms that support Declared Age Range API

## Building

### Step 1: Build the Swift XCFramework

First, build the Swift wrapper framework that bridges the Declared Age Range API for .NET consumption:

```bash
cd DeclaredAgeRangeWrapper
make
```

This will:
1. Archive the framework for iOS (device), iOS Simulator, and macOS
2. Generate debug symbols (dSYMs) for debugging
3. Create `DeclaredAgeRangeWrapper.xcframework` with all platform slices

To clean build artifacts:
```bash
make clean
```

### Step 2: Build the .NET Bindings

Build the .NET binding libraries for iOS and macOS:

```bash
# From the solution root
dotnet build DeclaredAgeRangeWrapperBinding.iOS/DeclaredAgeRangeWrapperBinding.iOS.csproj
dotnet build DeclaredAgeRangeWrapperBinding.macOS/DeclaredAgeRangeWrapperBinding.macOS.csproj
```

### Step 3: Build and Run the Sample App

Build and run the iOS sample application:

```bash
cd iOSSampleApp
dotnet build -t:Run -f net9.0-ios
```

> [!NOTE]
> While this runs in the Simulator, you will get a `Not Supported` error message so
> you will need to run this on device with a provisioning profile and a Bundle
> Identifier that has the correct entitlement `com.apple.developer.declared-age-range`.
> 
> See [documentation](https://learn.microsoft.com/en-us/dotnet/maui/ios/cli) for more information on how to run .NET Apps on device.

## Using the API

The sample app demonstrates requesting age range information with a minimum age requirement:

```csharp
using DeclaredAgeRangeWrapper;

// Request age range with a 16+ age gate
DeclaredAgeRangeBridge.RequestAgeRange(
    16,                          // Required minimum age
    null,                        // Optional second age threshold
    null,                        // Optional third age threshold
    viewController,              // iOS: UIViewController, macOS: NSWindow
    (response, error) => {
        if (error != null) {
            Console.WriteLine($"Error: {error.Description}");
            return;
        }
        
        switch (response.Type) {
            case MyAgeRangeResponseType.Sharing:
                var range = response.Range;
                Console.WriteLine($"Age Range: {range.LowerBound} - {range.UpperBound ?? "∞"}");
                Console.WriteLine($"Declaration Type: {range.Declaration}");
                break;
                
            case MyAgeRangeResponseType.DeclinedSharing:
                Console.WriteLine("User declined to share age information");
                break;
        }
    }
);
```

### Response Types

The API can return two types of responses:

1. **Sharing**: User agreed to share their age range
   - `LowerBound`: Minimum age (e.g., 16)
   - `UpperBound`: Maximum age or `null` for infinity
   - `Declaration`: How the age was declared
     - `SelfDeclared`: User declared their own age
     - `GuardianDeclared`: Guardian declared the user's age
     - `Unknown`: Declaration method unknown

2. **DeclinedSharing**: User declined to share age information

## Key Components

### Swift Wrapper (`DeclaredAgeRangeWrapper.swift`)

Provides an Objective-C compatible bridge to the Swift-only Declared Age Range API:
- `DeclaredAgeRangeBridge`: Main class for requesting age ranges
- `MyAgeRange`: Age range data structure
- `MyAgeRangeResponse`: Response container with sharing status
- Cross-platform support (iOS and macOS via `PlatformAnchor` typealias)

### .NET Bindings

- **ApiDefinition.cs**: Objective-C binding definitions using `[BaseType]` attributes
- **StructsAndEnums.cs**: .NET enumerations for response types and declarations
- Shared between iOS and macOS bindings via file linking

### Build System

The `Makefile` uses `xcodebuild` to:
- Archive frameworks with `BUILD_LIBRARY_FOR_DISTRIBUTION=YES` for ABI stability
- Generate dSYMs for debugging
- Create a universal XCFramework supporting all Apple platforms


## Related Links

* [Declared Age Range - Apple Developer Documentation](https://developer.apple.com/documentation/declaredagerange/)
* [Deliver age-appropriate experiences in your app - WWDC 2025](https://developer.apple.com/videos/play/wwdc2025/299/)

## License

See [LICENSE](LICENSE) file for details.
