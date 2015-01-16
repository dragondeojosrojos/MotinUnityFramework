//
//  TwitterBinding.m
//  Unity-iPhone
//
//  Created by Dragon De Ojos Rojos on 11/4/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//


#import "TwitterPlugin.h"
/*
#import "SHK.h"
#import "SHKTwitter.h"
*/

#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

/*
void ShowTwitter(const char* defaultMessage)
{
    NSLog(@"Twitt");
    NSString *text = GetStringParam(defaultMessage);
	    
	
	SHKItem *item = [SHKItem text:text];
	//SHKActionSheet *actionSheet = [SHKActionSheet actionSheetForItem:item];
    [SHKTwitter shareItem:item];
	//[actionSheet showFromToolbar:self.navigationController.toolbar];}
}
*/


extern "C" {
    
    void ShowTwitter(const char* defaultMessage)
    {
    [[TwitterPlugin sharedManager] showTwitter:[NSString stringWithCString:defaultMessage encoding:NSUTF8StringEncoding] ];
 
        
    }
}
