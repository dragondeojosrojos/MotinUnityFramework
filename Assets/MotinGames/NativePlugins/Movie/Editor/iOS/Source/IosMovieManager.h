//
//  IosMovieManager.h
//  GaturroDancer
//
//  Created by Dragon De Ojos Rojos on 10/4/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <MediaPlayer/MediaPlayer.h>

#import "MovieView.h"
@interface IosMovieManager : NSObject
{
    MovieView*      movieView_;
    MPMoviePlayerController* moviePlayer_;

}

+ (IosMovieManager*)sharedManager;

//-(void)setMovieDelegate:(MovieDelegate*)newDelegate;
-(void)playMovie:(NSString*)movieFile;
-(void)play;
-(void)stopMovie;
-(bool)getDeviceMusicIsPlaying;
- (void) moviePlayBackDidFinish:(NSNotification*)notification;
@end
