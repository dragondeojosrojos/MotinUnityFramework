//
//  iCloudSharedManager.h
//  Unity-iPhone
//
//  Created by Dragon De Ojos Rojos on 1/30/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface iCloudSharedManager : NSObject
{
    bool isAvaiable;
    NSUbiquitousKeyValueStore *cloudStore;
}


+ (iCloudSharedManager*)sharedManager;
-(bool)ContainsKey:(NSString*)key;
-(void)Initialize;
-(bool)isAvaiable;
-(NSUbiquitousKeyValueStore*)GetCloudStore;

@end
