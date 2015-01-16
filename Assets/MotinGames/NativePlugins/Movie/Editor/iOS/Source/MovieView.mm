//
//  MovieView.m
//  Unity-iPhone
//
//  Created by Juan Pablo Chandias on 10/26/13.
//
//

#import "MovieView.h"

@interface MovieView ()

@end

@implementation MovieView

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (void)dealloc {
    [_webView release];
    [super dealloc];
}
- (void)viewDidUnload {
    [self setWebView:nil];
    [super viewDidUnload];
}
- (IBAction)Play:(id)sender {
    NSURL *url = [NSURL URLWithString:@"http://www.youtube.com/watch?v=ZmayLwuubzY"];
    
    NSURLRequest *request_ = [NSURLRequest requestWithURL:url];
    [_webView loadRequest:request_];
}
@end
