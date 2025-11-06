using System;
using Foundation;
using ObjCRuntime;

#if __IOS__
using UIKit;
#elif __MACOS__
using AppKit;
using UIViewController = AppKit.NSWindow;
#endif

namespace DeclaredAgeRangeWrapper
{
	[BaseType (typeof (NSObject), Name = "_TtC23DeclaredAgeRangeWrapper22DeclaredAgeRangeBridge")]
	interface DeclaredAgeRangeBridge
	{
		[Static]
		[Export ("requestAgeRangeWithAgeGates:::in:completion:")]
		void RequestAgeRange (nint threshold1, [NullAllowed] NSNumber threshold2, [NullAllowed] NSNumber threshold3, UIViewController viewController, Action<MyAgeRangeResponse, NSError> completion);
	}

	[BaseType (typeof (NSObject), Name = "_TtC23DeclaredAgeRangeWrapper10MyAgeRange")]
	[DisableDefaultCtor]
	interface MyAgeRange
	{
		[NullAllowed, Export ("lowerBound", ArgumentSemantic.Strong)]
		NSNumber LowerBound { get; }

		[NullAllowed, Export ("upperBound", ArgumentSemantic.Strong)]
		NSNumber UpperBound { get; }

		[Export ("declaration")]
		MyAgeRangeDeclaration Declaration { get; }

		[Export ("initWithLowerBound:upperBound:declaration:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] NSNumber lowerBound, [NullAllowed] NSNumber upperBound, MyAgeRangeDeclaration declaration);
	}

	[BaseType (typeof(NSObject), Name = "_TtC23DeclaredAgeRangeWrapper18MyAgeRangeResponse")]
	[DisableDefaultCtor]
	interface MyAgeRangeResponse
	{
		[Export ("type")]
		MyAgeRangeResponseType Type { get; }

		[NullAllowed, Export ("range", ArgumentSemantic.Strong)]
		MyAgeRange Range { get; }

		[Export ("initWithType:range:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MyAgeRangeResponseType type, [NullAllowed] MyAgeRange range);
	}
}