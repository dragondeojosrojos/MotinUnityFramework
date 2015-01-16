#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <QuartzCore/QuartzCore.h>
#import "iOSPluginLoadingView.h"


@interface iOSPlugin : NSObject <UIAlertViewDelegate>
{
    bool pauseOnAlert;
    iOSPluginLoadingView* _loadingView;
  	CGRect displayAdFrame;
}

+ (iOSPlugin*)sharedManager;

-(void)ShowLoadingView;
-(void)HideLoadingView;
- (void)showAlertWithTitle:(NSString*)title Message:(NSString*)message ButtonTitle:(NSString*)buttonTitle PauseUnity:(bool)pauseUnity;
- (void)alertView:(UIAlertView *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex;
-(UIInterfaceOrientation)currentOrientation;

@end

