package com.motingames.device;

import com.google.android.gms.auth.GoogleAuthUtil;
import com.google.android.gms.common.AccountPicker;

import android.accounts.AccountManager;
import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;

public class AccountPickerActivity extends Activity {

	static final int PICK_ACCOUNT_REQUEST=1;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		Intent googlePicker =AccountPicker.newChooseAccountIntent(null,null,
		           new String[]{GoogleAuthUtil.GOOGLE_ACCOUNT_TYPE},false,"ÁSelecciona una cuenta para desbloquear todos los juegos!",null,null,null) ;
		    startActivityForResult(googlePicker,PICK_ACCOUNT_REQUEST);
	}
	
 
	@Override
	protected void onActivityResult(final int requestCode,final int resultCode, final Intent data) 
	{
	  if (requestCode == PICK_ACCOUNT_REQUEST && resultCode == RESULT_OK) {
	      String accountName = data.getStringExtra(AccountManager.KEY_ACCOUNT_NAME);
	      DeviceManager.sharedManager().onAccountPickerResult(accountName);
	     
	    }
	  else  
	  {
		  DeviceManager.sharedManager().onAccountPickerResult("");
	  }
	  
	  this.finish();
	}
	
}
