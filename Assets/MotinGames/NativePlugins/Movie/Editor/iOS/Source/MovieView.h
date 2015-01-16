//
//  MovieView.h
//  Unity-iPhone
//
//  Created by Juan Pablo Chandias on 10/26/13.
//
//

#import <UIKit/UIKit.h>

@interface MovieView : UIViewController
- (IBAction)Play:(id)sender;
@property (retain, nonatomic) IBOutlet UIWebView *webView;

@end
