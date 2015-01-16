//
//  TwitterPlugin.m
//  GaturroDancer
//
//  Created by Dragon De Ojos Rojos on 10/31/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "TwitterPlugin.h"
#import <Twitter/Twitter.h>
#import <Accounts/Accounts.h>


@implementation TwitterPlugin


+ (TwitterPlugin*)sharedManager
{
	static TwitterPlugin *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[TwitterPlugin alloc] init];
	
	return sharedSingleton;
}

- (id)init
{
    NSLog(@"Init Twitter Plugin");
    twitterViewController = nil;
    
	self = [super init];
	return self;
} 

-(void)showTwitter:(NSString*) message
{
    UIWindow *window = [[[UIApplication sharedApplication] windows] objectAtIndex:0];
    twitterViewController = [[UIViewController alloc] init];
    [window addSubview:twitterViewController.view];

    TWTweetComposeViewController *tweetViewController = [[TWTweetComposeViewController alloc] init];
    
    // Set the initial tweet text. See the framework for additional properties that can be set.
    [tweetViewController setInitialText:message];
    
    // Create the completion handler block.
    [tweetViewController setCompletionHandler:^(TWTweetComposeViewControllerResult result) {
        NSString *output;
        
        switch (result) {
            case TWTweetComposeViewControllerResultCancelled:
                // The cancel button was tapped.
                output = @"Tweet cancelled.";
                break;
            case TWTweetComposeViewControllerResultDone:
                // The tweet was sent.
                output = @"Tweet done.";
                break;
            default:
                break;
        }
        NSLog(@"TWITTER RESULT %@",output);
        //[self performSelectorOnMainThread:@selector(displayText:) withObject:output waitUntilDone:NO];
        
        // Dismiss the tweet composition view controller.
        [twitterViewController dismissModalViewControllerAnimated:YES];
       
        //[[[[UIApplication sharedApplication] windows] objectAtIndex:0] removeLastObject] ;
        [twitterViewController.view removeFromSuperview];
        [twitterViewController release];
        twitterViewController = nil;
        
        }];
    
    // Present the tweet composition view controller modally.
    [twitterViewController presentModalViewController:tweetViewController animated:YES]; 
}

@end
