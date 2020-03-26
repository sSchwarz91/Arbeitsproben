using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Values : MonoBehaviour {

	private float LetterDir;
	private float LetterSpeed;
	private float LetterPosX;
	private float LetterPosY;
	private float LetterPosZ;
	private float StartLetterPosX;
	private float StartLetterPosY;
	private float StartLetterPosZ;
	private string name;

	//Timers
	private float maxtimer = 5f;
	public float time = 0.0f;
	private float oldtime = 0.0f;
	private float timedifferenz = 0.0f;

	private float GewiSpeed = 0.2f;
	private float GewiPos = 0.8f;
	private float GewiDir = 0.0f;

	private float Gewi2Speed = 0.5f;
	private float Gewi2Pos = 0.5f;
	private float Gewi2Dir = 0.0f;

	//LetterValues for calculation


	public float LetterxPosFOV;
	public float LetteryPosFOV;
	public float LetterRealspeed;
	public float LetterDistance;
	private float LetterOldPosX;
	private float LetterOldPosY;
	private float LetterDirVecX;
	private float LetterDirVecY;
	public Vector2 RichtungsVecLetter;
    public Vector2 NorRichtungsVecLetter;
	public int MatchCounter = 0;
	public int MatchCounterGewichtet = 0;
	public int MatchCounterGewichtet2 = 0;
	public int MatchCounterAfterLayerSelection = 0;
	public int MatchCounterAfterLayerSelectionGewi2 = 0;
    public int MatchCounterWhole = 0;



    //SMI Values for Calculation
    private SMI.SMIEyeTrackingUnity smiInstance = null;
	public float EyeSpeed;
	public float EyeXPos;
	public float EyeYPos;
	public float EyeDistance;
	private float EyeOldPosX;
	private float EyeOldPosY;
	private float EyeDirVecX;
	private float EyeDirVecY;
	public Vector2 RichtungsVecEye;
    public Vector2 NorRichtungsVecEye;

    //Calculation for both
    public float NorDirAngelLetterToGazeVec;
	public float DirAngelLetterToGazeVec;
	public float DistanceEyeAndLetterPOR;
	public float SpeedDivision;
    public float angleCameraLetter;
    public float angleleftGazeDirection;
    public float angleRightGazeDirection;
    public float LetterDistanceCamera;
    public Vector3 targetDir;
    public Vector3 targetRightBaseDir;
    public Vector3 targetLeftBaseDir;
	public float MatchValueCalculation;
	public float MatchValueCalculationGewichtet;
	public float MatchValueCalculationGewichtet2;


    public bool timeron= false;


	public void setLetterDir (float direction){
		LetterDir = direction;
	}

	public void setLetterSpeed(float speed){
		LetterSpeed = speed;
	}

	public void setPosition (float posX, float posY, float posZ){
		LetterPosX = posX;
		LetterPosY = posY;
		LetterPosZ = posZ;
	}

	public void setmaxTime(float maxtime){
		maxtimer = maxtime;
	}

	public void setTime(float timer){
		time = timer;
	}


	// Use this for initialization
	void Start () {
		smiInstance = SMI.SMIEyeTrackingUnity.Instance;

       	name = this.gameObject.transform.name;

	//	Debug.Log ("My name is: " + name + "Position [x,y,z] :" + "[" + LetterPosX + "," + LetterPosY + "," + LetterPosZ + "]");
	//	Debug.Log ("My speed dn direction are: Speed: " + LetterSpeed + "Direction: " + LetterDir);

		StartLetterPosX = this.gameObject.transform.localPosition.x;
		StartLetterPosY = this.gameObject.transform.localPosition.y;
		StartLetterPosZ = this.gameObject.transform.localPosition.z;


	}
	
	// Update is called once per frame
	void Update () {
		//position of Letters relatet to its parents 
		LetterPosX = this.gameObject.transform.localPosition.x;
		LetterPosY = this.gameObject.transform.localPosition.y;
		LetterPosZ = this.gameObject.transform.localPosition.z;

        Transform cameraTransform = Camera.main.transform;

        ulong timeStamp = smiInstance.smi_GetTimeStamp();
		Vector3 leftGazeDirection = smiInstance.smi_GetLeftGazeDirection ();
		Vector3 rightGazeDirection = smiInstance.smi_GetRightGazeDirection ();
		Vector2 binocularPor = smiInstance.smi_GetBinocularPor ();
		Vector3 cameraRaycast = smiInstance.smi_GetCameraRaycast ();
		Vector2 leftPor = smiInstance.smi_GetLeftPor ();
		Vector2 rightPor = smiInstance.smi_GetRightPor ();
		Vector3 leftBasePoint = smiInstance.smi_GetLeftGazeBase ();
		Vector3 rightBasePoint = smiInstance.smi_GetRightGazeBase ();

		//Define real position of Letters related to the FOV of the Eyetracker 
		LetterxPosFOV = (1080 + this.transform.parent.transform.localPosition.x + LetterPosX);
		LetteryPosFOV = -(-600 + this.transform.parent.transform.localPosition.y + LetterPosY);

		 if (SMI.SMIEyeTrackingUnity.smi_IsValid (cameraRaycast)) {
			
        //Distance between Letter and Camera
        LetterDistanceCamera = Vector3.Distance(this.transform.position, cameraTransform.position);
        //Define the direction from Kamera to Letter
		targetDir = this.transform.position - cameraTransform.position;
		targetRightBaseDir = this.transform.position - cameraTransform.TransformPoint(rightBasePoint);
		targetLeftBaseDir = this.transform.position - cameraTransform.TransformPoint(leftBasePoint);
        //Calculate the Angle between the direction of Camera and Letter and the direction of the Raycast of the eyes
        angleCameraLetter = Vector3.Angle(cameraTransform.TransformDirection(cameraRaycast), targetDir);
       	angleleftGazeDirection = Vector3.Angle(cameraTransform.TransformDirection(leftGazeDirection), targetLeftBaseDir);
		angleRightGazeDirection = Vector3.Angle(cameraTransform.TransformDirection(rightGazeDirection), targetRightBaseDir);
	


		Vector2 angularTarget = new Vector2(Vector3.SignedAngle(Vector3.forward, new Vector3(this.transform.localPosition.x, 0, LetterDistanceCamera), Vector3.up),
			Vector3.SignedAngle(Vector3.forward, new Vector3(0, this.transform.localPosition.y, LetterDistanceCamera), Vector3.up));
		Vector2 angularResult = new Vector2( Vector3.SignedAngle(Vector3.forward, new Vector3(cameraRaycast.x, 0, cameraRaycast.z), Vector3.up),
			Vector3.SignedAngle(Vector3.forward, new Vector3(0, cameraRaycast.y, cameraRaycast.z), Vector3.up));

		float errX = angularResult.x - angularTarget.x;
		float errY = angularResult.y - angularTarget.y;
		float realAngle = Mathf.Sqrt(errX * errX + errY * errY);








			/*
			*
			*Einige Berechnungen wurden doppelt ausgeführt zur überprüfung des Wahrheitsgehaltes der Funktionen.
			* 
			* 
			*/


			time += Time.deltaTime;

		
				
				EyeXPos = binocularPor.x;
				EyeYPos = binocularPor.y;

				//Debug.Log ("LetterPos [x;y] " + LetterxPosFOV + "," + LetteryPosFOV + "EyeXPos " + EyeXPos + "," + EyeYPos);

				//Richtungsvektor des Eye berechnen
				EyeDirVecX = EyeXPos - EyeOldPosX;
				EyeDirVecY = EyeYPos - EyeOldPosY;

            
				//Richtungsvektor des Buchstaben
				LetterDirVecX = LetterxPosFOV - LetterOldPosX;
				LetterDirVecY = LetteryPosFOV - LetterOldPosY;

				//Richtungsvektoren in Vecotrschreibweise
				//Eye
				RichtungsVecEye = new Vector2 (EyeDirVecX, EyeDirVecY);
				//Letter
				RichtungsVecLetter = new Vector2 (LetterDirVecX, LetterDirVecY);

				//Distanz zwischen alter und neuer Position (Länge des Richtugnsvektors)
				EyeDistance = Vector2.Distance (new Vector2 (EyeXPos, EyeYPos), new Vector2 (EyeOldPosX, EyeOldPosY));
				LetterDistance = Vector2.Distance (new Vector2 (LetterxPosFOV, LetteryPosFOV), new Vector2 (LetterOldPosX, LetterOldPosY));


				//normierter Richtungsvektor Letter und Eye
				NorRichtungsVecLetter = (1 / LetterDistance) * RichtungsVecLetter;
				NorRichtungsVecEye = (1 / EyeDistance) * RichtungsVecEye;

	
				//Geschwindigkeitsberechnung der Buchstaben über Distanz und Zeit
			EyeSpeed = (EyeDistance * (time-oldtime));
			LetterRealspeed = (LetterDistance * (time-oldtime));

			
				//Geschwindigkeitsabgleich zwischen Auge und Buchstabe
				SpeedDivision = (LetterRealspeed / EyeSpeed); 

				//Winkelberechnung des Winkels zwischen Richtungsvektor Buchstabe und Richtungsvektor Auge
				DirAngelLetterToGazeVec = Vector2.Angle (RichtungsVecLetter, RichtungsVecEye);
				NorDirAngelLetterToGazeVec = Vector2.Angle (NorRichtungsVecLetter, NorRichtungsVecEye);
				
				//Abstand zwischen Buchstabe und Augen Anhand der POR und FOV Position
				DistanceEyeAndLetterPOR = Vector2.Distance (new Vector2 (EyeXPos, EyeYPos), new Vector2 (LetterxPosFOV, LetteryPosFOV));

            //Berechnung der Match-Werte
				calculateMatchValue(SpeedDivision, angleCameraLetter, DirAngelLetterToGazeVec);
				calculateMatchValueGewichtet (SpeedDivision, angleCameraLetter, DirAngelLetterToGazeVec);
				calculateMatchValueGewichtet2 (SpeedDivision, angleCameraLetter, DirAngelLetterToGazeVec);

				//PORPosition
				EyeOldPosX = EyeXPos;
				EyeOldPosY = EyeYPos;

				//Letterposition 
				LetterOldPosX = LetterxPosFOV;
				LetterOldPosY = LetteryPosFOV;

				
				timedifferenz = 0f;
			

				oldtime = time;
				
			}
	}


	public void calculateMatchValueGewichtet(float speed, float Pos, float Direction){
		MatchValueCalculationGewichtet = ((GewiSpeed*(Mathf.Abs(1-speed))) + (GewiPos*Pos) + (GewiDir*Direction));
		}

	public void calculateMatchValueGewichtet2(float speed, float Pos, float Direction){
		MatchValueCalculationGewichtet2 = ((Gewi2Speed*(Mathf.Abs(1-speed))) + (Gewi2Pos*Pos) + (Gewi2Dir*Direction));
	}

	public void calculateMatchValue(float speed, float Pos, float Direction){
		MatchValueCalculation = ((Mathf.Abs(1-speed)) + Pos + Direction);
		}



	public void resetMatchCounter(){
		MatchCounter = 0;
		MatchCounterGewichtet = 0;
		MatchCounterGewichtet2 = 0;
        MatchCounterWhole = 0;
		MatchCounterAfterLayerSelection = 0;
		MatchCounterAfterLayerSelectionGewi2 = 0;
	}


	public void increaseMatchCounterAfterLayerSelection(){
		MatchCounterAfterLayerSelection++;
	}

	public void increaseMatchCounterAfterLayerSelectionGewi2(){
		MatchCounterAfterLayerSelectionGewi2++;
	}


	public void increaseMatchCounter(){
		MatchCounter++;
	}

	public void increaseMatchCounterGewichtet(){
		MatchCounterGewichtet++;
	}

	public void increaseMatchCounterGewichtet2(){
		MatchCounterGewichtet2++;
	}

    public void increaseMatchCounterWhole()
    {
        MatchCounterWhole++;
    }




    public int getMatchCounterWhole()
    {
        return MatchCounterWhole;
    }


    public int getMatchCounter(){
		return MatchCounter;
	}

	public int getMatchCounterGewichtet(){
		return MatchCounterGewichtet;
	} 

	public int getMatchCounterGewichtet2(){
		return MatchCounterGewichtet2;
	} 

	public int getMatchCounterAfterLayerSelection(){
		return MatchCounterAfterLayerSelection;
	}

	public int getMatchCounterAfterLayerSelectionGewi2(){
		return MatchCounterAfterLayerSelectionGewi2;
	}


	public float getMatchValueGewichtet(){
		return MatchValueCalculationGewichtet;
	}

	public float getMatchValueGewichtet2(){
		return MatchValueCalculationGewichtet2;
	}

	public float getMatchValue(){
		return MatchValueCalculation;
	}


	public void setToStartPosition(){
		Vector3 StartPosition = new Vector3 (StartLetterPosX, StartLetterPosY, StartLetterPosZ);
		this.transform.localPosition = StartPosition;

	}


	public float getDirwinkel(){
			return DirAngelLetterToGazeVec;
	}

	public float getNormDirWinkel(){
		return NorDirAngelLetterToGazeVec;
	}

	public float getSpeedDivision(){
		return SpeedDivision;
	}

	public float getDistanceEyeAndLetterPOR(){
		return DistanceEyeAndLetterPOR;
	}

	public Vector2 getRichtungsvectorLetter(){
		return RichtungsVecLetter;
	}

	public Vector2 getRichtungsVecEye(){
		return RichtungsVecEye;
	}

	public float getRealSpeed(){
		return LetterRealspeed;
	}

	public float getEyeSpeed(){
		return EyeSpeed;
	}

	public float getXPosLetter(){
			return LetterxPosFOV;
	}

	public float getYPosLetter(){
		return LetteryPosFOV;
	}

	public float getEyeXPos(){
		return EyeXPos;
	}

	public float getEyeYPos(){
		return EyeYPos;
	}

	public string getName(){
		return name;
	}

	public float getangleCameraLetter(){
		return angleCameraLetter;
	}

	public float getangleleftGazeDirection(){
		return angleleftGazeDirection;
	}

	public float getangleRightGazeDirection(){
		return angleRightGazeDirection;
	}

}
