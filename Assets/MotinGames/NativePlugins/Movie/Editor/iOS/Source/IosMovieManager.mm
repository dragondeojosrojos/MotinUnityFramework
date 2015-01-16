//
//  IosMovieManager.m
//  GaturroDancer
//
//  Created by Dragon De Ojos Rojos on 10/4/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "IosMovieManager.h"
#import "iOSPlugin.h"


// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

@implementation IosMovieManager

+ (IosMovieManager*)sharedManager
{
	static IosMovieManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[IosMovieManager alloc] init];
	
	return sharedSingleton;
}

- (id)init
{
    
    NSLog(@"Init Movie Plugin");
    moviePlayer_ = nil;
    movieView_ =nil;
    //movieDelegate = NULL;
	self = [super init];
    
	return self;
} 

- (void)dealloc
{
	[super dealloc];	
}
/*
-(void)setMovieDelegate:(MovieDelegate*)newDelegate
{
    movieDelegate = newDelegate;
}*/

-(bool)getDeviceMusicIsPlaying
{
    if ([[MPMusicPlayerController iPodMusicPlayer] playbackState] == MPMusicPlaybackStatePlaying)
    {
        return true;	
    }
    return false;
}

-(void)play
{
    if(moviePlayer_!=nil)
    {
        [moviePlayer_ play];
    }
}
-(void)playMovie:(NSString*)movieFile
{

    NSLog(@"MOVIE  PLAY %@",movieFile);
    //NSLog(@"GET URL %@",[[NSBundle mainBundle] pathForResource:movieFile ofType:@""]);
    //NSURL *url = [NSURL fileURLWithPath:[[NSBundle mainBundle] pathForResource:movieFile  ofType:@""]];
    //@"http://www.ebookfrenzy.com/ios_book/movie/movie.mov"
    
    //NSLog(@"GETED URL");
    if(movieView_==nil)
    {
        movieView_ = [[MovieView alloc] init];
    }
    /*
    NSURL *url = [NSURL URLWithString:@"http://www.youtube.com/watch?v=ZmayLwuubzY"];
    NSURLRequest *request_ = [NSURLRequest requestWithURL:url];
    [movieView_.webView loadRequest:request_];
*/
    bool landscapeLeft = [[iOSPlugin sharedManager] currentOrientation] == UIDeviceOrientationLandscapeLeft ;
    
    [movieView_.view setTransform:CGAffineTransformMakeRotation((float)(landscapeLeft? M_PI_2 : -M_PI_2))];
   //
    CGRect screenRect = CGRectMake(50,0, 480, 320); //[[UIScreen mainScreen] bounds];
    movieView_.view.frame = screenRect;

    
    [[[[[UIApplication sharedApplication] keyWindow] rootViewController] view] addSubview:movieView_.view];

    
    NSLog(@"Movie END PLAY ");
}


-(void)playMovieFromWeb:(NSString*)movieFile
{
    
    NSLog(@"MOVIE  PLAY %@",movieFile);
    //NSLog(@"GET URL %@",[[NSBundle mainBundle] pathForResource:movieFile ofType:@""]);
    //NSURL *url = [NSURL fileURLWithPath:[[NSBundle mainBundle] pathForResource:movieFile  ofType:@""]];
    //@"http://www.ebookfrenzy.com/ios_book/movie/movie.mov"
    NSURL *url = [NSURL URLWithString:@"http://www.youtube.com/watch?v=ZmayLwuubzY"];
    //NSLog(@"GETED URL");
    if(moviePlayer_==nil)
    {
        moviePlayer_ = [[MPMoviePlayerController alloc] initWithContentURL:url];
    }
    
    moviePlayer_.view.userInteractionEnabled = NO;
    
    
    // Register to receive a notification when the movie has finished playing.
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(moviePlayBackDidFinish:)
                                                 name:MPMoviePlayerPlaybackDidFinishNotification
                                               object:moviePlayer_];
    
    if ([moviePlayer_ respondsToSelector:@selector(setFullscreen:animated:)])
    {
        // Use the new 3.2 style API
        
        //moviePlayer_.controlStyle = MPMovieControlStyleNone;
        // moviePlayer_.shouldAutoplay = YES;
        
        // This fails in cocos2d, so we'll resize manually
        // [moviePlayer setFullscreen:YES animated:YES];
        // bool landscapeLeft = [[iOSPlugin sharedManager] currentOrientation] == UIDeviceOrientationLandscapeLeft ;
        
        // [moviePlayer_.view setTransform:CGAffineTransformMakeRotation((float)(landscapeLeft? M_PI_2 : -M_PI_2))];
        
        CGRect screenRect = CGRectMake(0, 0, 480, 320); //[[UIScreen mainScreen] bounds];
        
        NSLog(@"SCREEN RECT %f %f %f %f", screenRect.origin.x,screenRect.origin.y,screenRect.size.width,screenRect.size.height);
        
        moviePlayer_.view.frame = screenRect;
        
        
        // Add the view - Use these three lines for Cocos 2D X
        //UIWindow* window = [[UIApplication sharedApplication] keyWindow];
        //[window addSubview: moviePlayer_.view];
        //[window makeKeyAndVisible];
        
        
        [[[[[UIApplication sharedApplication] keyWindow] rootViewController] view] addSubview:moviePlayer_.view];
        //[[[CCDirector sharedDirector] openGLView] addSubview:moviePlayer_.view];
    }
    else
    {
        // Use the old 2.0 style API
        moviePlayer_.movieControlMode = MPMovieControlModeHidden;
        
    }
	
	[moviePlayer_ play];
	
    NSLog(@"Movie END PLAY ");
}

-(void)stopMovie
{
    NSLog(@"Movie STOP PLAY ");
    
	[[NSNotificationCenter defaultCenter] removeObserver:self  
                                                    name:MPMoviePlayerPlaybackDidFinishNotification  
                                                  object:moviePlayer_];  
    
    [moviePlayer_ stop];
    
    if ([moviePlayer_ respondsToSelector:@selector(setFullscreen:animated:)]) 
    {  
        [moviePlayer_.view removeFromSuperview];  
    }  
    
    [moviePlayer_ release]; 
    moviePlayer_ = nil;
     NSLog(@"Movie END STOP PLAY ");
    
}
- (void) moviePlayBackDidFinish:(NSNotification*)notification 
{  
    
    NSLog(@"Movie playback DID FINISH");
    MPMoviePlayerController *moviePlayer = [notification object];  
    
    if (moviePlayer_ && moviePlayer == moviePlayer_) 
    {
       // if(movieDelegate!=NULL)
       // {
       //     movieDelegate->MovieEnded();
       // }
    }
} 
@end



extern "C" {
    /*
    void Movie_SetDelegate(MovieDelegate* newDelegate)
    {
        [[IosMovieManager sharedManager] setMovieDelegate:newDelegate];
    }
     */
    void Movie_PlayMovie(const char* movieFile)
    {
        NSLog(@"MOVIE PLAY  ");
        [[IosMovieManager sharedManager] playMovie:GetStringParam(movieFile)];
    }
    void Movie_Play()
    {
        [[IosMovieManager sharedManager] play];
    }
    void Movie_Stop()
    {
        [[IosMovieManager sharedManager] stopMovie];
    }
    
    /*
    bool Movie_GetDeviceMusicIsPlaying()
    {
        return [[IosMovieManager sharedManager] getDeviceMusicIsPlaying];
    }
    */
   
    
}
