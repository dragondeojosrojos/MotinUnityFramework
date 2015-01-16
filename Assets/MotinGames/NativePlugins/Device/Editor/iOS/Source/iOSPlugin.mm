#import "iOSPlugin.h"
void UnityPause( bool pause );


//#define FlurryAppId @"MDQZTM67CFTG644R2YYD"
// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

@implementation iOSPlugin

+ (iOSPlugin*)sharedManager
{
	static iOSPlugin *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[iOSPlugin alloc] init];
	
	return sharedSingleton;
}

- (id)init
{
    
    NSLog(@"Init IOS Plugin");
    pauseOnAlert = false;
    UIWindow *window = [[[UIApplication sharedApplication] windows] objectAtIndex:0];
    
    if (_loadingView)
    {
        [_loadingView release];
    }
    _loadingView = [[iOSPluginLoadingView alloc] initWithFrame:[window screen].bounds] ;
    [window addSubview:_loadingView.mainView];
    [_loadingView disable];

    [_loadingView hideLabel];
    
	self = [super init];

	return self;
} 

- (void)dealloc
{
    [_loadingView release];
	[super dealloc];	
}

-(void)ShowLoadingView
{
    [_loadingView fadeIn];
}

-(void)HideLoadingView
{
    [_loadingView fadeOut];
}

- (void)showAlertWithTitle:(NSString*)title Message:(NSString*)message ButtonTitle:(NSString*)buttonTitle PauseUnity:(bool)pauseUnity
{
    if(pauseUnity)
    {
        UnityPause(true);
        pauseOnAlert = true;
    }
    
	UIAlertView *alertview = [[UIAlertView alloc] initWithTitle:title
                                                        message:message
                                                       delegate:self 
                                              cancelButtonTitle:buttonTitle
                                              otherButtonTitles:nil];
	[alertview show];
	[alertview release];
    
    
}

- (void)alertView:(UIAlertView *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex {
    // the user clicked one of the OK/Cancel buttons
    if(pauseOnAlert)
    {
        pauseOnAlert = false;
        UnityPause( false );
    }
    
    UnitySendMessage("iOSPlugin", "AlertViewButtonClicked", "0");
    /*
    if (buttonIndex == 0)
    {
        NSLog(@"ok");
    }
    else
    {
        NSLog(@"cancel");
    }
     */
}


-(UIInterfaceOrientation)currentOrientation
{
    return [[UIApplication sharedApplication] statusBarOrientation];
}

@end


// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
extern "C" {
    
    void _iOSPluginInitialize()
	{
        [iOSPlugin sharedManager];
    }
    
    void _iOSPluginShowLoadingView()
	{
        [[iOSPlugin sharedManager] ShowLoadingView];
    }
    void _iOSPluginHideLoadingView()
	{
        [[iOSPlugin sharedManager] HideLoadingView];
    }
    void _iOSPluginshowAlertView(const char * title,const char*message,const char* buttonTitle,bool pauseUnity)
    {
        [[iOSPlugin sharedManager] showAlertWithTitle:GetStringParam(title) Message:GetStringParam(message) ButtonTitle:GetStringParam(buttonTitle) PauseUnity:pauseUnity];
    }
    
}

