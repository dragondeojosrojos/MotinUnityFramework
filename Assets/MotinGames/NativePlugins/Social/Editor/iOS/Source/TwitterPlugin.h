//
//  TwitterPlugin.h
//  GaturroDancer
//
//  Created by Dragon De Ojos Rojos on 10/31/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface TwitterPlugin : NSObject
{
    UIViewController* twitterViewController;
}

+ (TwitterPlugin*)sharedManager;

-(void)showTwitter:(NSString*) message;

@end
