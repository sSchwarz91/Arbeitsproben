package model;

import java.util.HashMap;

/**
 * Datenbank der registrierten Benutzer.
 * Nur zum Testen.
 * @author sSchwarz
 *
 */

public class PersonenDatenbank {
	
	/**
	 * HashMap um Personen zu speichern.
	 * Key ist Loginname und Wert das Passwort.
	 */
	HashMap<String,String> personen = new HashMap<String, String>();

	public HashMap<String, String> getPersonen() {
		return personen;
	}

}
