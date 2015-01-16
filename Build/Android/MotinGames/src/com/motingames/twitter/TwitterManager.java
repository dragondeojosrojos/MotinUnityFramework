/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org

http://www.cocos2d-x.org

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
package com.motingames.twitter;


import android.app.AlertDialog;
import android.content.DialogInterface;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.EditText;


























import com.loopj.android.http.AsyncHttpClient;
import com.loopj.android.http.JsonHttpResponseHandler;
import com.loopj.android.http.RequestParams;
import com.motingames.device.DeviceManager;
import com.motingames.twitter.R;
import com.sugree.twitter.DialogError;
import com.sugree.twitter.TwDialog;
import com.sugree.twitter.Twitter;
import com.sugree.twitter.TwitterError;
























import oauth.signpost.commonshttp.CommonsHttpOAuthConsumer;
import oauth.signpost.commonshttp.CommonsHttpOAuthProvider;
import oauth.signpost.exception.OAuthCommunicationException;
import oauth.signpost.exception.OAuthExpectationFailedException;
import oauth.signpost.exception.OAuthMessageSignerException;
import oauth.signpost.exception.OAuthNotAuthorizedException;

import org.apache.http.Header;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.NameValuePair;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import twitter4j.Status;
import twitter4j.TwitterException;
import twitter4j.TwitterFactory;
import twitter4j.conf.ConfigurationBuilder;

import java.text.MessageFormat;
import java.util.ArrayList;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import com.unity3d.player.UnityPlayer;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;


public class TwitterManager 
{
	static final String DEBUG_TAG = "TwitterManager";
	private static TwitterManager sharedInstance = null;
	public static TwitterManager sharedManager()
	{
		if(sharedInstance==null)
		{
			sharedInstance = new TwitterManager();
		}
		return sharedInstance;
	}
	
	public boolean isInitialized = false;
	
	private static AlertDialog mTweetEditDialog;  
	public static View  mTweetEditView;
	public AlertDialog.Builder mAlertDialog;
   
	//public static final String TWITTER_OAUTH_REQUEST_TOKEN_ENDPOINT = "http://twitter.com/oauth/request_token";
	//public static final String TWITTER_OAUTH_ACCESS_TOKEN_ENDPOINT = "http://twitter.com/oauth/access_token";
	//public static final String TWITTER_OAUTH_AUTHORIZE_ENDPOINT = "http://twitter.com/oauth/authorize";
	String twitter_oauth_consumer_key;
	String twitter_oauth_consumer_secret;
	String gameObjectName = null;
	
	
	public String twitter_access_token = null;
	public String twitter_secret_token = null;
	
	public static final String TWITTER_OAUTH_REQUEST_TOKEN_ENDPOINT = "https://api.twitter.com/oauth/request_token";
	public static final String TWITTER_OAUTH_ACCESS_TOKEN_ENDPOINT = "https://api.twitter.com/oauth/access_token";
	public static final String TWITTER_OAUTH_AUTHORIZE_ENDPOINT = "https://api.twitter.com/oauth/authorize";
	
	
	// public CommonsHttpOAuthProvider commonsHttpOAuthProvider;
	//public CommonsHttpOAuthConsumer commonsHttpOAuthConsumer;
	
	
	public CommonsHttpOAuthProvider commonsHttpOAuthProvider;
	public CommonsHttpOAuthConsumer commonsHttpOAuthConsumer;
	public static String tweetMessage;
	public static TwDialog twitterDialog=null;
	

	//From Unity
	public static void Initialize(String goName,String publicKey,String secretKey)
	{
		TwitterManager.sharedManager().Init(goName, publicKey, secretKey);
	}
	public static void Show(String message)
	{
		TwitterManager.sharedManager().show(message);
	}
	//Unity callbacks
	public static final String UNITY_ON_TWITTER_COMPLETED = "OnTwitterCompleted";
	public void OnTwitterCompleted()
	{
			//if(accountPicker==null)
			//	Log.e(DEBUG_TAG, "Account Picker is Null");
			
			//accountPicker.FinishActivity();
		UnityPlayer.UnitySendMessage(gameObjectName, UNITY_ON_TWITTER_COMPLETED,"");
	}
	
	public void Init(String goName,java.lang.String publicKey,java.lang.String secretKey)
	{
		Log.d(DEBUG_TAG,"TwitterManager.Init " + publicKey);
		
		if(isInitialized )
			return;

		gameObjectName = goName;
		//twitter.setOAuthConsumer("2ZtqcNpfyprzlEM5S7Q", "bTqRVCLZKEvLdpZk2ZgArJjKhNFoWpRKgJVJZeFuJdY");
		twitter_oauth_consumer_key = publicKey;
		twitter_oauth_consumer_secret = secretKey;
		//twitter_oauth_consumer_key = "pqpq4QiClTT5hSljIuBUng";//publicKey;
		//twitter_oauth_consumer_secret ="yBmo1e9BAkoCySbj5Ej1xVC5F2gbYGkL1jRExvbq0";// secretKey;
		
		twitter_access_token = null;
		twitter_secret_token = null;
		
		isInitialized = true;
	}
	
	public void show(String message )
	{
		if(!isInitialized ){
			Log.e(DEBUG_TAG, "Twitter not Initialized, Call TwitterManager.Init");
			return;
		}
		
		Log.d(DEBUG_TAG,"TwitterManager.Show");
		
        TwitterManager.mTweetEditDialog = null;
        TwitterManager.mTweetEditView = null;
        System.gc();
	
        TwitterManager.tweetMessage = message;
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
        	@Override
        	public void run() {
	

        		LayoutInflater factory = LayoutInflater.from(UnityPlayer.currentActivity);
        		TwitterManager.mTweetEditView = factory.inflate(R.layout.alert_dialog_tweet_edit, null);
	
        		((EditText) TwitterManager.mTweetEditView.findViewById(R.id.edit_tweet)).setText(TwitterManager.tweetMessage);
	
        		TwitterManager.mTweetEditDialog = new AlertDialog.Builder(UnityPlayer.currentActivity)
        		//.setIcon(R.drawable.icon)
        		.setTitle("Comparte!")
        		.setView(TwitterManager.mTweetEditView)
        		.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
        			public void onClick(DialogInterface dialog, int whichButton) {
	
        				System.out.println("TWITTER Dialog: Ok pressed");
        				TwitterManager.tweetMessage = ((EditText) TwitterManager.mTweetEditView.findViewById(R.id.edit_tweet)).getText().toString();
        				TwitterManager.mTweetEditDialog = null;
						if(TwitterManager.sharedManager().twitter_access_token!=null && TwitterManager.sharedManager().twitter_secret_token!=null)
		        		{ 
							System.out.println("TWITTER access token not Null tweet direct");
							new Thread() {
		                	@Override
		                	public void run() {
		                			new Tweeter(TwitterManager.sharedManager().twitter_access_token ,TwitterManager.sharedManager().twitter_secret_token).tweet(TwitterManager.tweetMessage);
		                		}
							}.start();
							return;
		        		}
						
						
        				UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
	
        					@Override
        					public void run() {
        						
        						
        						
        						TwitterManager.sharedManager().commonsHttpOAuthProvider = new CommonsHttpOAuthProvider(TWITTER_OAUTH_REQUEST_TOKEN_ENDPOINT,
        								TWITTER_OAUTH_ACCESS_TOKEN_ENDPOINT, TWITTER_OAUTH_AUTHORIZE_ENDPOINT);
        						TwitterManager.sharedManager().commonsHttpOAuthConsumer = new CommonsHttpOAuthConsumer(TwitterManager.sharedManager().twitter_oauth_consumer_key,
        								TwitterManager.sharedManager().twitter_oauth_consumer_secret);
        						TwitterManager.sharedManager().commonsHttpOAuthProvider.setOAuth10a(true);
        						TwitterManager.twitterDialog = new TwDialog(UnityPlayer.currentActivity, TwitterManager.sharedManager().commonsHttpOAuthProvider, TwitterManager.sharedManager().commonsHttpOAuthConsumer,
        								TwitterManager.sharedManager().dialogListener, R.drawable.android);
	
        						TwitterManager.twitterDialog.show();
	
        					}
	
        				}
        				);

        			}
        		}
        		)
        		.setNegativeButton("Cancelar", new DialogInterface.OnClickListener() {
        			public void onClick(DialogInterface dialog, int whichButton) {
	
        				System.out.println("TWITTER Dialog: Cancel pressed");
        				TwitterManager.mTweetEditDialog = null;
        				TwitterManager.sharedManager().OnTwitterCompleted();
        			}
        		})
        		.create();

        		TwitterManager.mTweetEditDialog.show();
	
        	}
        });
	
		}

	
	
	public Twitter.DialogListener dialogListener = new Twitter.DialogListener() {

		public void onComplete(Bundle values) {
			
			//if(TwitterManager.sharedManager().twitter_access_token==null && TwitterManager.sharedManager().twitter_secret_token==null)
			//{
				TwitterManager.sharedManager().twitter_secret_token = values.getString(Twitter.SECRET_TOKEN);
				//Log.i(TAG,"secret_token=" + secretToken);
				TwitterManager.sharedManager().twitter_access_token  = values.getString(Twitter.ACCESS_TOKEN);
			//}
				TwitterManager.twitterDialog.dismiss();	
			//System.out.println("TWITTER access token " + TwitterManager.sharedManager().twitter_secret_token );
			//System.out.println("TWITTER CONNECTED POST MESSAGE");
			new Tweeter(TwitterManager.sharedManager().twitter_access_token ,TwitterManager.sharedManager().twitter_secret_token).tweet(TwitterManager.tweetMessage);
			
				
			/*
			TwitterRestClient.setOauth(TwitterManager.sharedManager().twitter_access_token, TwitterManager.sharedManager().twitter_secret_token);
			TwitterRestClient.post("statuses/update.json", new RequestParams("status",TwitterManager.tweetMessage), new JsonHttpResponseHandler() {

			     @Override
			     public void onSuccess(int statusCode, Header[] headers, byte[] responseBody) {
			    	 System.out.println("TWITTER onSuccess " + statusCode );
			     }

			     @Override
			     public void onFailure(int statusCode, Header[] headers, byte[] responseBody, Throwable error)
			     {
			    	 System.out.println("TWITTER onFailure " + statusCode );
			     }
	        });
	        */
		}
	
		public void onTwitterError(TwitterError e) { 
			System.out.println("onTwitterError called for TwitterDialog " + e.getMessage()); 
	
			if(TwitterManager.twitterDialog.mSpinner.isShowing())
			{
				TwitterManager.twitterDialog.mSpinner.dismiss();
			}
			TwitterManager.twitterDialog.dismiss(); 
			TwitterManager.twitterDialog = null;
			 TwitterManager.sharedManager().OnTwitterCompleted();
			//TwitterManager.showAlert("Twitter error", e.getMessage());
		}
	
		public void onError(DialogError e) {
			System.out.println("onError called for TwitterDialog "+e.getMessage());
			if(TwitterManager.twitterDialog.mSpinner.isShowing())
			{
				TwitterManager.twitterDialog.mSpinner.dismiss();
			}
			TwitterManager.twitterDialog.dismiss();
			TwitterManager.twitterDialog = null;
			 TwitterManager.sharedManager().OnTwitterCompleted();
			//TwitterManager.showAlert("Twitter error", e.getMessage());
		}
	
	
		public void onCancel() {
			 TwitterManager.sharedManager().OnTwitterCompleted();
			//Log.e(TAG,"onCancel"); 
		}
	};
	
	
	
	
	
	
	
	public static final Pattern ID_PATTERN = Pattern.compile(".*?\"id_str\":\"(\\d*)\".*");
	public static final Pattern SCREEN_NAME_PATTERN = Pattern.compile(".*?\"screen_name\":\"([^\"]*).*");
	
	public class Tweeter {
		protected CommonsHttpOAuthConsumer oAuthConsumer;
	
		public Tweeter(String accessToken, String secretToken) {
			oAuthConsumer = new CommonsHttpOAuthConsumer(TwitterManager.sharedManager().twitter_oauth_consumer_key,
					TwitterManager.sharedManager().twitter_oauth_consumer_secret);
			oAuthConsumer.setTokenWithSecret(accessToken, secretToken);
		}
	
		public boolean tweet(String message) {
			if (message == null && message.length() > 140) {
				throw new IllegalArgumentException("message cannot be null and must be less than 140 chars");
			}
			
			try {
	            ConfigurationBuilder confbuilder  = new ConfigurationBuilder(); 
	            confbuilder.setOAuthAccessToken(TwitterManager.sharedManager().twitter_access_token) 
	            .setOAuthAccessTokenSecret(TwitterManager.sharedManager().twitter_secret_token) 
	            .setOAuthConsumerKey(TwitterManager.sharedManager().twitter_oauth_consumer_key) 
	            .setOAuthConsumerSecret(TwitterManager.sharedManager().twitter_oauth_consumer_secret); 
	            twitter4j.Twitter twitter = (twitter4j.Twitter) new TwitterFactory(confbuilder.build()).getInstance(); 
	            
	            Status status = twitter.updateStatus(message);
	            //status.
	            //System.out.println("Successfully updated the status to [" + status.getText() + "].");
	      
	            DeviceManager.sharedManager().showNotification("Tweet ok");
	            TwitterManager.sharedManager().OnTwitterCompleted();
	            return true;

	        } catch (TwitterException e) {
	            e.printStackTrace();
	            if(e.getErrorCode()==403)
	            {
	            	 DeviceManager.sharedManager().showNotification("Tweet duplicado");
	            }
	            TwitterManager.sharedManager().OnTwitterCompleted();
	            return false;
	        }
			
			
	// create a request that requires authentication
			//try {
				
				/*
				Uri.Builder builder = new Uri.Builder();
				builder.appendPath("statuses").appendPath("update.json")
				.appendQueryParameter("status", message);
				Uri man = builder.build();
	*/
				//Log.i(DEBUG_TAG,"TWITTER POST : " + "https://twitter.com/1.1" + man.toString());
				//https://api.twitter.com/1.1/statuses/update.json
				
				//URL url = new URL("https://api.twitter.com/1.1/statuses/update.json");
				//HttpURLConnection request = (HttpURLConnection) url.openConnection();
				//request.
				/*
			    HttpClient httpClient = new DefaultHttpClient();
				HttpPost post = new HttpPost("https://api.twitter.com/1.1/statuses/update.json");
				
				List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(1);
			        nameValuePairs.add(new BasicNameValuePair("status",message));
			       
			        post.setEntity(new UrlEncodedFormEntity(nameValuePairs));
			       */ 
				
				//post.
				//oAuthConsumer.sign(post);
				//Log.i(DEBUG_TAG,"POST SIGNATURE : " +  post.);
				
				//Header[] headers =  post.getAllHeaders();
				//for(int i = 0 ; i < headers.length;i++)
				//{
				//	Log.i(DEBUG_TAG,"header["+i+"] " + headers[i].getName() + " " + headers[i].getValue());
				//}
				
				//HttpResponse resp = httpClient.execute(post);
				
				
				//String jsonResponseStr = convertStreamToString(resp.getEntity().getContent());
				//Log.i(DEBUG_TAG,"response: " + jsonResponseStr);
				//String id = getFirstMatch(ID_PATTERN,jsonResponseStr);
				//Log.i(DEBUG_TAG,"id: " + id);
				//String screenName = getFirstMatch(SCREEN_NAME_PATTERN,jsonResponseStr);
				//Log.i(DEBUG_TAG,"screen name: " + screenName);
	
				//final String url = MessageFormat.format("https://api.twitter.com/#!/{0}/status/{1}",screenName,id);
				//Log.i(DEBUG_TAG,"url: " + url);
	
			
	
				//int response = resp.getStatusLine().getStatusCode() ;
				//int response = resp.getStatusLine().getStatusCode() ;
				//if(response == 200)
				//	DeviceManager.sharedManager().showNotification("Tweet ok");
				
				
				//System.out.println("TWITTER response " + response + "  " +   resp.getStatusLine().getReasonPhrase());
				//return response == 200;
	
			//} catch (Exception e) {
			//	Log.e(DEBUG_TAG,"trying to tweet: " + message, e);
			//	return false;
			//}
			
		}
	}//END TWEETER
	
	public static String convertStreamToString(java.io.InputStream is) {
		try {
				return new java.util.Scanner(is).useDelimiter("\\A").next();
		} catch (java.util.NoSuchElementException e) {
			return "";
		}
	}
	
	public static String getFirstMatch(Pattern pattern, String str){
		Matcher matcher = pattern.matcher(str);
		if(matcher.matches()){
			return matcher.group(1);
		}
		return null;
	}


    
}


