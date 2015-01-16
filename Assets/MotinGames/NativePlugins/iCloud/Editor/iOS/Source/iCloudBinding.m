//
//  FacebookBinding.m
//  Unity-iPhone
//
//  Created by Dragon De Ojos Rojos on 11/1/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//


#import "iCloudSharedManager.h"

// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]


void iCloudInitialize()
{
    [[iCloudSharedManager sharedManager] Initialize];
}
bool iCloudIsAvaiable()
{
    return [[iCloudSharedManager sharedManager] isAvaiable];
}
bool iCloudContainsKey(const char *key)
{
    if([[iCloudSharedManager sharedManager] isAvaiable])
    {
        return [[iCloudSharedManager sharedManager] ContainsKey:GetStringParam(key)];
    }
    else
    {
        return false;
    }
}
const char * iCloudGetString(const char *key)
{
    NSLog(@"ICLOUD GET STRING ");
    if([[iCloudSharedManager sharedManager] isAvaiable])
    {
        return MakeStringCopy( [[[iCloudSharedManager sharedManager] GetCloudStore] stringForKey:GetStringParam(key)]);
        
    }
    else
    {
       return MakeStringCopy(@""); 
    }
            
}

void iCloudSetString(const char *value,const char *key)
{
    NSLog(@"ICLOUD SET STRING");
    if([[iCloudSharedManager sharedManager] isAvaiable])
    {
        [[[iCloudSharedManager sharedManager] GetCloudStore] setString:GetStringParam(value) forKey:GetStringParam(key)];
    }
    NSLog(@"ICLOUD END STRING");
}

int iCloudGetInt(const char *key)
{
    NSLog(@"ICLOUD GET INT");
    if([[iCloudSharedManager sharedManager] isAvaiable])
    {
        return [[[iCloudSharedManager sharedManager] GetCloudStore] longLongForKey:GetStringParam(key)];
    }
    else
    {
        return 0; 
    }
    
}

void iCloudSetInt(long value,const char *key)
{
    NSLog(@"ICLOUD SET INT");
    if([[iCloudSharedManager sharedManager] isAvaiable])
    {
        [[[iCloudSharedManager sharedManager] GetCloudStore] setLongLong:(long long)value forKey:GetStringParam(key)];
    }
    
}

float iCloudGetFloat(const char *key)
{
    NSLog(@"ICLOUD GET FLOAT");
    if([[iCloudSharedManager sharedManager] isAvaiable])
    {
        return [[[iCloudSharedManager sharedManager] GetCloudStore] doubleForKey:GetStringParam(key)];
    }
    else
    {
        return 0; 
    }
    
}

void iCloudSetFloat(float value,const char *key)
{
    NSLog(@"ICLOUD SET FLOAT");
    if([[iCloudSharedManager sharedManager] isAvaiable])
    {
        [[[iCloudSharedManager sharedManager] GetCloudStore] setDouble:(double)value forKey:GetStringParam(key)];
    }
    
}

void iCloudSynchronize()
{
    NSLog(@"ICLOUD SYNC");
    if([[iCloudSharedManager sharedManager] isAvaiable])
    {
        [[[iCloudSharedManager sharedManager] GetCloudStore] synchronize];
    }
}






/*
void FBPostMessage(const char *title,const char *caption,const char *description,const char *link,const char *url)
{
    //NSLog(@"POst");
    //[[FacebookManager sharedManager] showPublishDialogWithTitle:title Caption:caption Description:description Link:link Url:url];
}
*/