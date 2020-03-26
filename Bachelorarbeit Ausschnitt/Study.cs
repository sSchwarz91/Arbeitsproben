using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Study : MonoBehaviour
{

	/*Dieses File enthält nur einen Ausschnitt aus dem Ursprünglichen Dokument und ist so nicht lauffähig*/

    [Header("Study Values: ")]


    /*...*/


	private bool Timedetected = false;
	private bool WordisShown = false;
	private int Worldcounter = 0;
	private GameObject Text;

	[Header("WordData: ")]
	public GameObject WordLayer;
	public Text Satz;
	public Text Word;
	public float WordMaxTime = 10f;
	public float WordTime = 0f;


	[Header("AudioData: ")]
	public AudioClip SoundClip;
	public AudioSource AudioSource;
	bool AudiClipPlayed = false;

	/*...*/


	void Start()
    {
		// Get eyetracker
		smiInstance = SMI.SMIEyeTrackingUnity.Instance;


        //init  of the keybord        
        keyBoardAlpha = GameObject.Find("keyboard_Alpha");

        //init of the feedback text under the keyboard
		Text = GameObject.Find ("Satz");
        Satz = Text.GetComponent<Text>();

        //inti the Wordlayer which displays the words to write
        WordLayer = GameObject.Find ("WordLayer");
		WordLayer.SetActive (false);
		Word = WordLayer.transform.GetChild (0).GetComponent<Text> ();

		//save all Layers in an Array
        Layers = new GameObject[keyBoardAlpha.transform.childCount];

        //saves all Letters in an Array
		AllLetter = new GameObject[32];

        //fency Soundclip to show the end of the Worddisplaytime
		AudioSource.clip = SoundClip;


        //init words of Session 1 the trainingssession

        Word1 = new string[] {"D" , "A" , "S" , "S" };
		Word2 = new string[] { "P", "R", "A", "L", "L" };
		Word3 = new string[] { "V", "O", "M" };
		Word4 = new string[] { "W", "H", "I" , "S" , "K", "Y" };
		Word5 = new string[] { "F", "L", "O" , "G" };
		Word6 = new string[] { "X", "Y", "Z" };
		Word7 = new string[] {"Q" , "U" , "A" , "X" };
		Word8 = new string[] { "D", "E", "N" };
		Word9 = new string[] { "J", "E", "T" };
		Word10 = new string[] { "Z", "U" };
		Word11 = new string[] { "B", "R", "U" , "C" , "H" };
        Word12 = new string[] { "J", "E", "D", "E", "R" };
        Word13 = new string[] { "B", "A", "Y" , "E" , "R" };
        Word14 = new string[] { "W", "A", "C", "K", "E", "R" , "E" };
        Word15 = new string[] { "V", "E", "R", "T" , "I" ,"L" , "G" , "T" };
        Word16 = new string[] { "B", "E", "Q" , "U" , "E" , "M" };
        Word17 = new string[] { "Z", "W", "O" };
        Word18 = new string[] { "K", "A", "L" , "B" , "S" , "H" , "A" , "X" , "E" };

        Session1 = new string[] {string.Join ("", Word1) , string.Join ("", Word2) , string.Join ("", Word3) , string.Join ("", Word4) , string.Join ("", Word5) , string.Join ("", Word6) , string.Join ("", Word7) , string.Join ("", Word8) , string.Join ("", Word9) , string.Join ("", Word10) , string.Join ("", Word11) , string.Join("", Word12), string.Join("", Word13), string.Join("", Word14), string.Join("", Word15), string.Join("", Word16), string.Join("", Word17), string.Join("", Word18)};



        //randomize the order of Words in the Sessions
		Session1.ListShuffle<string> ();
		

	 
       

        //Initialisation of all important Eye and Letter data
		EyeSpeed = new float[32];
		EyePosX = new float[32];
		EyePosY = new float[32];
		EyeDirVec = new Vector2 [32];
		Letterspeed = new float [32];
		LetterPosX = new float[32];
		LetterPosY = new float[32];
		LetterDirVec = new Vector2 [32];
		LetterName = new string[32];
		Layerspeed = new float [4];
		LayerPosX = new float[4];
		LayerPosY = new float[4];
		LayerDirVec = new Vector2 [4];
		LayerName = new string[4];
		AngelDirLetterDirEye = new float[32];
		DistLetterEyePOR = new float [32];
		SpeedDivision = new float[32];
		AngleCameraLetter = new float[32];
		LeftGazeDirAngleCameraLetter = new float[32];
		RightGazeDirAngleCameraLetter = new float[32];
		NormDirWinkel = new float [32];
		MatchValue = new float[32];
		MatchValueGewichtet = new float [32];
		MatchValueGewichtet2 = new float [32];
		MatchCounter = new float [32];
        MatchCounterWhole = new float[32];
		MatchCounterAfterLayer = new float[32];
		MatchCounterAfterLayerGewi2 = new float[32];
        MatchCounterGewichtet = new float [32];
		MatchCounterGewichtet2 = new float [32];
		LayerMatchValueGewichtet = new float [4];
		LayerCounter = new float[4];
		LayerCounterGewichtet = new float[4];
		Letters = new GameObject[8];
			   		 

        //Init Logger für Augendaten
        EyeLogger.getInstance().initLogger(participantNumber);
        EyeLogger.getInstance().log("timestamp", "ParticipantNumber", "Name", "ObjPosX", "ObjPosY", "EyePosX ", "EyePosY", "DistObjEyePOR", "Objspeed", " EyeSpeed", "SpeedDivision ", "ObjDirVec", "EyeDirVec", "normierDirectionAngle", "AngleCameraObj", "LeftGazeDirAngleObj", "RightGazeDirAngleObj", "MatchValue", "MatchValueGewichtet", "LetterMatchCounter", "LetterMatchCounterGewichtet", "LayerCunterGewichtet", "MatchCounterWhole", "durchgang", "MatchValueGewichtet2", "MatchCounterGewichtet2", "MatchCounterAfterLayerSelect", "MatchCounterAfterLayerSelectGewi2");


        //Init Logger für Studiendaten
        WritingLogger.getInstance().initLogger(participantNumber);
        WritingLogger.getInstance().log("timestamp", "ParticipantNumber", "maxLayertime", " maxtime ", "SessionNr", "StepNumber", "WantedKey", "PressedKey", "CorrectKeyPressed", "TimeBetweenLetterAnimationAndItsSelection", "TimeBetweenSelections", "TimeBetweenLastLetterSelectedAndNewOne", "NumberOfRound", "SelectedKeyGewi2", "CorrectKeyGewi", "SelectedKeyAfterLayerSelected", "correctKeyAfterLayer", "SelectedKeyAfterLayerGewi2", "correctKeyAfterLayerGewi2");




        //Write all Letters in an Array AllLetter[i], i = [0...31] by saving the children of the Layer [j] , j =[0...3]

        for (int j = 0; j < keyBoardAlpha.transform.childCount; j++) {
				if (keyBoardAlpha.transform.GetChild (j).transform.tag == "Layers") {
					Layers [j] = keyBoardAlpha.transform.GetChild (j).transform.gameObject;
					keyboardKeys = new GameObject[Layers [j].transform.childCount];
					for (int i = 0; i < Layers [j].transform.childCount; i++) {
						if (Layers [j].transform.GetChild (i).transform.tag == "KeyboardKey") {

							keyboardKeys [i] = Layers [j].transform.GetChild (i).transform.gameObject;

						AllLetter[lettercount] = keyboardKeys[i];
						lettercount++;	
						}
						}

				}
		}

    }

        // Use this for initialization
       
        void Update()
	{
		ulong timeStamp = smiInstance.smi_GetTimeStamp();
		Vector3 leftGazeDirection = smiInstance.smi_GetLeftGazeDirection ();
		Vector3 rightGazeDirection = smiInstance.smi_GetRightGazeDirection ();
		Vector2 binocularPor = smiInstance.smi_GetBinocularPor ();
		Vector3 cameraRaycast = smiInstance.smi_GetCameraRaycast ();
		Vector2 leftPor = smiInstance.smi_GetLeftPor ();
		Vector2 rightPor = smiInstance.smi_GetRightPor ();
		Vector3 leftBasePoint = smiInstance.smi_GetLeftGazeBase ();
		Vector3 rightBasePoint = smiInstance.smi_GetRightGazeBase ();



        //IF Space Button is pressed, the study starts
		if (Input.GetKey(KeyCode.Space))
		{
			StartStudy();
		}
		
			   
	
		//If WordIsShown go back in displayWord() to increase timer
		if (WordisShown == true) {
			displayWord ();
		}


			   

		//Test whether Eyetracking is detected an Animation has startet to do all calculations like Layerselection and Letterselection 
		//repeat until study ends
		if ((SMI.SMIEyeTrackingUnity.smi_IsValid (cameraRaycast)) && (AnimationStarted == true)) {
			if (Timedetected == false) {
                //saves the time of first eyecontact of a Layer
				TimeOfFirstEyeContact = Time.realtimeSinceStartup;
				Timedetected = true;
			}

			time += Time.deltaTime;
				if (time <= maxLayertime) {				
				searchLayer ();	
                //Logs Eye and Layer data while Layer selection is active
				logLayerData ();
                //Logs Letter and EyeData while Layer selection is active
                logEyeDataWhileLayer();
                //Test if maxtime of Layer selection is exceeded and check out which layer is selected
			} else if (time > maxLayertime && time <= maxtime) {
				if (LayerAuswahlFertig == false) {
                    //test which layer is selected
					TestLayerisPressed ();
				}
                //logs Eye Data
				logEyeData ();
			} else {
				if ((time > maxtime) &&  (LayerAuswahlFertig == true)) {
                    //check which letter is pressed after layer is selected and maxtimer of letterselection is exceeded
					TestLetterIsPressed ();
                    //reset time for next firstEyeContact check
					time = 0f;
					}
			}
		}
	}

	   	 


	//Function to display the Wordlayer to show parcitipant which world should be written next and initialisation of the sound signal
	public void displayWord(){		
		WordTime += Time.deltaTime;
		if (WordTime <= WordMaxTime) {
			WordisShown = true;
			WordLayer.SetActive(true);
			if ((WordTime > (WordMaxTime - 2f)) && (WordTime <= WordMaxTime) && (AudiClipPlayed == false)) {
				AudioSource.Play();
				AudiClipPlayed = true;
			}
		}else{
            //Deleting the shown word and set the Display invisible
			Satz.text = "";
			WordLayer.SetActive(false);
			WordisShown = false;
			WordTime = 0f;
			AudiClipPlayed = false;
		}
	}
	

	//Set the Currentword to WordLayer to show parcitipant next word to write
	public void setWordToLayer(string actualWord){		
		Word.text = actualWord;		
		displayWord ();
	}





	/*...*/



    //This function checks which session is active and set all values which are necessary for the current Session like maxtime until a Letter is selected 
    //saves the current Session and increments the Wordstep counter
    //Show up the Word on the Layer which presents the next Word to write and test which word is the actual one for further calculations
	public void nextSession(){
		if (SessionStep <= 0) {
			Debug.Log ("Session not started yet");
		} else if (SessionStep == 1) {
			WritingLogger.getInstance ().log("Session 1 Started");
			currentSession = Session1;
			setMaxTime (1.2f);
			setMaxLayerTime (0.8f);
			//Wordstep became 1
			WordStep++;
			setWordToLayer(Session1[WordStep -1]);
			testActualWord(WordStep);
			Debug.Log ("You Are in Session1");
            //increments the Session step after all important calculations
				SessionStep++;
		} else if (SessionStep == 2) {	
			WritingLogger.getInstance ().log("Session 2 Started");
			currentSession = Session2;
			setMaxTime (1.0f);
			setMaxLayerTime (0.66f);
			WordStep++;
			setWordToLayer(Session2[WordStep -1]);
			testActualWord(WordStep);
			Debug.Log ("You Are in Session2");
				SessionStep++;
		} else if (SessionStep == 3) {	
			WritingLogger.getInstance ().log("Session 3 Started");
			currentSession = Session3;
			setMaxTime (0.8f);
			setMaxLayerTime (0.6f);
			WordStep++;
			setWordToLayer(Session3[WordStep -1]);
			testActualWord(WordStep);
			Debug.Log ("You Are in Session3");
			SessionStep++;
		} else if (SessionStep == 4) {		
			WritingLogger.getInstance ().log("Session 4 Started");
			currentSession = Session4;
			setMaxTime (0.6f);
			setMaxLayerTime (0.5f);
			WordStep++;
			setWordToLayer(Session4[WordStep -1]);
			testActualWord(WordStep);
			Debug.Log ("You Are in Session4");

			SessionStep++;
		} else if (SessionStep == 5) {
			WritingLogger.getInstance ().log("Session 5 Started");
			currentSession = Session5;
			setMaxTime (0.4f);
			setMaxLayerTime (0.35f);
			WordStep++;
			setWordToLayer(Session5[WordStep -1]);
			testActualWord(WordStep);
			Debug.Log ("You Are in Session5");
				SessionStep++;
		} else if (SessionStep == 6) {
			WritingLogger.getInstance ().log("Session 6 Started");
			currentSession = Session6;
			setMaxTime (0.3f);
			setMaxLayerTime (0.28f);
			WordStep++;
			setWordToLayer(Session6[WordStep -1]);
		testActualWord(WordStep);
			Debug.Log ("You Are in Session6");

			SessionStep++;		
		}else {			

            //if session number is 7 or greater then the study is over and all Counters and Stats where set to default =  0;
			StudyIsRunning = false;
			SessionStep = 0;
			currentStep = 0;
			WordStep = 0;
			setWordToLayer("Danke für die Teilnahme");
				Debug.Log ("Study is done");
			}
			
		}



    //function to set the next word.
    //also displays next word to Wordlayer 
	public void nextWord(){
		if (WordStep <= 0) {
			Debug.Log ("Study not running");
            //test if Wordcounter is over 0 and less than the maximal number of words of the actual session
            //if so, compares the nextword and saves it to the currenword var . Displays new word to the Wordlayer
		} else if (WordStep > 0 && WordStep <= currentSession.Length) {
			testActualWord (WordStep);
			setWordToLayer (currentSession [WordStep - 1]);
			} else {
            //if Wordstep is greater than the maximal number of words in the current session. Reset the Wordstep and Letterstep and starts the next session
			WordStep = 0;
			currentStep = 0;
			nextSession ();
		}
	}


	/*...*/


    //Logs all Layer Data and Eye data like position, speed etc.
	public void logLayerData(){

		int LayerCountData = 0;

		foreach(GameObject i in Layers){		
			if (LayerCountData < 4) {
				LayerName [LayerCountData] = i.GetComponent<LayerValues> ().getName ();
				Layerspeed [LayerCountData] = i.GetComponent<LayerValues> ().getRealSpeed ();
				LayerPosX [LayerCountData] = i.GetComponent<LayerValues> ().getXPosLayer ();
				LayerPosY [LayerCountData] = i.GetComponent<LayerValues> ().getYPosLayer ();
				LayerDirVec [LayerCountData] = i.GetComponent<LayerValues> ().getRichtungsvectorLayer ();
				EyeSpeed [LayerCountData] = i.GetComponent<LayerValues> ().getEyeSpeed ();
				EyePosX [LayerCountData] = i.GetComponent<LayerValues> ().getEyeXPos ();
				EyePosY [LayerCountData] = i.GetComponent<LayerValues> ().getEyeYPos ();
				EyeDirVec [LayerCountData] = i.GetComponent<LayerValues> ().getRichtungsVecEye ();
				AngelDirLetterDirEye [LayerCountData] = i.GetComponent<LayerValues> ().getDirwinkel ();
				NormDirWinkel [LayerCountData] = i.GetComponent<LayerValues> ().getNormDirWinkel ();
				DistLetterEyePOR [LayerCountData] = i.GetComponent<LayerValues> ().getDistanceEyeAndLayerPOR ();
				SpeedDivision [LayerCountData] = i.GetComponent<LayerValues> ().getSpeedDivision ();
				AngleCameraLetter [LayerCountData] = i.GetComponent<LayerValues> ().getangleCameraLayer ();
				LeftGazeDirAngleCameraLetter [LayerCountData] = i.GetComponent<LayerValues> ().getangleleftGazeDirection ();
				RightGazeDirAngleCameraLetter [LayerCountData] = i.GetComponent<LayerValues> ().getangleRightGazeDirection ();
				MatchValue [LayerCountData] = i.GetComponent<LayerValues> ().getMatchValue ();
				MatchValueGewichtet [LayerCountData] = i.GetComponent<LayerValues> ().getMatchValueGewichtet ();
				LayerCountData++;
			}          
		}

		for (int i = 0; i < 4; i++) {			
			LayerCounterGewichtet [i] = Layers [i].GetComponent<LayerValues> ().getMatchCounterGewichtet ();
			EyeLogger.getInstance ().log (Time.realtimeSinceStartup, participantNumber, LayerName [i], LayerPosX [i].ToString ("F5"), LayerPosY [i].ToString ("F5"), EyePosX [i].ToString ("F5"), EyePosY [i].ToString ("F5"), DistLetterEyePOR [i].ToString ("F5"), Layerspeed [i].ToString ("F5"), EyeSpeed [i].ToString ("F5"), SpeedDivision [i], LayerDirVec [i].ToString ("F5"), EyeDirVec [i].ToString ("F5"), NormDirWinkel [i], AngleCameraLetter [i].ToString ("F5"), LeftGazeDirAngleCameraLetter [i], RightGazeDirAngleCameraLetter [i], MatchValue [i], MatchValueGewichtet [i], "" , "", LayerCounterGewichtet [i]);
		}
		EyeLogger.getInstance ().log ("");

	}

	       

	

    //function to find the MatchValues of the Layers
	public void searchLayer(){
		Layercount = 0;
		foreach (GameObject j in Layers) {	
			if (Layercount < 4) {
				LayerMatchValueGewichtet[Layercount] = j.GetComponent<LayerValues>().getMatchValueGewichtet();
				Layercount++;
			}
		}
        //call function to find the layer with the maximal match counter
		findLayerMatch(Layercount);
	}




    //function to find the Layer with the minimal match Value plus save the index of the Layer with minimal Match Value and increments the Matchcounter of the Layer with this index
	public void findLayerMatch(int Layercount){
		float minValue = 1000;
		int counter = Layercount;

		for (int k = 0; k < 4; k++) {
			float actualLayerMatchValue = (float)LayerMatchValueGewichtet.GetValue (k);
			if (minValue > actualLayerMatchValue) {
				minValue = (float)LayerMatchValueGewichtet.GetValue (k);
				minIndex = k;				
			}
		}
        //increase the MatchCounter of the layer with the minimum Match Value
		Layers[minIndex].GetComponent<LayerValues>().increaseMatchCounterGewichtet();
	
		}




    //test which Letter is pressed with take into consideration of the selected layer. Important for the calculation. rest is just for the logger
	public void TestLetterIsPressed(){
		int maxValue = 0;
		int maxValueGewi = 0;
		int maxValueAfterLayer = 0;
		int maxValueAfterLayerGewi2 = 0;
		bool findLetter = false;
        //selected Letter weighted 1 with Layer selection
			for (int k = 0; k < 32; k++) {
			int actualLetterMatchCountValue = AllLetter[k].GetComponent<Values>().getMatchCounterGewichtet ();
				if (maxValue < actualLetterMatchCountValue) {
				maxValue = AllLetter[k].GetComponent<Values>().getMatchCounterGewichtet ();
					maxIndex = k;					
				}
		}		

		SelectedKey = AllLetter[maxIndex].transform.gameObject;	




        //letter is selected
				findLetter = true;			
			//resets all counters and animations
			if (findLetter == true) {
				for (int i = 0; i < 32 ; i++){
				AllLetter[i].GetComponent<Values> ().resetMatchCounter ();
				}
				resetAnimation();
			nextStep (SelectedKey , SelectedKeyGewi , SelectedKeyAfterLayer , SelectedKeyAfterLayerGewi2);
				findLetter = false;
			}


	}



    //stops layer animation
	public void stopLayerAnimation(){
		for (int i = 0 ; i < 4 ; i++){
			//Layers [i].GetComponent<GazeInteractionGeneric>().resetGazeInProgress();
			animation = Layers [i].GetComponent<PolarAnimation> ();
			animation.setAnimationStatus (false);

		}
	}




    //reset all Animations, reset Layer status , set animation status to false
	public void resetAnimation (){		
		
		for (int k = 0; k < 32; k++) {
			AllLetter [k].GetComponent<Values> ().setToStartPosition ();
			animation = AllLetter [k].GetComponent<PolarAnimation> ();
			animation.setAnimationStatus (false);
			AnimationStarted = false;
			animation.setStartTime (0f);
		}			
		for (int i = 0 ; i < 4 ; i++){
				//Layers [i].GetComponent<GazeInteractionGeneric>().resetGazeInProgress();
				Layers[i].GetComponent<LayerValues>().setToStartPosition();
				animation = Layers [i].GetComponent<PolarAnimation> ();
				animation.setStartTime (0f);
		}
		SetLayerStatus (false, false, false, false);
		LayerAuswahlFertig = false;
		//Debug.Log ("Reseted layerStatus");
	}




    //resetMatchCounters
    public void resetMatchValue(){
		for (int k = 0 ; k < 32 ; k++){
			AllLetter[k].GetComponent<Values>().resetMatchCounter();
		}
		for (int i = 0; i < 4; i++) {
			Layers [i].GetComponent<LayerValues> ().resetMatchCounter ();
		}
	}



    //start Study. Increments Session and Letter Counter. Set Study status to active, Save the starttime of the study. Call the next Session function
    public void StartStudy()
    {
        if (StudyIsRunning == false)
        {
			
            Debug.Log("Study startet");
			currentStartTime = Time.realtimeSinceStartup;
            StudyIsRunning = true;
			resetMatchValue();
			currentStep++;
			SessionStep++;
			nextSession ();
        }
        else
        {
         //   Debug.Log("Study is already running");
        }

    }


    /* From here: all needed getter and Setter functions
     *
     *
     *
     *
     */
   



	public void setMaxTime(float max){
		maxtime = max;
	}



	public void setMaxLayerTime(float maxLayer){
		maxLayertime = maxLayer;
	}


	public bool getStudyStatus (){
		return StudyIsRunning;
	}


    public void setGazeDwellTime(float number){
		dwelltime = number;
	}


	public void setParticipantNumber(int numberOfPart){
		participantNumber = numberOfPart;
	}


	public void setAnimationStarted(bool status){
		AnimationStarted = status;
	}


	public void setAnimationStartTime (float AnimStartTime){
		timeOfAnimationStart = AnimStartTime;
	}


	public bool getWordLayerStatus(){
		return WordisShown; 
	}


	public void SetLayerStatus (bool Layer1, bool Layer2 , bool Layer3, bool Layer4){
		Layer1IsActive = Layer1;
		Layer2IsActive = Layer2;
		Layer3IsActive = Layer3;
		Layer4IsActive = Layer4;
	}


	public bool getAnimationStarted(){
		return AnimationStarted;
	}


    }

    

   


	

