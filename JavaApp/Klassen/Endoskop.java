/**
 * Klasse um Endoskope als Objekt abzubilden.
 * 
 * @author sSchwarz
 */
package model;

import java.util.Date;

public class Endoskop
{

	private String custom_id;
	private int id;
	private String hersteller;
	private String typ;
	private String lagerungsort;
	private Date letzteWaesche;
	private boolean checked = false;
	private boolean inReinigung = false;

	/**
	 * Konstruktor
	 * 
	 * @param id
	 *            ID des Gerätes.
	 * @param hersteller
	 *            Hersteller des Gerätes.
	 * @param typ
	 *            Typ des Ger�ts (flexibel,starr).
	 * @param lagerungsort
	 *            Lagerungsort des Gerätes.
	 * @param letzteWaesche
	 *            Datum der letzten Wäsche des Gerätes.
	 */
	public Endoskop(String custom_id, String hersteller, String typ, String lagerungsort, Date letzteWaesche)
	{
		this.custom_id = custom_id;
		this.hersteller = hersteller;
		this.typ = typ;
		this.lagerungsort = lagerungsort;
		this.letzteWaesche = letzteWaesche;
	}

	public Endoskop(String custom_id, String typ, String manufacturer)
	{

		this.id = -1;
		this.custom_id = custom_id;
		this.typ = typ;
		this.hersteller = manufacturer;
		this.lagerungsort = "";
		this.letzteWaesche = null;
	}

	public void setId(int id)
	{
		this.id = id;
	}

	public String getEndoId()
	{
		return custom_id;
	}

	public int getId()
	{
		return id;
	}

	public void setEndoId(String custom_id)
	{
		this.custom_id = custom_id;
	}

	public String getHersteller()
	{
		return hersteller;
	}

	public String getTyp()
	{
		return typ;
	}

	public String getLagerungsort()
	{
		return lagerungsort;
	}

	public Date getLetzteWaesche()
	{
		return letzteWaesche;
	}

	public void toggleChecked()
	{
		checked = !checked;
	}

	public boolean isChecked()
	{
		return checked;
	}

	public void setChecked(boolean checked)
	{
		this.checked = checked;
	}

	public void setInReinigung(boolean inReinigung)
	{
		this.inReinigung = inReinigung;
	}

	public boolean getInReinigung()
	{
		return this.inReinigung;
	}
}
