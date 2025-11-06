//
//  DeclaredAgeRangeWrapper.swift
//  DeclaredAgeRangeWrapper
//
//  Created by Alex Soto on 10/30/25.
//

import Foundation
import DeclaredAgeRange

#if canImport(UIKit)
import UIKit
public typealias PlatformAnchor = UIViewController
#endif

#if canImport(AppKit) && !targetEnvironment(macCatalyst)
import AppKit
public typealias PlatformAnchor = NSWindow
#endif

@objc public enum MyAgeRangeDeclaration: Int {
    case selfDeclared
    case guardianDeclared
    case unknown
}

@objc public class MyAgeRange: NSObject {
    @objc public let lowerBound: NSNumber?
    @objc public let upperBound: NSNumber?
    @objc public let declaration: MyAgeRangeDeclaration

    @objc public init(lowerBound: NSNumber?, upperBound: NSNumber?, declaration: MyAgeRangeDeclaration) {
        self.lowerBound = lowerBound
        self.upperBound = upperBound
        self.declaration = declaration
    }
}

@objc public enum MyAgeRangeResponseType: Int {
    case sharing
    case declinedSharing
}

@objc public class MyAgeRangeResponse: NSObject {
    @objc public let type: MyAgeRangeResponseType
    @objc public let range: MyAgeRange?

    @objc public init(type: MyAgeRangeResponseType, range: MyAgeRange?) {
        self.type = type
        self.range = range
    }
}

@objc public class DeclaredAgeRangeBridge: NSObject {
    /// Objective-C bridge for requestAgeRange API.
    /// - Parameters:
    ///   - threshold1: The required minimum age for your app.
    ///   - threshold2: An optional additional minimum age for your app. Pass nil to omit.
    ///   - threshold3: An optional additional minimum age for your app. Pass nil to omit.
    ///   - viewController: The view controller to anchor and present system UI off of.
    ///   - completion: Completion handler called on main queue with response or error.
    @objc public static func requestAgeRange(withAgeGates threshold1: Int,
                                             _ threshold2: NSNumber?,
                                             _ threshold3: NSNumber?,
                                             in viewController: PlatformAnchor,
                                             completion: @escaping (MyAgeRangeResponse?, NSError?) -> Void) {
        Task {
            do {
                // Convert NSNumber? to Int? for the optional thresholds
                let t2: Int? = threshold2?.intValue
                let t3: Int? = threshold3?.intValue

                // Call the Swift API with correct labels and positional optional args
                let response = try await AgeRangeService.shared.requestAgeRange(ageGates: threshold1, t2, t3, in: viewController)
                switch response {
                case .sharing(let swiftRange):
                    let declaration: MyAgeRangeDeclaration
                    switch swiftRange.ageRangeDeclaration {
                    case .selfDeclared: declaration = .selfDeclared
                    case .guardianDeclared: declaration = .guardianDeclared
                    case .none: declaration = .unknown
                    @unknown default:
                        declaration = .unknown
                    }
                    // Build the response and call completion on the main actor to avoid capturing non-Sendable across a @Sendable boundary.
                    await MainActor.run {
                        let range = MyAgeRange(
                            lowerBound: swiftRange.lowerBound as NSNumber?,
                            upperBound: swiftRange.upperBound as NSNumber?,
                            declaration: declaration
                        )
                        let bridgeResponse = MyAgeRangeResponse(type: .sharing, range: range)
                        completion(bridgeResponse, nil)
                    }
                case .declinedSharing:
                    await MainActor.run {
                        let bridgeResponse = MyAgeRangeResponse(type: .declinedSharing, range: nil)
                        completion(bridgeResponse, nil)
                    }
                @unknown default:
                    // Future-proof fallback: treat unknown responses as a safe decline.
                    await MainActor.run {
                        let bridgeResponse = MyAgeRangeResponse(type: .declinedSharing, range: nil)
                        completion(bridgeResponse, nil)
                    }
                }
            } catch {
                await MainActor.run {
                    completion(nil, error as NSError)
                }
            }
        }
    }
}
