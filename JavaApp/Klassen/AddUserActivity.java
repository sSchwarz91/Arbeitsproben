package controller;

import handlers.ActionbarHandler;
import handlers.DatabaseHandler;
import model.Newsentry;
import model.User;
import android.app.Activity;
import android.app.AlertDialog;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.EditText;
import de.racoon.R;

/**
 * @author sSchwarz
 */

public class AddUserActivity extends Activity
{
	// Mindestens 3 Zeichen, nur Klein-, Großbuchstaben und Zahlen,
	// keine Zahl an erster Stelle
	private static final String REGEXP_USERNAME = "^[^(0-9)][A-Za-z0-9]{2,}$";

	// Mindestens 5 Zeichen, nur Klein-, Gro�buchstaben, Zahlen, Sonderzeichen
	// "@, #, $, %, _,!, -"
	private static final String REGEXP_PASSWORD = "^[a-zA-Z0-9@#$%_!-]{5,}$";

	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_add_user);
	}

	public void onResume()
	{
		super.onResume();
		ActionbarHandler.resumeActivity(this);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu)
	{
		ActionbarHandler.setupActionBar(this, true, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item)
	{
		ActionbarHandler.handleActionbarClick(item, this);
		return super.onOptionsItemSelected(item);
	}

	public void doAddUser(View view)
	{
		String userName = ((EditText) findViewById(R.id.edtAddUserUsername)).getText().toString();
		String userPassword = ((EditText) findViewById(R.id.edtAddUserPassword)).getText().toString();

		User user = new User(userName, userPassword);

		DatabaseHandler databaseHandler = new DatabaseHandler(this);

		// ACHTUNG!! Wegen encode/decode Funktionen NUR ASCII 32 bis 126,
		// (kann in der SecurityUtils.java verändert werden)!

		if (!userName.matches(REGEXP_USERNAME))
		{
			new AlertDialog.Builder(this).setMessage("Keinen gültigen Benutzernamen eingegeben!").setNeutralButton("OK", null).show();
		}
		else if (!userPassword.matches(REGEXP_PASSWORD))
		{
			new AlertDialog.Builder(this).setMessage("Kein (gültiges) Passwort eingegeben!").setNeutralButton("OK", null).show();
		}
		else
		{
			boolean alreadyExists = (databaseHandler.getUserByName(userName) != null);

			if (alreadyExists)
			{
				new AlertDialog.Builder(this).setMessage("Dieser Benutzername existiert bereits.").setNeutralButton("OK", null).show();
			}
			else
			{
				boolean addSuccess = databaseHandler.addUser(user);

				if (addSuccess)
				{
					NewsManager newsManager = new NewsManager(getApplicationContext());
					newsManager.addEntry(new Newsentry("Neuer Benutzer","Benutzer " + user.getUsername() + " wurde hinzugefügt.", newsManager.NEWS_PRIORITY_LOW));

					new AlertDialog.Builder(this).setMessage("Erfolgreich angelegt!").setNeutralButton("OK", null).show();
				}
				else
				{
					new AlertDialog.Builder(this).setMessage("Es ist ein interner Fehler aufgetreten.").setNeutralButton("OK", null).show();
				}
			}

		}

	}

}
