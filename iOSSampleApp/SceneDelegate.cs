using DeclaredAgeRangeWrapper;

namespace iOSSampleApp;

[Register("SceneDelegate")]
public class SceneDelegate : UIResponder, IUIWindowSceneDelegate
{
    [Export("window")] public UIWindow? Window { get; set; }
    
    UIButton? requestButton;

    [Export("scene:willConnectToSession:options:")]
    public void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
    {
        // Use this method to optionally configure and attach the UIWindow 'Window' to the provided UIWindowScene 'scene'.
        // Since we are not using a storyboard, the 'Window' property needs to be initialized and attached to the scene.
        // This delegate does not imply the connecting scene or session are new (see UIApplicationDelegate 'GetConfiguration' instead).
        if (scene is UIWindowScene windowScene)
        {
            Window ??= new UIWindow(windowScene);

                // Create a 'UIViewController' with a stack panel and button
                var vc = new UIViewController();
                vc.View!.BackgroundColor = UIColor.SystemBackground;
                
                // Create the button
                requestButton = UIButton.FromType(UIButtonType.System);
                requestButton.SetTitle("Request Age Permissions", UIControlState.Normal);
                requestButton.TranslatesAutoresizingMaskIntoConstraints = false;
                requestButton.TouchUpInside += RequestButtonOnTouchUpInside;
                
                // Create a stack view
                var stackView = new UIStackView();
                stackView.Axis = UILayoutConstraintAxis.Vertical;
                stackView.Alignment = UIStackViewAlignment.Center;
                stackView.Distribution = UIStackViewDistribution.EqualCentering;
                stackView.TranslatesAutoresizingMaskIntoConstraints = false;
                stackView.AddArrangedSubview(requestButton);
                
                // Add stack view to the view
                vc.View.AddSubview(stackView);
                
                // Center the stack view horizontally and vertically
                NSLayoutConstraint.ActivateConstraints(new[]
                {
                    stackView.CenterXAnchor.ConstraintEqualTo(vc.View.CenterXAnchor),
                    stackView.CenterYAnchor.ConstraintEqualTo(vc.View.CenterYAnchor)
                });

            Window.RootViewController = vc;
            Window.MakeKeyAndVisible();
        }
    }

    private void RequestButtonOnTouchUpInside(object? sender, EventArgs e) 
    {
        DeclaredAgeRangeBridge.RequestAgeRange (16, null, null, Window?.RootViewController!, MyResponseHandler);
    }

    void MyResponseHandler(MyAgeRangeResponse response, NSError? error)
    {
        if (response is null)
        {
            Console.WriteLine($"No response received: {error?.Description}");
            return;
        }
        
        switch (response.Type)
        {
            case MyAgeRangeResponseType.DeclinedSharing:
                Console.WriteLine("User declined sharing age range");
                break;
            case MyAgeRangeResponseType.Sharing:
                var ar = response.Range;
                if (ar is not null) {
                    Console.WriteLine("User is sharing age range:");
                    Console.WriteLine($"LowerBound: {ar.LowerBound?.StringValue}");
                    Console.WriteLine($"UpperBound: {ar.UpperBound?.StringValue ?? "Infinity"}");
                    Console.WriteLine($"Declaration: {ar.Declaration}");
                }
                break;
        }
            
    }

    [Export("sceneDidDisconnect:")]
    public void DidDisconnect(UIScene scene)
    {
        // Called as the scene is being released by the system.
        // This occurs shortly after the scene enters the background, or when its session is discarded.
        // Release any resources associated with this scene that can be re-created the next time the scene connects.
        // The scene may re-connect later, as its session was not neccessarily discarded (see UIApplicationDelegate `DidDiscardSceneSessions` instead).
    }

    [Export("sceneDidBecomeActive:")]
    public void DidBecomeActive(UIScene scene)
    {
        // Called when the scene has moved from an inactive state to an active state.
        // Use this method to restart any tasks that were paused (or not yet started) when the scene was inactive.
    }

    [Export("sceneWillResignActive:")]
    public void WillResignActive(UIScene scene)
    {
        // Called when the scene will move from an active state to an inactive state.
        // This may occur due to temporary interruptions (ex. an incoming phone call).
    }

    [Export("sceneWillEnterForeground:")]
    public void WillEnterForeground(UIScene scene)
    {
        // Called as the scene transitions from the background to the foreground.
        // Use this method to undo the changes made on entering the background.
    }

    [Export("sceneDidEnterBackground:")]
    public void DidEnterBackground(UIScene scene)
    {
        // Called as the scene transitions from the foreground to the background.
        // Use this method to save data, release shared resources, and store enough scene-specific state information
        // to restore the scene back to its current state.
    }
}