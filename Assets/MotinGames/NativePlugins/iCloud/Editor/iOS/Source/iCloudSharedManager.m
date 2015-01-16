//
//  iCloudSharedManager.m
//  Unity-iPhone
//
//  Created by Dragon De Ojos Rojos on 1/30/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "iCloudSharedManager.h"
#import "UnityAppController.h"
@implementation iCloudSharedManager

+ (iCloudSharedManager*)sharedManager
{
	static iCloudSharedManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[iCloudSharedManager alloc] init];
	
	return sharedSingleton;
}

- (id)init
{
	if( ( self = [super init] ) )
	{
        /*
        if([AppController SystemVersion] >= __IPHONE_5_0)
        {
            isAvaiable = true;
            NSFileManager *fileManager = [NSFileManager defaultManager];
            // Team-ID + Bundle Identifier
            NSURL *iCloudURL = [fileManager URLForUbiquityContainerIdentifier:@"VQ83536EV8.com.motingames.beasthunter"];
            if(iCloudURL)
            {
                NSLog(@"%@", [iCloudURL absoluteString]);
                cloudStore = [NSUbiquitousKeyValueStore defaultStore];
                [cloudStore setString:[iCloudURL absoluteString] forKey:@"iCloudURL"];
                [cloudStore synchronize]; // Important as it stores the values you set before on iCloud     
                isAvaiable = true;
            }
            else
            {
                isAvaiable = false;
            }
           
        }
        else
        {
         */
            isAvaiable = false;
       // }
         
    }
	return self;
}
-(void)Initialize
{
    
}
-(bool)isAvaiable
{
    return isAvaiable; 
}
-(NSUbiquitousKeyValueStore*)GetCloudStore
{
    if(isAvaiable)
        return  cloudStore;
    else
        return nil;
}

-(bool)ContainsKey:(NSString*)key
{
    if(isAvaiable)
    {
        if([[cloudStore dictionaryRepresentation] valueForKey:key]!=nil )
        {
            return true;
        }
        
    }
    return false;
}



@end
